using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchHistoryAggregate
{
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public int UserOneWinCount { get; set; }
    public int UserTwoWinCount { get; set; }
    public string UserOneVersion { get; set; }
    public string UserOneAlgorithm { get; set; }
    public string UserOneConfig { get; set; }
    public string UserTwoVersion { get; set; }
    public string UserTwoAlgorithm { get; set; }
    public string UserTwoConfig { get; set; }

    public MatchHistoryAggregate()
    {
    }
}