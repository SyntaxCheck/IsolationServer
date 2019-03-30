namespace NetworkReceiver
{
    partial class MatchSessionGameSelector
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MatchSessionGameSelector));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvMatches = new System.Windows.Forms.DataGridView();
            this.MatchId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StartingPlayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Winner = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchTotalMoves = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstOpeningMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondOpeningMove = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FirstPlayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SecondPlayer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MatchTotalCommands = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnViewMatch = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnViewMatch);
            this.groupBox1.Controls.Add(this.dgvMatches);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(706, 389);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Select A Match";
            // 
            // dgvMatches
            // 
            this.dgvMatches.AllowUserToAddRows = false;
            this.dgvMatches.AllowUserToDeleteRows = false;
            this.dgvMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MatchId,
            this.MatchDate,
            this.StartingPlayer,
            this.Winner,
            this.MatchTotalMoves,
            this.FirstOpeningMove,
            this.SecondOpeningMove,
            this.MatchLength,
            this.FirstPlayer,
            this.SecondPlayer,
            this.MatchTotalCommands});
            this.dgvMatches.Location = new System.Drawing.Point(6, 19);
            this.dgvMatches.MultiSelect = false;
            this.dgvMatches.Name = "dgvMatches";
            this.dgvMatches.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMatches.Size = new System.Drawing.Size(613, 364);
            this.dgvMatches.TabIndex = 0;
            // 
            // MatchId
            // 
            this.MatchId.FillWeight = 40F;
            this.MatchId.HeaderText = "Id";
            this.MatchId.Name = "MatchId";
            this.MatchId.ReadOnly = true;
            this.MatchId.Width = 40;
            // 
            // MatchDate
            // 
            this.MatchDate.HeaderText = "Date";
            this.MatchDate.Name = "MatchDate";
            this.MatchDate.ReadOnly = true;
            // 
            // StartingPlayer
            // 
            this.StartingPlayer.HeaderText = "Starting Player";
            this.StartingPlayer.Name = "StartingPlayer";
            this.StartingPlayer.ReadOnly = true;
            // 
            // Winner
            // 
            this.Winner.HeaderText = "Winner";
            this.Winner.Name = "Winner";
            this.Winner.ReadOnly = true;
            // 
            // MatchTotalMoves
            // 
            this.MatchTotalMoves.FillWeight = 45F;
            this.MatchTotalMoves.HeaderText = "Total Moves";
            this.MatchTotalMoves.Name = "MatchTotalMoves";
            this.MatchTotalMoves.ReadOnly = true;
            this.MatchTotalMoves.Width = 45;
            // 
            // FirstOpeningMove
            // 
            this.FirstOpeningMove.FillWeight = 50F;
            this.FirstOpeningMove.HeaderText = "First Move";
            this.FirstOpeningMove.Name = "FirstOpeningMove";
            this.FirstOpeningMove.ReadOnly = true;
            this.FirstOpeningMove.Width = 50;
            // 
            // SecondOpeningMove
            // 
            this.SecondOpeningMove.FillWeight = 50F;
            this.SecondOpeningMove.HeaderText = "Second Move";
            this.SecondOpeningMove.Name = "SecondOpeningMove";
            this.SecondOpeningMove.ReadOnly = true;
            this.SecondOpeningMove.Width = 50;
            // 
            // MatchLength
            // 
            this.MatchLength.HeaderText = "Match Length";
            this.MatchLength.Name = "MatchLength";
            this.MatchLength.ReadOnly = true;
            // 
            // FirstPlayer
            // 
            this.FirstPlayer.HeaderText = "First Player";
            this.FirstPlayer.Name = "FirstPlayer";
            this.FirstPlayer.ReadOnly = true;
            // 
            // SecondPlayer
            // 
            this.SecondPlayer.HeaderText = "Second Player";
            this.SecondPlayer.Name = "SecondPlayer";
            this.SecondPlayer.ReadOnly = true;
            // 
            // MatchTotalCommands
            // 
            this.MatchTotalCommands.FillWeight = 50F;
            this.MatchTotalCommands.HeaderText = "Total Commands";
            this.MatchTotalCommands.Name = "MatchTotalCommands";
            this.MatchTotalCommands.ReadOnly = true;
            this.MatchTotalCommands.Width = 50;
            // 
            // btnViewMatch
            // 
            this.btnViewMatch.Location = new System.Drawing.Point(625, 19);
            this.btnViewMatch.Name = "btnViewMatch";
            this.btnViewMatch.Size = new System.Drawing.Size(75, 23);
            this.btnViewMatch.TabIndex = 1;
            this.btnViewMatch.Text = "View Match";
            this.btnViewMatch.UseVisualStyleBackColor = true;
            this.btnViewMatch.Click += new System.EventHandler(this.btnViewMatch_Click);
            // 
            // MatchSessionGameSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 409);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MatchSessionGameSelector";
            this.Text = "Match Selector";
            this.Load += new System.EventHandler(this.MatchSessionGameSelector_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dgvMatches;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchId;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn StartingPlayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn Winner;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchTotalMoves;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstOpeningMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondOpeningMove;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn FirstPlayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn SecondPlayer;
        private System.Windows.Forms.DataGridViewTextBoxColumn MatchTotalCommands;
        private System.Windows.Forms.Button btnViewMatch;
    }
}