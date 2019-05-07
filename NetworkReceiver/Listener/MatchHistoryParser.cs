using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class MatchHistoryParser
{
    List<Point> possibleMoveDirections;

    public MatchHistoryParser()
    {
        possibleMoveDirections = new List<Point>();
        possibleMoveDirections.Add(new Point(2, 1));
        possibleMoveDirections.Add(new Point(2, -1));
        possibleMoveDirections.Add(new Point(-2, 1));
        possibleMoveDirections.Add(new Point(-2, -1));
        possibleMoveDirections.Add(new Point(1, 2));
        possibleMoveDirections.Add(new Point(1, -2));
        possibleMoveDirections.Add(new Point(-1, 2));
        possibleMoveDirections.Add(new Point(-1, -2));
    }

    public List<NeuralNetworkDataSet> GetIndividualMovesForWinner(MatchHistory history)
    {
        List<NeuralNetworkDataSet> data = new List<NeuralNetworkDataSet>();
        GameGridSquare[,] matchGrid = new GameGridSquare[history.MatchGrid.GetLength(0), history.MatchGrid.GetLength(1)]; //Fresh empty Grid
        Point moveMade = new Point();
        Point prevMove = new Point();
        int startingPos = 0;

        for (int y = 0; y < matchGrid.GetLength(1); y++)
        {
            for (int x = 0; x < matchGrid.GetLength(0); x++)
            {
                matchGrid[x, y] = new GameGridSquare();
                matchGrid[x, y].State = GridState.Blank;
                matchGrid[x, y].GridUser = String.Empty;
            }
        }

        if (history.StartingUser != history.Winner)
        {
            SetMove(1, ref matchGrid, history.MatchGrid, ref moveMade, ref moveMade);
            startingPos++;
            SetMove(2, ref matchGrid, history.MatchGrid, ref moveMade, ref moveMade);
            startingPos++;
            SetMove(3, ref matchGrid, history.MatchGrid, ref moveMade, ref moveMade);
            startingPos++;
        }
        else
        {
            SetMove(1, ref matchGrid, history.MatchGrid, ref moveMade, ref moveMade);
            startingPos++;
            SetMove(2, ref matchGrid, history.MatchGrid, ref moveMade, ref moveMade);
            startingPos++;
        }

        for (int i = startingPos; i < history.MatchTotalMoves; i++)
        {
            NeuralNetworkDataSet indMove = new NeuralNetworkDataSet();
            indMove.GridSize = history.MatchGrid.GetLength(0);
            indMove.GridState = GameGridSquareToStringGrid(matchGrid, history.Winner);

            SetMove(i + 1, ref matchGrid, history.MatchGrid, ref moveMade, ref prevMove);
            indMove.MoveMade = MovesToDirection(new Point(moveMade.X, moveMade.Y),new Point(prevMove.X, prevMove.Y));
            indMove.PossibleMoves = GetPossibleMoves(matchGrid, moveMade, prevMove);

            if ((i + 2) < history.MatchTotalMoves)
            {
                SetMove((i + 2), ref matchGrid, history.MatchGrid, ref moveMade, ref prevMove);
                i++;
            }

            data.Add(indMove);
        }

        return data;
    }
    public string[] GameGridSquareToStringGrid(GameGridSquare[,] gridIn, string ourPlayer)
    {
        string[] stringGrid = new string[gridIn.GetLength(0) * gridIn.GetLength(1)];

        for (int y = 0; y < gridIn.GetLength(1); y++)
        {
            for (int x = 0; x < gridIn.GetLength(0); x++)
            {
                stringGrid[y * gridIn.GetLength(0) + x] = GridStateToString(gridIn[x, y], ourPlayer);
            }
        }

        return stringGrid;
    }
    public string GridStateToString(GameGridSquare ggs, string ourPlayer)
    {
        switch (ggs.State)
        {
            case GridState.Blank:
                return " ";
            case GridState.Blocked:
                if (!String.IsNullOrEmpty(ggs.GridUser) && ggs.GridUser == ourPlayer)
                    return "U";
                else if (!String.IsNullOrEmpty(ggs.GridUser))
                    return "O";

                return "X";
            case GridState.Occupied:
                if (ggs.GridUser == ourPlayer)
                    return "U";
                else
                    return "O";
            default:
                return " ";
        }
    }
    public void SetMove(int move, ref GameGridSquare[,] gridIn, GameGridSquare[,] baseBoard, ref Point moveMade, ref Point PrevMove)
    {
        string moveUser = String.Empty;
        int moveX = -1;
        int moveY = -1;

        bool foundPos = false;
        for (int y = 0; y < baseBoard.GetLength(1); y++)
        {
            for (int x = 0; x < baseBoard.GetLength(0); x++)
            {
                if (baseBoard[x, y].GridMoveNumber == move)
                {
                    foundPos = true;
                    moveX = x;
                    moveY = y;
                    gridIn[x, y] = new GameGridSquare();
                    gridIn[x, y].X = baseBoard[x, y].X;
                    gridIn[x, y].Y = baseBoard[x, y].Y;
                    gridIn[x, y].State = baseBoard[x, y].State;
                    gridIn[x, y].GridUser = baseBoard[x, y].GridUser;
                    moveUser = gridIn[x, y].GridUser;
                    moveMade = new Point(x, y);

                    break;
                }
            }
            if (foundPos)
                break;
        }

        for (int y = 0; y < gridIn.GetLength(1); y++)
        {
            for (int x = 0; x < gridIn.GetLength(0); x++)
            {
                if (moveX != x || moveY != y)
                {
                    if (gridIn[x, y].GridUser == moveUser)
                    {
                        gridIn[x, y].State = GridState.Blocked;
                        gridIn[x, y].GridUser = String.Empty;
                        PrevMove = new Point(x, y);
                    }
                }
            }
        }
    }
    public int MovesToDirection(Point move, Point prevMove)
    {
        Point direction = new Point(move.X - prevMove.X, move.Y - prevMove.Y);

        for (int i = 0; i < possibleMoveDirections.Count(); i++)
        {
            if (direction.Equals(possibleMoveDirections[i]))
                return i;
        }

        return 0;
    }
    public int[] GetPossibleMoves(GameGridSquare[,] gridIn, Point moveMade, Point PrevMove)
    {
        int[] possibleMoves = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 };

        for (int i = 0; i < possibleMoveDirections.Count(); i++)
        {
            Point tmpMoveP = new Point(PrevMove.X + possibleMoveDirections[i].X, PrevMove.Y + possibleMoveDirections[i].Y);

            if (tmpMoveP.X < 0 || tmpMoveP.Y < 0 || tmpMoveP.X >= gridIn.GetLength(0) || tmpMoveP.Y >= gridIn.GetLength(1))
            {
                possibleMoves[i] = 0;
            }
            else if (gridIn[tmpMoveP.X, tmpMoveP.Y].State == GridState.Blocked || gridIn[tmpMoveP.X, tmpMoveP.Y].State == GridState.Occupied)
            {
                if (tmpMoveP.Equals(moveMade))
                {
                    possibleMoves[i] = 1;
                }
                else
                {
                    possibleMoves[i] = 0;
                }
            }
            else
            {
                possibleMoves[i] = 1;
            }
        }

        return possibleMoves;
    }
}