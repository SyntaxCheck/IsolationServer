using SynUtil.Database.MSSQL;
using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkReceiver
{
    public partial class SessionViewer : Form
    {
        private const int EM_LINESCROLL = 0x00B6;

        [DllImport("user32.dll")]
        static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll")]
        static extern int SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        [DllImport("User32.Dll", EntryPoint = "PostMessageA")]
        static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, int lParam);

        private LogInfo logInfo;
        private DateTime StartTime;
        private Thread ListenerThread, DatabaseThread;
        private MatchListenerThread MatchListener;
        private DatabaseWriterThread DatabaseWriter;
        private SqlServerConInfo conInfo;
        private bool WriteStatsToDB = true;
        List<MatchHistoryAggregate> AggragateStats;
        private DateTime StatsLastWritten;
        private List<long> LastCommandsPerSecond;
        private long LastCps; 

        public SessionViewer()
        {
            InitializeComponent();
        }

        private void SessionViewer_Load(object sender, EventArgs e)
        {
            logInfo = new LogInfo();
            logInfo.RootFolder = Directory.GetCurrentDirectory();
            logInfo.LogFolder = "LogsFolder";
            logInfo.LogFileName = "Log.txt";
            logInfo.AppendDateTime = true;
            logInfo.AppendDateTimeFormat = "yyyyMMdd";
            logInfo.IsDebug = false;
            Logger.ValidateLogPath(ref logInfo);

            StartService();
            AggragateStats = new List<MatchHistoryAggregate>();
            LastCommandsPerSecond = new List<long>();
            LastCps = 0;
            lblLastDbWrite.Text = "Never";
        }
        private void SessionViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            StopService();
        }
        private void timerSessionViewer_Tick(object sender, EventArgs e)
        {
            try
            {
                string displayText = MatchListener.ServerError;

                if (!String.IsNullOrEmpty(displayText))
                {
                    displayText += Environment.NewLine;
                }

                long totalSessions = MatchListener.Sessions.Count();
                long totalMatches = 0;
                long totalClients = 0;
                long totalCommands = 0;
                long totalMoves = 0;

                for (int i = 0; i < MatchListener.Sessions.Count(); i++)
                {
                    totalMatches += MatchListener.Sessions[i].FirstUserWinCount + MatchListener.Sessions[i].SecondUserWinCount;
                    totalClients += MatchListener.Sessions[i].GetNumberOfConnected();
                    totalCommands += MatchListener.Sessions[i].ServerTotalCommands;
                    totalMoves += MatchListener.Sessions[i].ServerTotalMoves;

                    DateTime sessionEndDate = DateTime.Now;
                    if (MatchListener.Sessions[i].SessionEndDate != null)
                        sessionEndDate = (DateTime)MatchListener.Sessions[i].SessionEndDate;

                    displayText += "Channel Name(" + MatchListener.Sessions[i].SessionId.ToString() + "): " + MatchListener.Sessions[i].Channel + ", Connected: " + MatchListener.Sessions[i].GetNumberOfConnected().ToString() + ", Matches: " + LargeNumberToReadableText(MatchListener.Sessions[i].CompletedMatches.Count()) + Environment.NewLine;
                    displayText += "Session Start: " + MatchListener.Sessions[i].SessionStartDate.ToString("dddd, dd MMMM yyyy hh:mm:ss tt") + Environment.NewLine;
                    if (MatchListener.Sessions[i].CompletedMatches.Count() > 0)
                    {
                        TimeSpan avgDuration = GetTimeSpanAverage(MatchListener.Sessions[i].CompletedMatches.Select(ts => ts.MatchLength).ToList());
                        displayText += "Elapsed: " + TimeSpanFormatter.TimeSpanHighestDecimal((sessionEndDate - MatchListener.Sessions[i].SessionStartDate)) + ", Average: " + TimeSpanFormatter.TimeSpanHighestDecimal(avgDuration) + Environment.NewLine;
                    }
                    if (!String.IsNullOrEmpty(MatchListener.Sessions[i].FirstUser))
                        displayText += MatchListener.Sessions[i].FirstUser + ": " + LargeNumberToReadableText(MatchListener.Sessions[i].FirstUserWinCount) + " (" + (100 * (Math.Round((double)MatchListener.Sessions[i].FirstUserWinCount / (MatchListener.Sessions[i].FirstUserWinCount + MatchListener.Sessions[i].SecondUserWinCount), 2))).ToString() + "%)" + Environment.NewLine;
                    if (!String.IsNullOrEmpty(MatchListener.Sessions[i].SecondUser))
                        displayText += MatchListener.Sessions[i].SecondUser + ": " + LargeNumberToReadableText(MatchListener.Sessions[i].SecondUserWinCount) + " (" + (100 * (Math.Round((double)MatchListener.Sessions[i].SecondUserWinCount / (MatchListener.Sessions[i].FirstUserWinCount + MatchListener.Sessions[i].SecondUserWinCount), 2))).ToString() + "%)" + Environment.NewLine;
                    displayText += "____________________________________________" + Environment.NewLine;

                    //Add files to Aggragator list
                    if (WriteStatsToDB)
                    {
                        for (int k = 0; k < MatchListener.Sessions[i].CompletedMatches.Count(); k++)
                        {
                            if (!MatchListener.Sessions[i].CompletedMatches[k].HasBeenWrittenToDB)
                            {
                                bool foundInAgg = false;
                                for (int j = 0; j < AggragateStats.Count(); j++)
                                {
                                    if ((AggragateStats[j].GridWidth == MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(0) &&
                                        AggragateStats[j].GridHeight == MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(1) ||
                                        AggragateStats[j].GridWidth == MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(1) &&
                                        AggragateStats[j].GridHeight == MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(0)))
                                    {
                                        if (AggragateStats[j].UserOneVersion == MatchListener.Sessions[i].CompletedMatches[k].UserOneVersion &&
                                            AggragateStats[j].UserOneAlgorithm == MatchListener.Sessions[i].CompletedMatches[k].UserOneAlgorithm &&
                                            AggragateStats[j].UserOneConfig == MatchListener.Sessions[i].CompletedMatches[k].UserOneConfig &&
                                            AggragateStats[j].UserTwoVersion == MatchListener.Sessions[i].CompletedMatches[k].UserTwoVersion &&
                                            AggragateStats[j].UserTwoAlgorithm == MatchListener.Sessions[i].CompletedMatches[k].UserTwoAlgorithm &&
                                            AggragateStats[j].UserTwoConfig == MatchListener.Sessions[i].CompletedMatches[k].UserTwoConfig)
                                        {
                                            if (MatchListener.Sessions[i].CompletedMatches[k].UserOneIsWinner)
                                            {
                                                AggragateStats[j].UserOneWinCount++;
                                            }
                                            else
                                            {
                                                AggragateStats[j].UserTwoWinCount++;
                                            }

                                            foundInAgg = true;

                                            break;
                                        }
                                        else if (AggragateStats[j].UserOneVersion == MatchListener.Sessions[i].CompletedMatches[k].UserTwoVersion &&
                                            AggragateStats[j].UserOneAlgorithm == MatchListener.Sessions[i].CompletedMatches[k].UserTwoAlgorithm &&
                                            AggragateStats[j].UserOneConfig == MatchListener.Sessions[i].CompletedMatches[k].UserTwoConfig &&
                                            AggragateStats[j].UserTwoVersion == MatchListener.Sessions[i].CompletedMatches[k].UserOneVersion &&
                                            AggragateStats[j].UserTwoAlgorithm == MatchListener.Sessions[i].CompletedMatches[k].UserOneAlgorithm &&
                                            AggragateStats[j].UserTwoConfig == MatchListener.Sessions[i].CompletedMatches[k].UserOneConfig)
                                        {
                                            if (!MatchListener.Sessions[i].CompletedMatches[k].UserOneIsWinner) //Flip it since we matched on opposite users
                                            {
                                                AggragateStats[j].UserOneWinCount++;
                                            }
                                            else
                                            {
                                                AggragateStats[j].UserTwoWinCount++;
                                            }

                                            foundInAgg = true;

                                            break;
                                        }
                                    }
                                }

                                if (!foundInAgg)
                                {
                                    MatchHistoryAggregate mha = new MatchHistoryAggregate();
                                    mha.GridWidth = MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(0);
                                    mha.GridHeight = MatchListener.Sessions[i].CompletedMatches[k].MatchGrid.GetLength(1);
                                    mha.UserOneWinCount = MatchListener.Sessions[i].CompletedMatches[k].UserOneIsWinner ? 1 : 0;
                                    mha.UserOneVersion = MatchListener.Sessions[i].CompletedMatches[k].UserOneVersion;
                                    mha.UserOneAlgorithm = MatchListener.Sessions[i].CompletedMatches[k].UserOneAlgorithm;
                                    mha.UserOneConfig = MatchListener.Sessions[i].CompletedMatches[k].UserOneConfig;
                                    mha.UserTwoWinCount = !MatchListener.Sessions[i].CompletedMatches[k].UserOneIsWinner ? 1 : 0;
                                    mha.UserTwoVersion = MatchListener.Sessions[i].CompletedMatches[k].UserTwoVersion;
                                    mha.UserTwoAlgorithm = MatchListener.Sessions[i].CompletedMatches[k].UserTwoAlgorithm;
                                    mha.UserTwoConfig = MatchListener.Sessions[i].CompletedMatches[k].UserTwoConfig;

                                    AggragateStats.Add(mha);
                                }

                                MatchListener.Sessions[i].CompletedMatches[k].HasBeenWrittenToDB = true;
                            }
                        }
                    }
                }

                //On interval pass the aggragate stats to the Database writer thread
                if ((DateTime.Now - StatsLastWritten).TotalMinutes >= 5)
                {
                    lock (DatabaseWriter.MatchesToWrite)
                    {
                        DatabaseWriter.MatchesToWrite.AddRange(AggragateStats);
                        AggragateStats = new List<MatchHistoryAggregate>();
                        StatsLastWritten = DateTime.Now;
                        lblLastDbWrite.Text = StatsLastWritten.ToString("yyyy-MM-dd hh:mm:ss");
                    }
                }

                //Calculate Commands per second
                long calculatedCps = totalCommands - LastCps;
                long cps = 0;

                if (calculatedCps > 0) //If we clear out sessions dont count those stats
                {
                    LastCommandsPerSecond.Add(calculatedCps);

                    while (LastCommandsPerSecond.Count() > 100)
                    {
                        LastCommandsPerSecond.RemoveAt(0);
                    }
                }
                LastCps = totalCommands;
                cps = (long)Math.Round(LastCommandsPerSecond.Average(),0);

                int scrollPos = GetScroll(tbxOutput, ScrollBars.Horizontal);

                tbxOutput.Text = displayText;
                lblTotalClients.Text = LargeNumberToReadableText(totalClients).ToString();
                lblTotalMatches.Text = LargeNumberToReadableText(totalMatches).ToString();
                lblTotalSessions.Text = LargeNumberToReadableText(totalSessions).ToString();
                lblCommands.Text = LargeNumberToReadableText(totalCommands).ToString();
                lblMoves.Text = LargeNumberToReadableText(totalMoves).ToString();
                lblCommandsPerSecond.Text = LargeNumberToReadableText(cps).ToString();

                SetServerUptime();
                ScrollTo(scrollPos, tbxOutput);
            }
            catch (Exception ex)
            {
                Logger.Write(logInfo, "Uncaught timer exception", ex);
            }
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            StopService();
            btnStart.Enabled = true;
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            StartService();
        }
        private void btnClearInactiveSessions_Click(object sender, EventArgs e)
        {
            for (int i = MatchListener.Sessions.Count() - 1; i >= 0; i--)
            {
                if (MatchListener.Sessions[i].SessionExpired)
                {
                    MatchListener.Sessions.RemoveAt(i);
                }
            }
        }
        private void btnKillSession_Click(object sender, EventArgs e)
        {
            int intOut;

            if (int.TryParse(tbxKillSession.Text, out intOut))
            {
                for (int i = MatchListener.Sessions.Count() - 1; i >= 0; i--)
                {
                    if (MatchListener.Sessions[i].SessionId == intOut)
                    {
                        MatchListener.Sessions.RemoveAt(i);
                    }
                }
            }
        }
        private void btnGetSessionMatches_Click(object sender, EventArgs e)
        {
            int intOut;

            if (int.TryParse(tbxMatchRetriever.Text, out intOut))
            {
                for (int i = MatchListener.Sessions.Count() - 1; i >= 0; i--)
                {
                    if (MatchListener.Sessions[i].SessionId == intOut)
                    {
                        MatchSessionGameSelector matchSelector = new MatchSessionGameSelector();
                        matchSelector.Matches = MatchListener.Sessions[i].CompletedMatches;
                        matchSelector.ShowDialog();

                        break;
                    }
                }
            }
        }
        private void btnWriteNow_Click(object sender, EventArgs e)
        {
            StatsLastWritten = new DateTime(1900, 01, 01);
        }
        private void btnDbViewer_Click(object sender, EventArgs e)
        {
            DatabaseViewer dbViewer = new DatabaseViewer();
            dbViewer.SqlServerInfo = conInfo;
            dbViewer.logInfo = logInfo;
            dbViewer.ShowDialog();
        }

        //Private functions
        public int GetScroll(TextBox tbxToScroll, ScrollBars scrollBar)
        {
            return GetScrollPos((IntPtr)tbxToScroll.Handle, (int)scrollBar);
        }
        public void ScrollTo(int Position, TextBox tbxToScroll)
        {
            SetScrollPos((IntPtr)tbxToScroll.Handle, 0x1, Position, true);
            PostMessage((IntPtr)tbxToScroll.Handle, 0x115, 4 + 0x10000 * Position, 0);
        }
        private int GetPort()
        {
            int port = 25174;

            int portOut;
            if (int.TryParse(tbxPort.Text, out portOut))
            {
                if (portOut > 0 && portOut < 49151)
                {
                    port = portOut;
                }
            }

            return port;
        }
        private void StopService()
        {
            MatchListener.Stop = true;
            if (WriteStatsToDB)
            {
                //TODO Write stats
                DatabaseWriter.Stop = true;
            }

            bool hasStopped = false;
            for (int i = 0; i < 300; i++)
            {
                if (MatchListener.HasStopped && (WriteStatsToDB && DatabaseWriter.HasStopped))
                {
                    hasStopped = true;

                    ListenerThread.Abort();
                    if(WriteStatsToDB)
                        DatabaseThread.Abort();

                    break;
                }
                else
                {
                    Thread.Sleep(10);
                }
            }

            if (!hasStopped)
            {
                //Took too long to stop, abort the threads
                ListenerThread.Abort();
                if(WriteStatsToDB)
                    DatabaseThread.Abort();
            }
        }
        private void StartService()
        {
            StartTime = DateTime.Now;
            StatsLastWritten = DateTime.Now;

            MatchListener = new MatchListenerThread();
            MatchListener.Port = GetPort();
            ListenerThread = new Thread(new ThreadStart(MatchListener.StartListening));
            ListenerThread.Start();

            if (Properties.Settings.Default.WriteSettings)
            {
                WriteStatsToDB = true;

                conInfo = new SqlServerConInfo();
                conInfo.Server = Properties.Settings.Default.SQLServerName;
                conInfo.Database = Properties.Settings.Default.Database;
                conInfo.User = Properties.Settings.Default.User;
                conInfo.Password = Properties.Settings.Default.Password;
                conInfo.DbParm = Properties.Settings.Default.DBParm;

                try
                {
                    SqlServer con = new SqlServer();
                    SqlConnection dbCon = con.GetConnection(conInfo);

                    dbCon.Open();
                    dbCon.Close();
                }
                catch (Exception sqlEx)
                {
                    MessageBox.Show("Failed to connect to DB. Ex: " + sqlEx.Message, "Failed to connect");
                    this.Close();
                }

                DatabaseWriter = new DatabaseWriterThread();
                DatabaseWriter.SqlServerInfo = conInfo;
                DatabaseThread = new Thread(new ThreadStart(DatabaseWriter.CheckForNewWrites));
                DatabaseThread.Start();
            }

            timerSessionViewer.Interval = 1000;
            timerSessionViewer.Start();
        }
        private void SetServerUptime()
        {
            string timespFormatted = String.Empty;
            TimeSpan ts = (DateTime.Now - StartTime);

            timespFormatted = TimeSpanFormatter.TimeSpanFormattedFull(ts);

            lblUptime.Text = "Server Uptime: " + timespFormatted;
        }
        private TimeSpan GetTimeSpanAverage(List<TimeSpan> list)
        {
            //double avgTicks = list.Average(timeSpan => timeSpan.Ticks);
            double avgTicks = 0;
            double totalTicks = 0;
            int avgCount = 0;
            long convAvgTicks = 0;

            for (int i = 0; i < list.Count(); i++)
            {
                totalTicks += list[i].Ticks;
                avgCount++;
            }

            avgTicks = Math.Round(totalTicks / avgCount,0);
            convAvgTicks = Convert.ToInt64(avgTicks);

            return new TimeSpan(convAvgTicks);
        }
        private string LargeNumberToReadableText(long num)
        {
            string formatted = String.Empty;

            if (num < 999)
            {
                formatted = num.ToString();
            }
            else if (num >= 1000 && num < 10000)
            {
                formatted = Math.Round(num / 1000.0, 2).ToString("#.##") + "k";
            }
            else if (num >= 10000 && num < 999999)
            {
                formatted = Math.Round(num / 1000.0, 0).ToString("#.##") + "k";
            }
            else if (num >= 1000000 && num < 10000000)
            {
                formatted = Math.Round(num / 1000000.0, 2).ToString("#.##") + "m";
            }
            else if (num >= 10000000 && num < 999999999)
            {
                formatted = Math.Round(num / 1000000.0, 0).ToString("#.##") + "m";
            }
            else if (num >= 1000000000 && num < 10000000000)
            {
                formatted = Math.Round(num / 1000000000.0, 2).ToString("#.##") + "b";
            }
            else if (num >= 10000000000 && num < 999999999999)
            {
                formatted = Math.Round(num / 1000000000.0, 0).ToString("#.##") + "b";
            }
            else if (num >= 1000000000000 && num < 10000000000000)
            {
                formatted = Math.Round(num / 1000000000000.0, 2).ToString("#.##") + "t";
            }
            else if (num >= 10000000000000 && num < 999999999999999)
            {
                formatted = Math.Round(num / 1000000000000.0, 0).ToString("#.##") + "t";
            }
            else
            {
                formatted = num.ToString();
            }

            return formatted;
        }
    }
}
