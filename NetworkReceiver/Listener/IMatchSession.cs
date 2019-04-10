using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IMatchSession
{
    //Properties
    //  base
    string Type { get; set; }
    int SessionId { get; set; }
    bool SessionExpired { get; set; }
    bool IsAutoJoinMatch { get; set; }
    bool IsMatchFull { get; set; }
    DateTime SessionStartDate { get; set; }
    DateTime? SessionEndDate { get; set; }
    //  stats
    int ServerTotalCommands { get; set; }
    int MatchTotalCommands { get; set; }

    //Methods
    string HandleRequest(string data);
    int GetNumberOfConnected();
}