using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchHistory
{
    public int MatchId { get; set; }
    public GameGridSquare[,] MatchGrid { get; set; }
    public string UserOne { get; set; }
    public string UserOneVersion { get; set; }
    public string UserOneAlgorithm { get; set; }
    public string UserOneConfig { get; set; }
    public string UserTwo { get; set; }
    public string UserTwoVersion { get; set; }
    public string UserTwoAlgorithm { get; set; }
    public string UserTwoConfig { get; set; }
    public string StartingUser { get; set; }
    public string Winner { get; set; }
    public DateTime MatchStartDate { get; set; }
    public TimeSpan MatchLength { get; set; }
    public Point OpeningMove { get; set; }
    public Point SecondOpeningMove { get; set; }
    public int MatchTotalCommands { get; set; }
    public int MatchTotalMoves { get; set; }
    public bool HasBeenWrittenToDB { get; set; }
    public bool UserOneIsWinner
    {
        get
        {
            if (UserOne == Winner)
                return true;
            else
                return false;
        }
    }

    public MatchHistory()
    {
        HasBeenWrittenToDB = false;
    }
}