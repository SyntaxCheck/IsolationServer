using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class MatchListenerThread
{
    private const int MAX_LOOPS_WITHOUT_CLEANUP = 10000;
    private LogInfo logInfo;
    public int Port { get; set; }
    public string ServerError { get; set; }
    private TcpListener Server { get; set; }
    public bool Stop { get; set; }
    public bool HasStopped { get; set; }
    public List<MatchSession> Sessions { get; set; }
    public int LoopsWithoutCleanup { get; set; }

    public MatchListenerThread()
    {
        //Set Current Directory when running from service
        //System.IO.Directory.SetCurrentDirectory(System.AppDomain.CurrentDomain.BaseDirectory);

        Stop = false;
        HasStopped = false;
        LoopsWithoutCleanup = 0;

        ServerError = String.Empty;
        logInfo = new LogInfo();
        logInfo.RootFolder = Directory.GetCurrentDirectory();
        logInfo.LogFolder = "LogsFolder";
        logInfo.LogFileName = "Log.txt";
        logInfo.AppendDateTime = true;
        logInfo.AppendDateTimeFormat = "yyyyMMdd";
        logInfo.IsDebug = false;
        Logger.ValidateLogPath(ref logInfo);

        if (!logInfo.PathValidated)
        {
            ServerError = "Failed to initialize the log path";
        }

        Sessions = new List<MatchSession>();
    }

    public void StartListening()
    {
        try
        {
            Server = new TcpListener(GetIpAddress(), Port);
            Server.Start();

            Byte[] bytes = new Byte[32768];
            String data = null, response = null;

            while (!Stop)
            {
                try
                {
                    //If there is a pending message to the server or we have gone to long without checking for inactive sessions
                    if (Server.Pending() && LoopsWithoutCleanup < MAX_LOOPS_WITHOUT_CLEANUP)
                    {
                        LoopsWithoutCleanup++;
                        TcpClient client = Server.AcceptTcpClient();
                        data = null;
                        NetworkStream stream = client.GetStream();

                        int i;
                        while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            // Translate data bytes to a ASCII string.
                            data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);

                            bool foundEndChar = false;
                            if (data.Trim().EndsWith("<END>"))
                            {
                                foundEndChar = true;
                                data = data.Replace("<END>", String.Empty);
                            }

                            //Determine what to do with the data
                            response = HandleRequest(data);

                            byte[] msg = System.Text.Encoding.ASCII.GetBytes(response);
                            // Send back a response.
                            stream.Write(msg, 0, msg.Length);

                            if (foundEndChar)
                                break;
                        }

                        // Shutdown and end connection
                        client.Close();
                        stream.Close();
                        client.Dispose();
                        stream.Dispose();
                    }
                    else
                    {
                        bool needSleep = (LoopsWithoutCleanup < MAX_LOOPS_WITHOUT_CLEANUP);
                        LoopsWithoutCleanup = 0;

                        for (int i = 0; i < Sessions.Count(); i++)
                        {
                            if (!Sessions[i].SessionExpired)
                            {
                                //Check for inactive client
                                if (!String.IsNullOrEmpty(Sessions[i].FirstUser) && Sessions[i].FirstUserLastPing != null)
                                {
                                    if ((DateTime.Now - (DateTime)Sessions[i].FirstUserLastPing).TotalSeconds > MatchSession.CLIENT_TIMEOUT_SECONDS)
                                    {
                                        Sessions[i].DoppedConnectionStatusMsg = "User Connection dropped, restart with new connection";
                                        Sessions[i].SessionExpired = true;
                                        Sessions[i].SessionEndDate = DateTime.Now;
                                        Logger.Write(logInfo, "Session(" + Sessions[i].SessionId.ToString() + ") Dropped user due to lack of response within timeout period. User name: " + Sessions[i].FirstUser);
                                    }
                                }
                                if (!String.IsNullOrEmpty(Sessions[i].SecondUser) && Sessions[i].SecondUserLastPing != null)
                                {
                                    if ((DateTime.Now - (DateTime)Sessions[i].SecondUserLastPing).TotalSeconds > MatchSession.CLIENT_TIMEOUT_SECONDS)
                                    {
                                        Sessions[i].DoppedConnectionStatusMsg = "User Connection dropped, restart with new connection";
                                        Sessions[i].SessionExpired = true;
                                        Sessions[i].SessionEndDate = DateTime.Now;
                                        Logger.Write(logInfo, "Session(" + Sessions[i].SessionId.ToString() + ") Dropped user due to lack of response within timeout period. User name: " + Sessions[i].SecondUser);
                                    }
                                }
                            }
                        }

                        if(needSleep) //If we got into this function by a force then we do not need to sleep the thread
                            Thread.Sleep(10);
                    }
                }
                catch (Exception ex)
                {
                    Logger.Write(logInfo, "StartListening() Loop Exception", ex);
                }
            }

            Server.Stop();
        }
        catch (Exception oex)
        {
            Logger.Write(logInfo, "StartListening() General Exception", oex);
        }
        HasStopped = true;
    }
    private string HandleRequest(string data)
    {
        string rtn = String.Empty;

        if (!String.IsNullOrEmpty(data))
        {
            if (data.Trim().ToUpper() == "TESTPING")
            {
                rtn = "ONLINE";
            }
            else
            { 
                string[] split = data.Split('|');

                for (int i = 0; i < split.Length; i++)
                {
                    split[i] = split[i].Trim();
                }

                if (split.Length > 0)
                {
                    string command = split[0];
                    string channel = String.Empty;
                    string channelPwd = String.Empty;
                    bool messageForwarded = false;
                    bool hasChannelInfo = false;

                    if (split.Length >= 4)
                    {
                        channel = split[2];
                        channelPwd = split[3];
                        hasChannelInfo = true;
                    }

                    if (command.Trim().ToUpper() == "CONNECTION")
                    {
                        if (!hasChannelInfo) //Handle Auto join
                        {
                            for (int i = 0; i < Sessions.Count(); i++)
                            {
                                if (!Sessions[i].IsMatchFull && Sessions[i].IsAutoJoinMatch)
                                {
                                    messageForwarded = true;
                                    rtn = Sessions[i].HandleRequest(data);
                                    break;
                                }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < Sessions.Count(); i++)
                            {
                                string validateMessage = String.Empty;
                                if (IsCorrectSession(channel, channelPwd, Sessions[i], ref validateMessage))
                                {
                                    messageForwarded = true;
                                    rtn = Sessions[i].HandleRequest(data);
                                    break;
                                }
                                else if (!String.IsNullOrEmpty(validateMessage))
                                {
                                    messageForwarded = true;
                                    rtn = validateMessage;
                                    break;
                                }
                            }
                        }

                        //No match exists if the message has not been forwarded, create new one
                        if (!messageForwarded)
                        {
                            int newSessionId = 1;
                            if (Sessions.Count() > 0)
                            {
                                newSessionId = Sessions.Max(t => t.SessionId) + 1;
                            }

                            MatchSession newSession = new MatchSession(newSessionId); //Pass in the count for the session ID
                            rtn = newSession.HandleRequest(data);
                            Sessions.Add(newSession);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < Sessions.Count(); i++)
                        {
                            string validateMessage = String.Empty;
                            if (IsCorrectSession(channel, channelPwd, Sessions[i], ref validateMessage))
                            {
                                messageForwarded = true;
                                rtn = Sessions[i].HandleRequest(data);
                                break;
                            }
                            else if (!String.IsNullOrEmpty(validateMessage))
                            {
                                rtn = validateMessage;
                                break;
                            }
                        }
                    }

                }
            }
        }

        return rtn;
    }
    private IPAddress GetIpAddress()
    {
        IPHostEntry host;
        IPAddress localIP = IPAddress.Loopback;
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip;
                break;
            }
        }
        return localIP;
    }
    private bool IsCorrectSession(string channelIn, string channelPwnIn, MatchSession session, ref string message)
    {
        message = String.Empty;

        if (channelIn.Trim().ToUpper() == session.Channel.Trim().ToUpper())
        {
            if (channelPwnIn.Trim() == session.ChannelPassword.Trim())
            {
                if (!session.SessionExpired)
                {
                    return true;
                }
                else
                {
                    message = "Reject|Channel already exists. Session expired";
                }
            }
            else
            {
                message = "Reject|Channel already exists";
            }
        }

        return false;
    }
}

public enum GridState
{
    Blank = 0,
    Blocked = 1,
    Occupied = 2
}