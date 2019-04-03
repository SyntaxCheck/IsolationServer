using SynUtil.FileSystem;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;

public class MatchSession
{
    public const bool DEBUG = false;
    public const int CLIENT_TIMEOUT_SECONDS = 300; //How long before the server assumes you have disconnected
    public const int MATCH_HISTORY_COUNT_TO_KEEP = 1000;
    private LogInfo logInfo;
    public bool SessionExpired { get; set; }
    public int GridWidth { get; set; }
    public int GridHeight { get; set; }
    public int SessionId { get; set; }
    public int MatchId { get; set; }
    public int ServerTotalCommands { get; set; }
    public int MatchTotalCommands { get; set; }
    public int ServerTotalMoves { get; set; }
    public int MatchTotalMoves { get; set; }
    public DateTime SessionStartDate { get; set; }
    public DateTime? SessionEndDate { get; set; }
    public DateTime MatchStartDate { get; set; }
    public GameGridSquare[,] Grid { get; set; }
    public int GridMoveNumber { get; set; }
    public Point? OpeningMove { get; set; }
    public Point? SecondOpeningMove { get; set; }
    public string FirstUser { get; set; } 
    public string FirstUserToken { get; set; }
    public string FirstUserVersion { get; set; }
    public string FirstUserAlgorithm { get; set; }
    public string FirstUserConfig { get; set; }
    public bool FirstUserIsXYFormat { get; set; }
    public bool FirstUserReady { get; set; }
    public bool FirstUsersTurn { get; set; }
    public bool FirstUserSetOpeningMove { get; set; }
    public bool FirstUserGotEndOfGameResponse { get; set; }
    public int FirstUserWinCount { get; set; }
    public Point? FirstUserLastMove { get; set; }
    public DateTime? FirstUserLastPing { get; set; }
    public string SecondUser { get; set; }
    public string SecondUserToken { get; set; }
    public string SecondUserVersion { get; set; }
    public string SecondUserAlgorithm { get; set; }
    public string SecondUserConfig { get; set; }
    public bool SecondUserIsXYFormat { get; set; }
    public bool SecondUserReady { get; set; }
    public bool SecondUsersTurn { get; set; }
    public bool SecondUserSetOpeningMove { get; set; }
    public bool SecondUserGotEndOfGameResponse { get; set; }
    public int SecondUserWinCount { get; set; }
    public Point? SecondUserLastMove { get; set; }
    public DateTime? SecondUserLastPing { get; set; }
    public string Channel { get; set; }
    public string ChannelPassword { get; set; }
    public string StartingUser { get; set; }
    public string WinningUser { get; set; }
    public string DoppedConnectionStatusMsg { get; set; }
    public bool GameStarted { get; set; }
    public bool GameHasEndedWithWinner { get; set; }
    private Random Rng { get; set; }
    public List<MatchHistory> CompletedMatches { get; set; }
    private List<Point> PossibleMoveDirections { get; set; }

    public MatchSession(int sessionIdIn)
    {
        logInfo = new LogInfo();
        logInfo.RootFolder = Directory.GetCurrentDirectory();
        logInfo.LogFolder = "LogsFolder";
        logInfo.LogFileName = "Session" + sessionIdIn.ToString() + ".txt";
        logInfo.AppendDateTime = true;
        logInfo.AppendDateTimeFormat = "yyyyMMdd";
        logInfo.IsDebug = DEBUG;
        Logger.ValidateLogPath(ref logInfo);

        MatchId = 0;
        ServerTotalCommands = 0;
        MatchTotalCommands = 0;
        ServerTotalMoves = 0;
        MatchTotalMoves = 0;
        CompletedMatches = new List<MatchHistory>();
        SessionId = sessionIdIn;
        SessionStartDate = DateTime.Now;
        Rng = new Random();
        SessionExpired = false;
        FirstUser = String.Empty;
        FirstUserToken = String.Empty;
        FirstUserVersion = String.Empty;
        FirstUserAlgorithm = String.Empty;
        FirstUserConfig = String.Empty;
        FirstUserIsXYFormat = true;
        FirstUserReady = false;
        FirstUsersTurn = false;
        FirstUserWinCount = 0;
        FirstUserLastPing = null;
        SecondUser = String.Empty;
        SecondUserToken = String.Empty;
        SecondUserVersion = String.Empty;
        SecondUserAlgorithm = String.Empty;
        SecondUserConfig = String.Empty;
        SecondUserIsXYFormat = true;
        SecondUserReady = false;
        SecondUsersTurn = false;
        SecondUserWinCount = 0;
        SecondUserLastPing = null;
        GameStarted = false;
        Channel = String.Empty;
        ChannelPassword = String.Empty;
        StartingUser = String.Empty;
        SessionEndDate = null;

        PossibleMoveDirections = new List<Point>();
        PossibleMoveDirections.Add(new Point(2, 1));
        PossibleMoveDirections.Add(new Point(2, -1));
        PossibleMoveDirections.Add(new Point(-2, 1));
        PossibleMoveDirections.Add(new Point(-2, -1));
        PossibleMoveDirections.Add(new Point(1, 2));
        PossibleMoveDirections.Add(new Point(1, -2));
        PossibleMoveDirections.Add(new Point(-1, 2));
        PossibleMoveDirections.Add(new Point(-1, -2));

        //First game needs to have these set to true
        FirstUserGotEndOfGameResponse = true;
        SecondUserGotEndOfGameResponse = true;
    }

    public string HandleRequest(string data)
    {
        string rtn = String.Empty;
        bool unhandledCommand = false;

        Logger.WriteDbg(logInfo, "Request: " + data);

        if (!String.IsNullOrEmpty(data))
        {
            string[] split = data.Split('|');

            for (int i = 0; i < split.Length; i++)
            {
                split[i] = split[i].Trim();
            }

            switch (split[0])
            {
                case "Connection":
                    //Connection request: Username, Channel, Password, IsXYFormat       //Connect to opponent. IsXYFormat set to true if the first number is X, set to False if first number means Row
                    //  Response: Accept/Reject, Token, Current Users connected
                    //Example Req  -     Connection|TestUser|Test|abCd123!|T
                    //Example Resp -     Accept|55673925786456|2|YourChannelName|YourPassword

                    if (split.Length == 3)
                    {
                        rtn = HandleRequestConnection(split[1], split[2]);
                    }
                    else if (split.Length == 5)
                    {
                        rtn = HandleRequestConnection(split[1], split[2], split[3], split[4]);
                    }
                    else if (split.Length == 7)
                    {
                        int gridWidth, gridHeight;

                        if (Int32.TryParse(split[5], out gridWidth) && Int32.TryParse(split[6], out gridHeight))
                        {
                            rtn = HandleRequestConnection(split[1], split[2], split[3], split[4], gridWidth, gridHeight);
                        }
                        else
                        {
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. Connection request takes 4 parameters";
                    }

                    break;
                case "Start":
                    //Start request: Token, Channel, Password, (Optional) Version, (Optional) Algorithm, (Optional) Config      //Request start of game
                    //  Response: Starting user, Opponent
                    //Example Req  -     Start|55673925786456|Test|abCd123!
                    //Example Resp -     TestOpp|TestOpp

                    if (split.Length >= 4 && split.Length <= 7)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            string version = String.Empty;
                            string algorithm = String.Empty;
                            string config = String.Empty;

                            if (split.Length >= 5)
                                version = split[4];
                            if (split.Length >= 6)
                                algorithm = split[5];
                            if (split.Length >= 7)
                                config = split[6];

                            rtn = HandleRequestStart(split[1], version, algorithm, config);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. Start request takes 4 parameters";
                    }

                    break;
                case "GetStatus":
                    //Status request: Token, Channel, Password     //Client requests what the current action for the server is
                    //  Response: 
                    //      If waiting on users: WaitForConnection
                    //      If users turn: MakeMove
                    //      If not users turn: WaitForOpp
                    //      If User has Won/Lost: Win/Lose
                    //      If the Opponent has not responded in 30 seconds: OpponentQuit
                    //      If two users have connected and the game is pending start: WaitingForStart
                    //Example Req  -     Status|55673925786456|Test|abCd123!
                    //Example Resp -     WaitForConnection
                    //Example Resp -     MakeMove
                    //Example Resp -     OpponentsMove
                    //Example Resp -     Win
                    //Example Resp -     OpponentQuit
                    //Example Resp -     InvalidRequest|Incorrect user Token
                    //Example Resp -     InvalidRequest|Incorrect Channel/Password

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestGetStatus(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. GetStatus request takes 4 parameters";
                    }

                    break;
                case "GetBoard":
                    //Request Board: Token, Channel, Password     //Once it is the users turn request the board
                    //  Response: board
                    //Example Req  -     GetBoard|55673925786456|Test|abCd123!
                    //Example Resp -     -,-,-,X,-,-,X(/r/n)-,-,-,-,-,X,-(/r/n)X,X,-,-,-,-,-(/r/n)X,-,X,X,-,X,-(/r/n)-,-,X,-,-,-,-(/r/n)-,-,-,-,-,-,-(/r/n)-,-,X,-,-,-,TestOpp(/r/n)

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestGetBoard(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. GetBoard request takes 4 parameters";
                    }

                    break;
                case "GetOpponentLastMove":
                    //Request Board: Token, Channel, Password     //Once it is the users turn request the board
                    //  Response: board
                    //Example Req  -     GetOpponentLastMove|55673925786456|Test|abCd123!
                    //Example Resp -     (0,3)     //Row,Column format

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            //This function could return blanks if it fails to find the opponents last move
                            rtn = HandleRequestGetOpponentLastMove(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. GetBoard request takes 4 parameters";
                    }

                    break;
                case "MakeMove":
                    //MakeMove request: Token, Channel, Password, Coordinate Point     //Make move on the board
                    //  Response: Accept/Reject, board
                    //Example Req  -     MakeMove|55673925786456|Test|abCd123!|(3,4)
                    //Example Resp -     Accept|-,-,-,X,-,-,X(/r/n)-,-,-,-,-,X,-(/r/n)X,X,-,-,-,-,-(/r/n)X,-,X,X,-,X,-(/r/n)-,-,TestUser,-,-,-,-(/r/n)-,-,-,-,-,-,-(/r/n)-,-,X,-,-,-,TestOpp(/r/n)

                    if (split.Length == 5)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestMakeMove(split[1], split[4]);
                            if (String.IsNullOrEmpty(rtn))
                            {
                                rtn = "Accept";
                            }
                            else
                            {
                                rtn = "Reject|" + rtn;
                            }
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. MakeMove request takes 5 parameters";
                    }

                    break;
                case "GetSessionsStats":
                    //Request GetSessionsStats: Token, Channel, Password     //Once it is the users turn request the board
                    //  Response: board
                    //Example Req  -     GetSessionsStats|55673925786456|Test|abCd123!
                    //Example Resp -     TestUserBill:6|TestUserSteve:4

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestGetSessionsStats(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. GetBoard request takes 4 parameters";
                    }

                    break;
                case "ForfeitMatch":
                    //Request ForfeitMatch: Token, Channel, Password     
                    //  Response: board
                    //Example Req  -     ForfeitMatch|55673925786456|Test|abCd123!
                    //Example Resp -     Accept

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestForfeitMatch(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. Quit request takes 4 parameters";
                    }

                    break;
                case "Quit":
                    //Request Quit: Token, Channel, Password     
                    //  Response: board
                    //Example Req  -     Quit|55673925786456|Test|abCd123!
                    //Example Resp -     Accept

                    if (split.Length == 4)
                    {
                        string validateMessage = String.Empty;
                        if (ValidateSession(split[1], split[2], split[3], ref validateMessage))
                        {
                            rtn = HandleRequestQuit(split[1]);
                        }
                        else
                        {
                            rtn = "InvalidRequest|" + validateMessage;
                        }
                    }
                    else
                    {
                        rtn = "InvalidRequest|Invalid parameter count for request type. Quit request takes 4 parameters";
                    }

                    break;
                default:
                    rtn = "InvalidRequest|Invalid request type";
                    unhandledCommand = true;

                    break;
            }
        }

        Logger.WriteDbg(logInfo, "Response: " + rtn);

        if (!unhandledCommand)
        {
            ServerTotalCommands++;
            if(!GameHasEndedWithWinner)
                MatchTotalCommands++;
        }

        return rtn;
    }
    private string HandleRequestConnection(string userName, string isYXFormat)
    {
        //Default game board to 7x7 and default channel/pwd
        if (String.IsNullOrEmpty(Channel))
        {
            //If this is the first client to connect this way then generate a random id
            Channel = Rng.Next(1000000000, 999999999).ToString();
            ChannelPassword = "DefaulPwd";
        }

        return HandleRequestConnection(userName, Channel, ChannelPassword, isYXFormat, 7, 7);
    }
    private string HandleRequestConnection(string userName, string channelIn, string channelPwdIn, string isYXFormat)
    {
        //Default game board is 7x7
        return HandleRequestConnection(userName, channelIn, channelPwdIn, isYXFormat, 7, 7);
    }
    private string HandleRequestConnection(string userName, string channelIn, string channelPwdIn, string isYXFormat, int gridWidthIn, int gridHeightIn)
    {
        string rtn = String.Empty;

        if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(channelIn) && !String.IsNullOrEmpty(channelPwdIn) && gridWidthIn > 0 && gridHeightIn > 0)
        {
            GridWidth = gridWidthIn;
            GridHeight = gridHeightIn;

            if (ValidateText(userName) && ValidateText(channelIn) && ValidateText(channelPwdIn))
            {
                if (String.IsNullOrEmpty(FirstUser) || String.IsNullOrEmpty(SecondUser))
                {
                    if (FirstUser.Trim().ToUpper() != userName.Trim().ToUpper() && SecondUser.Trim().ToUpper() != userName.Trim().ToUpper())
                    {
                        if (String.IsNullOrEmpty(Channel))
                        {
                            Channel = channelIn;
                            ChannelPassword = channelPwdIn;
                        }
                        else if (Channel.ToUpper() != channelIn.ToUpper())
                        {
                            rtn = "Reject|Channel Name does not match connected user";
                        }
                        else if (ChannelPassword != channelPwdIn)
                        {
                            rtn = "Reject|Channel Name matches but password does not for connected user";
                        }

                        if (String.IsNullOrEmpty(rtn))
                        {
                            if (String.IsNullOrEmpty(FirstUser))
                            {
                                FirstUser = userName;
                                FirstUserToken = GetRandomToken();
                                rtn = "Accept|" + FirstUserToken + "|" + GetNumberOfConnected().ToString() + "|" + Channel + "|" + ChannelPassword;
                                SetXYFormat(FirstUserToken, isYXFormat);
                                SetUserPing(FirstUserToken);
                            }
                            else if (String.IsNullOrEmpty(SecondUser))
                            {
                                SecondUser = userName;
                                SecondUserToken = GetRandomToken();
                                rtn = "Accept|" + SecondUserToken + "|" + GetNumberOfConnected().ToString();
                                SetXYFormat(SecondUserToken, isYXFormat);
                                SetUserPing(SecondUserToken);
                            }
                        }
                    }
                    else
                    {
                        rtn = "Reject|Opponent has the same Username";
                    }
                }
                else
                {
                    rtn = "Reject|Two users are already connected";
                }
            }
            else
            {
                rtn = "Reject|Username, channel name or channel password contains an banned character";
            }
        }
        else
        {
            rtn = "Reject|Blank User, Channel, Channel Password or invalid Grid size";
        }

        return rtn;
    }
    private string HandleRequestStart(string token, string version, string algorithm, string config)
    {
        string rtn = String.Empty;

        if (GetNumberOfConnected() == 2)
        {
            string setMessage = String.Empty;
            if (SetUserReadyStatus(token, ref setMessage))
            {
                //Set the passed in optional parms
                SetUserStartParms(token, version, algorithm, config);

                //If both users are set to start game then generate who goes first and start the game
                if (FirstUserReady && SecondUserReady && FirstUserGotEndOfGameResponse && SecondUserGotEndOfGameResponse)
                {
                    FirstUserGotEndOfGameResponse = false;
                    SecondUserGotEndOfGameResponse = false;

                    if (GameHasEndedWithWinner)
                    {
                        //Swap starting users, the function will also set plays turn
                        SwapStartingUsers();
                    }
                    else
                    {
                        //First match set random starter
                        if (Rng.Next(0, 100000) < 50000)
                        {
                            StartingUser = FirstUser;
                        }
                        else
                        {
                            StartingUser = SecondUser;
                        }
                    }

                    //Sets the users turns based on "startingUser"
                    SetStartingUser();
                    Reset();
                    GameStarted = true;
                    MatchStartDate = DateTime.Now;
                    MatchId++;
                    MatchTotalCommands = 0;
                    MatchTotalMoves = 0;
                }

                rtn = "Accept|" + GetOpponentName(token);
            }
            else
            {
                rtn = "InvalidRequest|" + setMessage;
            }
        }
        else
        {
            rtn = "WaitForConnection";
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestGetStatus(string token)
    {
        string rtn = String.Empty;

        if (GetNumberOfConnected() < 2)
        {
            rtn = "WaitForConnection";
        }
        else if (GameHasEndedWithWinner && !HasUserGottenEndOfGameMessage(token))
        {
            if (IsUserWinner(token))
            {
                rtn = "Winner";
            }
            else
            {
                rtn = "Loser";
            }

            //User has checked the win status of the game, set their ready status to false so they can prepare for the next game
            SetUserReadyStatusToFalse(token);
        }
        else if (!FirstUserReady || !SecondUserReady || GameHasEndedWithWinner)
        {
            rtn = "WaitingForOpponentToGetReady";
        }
        else if (IsUsersTurn(token))
        {
            rtn = "YourMove";
        }
        else if (IsOpponentTurn(token))
        {
            rtn = "OpponentsMove";
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestMakeMove(string token, string movePoint)
    {
        string rtn = String.Empty;

        if (IsUsersTurn(token))
        {
            Point point = new Point();
            string validateMessage = StringPointToPoint(token, movePoint, ref point);

            if (!String.IsNullOrEmpty(validateMessage))
            {
                rtn = validateMessage;
            }
            else
            {
                if (IsMoveValid(token, point, ref validateMessage))
                {
                    validateMessage = MakeMove(token, point);
                    if (!String.IsNullOrEmpty(validateMessage))
                    {
                        rtn = validateMessage;
                    }
                }
                else
                {
                    rtn = validateMessage;
                }
            }
        }
        else
        {
            rtn = "NotYourTurn|Call GetStatus and wait for it to be your turn";
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestGetOpponentLastMove(string token)
    {
        string rtn = String.Empty;

        Point? point = GetOpponentLastMove(token);
        if (point.HasValue)
        {
            if (IsUserXYFormat(token))
            {
                rtn = "(" + point.Value.X.ToString() + "," + point.Value.Y.ToString() + ")";
            }
            else
            {
                rtn = "(" + point.Value.Y.ToString() + "," + point.Value.X.ToString() + ")";
            }
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestGetSessionsStats(string token)
    {
        string rtn = String.Empty;

        if (FirstUserWinCount > 0 || SecondUserWinCount > 0)
        {
            rtn = FirstUser + ": " + FirstUserWinCount.ToString() + "|" + SecondUser + ": " + SecondUserWinCount.ToString();
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestGetBoard(string token)
    {
        string rtn = String.Empty;

        for (int y = 0; y < Grid.GetLength(1); y++)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                if (x > 0)
                {
                    rtn += ",";
                }

                rtn += GameGridSquareToString(Grid[x, y]);
            }
            rtn += Environment.NewLine;
        }

        SetUserPing(token);

        return rtn;
    }
    private string HandleRequestQuit(string token)
    {
        SessionExpired = true;

        return "Accept";
    }
    private string HandleRequestForfeitMatch(string token)
    {
        SetGameWinner(GetTokenForUser(GetOpponentName(token)));

        return "Accept";
    }

    private void Reset()
    {
        OpeningMove = null;
        SecondOpeningMove = null;
        FirstUserLastMove = null;
        FirstUserSetOpeningMove = false;
        SecondUserLastMove = null;
        SecondUserSetOpeningMove = false;
        GameHasEndedWithWinner = false;
        WinningUser = String.Empty;

        GridMoveNumber = 0;
        Grid = new GameGridSquare[GridWidth, GridHeight];
        for (int y = 0; y < Grid.GetLength(1); y++)
        {
            for (int x = 0; x < Grid.GetLength(0); x++)
            {
                GameGridSquare ggs = new GameGridSquare();

                ggs.State = GridState.Blank;
                ggs.X = x;
                ggs.Y = y;
                ggs.GridUser = String.Empty;
                ggs.GridMoveNumber = GridMoveNumber;

                Grid[x, y] = ggs;
            }
        }

        //Remove old matches, batch them up so that each end game does not need to run this cleanup
        if (CompletedMatches.Count() > (MATCH_HISTORY_COUNT_TO_KEEP + 100))
        {
            int ctr = 0;
            while (CompletedMatches.Count() > MATCH_HISTORY_COUNT_TO_KEEP && ctr < 1000)
            {
                ctr++;

                if (CompletedMatches[0].HasBeenWrittenToDB)
                {
                    CompletedMatches.RemoveAt(0);
                }
                else
                {
                    break;
                }
            }
        }
    }
    private string MakeMove(string token, Point movePoint)
    {
        string rtn = String.Empty;
        string userForToken = GetUserForToken(token);

        if (!String.IsNullOrEmpty(userForToken))
        {
            //Set the last User move to blocked if the user has made their first move
            if (HasUserMadeFirstMove(token))
            {
                Point? point = GetUserLastMove(token);

                if (point.HasValue)
                {
                    Grid[point.Value.X, point.Value.Y].State = GridState.Blocked;
                }
                else
                {
                    return "Could not find last point when user HAS made first move";
                }
            }

            //Used for debug, if the user requests the final game board at the end we can display which move was made in which order and by which user
            GridMoveNumber++;
            Grid[movePoint.X, movePoint.Y].GridMoveNumber = GridMoveNumber;
            Grid[movePoint.X, movePoint.Y].GridUser = userForToken;
            Grid[movePoint.X, movePoint.Y].State = GridState.Occupied;

            ServerTotalMoves++;
            MatchTotalMoves++;

            SetUserLastMove(token, movePoint);

            if (!IsGameOver(token))
            {
                SetNextUserEndTurn(token);
            }
            else
            {
                SetGameWinner(token);
            }
        }
        else
        {
            rtn = "Could not find user for the given token";
        }

        return rtn;
    }
    private void SetXYFormat(string token, string flag)
    {
        if (FirstUserToken == token)
        {
            FirstUserIsXYFormat = GetFlagBool(flag);
        }
        else if (SecondUserToken == token)
        {
            SecondUserIsXYFormat = GetFlagBool(flag);
        }
    }
    private bool SetUserReadyStatus(string token, ref string message)
    {
        if (FirstUserToken == token)
        {
            if (!FirstUserReady)
            {
                FirstUserReady = true;
                return true;
            }
            else
            {
                message = "User already set ready status to 'true'";
                return false;
            }
        }
        else if (SecondUserToken == token)
        {
            if (!SecondUserReady)
            {
                SecondUserReady = true;
                return true;
            }
            else
            {
                message = "User already set ready status to 'true'";
                return false;
            }
        }

        message = "Token invalid";
        return false;
    }
    private void SetUserReadyStatusToFalse(string token)
    {
        if (FirstUserToken == token)
        {
            FirstUserReady = false;
            FirstUserGotEndOfGameResponse = true;
        }
        else if (SecondUserToken == token)
        {
            SecondUserReady = false;
            SecondUserGotEndOfGameResponse = true;
        }
    }
    private void SetUserStartParms(string token, string version, string algorithm, string config)
    {
        if (FirstUserToken == token)
        {
            FirstUserVersion = version;
            FirstUserAlgorithm = algorithm;
            FirstUserConfig = config;
        }
        else if (SecondUserToken == token)
        {
            SecondUserVersion = version;
            SecondUserAlgorithm = algorithm;
            SecondUserConfig = config;
        }
    }
    private void SetUserLastMove(string token, Point? moveMade)
    {
        if (FirstUserToken == token)
        {
            FirstUserLastMove = moveMade;
            if (!FirstUserSetOpeningMove)
            {
                FirstUserSetOpeningMove = true;
            }
        }
        else if (SecondUserToken == token)
        {
            SecondUserLastMove = moveMade;
            if (!SecondUserSetOpeningMove)
            {
                SecondUserSetOpeningMove = true;
            }
        }

        if (OpeningMove == null)
        {
            OpeningMove = moveMade;
        }
        else if (SecondOpeningMove == null)
        {
            SecondOpeningMove = moveMade;
        }
    }
    private void SetUserPing(string token)
    {
        if (token == FirstUserToken)
        {
            FirstUserLastPing = DateTime.Now;
        }
        else if (token == SecondUserToken)
        {
            SecondUserLastPing = DateTime.Now;
        }
    }
    private void SetStartingUser()
    {
        if (StartingUser == FirstUser)
        {
            FirstUsersTurn = true;
            SecondUsersTurn = false;
        }
        else if (StartingUser == SecondUser)
        {
            FirstUsersTurn = false;
            SecondUsersTurn = true;
        }
        else
        {
            FirstUsersTurn = false;
            SecondUsersTurn = false;
        }
    }
    private void SetGameWinner(string token)
    {
        if (FirstUserToken == token)
        {
            FirstUserWinCount++;
            WinningUser = FirstUser;
        }
        else if (SecondUserToken == token)
        {
            SecondUserWinCount++;
            WinningUser = SecondUser;
        }

        MatchHistory mh = new MatchHistory();
        mh.MatchGrid = Grid;
        mh.MatchId = MatchId;
        mh.UserOne = FirstUser;
        mh.UserOneVersion = FirstUserVersion;
        mh.UserOneAlgorithm = FirstUserAlgorithm;
        mh.UserOneConfig = FirstUserConfig;
        mh.UserTwo = SecondUser;
        mh.UserTwoVersion = SecondUserVersion;
        mh.UserTwoAlgorithm = SecondUserAlgorithm;
        mh.UserTwoConfig = SecondUserConfig;
        mh.StartingUser = StartingUser;
        mh.Winner = WinningUser;
        mh.OpeningMove = (Point)OpeningMove;
        mh.SecondOpeningMove = (Point)SecondOpeningMove;
        mh.MatchStartDate = MatchStartDate;
        mh.MatchLength = (DateTime.Now - mh.MatchStartDate);
        mh.MatchTotalCommands = MatchTotalCommands;
        mh.MatchTotalMoves = MatchTotalMoves;

        CompletedMatches.Add(mh);

        FirstUsersTurn = false;
        SecondUsersTurn = false;
        GameHasEndedWithWinner = true;
        MatchStartDate = DateTime.MinValue;
    }
    private void SetNextUserEndTurn(string token)
    {
        if (FirstUserToken == token)
        {
            FirstUsersTurn = false;
            SecondUsersTurn = true;
        }
        else if (SecondUserToken == token)
        {
            FirstUsersTurn = true;
            SecondUsersTurn = false;
        }
    }
    private string GetUserForToken(string token)
    {
        if (FirstUserToken == token)
        {
            return FirstUser;
        }
        else if (SecondUserToken == token)
        {
            return SecondUser;
        }

        return String.Empty;
    }
    private string GetTokenForUser(string user)
    {
        if (FirstUser == user)
        {
            return FirstUserToken;
        }
        else if (SecondUser == user)
        {
            return SecondUserToken;
        }

        return String.Empty;
    }
    private string GetOpponentName(string token)
    {
        if (GetNumberOfConnected() == 2)
        {
            if (FirstUserToken == token)
            {
                return SecondUser;
            }
            else if (SecondUserToken == token)
            {
                return FirstUser;
            }
        }

        return String.Empty;
    }
    private Point? GetUserLastMove(string token)
    {
        if (token == FirstUserToken)
        {
            return FirstUserLastMove;
        }
        else if (token == SecondUserToken)
        {
            return SecondUserLastMove;
        }

        return null;
    }
    private Point? GetOpponentLastMove(string currentUserToken)
    {
        string opponentToken = String.Empty;

        if (FirstUserToken == currentUserToken)
        {
            opponentToken = SecondUserToken;
        }
        else if (SecondUserToken == currentUserToken)
        {
            opponentToken = FirstUserToken;
        }

        return GetUserLastMove(opponentToken);
    }
    private string GetRandomToken()
    {
        string token = String.Empty;
        int tokenLength = 10;

        for (int i = 0; i < tokenLength; i++)
        {
            token += Rng.Next(0, 9).ToString();
        }

        return token;
    }
    public int GetNumberOfConnected()
    {
        int count = 0;

        if (!SessionExpired)
        {
            if (!String.IsNullOrEmpty(FirstUser))
            {
                count++;
            }
            if (!String.IsNullOrEmpty(SecondUser))
            {
                count++;
            }
        }

        return count;
    }
    private bool GetFlagBool(string flag)
    {
        flag = flag.Trim().ToUpper();

        if (flag == "F" || flag == "FALSE" || flag == "N" || flag == "NO")
            return false;

        return true;
    }
    private bool HasUserMadeFirstMove(string token)
    {
        if (FirstUserToken == token && FirstUserSetOpeningMove)
        {
            return true;
        }
        else if (SecondUserToken == token && SecondUserSetOpeningMove)
        {
            return true;
        }

        return false;
    }
    private bool HasUserGottenEndOfGameMessage(string token)
    {
        if (FirstUserToken == token)
        {
            return FirstUserGotEndOfGameResponse;
        }
        else if (SecondUserToken == token)
        {
            return SecondUserGotEndOfGameResponse;
        }

        return false;
    }
    private bool IsUsersTurn(string token)
    {
        if (FirstUserToken == token && FirstUsersTurn)
        {
            return true;
        }
        else if (SecondUserToken == token && SecondUsersTurn)
        {
            return true;
        }

        return false;
    }
    private bool IsOpponentTurn(string token)
    {
        if (ValidateToken(token))
        {
            if (FirstUserToken == token && FirstUsersTurn)
            {
                return false;
            }
            else if (SecondUserToken == token && SecondUsersTurn)
            {
                return false;
            }
        }

        return true;
    }
    private bool IsUserXYFormat(string token)
    {
        if (FirstUserToken == token && FirstUserIsXYFormat)
        {
            return true;
        }
        else if (SecondUserToken == token && SecondUserIsXYFormat)
        {
            return true;
        }

        return false;
    }
    private bool IsMoveValid(Point movePoint)
    {
        string eatValidateError = String.Empty;

        return IsMoveValid(movePoint, ref eatValidateError);
    }
    private bool IsMoveValid(Point movePoint, ref string validateMessage)
    {
        if (movePoint.X < 0 || movePoint.Y < 0 || movePoint.X > (GridWidth - 1) || movePoint.Y > (GridHeight - 1))
        {
            validateMessage = "Move point outside of grid";
            return false;
        }
        else if (Point.Equals(movePoint, FirstUserLastMove) || Point.Equals(movePoint, SecondUserLastMove))
        {
            validateMessage = "Move point already occupied by user";
            return false;
        }
        else if (Grid[movePoint.X, movePoint.Y].State == GridState.Blocked || Grid[movePoint.X, movePoint.Y].State == GridState.Occupied)
        {
            validateMessage = "Move point is already exhausted";
            return false;
        }

        return true;
    }
    private bool IsMoveValid(string token, Point movePoint, ref string validateMessage)
    {
        validateMessage = String.Empty;

        if (!IsMoveValid(movePoint, ref validateMessage))
        {
            return false;
        }
        else if (!HasUserMadeFirstMove(token))
        {
            return true;
        }
        else
        {
            Point? oldPosition = GetUserLastMove(token);

            if (oldPosition.HasValue)
            {
                for (int i = 0; i < PossibleMoveDirections.Count(); i++)
                {
                    Point? calculatedPos = new Point(oldPosition.Value.X + PossibleMoveDirections[i].X, oldPosition.Value.Y + PossibleMoveDirections[i].Y);

                    //Check if the current move matched a valid move
                    if (Point.Equals(movePoint, calculatedPos))
                    {
                        return true;
                    }
                }

                validateMessage = "Move point was not a valid move on the grid. Valid moves are in the (+-2,+-1) moving direction. Did you set the correct XYFormat on the connection?";
                return false;
            }
            else
            {
                validateMessage = "Failed to get last position for user";
                return false;
            }
        }
    }
    private bool IsGameOver(string token)
    {
        //Only check for GameOver if both users have made their first move
        if (FirstUserSetOpeningMove && SecondUserSetOpeningMove)
        {
            //Pass in the current user to get the opponents last move
            Point? opponentLastMove = GetOpponentLastMove(token);

            for (int i = 0; i < PossibleMoveDirections.Count(); i++)
            {
                Point calculatedPos = new Point(opponentLastMove.Value.X + PossibleMoveDirections[i].X, opponentLastMove.Value.Y + PossibleMoveDirections[i].Y);

                //Check if the current move matched a valid move
                if (IsMoveValid(calculatedPos))
                {
                    return false;
                }
            }
        }
        else
        {
            return false;
        }

        return true;
    }
    private bool IsUserWinner(string token)
    {
        if (FirstUserToken == token && WinningUser == FirstUser)
        {
            return true;
        }
        else if (SecondUserToken == token && WinningUser == SecondUser)
        {
            return true;
        }

        return false;
    }
    private void SwapStartingUsers()
    {
        if (StartingUser == FirstUser)
        {
            StartingUser = SecondUser;
        }
        else if (StartingUser == SecondUser)
        {
            StartingUser = FirstUser;
        }
    }
    private string StringPointToPoint(string token, string stringPoint, ref Point point)
    {
        string rtn = String.Empty;

        stringPoint = stringPoint.Trim();

        if (!stringPoint.StartsWith("(") || !stringPoint.EndsWith(")"))
        {
            rtn = "Invalid Move format, no opening or closing parens. Valid move is formatted like: (0,6)";
        }
        else if (!stringPoint.Contains(","))
        {
            rtn = "Invalid Move format, move must be formatted with a comma to split the coordinates. Format like: (3,0)";
        }
        else
        {
            //Strip off the opeing/closing parens
            stringPoint = stringPoint.Substring(1, stringPoint.Length - 2);
            string[] split = stringPoint.Split(',');

            if (split.Length != 2)
            {
                rtn = "Invalid Move format, move must have two number separated by a comma. Format like: (2,4)";
            }
            else
            {
                int firstNumber = -1;
                int secondNumber = -1;

                if (!int.TryParse(split[0], out firstNumber))
                {
                    rtn = "Invalid Move format, first number must be a number 0-9. Format like: (4,2)";
                }
                if (!int.TryParse(split[1], out secondNumber))
                {
                    rtn = "Invalid Move format, first number must be a number 0-9. Format like: (4,2)";
                }
                if (firstNumber >= 0 && secondNumber >= 0)
                {
                    if (IsUserXYFormat(token))
                    {
                        point = new Point(firstNumber, secondNumber);
                    }
                    else
                    {
                        point = new Point(secondNumber, firstNumber);
                    }
                }
            }
        }

        return rtn;
    }
    private string GameGridSquareToString(GameGridSquare ggs)
    {
        string rtn = String.Empty;

        switch (ggs.State)
        {
            case GridState.Blank:
                rtn = "-";
                break;
            case GridState.Blocked:
                rtn = "x - " + ggs.GridUser + "(" + ggs.GridMoveNumber.ToString() + ")";
                break;
            case GridState.Occupied:
                rtn = ggs.GridUser + "(" + ggs.GridMoveNumber.ToString() + ")";
                break;
            default:
                rtn = "HowDidWeGetAnInvalidGridStateHere?";
                break;
        }

        return rtn;
    }
    public bool ValidateToken(string token)
    {
        if (FirstUserToken.Trim() == token.Trim())
        {
            return true;
        }
        else if (SecondUserToken.Trim() == token.Trim())
        {
            return true;
        }

        return false;
    }
    private bool ValidateSession(string token, string channelIn, string channelPwdIn, ref string message)
    {
        if (ValidateToken(token))
        {
            if (Channel.ToUpper() == channelIn.ToUpper() && ChannelPassword == channelPwdIn)
            {
                message = String.Empty;
                return true;
            }
            else
            {
                message = "Invalid channel name/password";

                return false;
            }
        }
        else
        {
            message = "Invalid user token";

            return false;
        }
    }
    private bool ValidateText(string text)
    {
        if (text.Contains(","))
            return false;

        return true;
    }
}