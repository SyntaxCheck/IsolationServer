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
    public partial class MatchReplayer : Form
    {
        private const string PLAYER_ONE_ICON = "_O_";
        private const string PLAYER_TWO_ICON = "-T-";
        private const string PLAYER_ONE_BLOCKED_ICON = "O";
        private const string PLAYER_TWO_BLOCKED_ICON = "T";
        private Color BLOCKED_COLOR = Color.SlateGray;
        private Color PLAYER_ONE_COLOR = Color.LimeGreen;
        private Color PLAYER_TWO_COLOR = Color.LightBlue;
        private List<Point> possibleMoveDirections;
        public GameGridSquare[,] GameGrid;
        public MatchHistory Match;
        private int CurrentReplayCtr;
        private int MatchMaxMove;
        private List<Button> HighlightedButtons;

        public MatchReplayer()
        {
            InitializeComponent();

            CurrentReplayCtr = 1;
            btnPlayerOneLegend.Text = PLAYER_ONE_ICON;
            btnPlayerTwoLegend.Text = PLAYER_TWO_ICON;
            btnPlayerOneLegend.BackColor = PLAYER_ONE_COLOR;
            btnPlayerTwoLegend.BackColor = PLAYER_TWO_COLOR;

            possibleMoveDirections = new List<Point>();
            possibleMoveDirections.Add(new Point(2, 1));
            possibleMoveDirections.Add(new Point(2, -1));
            possibleMoveDirections.Add(new Point(-2, 1));
            possibleMoveDirections.Add(new Point(-2, -1));
            possibleMoveDirections.Add(new Point(1, 2));
            possibleMoveDirections.Add(new Point(1, -2));
            possibleMoveDirections.Add(new Point(-1, 2));
            possibleMoveDirections.Add(new Point(-1, -2));

            HighlightedButtons = new List<Button>();
        }

        private void MatchReplayer_Load(object sender, EventArgs e)
        {
            //Load all the labels on screen
            lblId.Text = Match.MatchId.ToString();
            lblDate.Text = Match.MatchStartDate.ToString("yyyy-MM-dd hh:mm:ss");
            lblDuration.Text = TimeSpanFormatter.TimeSpanFormattedFull(Match.MatchLength);
            lblCommands.Text = Match.MatchTotalCommands.ToString();
            lblMoves.Text = Match.MatchTotalMoves.ToString();
            lblPlayerOne.Text = Match.PlayerOne;
            lblPlayerOneLegend.Text = Match.PlayerOne;
            lblPlayerTwo.Text = Match.PlayerTwo;
            lblPlayerTwoLegend.Text = Match.PlayerTwo;
            lblStartingPlayer.Text = Match.StartingPlayer;
            lblWinner.Text = Match.Winner;
            GameGrid = Match.MatchGrid;

            BuildButtonsAndLabels();
        }
        private void btnNextMove_Click(object sender, EventArgs e)
        {
            ClearHighLights();
            SetMove(CurrentReplayCtr);
            CurrentReplayCtr++;
        }
        private void btnRunAllMoves_Click(object sender, EventArgs e)
        {
            ClearHighLights();
            for (int i = CurrentReplayCtr; i < (MatchMaxMove + 1); i++)
            {
                SetMove(i);
                CurrentReplayCtr++;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            CurrentReplayCtr = 1;

            for (int y = 0; y < GameGrid.GetLength(1); y++)
            {
                for (int x = 0; x < GameGrid.GetLength(0); x++)
                {
                    Button btn = this.Controls.Find("Btn_X" + x.ToString() + "_Y" + y.ToString(), true).FirstOrDefault() as Button;
                    SetButtonProperties(ref btn);
                }
            }
        }
        private void Generic_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            Point clickedPoint = ButtonNameToPoint(btn.Name);

            if (clickedPoint.X != -1)
            {
                ClearHighLights();

                for (int i = 0; i < possibleMoveDirections.Count(); i++)
                {
                    Point highlightPoint = new Point(clickedPoint.X + possibleMoveDirections[i].X, clickedPoint.Y + possibleMoveDirections[i].Y);

                    if (highlightPoint.X > 0 && highlightPoint.Y > 0)
                    {
                        string highlightName = "Btn_X" + highlightPoint.X.ToString() + "_Y" + highlightPoint.Y.ToString();
                        Button foundBtn = this.Controls.Find(highlightName, true).FirstOrDefault() as Button;

                        //If the button exists on the grid and and there is no text in it. If there is text that means it is unavailable
                        if (foundBtn != null && foundBtn.Text == String.Empty)
                        {
                            HighlightedButtons.Add(foundBtn);
                            foundBtn.BackColor = Color.Yellow;
                        }
                    }
                }
            }
        }

        //Private functions
        private void BuildButtonsAndLabels()
        {
            //Build Game board, resize the window to match size of grid, adjust the button size and font size based on board size
            int buttonSize = 40;
            int buttonSpacing = 2;
            int gridOuterBuffer = 40;
            int startingY = 175;
            int labelBuffer = 5;
            int labelWidthOffset = 7;

            //build out labels 0 index based
            for (int y = 0; y < GameGrid.GetLength(1); y++)
            {
                Label newLabelY = new Label();
                newLabelY.Text = y.ToString();
                newLabelY.AutoSize = true;
                newLabelY.Location = new Point(gridOuterBuffer - labelBuffer - (labelWidthOffset * 2) - labelWidthOffset, startingY + gridOuterBuffer + (y * buttonSize) + (y * buttonSpacing) + (buttonSize / 2) - (newLabelY.Height / 2) + labelWidthOffset);

                //gridLabels.Add(newLabel);
                this.Controls.Add(newLabelY);

                if (y == 0)
                {
                    for (int x = 0; x < GameGrid.GetLength(0); x++)
                    {
                        Label newLabelX = new Label();
                        newLabelX.Text = x.ToString();
                        newLabelX.AutoSize = true;
                        newLabelX.Location = new Point(gridOuterBuffer + (x * buttonSize) + (x * buttonSpacing) + ((buttonSize / 2) - labelWidthOffset), startingY + (gridOuterBuffer - newLabelX.Height - labelBuffer) + (y * buttonSize) + (y * buttonSpacing));

                        this.Controls.Add(newLabelX);
                    }
                }
            }

            //build button grid
            MatchMaxMove = 0;
            for (int y = 0; y < GameGrid.GetLength(1); y++)
            {
                for (int x = 0; x < GameGrid.GetLength(0); x++)
                {
                    GameGridSquare ggs = new GameGridSquare();

                    Button newBtn = new Button();
                    newBtn.Width = buttonSize;
                    newBtn.Height = buttonSize;
                    newBtn.Location = new Point(gridOuterBuffer + (x * buttonSize) + (x * buttonSpacing), startingY + gridOuterBuffer + (y * buttonSize) + (y * buttonSpacing));
                    newBtn.Name = "Btn_X" + x.ToString() + "_Y" + y.ToString();
                    newBtn.Click += new EventHandler(Generic_Click);
                    SetButtonProperties(ref newBtn);

                    toolTipMain.SetToolTip(newBtn, "(" + x.ToString() + "," + y.ToString() + ")");

                    this.Controls.Add(newBtn);

                    if (GameGrid[x, y].GridMoveNumber > MatchMaxMove)
                    {
                        MatchMaxMove = GameGrid[x, y].GridMoveNumber;
                    }
                }
            }

            //adjust the screen size
            int formHeight = (startingY + gridOuterBuffer + ((GameGrid.GetLength(1) + 1) * buttonSize) + ((GameGrid.GetLength(1) + 1) * buttonSpacing) + buttonSpacing); //Add one extra spacing for the margin
            this.Height = formHeight;
        }
        private void SetButtonProperties(ref Button btnIn)
        {
            btnIn.BackColor = Color.White;
            btnIn.Font = new Font(btnIn.Font, FontStyle.Bold);
            btnIn.Text = String.Empty;
        }
        private void SetMove(int move)
        {
            for (int y = 0; y < GameGrid.GetLength(1); y++)
            {
                for (int x = 0; x < GameGrid.GetLength(0); x++)
                {
                    if (GameGrid[x, y].GridMoveNumber == move)
                    {
                        string playerIcon = String.Empty;
                        Color playerColor = Color.White;
                        if (GameGrid[x, y].GridPlayer == Match.PlayerOne)
                        {
                            playerIcon = PLAYER_ONE_ICON;
                            playerColor = PLAYER_ONE_COLOR;
                        }
                        else if (GameGrid[x, y].GridPlayer == Match.PlayerTwo)
                        {
                            playerIcon = PLAYER_TWO_ICON;
                            playerColor = PLAYER_TWO_COLOR;
                        }

                        Button btn = this.Controls.Find("Btn_X" + x.ToString() + "_Y" + y.ToString(), true).FirstOrDefault() as Button;
                        btn.BackColor = playerColor;
                        btn.Text = playerIcon;
                    }
                    else if (move > 2 && move <= MatchMaxMove && GameGrid[x, y].GridMoveNumber == (move - 2)) //GridMoveNumber starts at 1 not 0
                    {
                        for (int y2 = 0; y2 < GameGrid.GetLength(1); y2++)
                        {
                            for (int x2 = 0; x2 < GameGrid.GetLength(0); x2++)
                            {
                                if (GameGrid[x2, y2].GridMoveNumber == (move - 2))
                                {
                                    string gridBlockedText = String.Empty;

                                    if (GameGrid[x2, y2].GridPlayer == Match.PlayerOne)
                                    {
                                        gridBlockedText = PLAYER_ONE_BLOCKED_ICON;
                                    }
                                    else if (GameGrid[x2, y2].GridPlayer == Match.PlayerTwo)
                                    {
                                        gridBlockedText = PLAYER_TWO_BLOCKED_ICON;
                                    }
                                    gridBlockedText += " - " + GameGrid[x2, y2].GridMoveNumber.ToString();

                                    //Two moves ago was this players last move, clear the button
                                    Button btn = this.Controls.Find("Btn_X" + x.ToString() + "_Y" + y.ToString(), true).FirstOrDefault() as Button;
                                    btn.Text = gridBlockedText;
                                    btn.BackColor = BLOCKED_COLOR;
                                }
                            }
                        }
                    }
                }
            }
        }
        private Point ButtonNameToPoint(string name)
        {
            Point point = new Point(-1, -1);
            string xStr = String.Empty;
            string yStr = String.Empty;
            int x;
            int y;

            //Should result in something like 3_Y6
            name = name.Substring(name.IndexOf('X') + 1, name.Length - (name.IndexOf('X') + 1));

            xStr = name.Substring(0, name.IndexOf('_'));
            yStr = name.Substring(name.IndexOf('Y') + 1);

            if (int.TryParse(xStr, out x) && int.TryParse(yStr, out y))
            {
                point.X = x;
                point.Y = y;
            }
            else
            {
                MessageBox.Show("Failed to convert button text to Point: " + name, "Failed to convert");
            }

            return point;
        }
        private void ClearHighLights()
        {
            if (HighlightedButtons.Count() > 0)
            {
                for (int i = HighlightedButtons.Count() - 1; i >= 0; i--)
                {
                    HighlightedButtons[i].BackColor = Color.White;
                }
            }
        }
    }
}