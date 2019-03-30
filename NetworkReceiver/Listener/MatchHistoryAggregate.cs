using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchHistoryAggregate
{
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public int PlayerOneWinCount { get; set; }
    public int PlayerTwoWinCount { get; set; }
    public string PlayerOneVersion { get; set; }
    public string PlayerOneAlgorithm { get; set; }
    public string PlayerOneConfig { get; set; }
    public string PlayerTwoVersion { get; set; }
    public string PlayerTwoAlgorithm { get; set; }
    public string PlayerTwoConfig { get; set; }

    public MatchHistoryAggregate()
    {
    }
}