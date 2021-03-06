﻿using NetworkReceiver.Listener;
using SynUtil.Database.MSSQL;
using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkReceiver
{
    public partial class DatabaseViewer : Form
    {
        public SqlServerConInfo SqlServerInfo;
        public LogInfo logInfo;
        public List<MatchHistoryAggregate> Records;

        public DatabaseViewer()
        {
            InitializeComponent();
        }

        private void DatabaseViewer_Load(object sender, EventArgs e)
        {
            foreach (AiStartingPosition r in Enum.GetValues(typeof(AiStartingPosition)))
            {
                cbxStartingPos.Items.Add(r.ToString());
            }
        }
        private void cbxStartingPos_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool error = false;
            string erroMessage = String.Empty;
            Records = new List<MatchHistoryAggregate>();
            SqlServer sqlSvr = new SqlServer();
            SqlConnection con = sqlSvr.GetConnection(SqlServerInfo);
            con.Open();

            string sql = "Select GridWidth, GridHeight, ClientOneVersion, ClientOneAlgorithm, ClientOneConfig, Cast(ClientOneWin as Int), ClientTwoVersion, ClientTwoAlgorithm, ClientTwoConfig, Cast(ClientTwoWin as Int) From MatchHistory Where (ClientOneVersion <> ClientTwoVersion OR ClientOneAlgorithm <> ClientTwoAlgorithm OR ClientOneConfig <> ClientTwoConfig) AND (ClientOneWin + ClientTwoWin) > 500 AND ClientOneConfig like '%' + @StartingPos1 + '%'";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@StartingPos1", "StartPos:" + cbxStartingPos.Text.Trim() + ";");

            SqlDataReader reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    MatchHistoryAggregate match = new MatchHistoryAggregate();
                    match.GridWidth = reader.GetInt32(0);
                    match.GridHeight = reader.GetInt32(1);
                    match.UserOneVersion = reader.GetString(2);
                    match.UserOneAlgorithm = reader.GetString(3);
                    match.UserOneConfig = reader.GetString(4);
                    match.UserOneWinCount = (int)reader.GetInt32(5);
                    match.UserTwoVersion = reader.GetString(6);
                    match.UserTwoAlgorithm = reader.GetString(7);
                    match.UserTwoConfig = reader.GetString(8);
                    match.UserTwoWinCount = (int)reader.GetInt32(9);

                    Records.Add(match);
                }
            }
            catch (Exception readEx)
            {
                error = true;
                erroMessage = readEx.Message;
                Logger.Write(logInfo, "Failed to read database pass 1", readEx);
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
            }

            sql = "Select GridWidth, GridHeight, ClientTwoVersion, ClientTwoAlgorithm, ClientTwoConfig, Cast(ClientTwoWin as Int), ClientOneVersion, ClientOneAlgorithm, ClientOneConfig, Cast(ClientOneWin as Int) From MatchHistory Where (ClientOneVersion <> ClientTwoVersion OR ClientOneAlgorithm <> ClientTwoAlgorithm OR ClientOneConfig <> ClientTwoConfig) AND (ClientOneWin + ClientTwoWin) > 500 AND ClientTwoConfig like '%' + @StartingPos1 + '%'";
            cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@StartingPos1", "StartPos:" + cbxStartingPos.Text.Trim() + ";");

            reader = cmd.ExecuteReader();

            try
            {
                while (reader.Read())
                {
                    MatchHistoryAggregate match = new MatchHistoryAggregate();
                    match.GridWidth = reader.GetInt32(0);
                    match.GridHeight = reader.GetInt32(1);
                    match.UserOneVersion = reader.GetString(2);
                    match.UserOneAlgorithm = reader.GetString(3);
                    match.UserOneConfig = reader.GetString(4);
                    match.UserOneWinCount = reader.GetInt32(5);
                    match.UserTwoVersion = reader.GetString(6);
                    match.UserTwoAlgorithm = reader.GetString(7);
                    match.UserTwoConfig = reader.GetString(8);
                    match.UserTwoWinCount = reader.GetInt32(9);

                    Records.Add(match);
                }
            }
            catch (Exception readEx)
            {
                error = true;
                erroMessage = readEx.Message;
                Logger.Write(logInfo, "Failed to read database pass 2", readEx);
            }
            finally
            {
                reader.Close();
                cmd.Dispose();
            }

            //Fill the grid
            if (!error)
            {
                this.dgvMatches.Rows.Clear();
                for (int i = 0; i < Records.Count(); i++)
                {
                    string winPercent = (100 * Math.Round(((double)Records[i].UserOneWinCount / (Records[i].UserOneWinCount + Records[i].UserTwoWinCount)), 2)).ToString() + "%";

                    if (winPercent.Length == 2)
                    {
                        winPercent = "0" + winPercent;
                    }

                    dgvMatches.Rows.Add(new object[] { ("(" + Records[i].GridWidth.ToString() + "," + Records[i].GridHeight.ToString() + ")"), Records[i].UserOneVersion, Records[i].UserOneAlgorithm, Records[i].UserOneConfig, Records[i].UserOneWinCount, Records[i].UserTwoVersion, Records[i].UserTwoAlgorithm, Records[i].UserTwoConfig, Records[i].UserTwoWinCount, winPercent });
                }
            }
            else
            {
                MessageBox.Show("Failed to read data: " + erroMessage, "Failed to read DB");
            }
        }
    }
}
