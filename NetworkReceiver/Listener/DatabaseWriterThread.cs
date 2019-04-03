using SynUtil.Database.MSSQL;
using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

public class DatabaseWriterThread
{
    private LogInfo logInfo;
    public SqlServerConInfo SqlServerInfo;
    public List<MatchHistoryAggregate> MatchesToWrite;
    public bool Stop;
    public bool HasStopped;
    public string ServerError;

    public DatabaseWriterThread()
    {
        Stop = false;
        HasStopped = false;
        MatchesToWrite = new List<MatchHistoryAggregate>();

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
    }

    public void CheckForNewWrites()
    {
        try
        {
            SqlServer sqlSvr = new SqlServer();
            SqlConnection con = sqlSvr.GetConnection(SqlServerInfo);
            con.Open();

            bool sqlConOpen = false;

            while (!Stop)
            {
                if (MatchesToWrite.Count() > 0)
                {
                    //Recheck to see if we need to re-open the SQL Server connection
                    if (con.State != System.Data.ConnectionState.Open)
                    {
                        try
                        {
                            con.Open();
                        }
                        catch (Exception sqlEx)
                        {
                            Logger.Write(logInfo, "Failed to open SQL Connection", sqlEx);
                        }
                    }

                    if (con.State == System.Data.ConnectionState.Open)
                    {
                        sqlConOpen = true;
                    }

                    if (sqlConOpen)
                    {
                        lock (MatchesToWrite)
                        {
                            for (int i = 0; i < MatchesToWrite.Count(); i++)
                            {
                                //First lookup to see if that record exists
                                SqlCommand cmd = new SqlCommand("Select Coalesce(Max(Matchup),-1) From MatchHistory Where GridWidth = @GridWidth And GridHeight = @GridHeight And ClientOneVersion = @ClientOneVersion And ClientOneAlgorithm = @ClientOneAlgorithm And ClientOneConfig = @ClientOneConfig And ClientTwoVersion = @ClientTwoVersion And ClientTwoAlgorithm = @ClientTwoAlgorithm And ClientTwoConfig = @ClientTwoConfig", con);
                                cmd.Parameters.AddWithValue("@GridWidth", MatchesToWrite[i].GridWidth);
                                cmd.Parameters.AddWithValue("@GridHeight", MatchesToWrite[i].GridHeight);
                                cmd.Parameters.AddWithValue("@ClientOneVersion", MatchesToWrite[i].UserOneVersion);
                                cmd.Parameters.AddWithValue("@ClientOneAlgorithm", MatchesToWrite[i].UserOneAlgorithm);
                                cmd.Parameters.AddWithValue("@ClientOneConfig", MatchesToWrite[i].UserOneConfig);
                                cmd.Parameters.AddWithValue("@ClientTwoVersion", MatchesToWrite[i].UserTwoVersion);
                                cmd.Parameters.AddWithValue("@ClientTwoAlgorithm", MatchesToWrite[i].UserTwoAlgorithm);
                                cmd.Parameters.AddWithValue("@ClientTwoConfig", MatchesToWrite[i].UserTwoConfig);

                                int foundKey = -1;
                                bool correctParms = false;
                                SqlDataReader reader = cmd.ExecuteReader();

                                try
                                {
                                    while (reader.Read())
                                    {
                                        foundKey = reader.GetInt32(0);
                                    }
                                }
                                catch (Exception readEx)
                                {
                                    Logger.Write(logInfo, "Failed to read database", readEx);
                                }
                                finally
                                {
                                    reader.Close();
                                    cmd.Dispose();
                                }

                                //Check for flipped clients
                                if (foundKey >= 0)
                                {
                                    correctParms = true;
                                }
                                else if (foundKey == -1)
                                {
                                    cmd = new SqlCommand("Select Coalesce(Max(Matchup),-1) From MatchHistory Where GridWidth = @GridWidth And GridHeight = @GridHeight And ClientOneVersion = @ClientOneVersion And ClientOneAlgorithm = @ClientOneAlgorithm And ClientOneConfig = @ClientOneConfig And ClientTwoVersion = @ClientTwoVersion And ClientTwoAlgorithm = @ClientTwoAlgorithm And ClientTwoConfig = @ClientTwoConfig", con);
                                    cmd.Parameters.AddWithValue("@GridWidth", MatchesToWrite[i].GridWidth);
                                    cmd.Parameters.AddWithValue("@GridHeight", MatchesToWrite[i].GridHeight);
                                    cmd.Parameters.AddWithValue("@ClientOneVersion", MatchesToWrite[i].UserTwoVersion);
                                    cmd.Parameters.AddWithValue("@ClientOneAlgorithm", MatchesToWrite[i].UserTwoAlgorithm);
                                    cmd.Parameters.AddWithValue("@ClientOneConfig", MatchesToWrite[i].UserTwoConfig);
                                    cmd.Parameters.AddWithValue("@ClientTwoVersion", MatchesToWrite[i].UserOneVersion);
                                    cmd.Parameters.AddWithValue("@ClientTwoAlgorithm", MatchesToWrite[i].UserOneAlgorithm);
                                    cmd.Parameters.AddWithValue("@ClientTwoConfig", MatchesToWrite[i].UserOneConfig);

                                    reader = cmd.ExecuteReader();

                                    try
                                    {
                                        while (reader.Read())
                                        {
                                            foundKey = reader.GetInt32(0);
                                        }
                                    }
                                    catch (Exception readEx)
                                    {
                                        Logger.Write(logInfo, "Failed to read database", readEx);
                                    }
                                    finally
                                    {
                                        reader.Close();
                                        cmd.Dispose();
                                    }
                                }

                                //Update the record if it exists
                                if (foundKey >= 0)
                                {
                                    cmd = new SqlCommand("Update MatchHistory Set ClientOneWin = (Select Coalesce(ClientOneWin, 0) + @ClientOneWin From MatchHistory Where Matchup = @Matchup1), ClientTwoWin = (Select Coalesce(ClientTwoWin, 0) + @ClientTwoWin From MatchHistory Where Matchup = @Matchup2) Where Matchup = @Matchup3", con);
                                    if (correctParms)
                                        cmd.Parameters.AddWithValue("@ClientOneWin", MatchesToWrite[i].UserOneWinCount);
                                    else
                                        cmd.Parameters.AddWithValue("@ClientOneWin", MatchesToWrite[i].UserTwoWinCount);
                                    cmd.Parameters.AddWithValue("@Matchup1", foundKey);
                                    if (correctParms)
                                        cmd.Parameters.AddWithValue("@ClientTwoWin", MatchesToWrite[i].UserTwoWinCount);
                                    else
                                        cmd.Parameters.AddWithValue("@ClientTwoWin", MatchesToWrite[i].UserOneWinCount);
                                    cmd.Parameters.AddWithValue("@Matchup2", foundKey);
                                    cmd.Parameters.AddWithValue("@Matchup3", foundKey);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected <= 0)
                                    {
                                        Logger.Write(logInfo, "Update to MatchHistory resulted in " + rowsAffected.ToString() + " rows affected. Matchup: " + foundKey.ToString());
                                        break;
                                    }
                                    cmd.Dispose();
                                }
                                else //Insert the record if it does not exist
                                {
                                    cmd = new SqlCommand("Insert Into MatchHistory (GridWidth, GridHeight, ClientOneVersion, ClientOneAlgorithm, ClientOneConfig, ClientOneWin, ClientTwoVersion, ClientTwoAlgorithm, ClientTwoConfig, ClientTwoWin) Values (@GridWidth, @GridHeight, @ClientOneVersion, @ClientOneAlgorithm, @ClientOneConfig, @ClientOneWin, @ClientTwoVersion, @ClientTwoAlgorithm, @ClientTwoConfig, @ClientTwoWin)", con);
                                    cmd.Parameters.AddWithValue("@GridWidth", MatchesToWrite[i].GridWidth);
                                    cmd.Parameters.AddWithValue("@GridHeight", MatchesToWrite[i].GridHeight);
                                    cmd.Parameters.AddWithValue("@ClientOneVersion", MatchesToWrite[i].UserOneVersion);
                                    cmd.Parameters.AddWithValue("@ClientOneAlgorithm", MatchesToWrite[i].UserOneAlgorithm);
                                    cmd.Parameters.AddWithValue("@ClientOneConfig", MatchesToWrite[i].UserOneConfig);
                                    cmd.Parameters.AddWithValue("@ClientOneWin", MatchesToWrite[i].UserOneWinCount);
                                    cmd.Parameters.AddWithValue("@ClientTwoVersion", MatchesToWrite[i].UserTwoVersion);
                                    cmd.Parameters.AddWithValue("@ClientTwoAlgorithm", MatchesToWrite[i].UserTwoAlgorithm);
                                    cmd.Parameters.AddWithValue("@ClientTwoConfig", MatchesToWrite[i].UserTwoConfig);
                                    cmd.Parameters.AddWithValue("@ClientTwoWin", MatchesToWrite[i].UserTwoWinCount);

                                    int rowsAffected = cmd.ExecuteNonQuery();

                                    if (rowsAffected <= 0)
                                    {
                                        Logger.Write(logInfo, "Insert into MatchHistory resulted in " + rowsAffected.ToString() + " rows affected.");
                                        break;
                                    }
                                    cmd.Dispose();
                                }
                                
                            }

                            MatchesToWrite = new List<MatchHistoryAggregate>();
                        }
                    }
                    else
                    {
                        Thread.Sleep(300000); //Our connection was dropped, Sleep for 5 minutes
                    }
                }
                else
                {
                    Thread.Sleep(10000);
                }
            }
        }
        catch (Exception oex)
        {
            Logger.Write(logInfo, "CheckForNewWrites() General Exception", oex);
        }

        HasStopped = true;
    }
}