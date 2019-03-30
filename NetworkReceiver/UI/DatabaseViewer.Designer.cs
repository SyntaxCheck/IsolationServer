namespace NetworkReceiver
{
    partial class DatabaseViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DatabaseViewer));
            this.dgvMatches = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.cbxStartingPos = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.GridSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserOneVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserOneAlgorithm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserOneConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserOneWins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserTwoVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserTwoAlgorithm = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserTwoConfig = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UserTwoWins = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WinPercent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvMatches
            // 
            this.dgvMatches.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMatches.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GridSize,
            this.UserOneVersion,
            this.UserOneAlgorithm,
            this.UserOneConfig,
            this.UserOneWins,
            this.UserTwoVersion,
            this.UserTwoAlgorithm,
            this.UserTwoConfig,
            this.UserTwoWins,
            this.WinPercent});
            this.dgvMatches.Location = new System.Drawing.Point(12, 49);
            this.dgvMatches.Name = "dgvMatches";
            this.dgvMatches.Size = new System.Drawing.Size(918, 389);
            this.dgvMatches.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Starting Position:";
            // 
            // cbxStartingPos
            // 
            this.cbxStartingPos.FormattingEnabled = true;
            this.cbxStartingPos.Location = new System.Drawing.Point(137, 17);
            this.cbxStartingPos.Name = "cbxStartingPos";
            this.cbxStartingPos.Size = new System.Drawing.Size(121, 21);
            this.cbxStartingPos.TabIndex = 2;
            this.cbxStartingPos.SelectedIndexChanged += new System.EventHandler(this.cbxStartingPos_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(298, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "OR";
            this.label2.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(360, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "DB Filter:";
            this.label3.Visible = false;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(416, 17);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(514, 20);
            this.textBox1.TabIndex = 5;
            this.textBox1.Visible = false;
            // 
            // GridSize
            // 
            this.GridSize.FillWeight = 65F;
            this.GridSize.HeaderText = "GridSize";
            this.GridSize.Name = "GridSize";
            this.GridSize.Width = 65;
            // 
            // UserOneVersion
            // 
            this.UserOneVersion.FillWeight = 50F;
            this.UserOneVersion.HeaderText = "User One Version";
            this.UserOneVersion.Name = "UserOneVersion";
            this.UserOneVersion.Width = 50;
            // 
            // UserOneAlgorithm
            // 
            this.UserOneAlgorithm.FillWeight = 75F;
            this.UserOneAlgorithm.HeaderText = "User One Algorithm";
            this.UserOneAlgorithm.Name = "UserOneAlgorithm";
            this.UserOneAlgorithm.Width = 75;
            // 
            // UserOneConfig
            // 
            this.UserOneConfig.FillWeight = 140F;
            this.UserOneConfig.HeaderText = "User One Config";
            this.UserOneConfig.Name = "UserOneConfig";
            this.UserOneConfig.Width = 140;
            // 
            // UserOneWins
            // 
            this.UserOneWins.HeaderText = "User One Wins";
            this.UserOneWins.Name = "UserOneWins";
            // 
            // UserTwoVersion
            // 
            this.UserTwoVersion.FillWeight = 50F;
            this.UserTwoVersion.HeaderText = "User Two Version";
            this.UserTwoVersion.Name = "UserTwoVersion";
            this.UserTwoVersion.Width = 50;
            // 
            // UserTwoAlgorithm
            // 
            this.UserTwoAlgorithm.FillWeight = 75F;
            this.UserTwoAlgorithm.HeaderText = "User Two Algorithm";
            this.UserTwoAlgorithm.Name = "UserTwoAlgorithm";
            this.UserTwoAlgorithm.Width = 75;
            // 
            // UserTwoConfig
            // 
            this.UserTwoConfig.FillWeight = 140F;
            this.UserTwoConfig.HeaderText = "User Two Config";
            this.UserTwoConfig.Name = "UserTwoConfig";
            this.UserTwoConfig.Width = 140;
            // 
            // UserTwoWins
            // 
            this.UserTwoWins.HeaderText = "User Two Wins";
            this.UserTwoWins.Name = "UserTwoWins";
            // 
            // WinPercent
            // 
            this.WinPercent.FillWeight = 50F;
            this.WinPercent.HeaderText = "Win Percent";
            this.WinPercent.Name = "WinPercent";
            this.WinPercent.Width = 50;
            // 
            // DatabaseViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 450);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbxStartingPos);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvMatches);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DatabaseViewer";
            this.Text = "DatabaseViewer";
            this.Load += new System.EventHandler(this.DatabaseViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMatches)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvMatches;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbxStartingPos;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GridSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserOneVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserOneAlgorithm;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserOneConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserOneWins;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserTwoVersion;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserTwoAlgorithm;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserTwoConfig;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserTwoWins;
        private System.Windows.Forms.DataGridViewTextBoxColumn WinPercent;
    }
}