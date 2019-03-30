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
    public string PlayerOne { get; set; }
    public string PlayerOneVersion { get; set; }
    public string PlayerOneAlgorithm { get; set; }
    public string PlayerOneConfig { get; set; }
    public string PlayerTwo { get; set; }
    public string PlayerTwoVersion { get; set; }
    public string PlayerTwoAlgorithm { get; set; }
    public string PlayerTwoConfig { get; set; }
    public string StartingPlayer { get; set; }
    public string Winner { get; set; }
    public DateTime MatchStartDate { get; set; }
    public TimeSpan MatchLength { get; set; }
    public Point OpeningMove { get; set; }
    public Point SecondOpeningMove { get; set; }
    public int MatchTotalCommands { get; set; }
    public int MatchTotalMoves { get; set; }
    public bool HasBeenWrittenToDB { get; set; }
    public bool PlayerOneIsWinner
    {
        get
        {
            if (PlayerOne == Winner)
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