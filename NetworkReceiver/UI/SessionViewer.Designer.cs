namespace NetworkReceiver
{
    partial class SessionViewer
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionViewer));
            this.tbxOutput = new System.Windows.Forms.TextBox();
            this.timerSessionViewer = new System.Windows.Forms.Timer(this.components);
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lblMoves = new System.Windows.Forms.Label();
            this.lblCommands = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblTotalClients = new System.Windows.Forms.Label();
            this.lblTotalMatches = new System.Windows.Forms.Label();
            this.lblTotalSessions = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnWriteNow = new System.Windows.Forms.Button();
            this.lblLastDbWrite = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnGetSessionMatches = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbxMatchRetriever = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.tbxPort = new System.Windows.Forms.TextBox();
            this.btnClearInactiveSessions = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.btnKillSession = new System.Windows.Forms.Button();
            this.tbxKillSession = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblUptime = new System.Windows.Forms.Label();
            this.btnDbViewer = new System.Windows.Forms.Button();
            this.lblCommandsPerSecond = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbxOutput
            // 
            this.tbxOutput.Location = new System.Drawing.Point(6, 19);
            this.tbxOutput.Multiline = true;
            this.tbxOutput.Name = "tbxOutput";
            this.tbxOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbxOutput.Size = new System.Drawing.Size(299, 401);
            this.tbxOutput.TabIndex = 0;
            // 
            // timerSessionViewer
            // 
            this.timerSessionViewer.Interval = 2500;
            this.timerSessionViewer.Tick += new System.EventHandler(this.timerSessionViewer_Tick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbxOutput);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(311, 426);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Session Log Viewer";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lblCommandsPerSecond);
            this.groupBox2.Controls.Add(this.lblMoves);
            this.groupBox2.Controls.Add(this.lblCommands);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.lblTotalClients);
            this.groupBox2.Controls.Add(this.lblTotalMatches);
            this.groupBox2.Controls.Add(this.lblTotalSessions);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(332, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(251, 100);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Total Statistics";
            // 
            // lblMoves
            // 
            this.lblMoves.AutoSize = true;
            this.lblMoves.Location = new System.Drawing.Point(189, 49);
            this.lblMoves.Name = "lblMoves";
            this.lblMoves.Size = new System.Drawing.Size(27, 13);
            this.lblMoves.TabIndex = 9;
            this.lblMoves.Text = "N/A";
            // 
            // lblCommands
            // 
            this.lblCommands.AutoSize = true;
            this.lblCommands.Location = new System.Drawing.Point(189, 26);
            this.lblCommands.Name = "lblCommands";
            this.lblCommands.Size = new System.Drawing.Size(27, 13);
            this.lblCommands.TabIndex = 8;
            this.lblCommands.Text = "N/A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(141, 49);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(42, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Moves:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(62, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Commands:";
            // 
            // lblTotalClients
            // 
            this.lblTotalClients.AutoSize = true;
            this.lblTotalClients.Location = new System.Drawing.Point(74, 72);
            this.lblTotalClients.Name = "lblTotalClients";
            this.lblTotalClients.Size = new System.Drawing.Size(27, 13);
            this.lblTotalClients.TabIndex = 5;
            this.lblTotalClients.Text = "N/A";
            // 
            // lblTotalMatches
            // 
            this.lblTotalMatches.AutoSize = true;
            this.lblTotalMatches.Location = new System.Drawing.Point(74, 49);
            this.lblTotalMatches.Name = "lblTotalMatches";
            this.lblTotalMatches.Size = new System.Drawing.Size(27, 13);
            this.lblTotalMatches.TabIndex = 4;
            this.lblTotalMatches.Text = "N/A";
            // 
            // lblTotalSessions
            // 
            this.lblTotalSessions.AutoSize = true;
            this.lblTotalSessions.Location = new System.Drawing.Point(74, 26);
            this.lblTotalSessions.Name = "lblTotalSessions";
            this.lblTotalSessions.Size = new System.Drawing.Size(27, 13);
            this.lblTotalSessions.TabIndex = 3;
            this.lblTotalSessions.Text = "N/A";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Clients:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(51, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Matches:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Sessions:";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnWriteNow);
            this.groupBox3.Controls.Add(this.lblLastDbWrite);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.btnGetSessionMatches);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.tbxMatchRetriever);
            this.groupBox3.Controls.Add(this.btnStart);
            this.groupBox3.Controls.Add(this.btnStop);
            this.groupBox3.Controls.Add(this.tbxPort);
            this.groupBox3.Controls.Add(this.btnClearInactiveSessions);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.btnKillSession);
            this.groupBox3.Controls.Add(this.tbxKillSession);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Location = new System.Drawing.Point(332, 118);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(251, 200);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Server Controls";
            // 
            // btnWriteNow
            // 
            this.btnWriteNow.Location = new System.Drawing.Point(169, 166);
            this.btnWriteNow.Name = "btnWriteNow";
            this.btnWriteNow.Size = new System.Drawing.Size(75, 22);
            this.btnWriteNow.TabIndex = 14;
            this.btnWriteNow.Text = "Write Now";
            this.btnWriteNow.UseVisualStyleBackColor = true;
            this.btnWriteNow.Click += new System.EventHandler(this.btnWriteNow_Click);
            // 
            // lblLastDbWrite
            // 
            this.lblLastDbWrite.AutoSize = true;
            this.lblLastDbWrite.Location = new System.Drawing.Point(136, 136);
            this.lblLastDbWrite.Name = "lblLastDbWrite";
            this.lblLastDbWrite.Size = new System.Drawing.Size(106, 13);
            this.lblLastDbWrite.TabIndex = 13;
            this.lblLastDbWrite.Text = "2019-01-01 00:00:00";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(57, 136);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(73, 13);
            this.label10.TabIndex = 12;
            this.label10.Text = "DB Last write:";
            // 
            // btnGetSessionMatches
            // 
            this.btnGetSessionMatches.Location = new System.Drawing.Point(205, 104);
            this.btnGetSessionMatches.Name = "btnGetSessionMatches";
            this.btnGetSessionMatches.Size = new System.Drawing.Size(29, 22);
            this.btnGetSessionMatches.TabIndex = 11;
            this.btnGetSessionMatches.Text = "Go";
            this.btnGetSessionMatches.UseVisualStyleBackColor = true;
            this.btnGetSessionMatches.Click += new System.EventHandler(this.btnGetSessionMatches_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(111, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Get Session Matches:";
            // 
            // tbxMatchRetriever
            // 
            this.tbxMatchRetriever.Location = new System.Drawing.Point(136, 106);
            this.tbxMatchRetriever.Name = "tbxMatchRetriever";
            this.tbxMatchRetriever.Size = new System.Drawing.Size(63, 20);
            this.tbxMatchRetriever.TabIndex = 9;
            // 
            // btnStart
            // 
            this.btnStart.Enabled = false;
            this.btnStart.Location = new System.Drawing.Point(88, 166);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 22);
            this.btnStart.TabIndex = 8;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(7, 166);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 22);
            this.btnStop.TabIndex = 7;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // tbxPort
            // 
            this.tbxPort.Location = new System.Drawing.Point(136, 26);
            this.tbxPort.Name = "tbxPort";
            this.tbxPort.Size = new System.Drawing.Size(63, 20);
            this.tbxPort.TabIndex = 6;
            this.tbxPort.Text = "25174";
            // 
            // btnClearInactiveSessions
            // 
            this.btnClearInactiveSessions.Location = new System.Drawing.Point(136, 78);
            this.btnClearInactiveSessions.Name = "btnClearInactiveSessions";
            this.btnClearInactiveSessions.Size = new System.Drawing.Size(29, 22);
            this.btnClearInactiveSessions.TabIndex = 5;
            this.btnClearInactiveSessions.Text = "Go";
            this.btnClearInactiveSessions.UseVisualStyleBackColor = true;
            this.btnClearInactiveSessions.Click += new System.EventHandler(this.btnClearInactiveSessions_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(10, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(120, 13);
            this.label9.TabIndex = 4;
            this.label9.Text = "Clear Inactive Sessions:";
            // 
            // btnKillSession
            // 
            this.btnKillSession.Location = new System.Drawing.Point(205, 50);
            this.btnKillSession.Name = "btnKillSession";
            this.btnKillSession.Size = new System.Drawing.Size(29, 22);
            this.btnKillSession.TabIndex = 3;
            this.btnKillSession.Text = "Go";
            this.btnKillSession.UseVisualStyleBackColor = true;
            this.btnKillSession.Click += new System.EventHandler(this.btnKillSession_Click);
            // 
            // tbxKillSession
            // 
            this.tbxKillSession.Location = new System.Drawing.Point(136, 52);
            this.tbxKillSession.Name = "tbxKillSession";
            this.tbxKillSession.Size = new System.Drawing.Size(63, 20);
            this.tbxKillSession.TabIndex = 2;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(38, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(92, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Kill Session By ID:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(67, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Server Port:";
            // 
            // lblUptime
            // 
            this.lblUptime.AutoSize = true;
            this.lblUptime.Location = new System.Drawing.Point(329, 428);
            this.lblUptime.Name = "lblUptime";
            this.lblUptime.Size = new System.Drawing.Size(77, 13);
            this.lblUptime.TabIndex = 4;
            this.lblUptime.Text = "Server Uptime:";
            // 
            // btnDbViewer
            // 
            this.btnDbViewer.Location = new System.Drawing.Point(508, 335);
            this.btnDbViewer.Name = "btnDbViewer";
            this.btnDbViewer.Size = new System.Drawing.Size(75, 23);
            this.btnDbViewer.TabIndex = 5;
            this.btnDbViewer.Text = "DB Viewer";
            this.btnDbViewer.UseVisualStyleBackColor = true;
            this.btnDbViewer.Click += new System.EventHandler(this.btnDbViewer_Click);
            // 
            // lblCommandsPerSecond
            // 
            this.lblCommandsPerSecond.AutoSize = true;
            this.lblCommandsPerSecond.Location = new System.Drawing.Point(189, 72);
            this.lblCommandsPerSecond.Name = "lblCommandsPerSecond";
            this.lblCommandsPerSecond.Size = new System.Drawing.Size(27, 13);
            this.lblCommandsPerSecond.TabIndex = 10;
            this.lblCommandsPerSecond.Text = "N/A";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(111, 72);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(72, 13);
            this.label11.TabIndex = 11;
            this.label11.Text = "Cmd Per Sec:";
            // 
            // SessionViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(595, 446);
            this.Controls.Add(this.btnDbViewer);
            this.Controls.Add(this.lblUptime);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "SessionViewer";
            this.Text = "Session Viewer";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SessionViewer_FormClosing);
            this.Load += new System.EventHandler(this.SessionViewer_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbxOutput;
        private System.Windows.Forms.Timer timerSessionViewer;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblTotalClients;
        private System.Windows.Forms.Label lblTotalMatches;
        private System.Windows.Forms.Label lblTotalSessions;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnKillSession;
        private System.Windows.Forms.TextBox tbxKillSession;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnClearInactiveSessions;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbxPort;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label lblUptime;
        private System.Windows.Forms.Button btnGetSessionMatches;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbxMatchRetriever;
        private System.Windows.Forms.Label lblMoves;
        private System.Windows.Forms.Label lblCommands;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblLastDbWrite;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnWriteNow;
        private System.Windows.Forms.Button btnDbViewer;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblCommandsPerSecond;
    }
}

