using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NetworkReceiver
{
    public partial class MatchSessionGameSelector : Form
    {
        public List<MatchHistory> Matches;

        public MatchSessionGameSelector()
        {
            InitializeComponent();
        }

        private void MatchSessionGameSelector_Load(object sender, EventArgs e)
        {
            if (Matches != null)
            {
                for (int i = 0; i < Matches.Count(); i++)
                {
                    this.dgvMatches.Rows.Add(new object[] { Matches[i].MatchId.ToString(), Matches[i].MatchStartDate.ToString("yyyyMMdd hh:mm:ss"), Matches[i].StartingUser, Matches[i].Winner, Matches[i].MatchTotalMoves.ToString(), PointToString(Matches[i].OpeningMove), PointToString(Matches[i].SecondOpeningMove), TimeSpanFormatter.TimeSpanFormattedFull(Matches[i].MatchLength), Matches[i].UserOne, Matches[i].UserTwo, Matches[i].MatchTotalCommands.ToString() });
                }
            }
            else
            {
                MessageBox.Show("Failed to load Match History from parent","Failed to load");
            }
        }
        private void btnViewMatch_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dgvMatches.SelectedRows)
            {
                string id = row.Cells[0].Value.ToString();
                int idOut;

                if (Int32.TryParse(id, out idOut))
                {
                    for (int i = 0; i < Matches.Count(); i++)
                    {
                        if (Matches[i].MatchId == idOut)
                        {
                            MatchReplayer matchReplayer = new MatchReplayer();
                            matchReplayer.Match = Matches[i];
                            matchReplayer.ShowDialog();

                            break;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("ID For Cell is not a number", "Failed to open match");
                }

                break;
            }
        }

        //Private functions
        private string PointToString(Point p)
        {
            return "(" + p.X + "," + p.Y + ")";
        }
    }
}
