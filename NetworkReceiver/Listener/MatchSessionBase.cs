using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public abstract class MatchSessionBase : IMatchSession
{
    public string Type { get; set; }
    public int SessionId { get; set; }
    public bool SessionExpired { get; set; }
    public bool IsAutoJoinMatch { get; set; }
    public bool IsMatchFull { get; set; }
    public DateTime SessionStartDate { get; set; }
    public DateTime? SessionEndDate { get; set; }
    //  stats
    public int ServerTotalCommands { get; set; }
    public int MatchTotalCommands { get; set; }

    public MatchSessionBase()
    {
    }

    public abstract string HandleRequest(string data);
    public abstract int GetNumberOfConnected();
}