namespace Checkers
{
    partial class MainWin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWin));
            this.newBtn = new System.Windows.Forms.Button();
            this.optionsBtn = new System.Windows.Forms.Button();
            this.helpBtn = new System.Windows.Forms.Button();
            this.exitBtn = new System.Windows.Forms.Button();
            this.MsgLbl = new System.Windows.Forms.Label();
            this.TakeBackBtn = new System.Windows.Forms.Button();
            this.EndMoveBtn = new System.Windows.Forms.Button();
            this.checkerBoard = new Checkers.CheckerBoard();
            this.PauseBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // newBtn
            // 
            this.newBtn.AllowDrop = true;
            this.newBtn.Location = new System.Drawing.Point(516, 12);
            this.newBtn.Name = "newBtn";
            this.newBtn.Size = new System.Drawing.Size(75, 23);
            this.newBtn.TabIndex = 1;
            this.newBtn.Text = "&New Game";
            this.newBtn.UseVisualStyleBackColor = true;
            this.newBtn.Click += new System.EventHandler(this.newBtn_Click);
            // 
            // optionsBtn
            // 
            this.optionsBtn.AllowDrop = true;
            this.optionsBtn.Location = new System.Drawing.Point(516, 41);
            this.optionsBtn.Name = "optionsBtn";
            this.optionsBtn.Size = new System.Drawing.Size(75, 23);
            this.optionsBtn.TabIndex = 2;
            this.optionsBtn.Text = "&Options";
            this.optionsBtn.UseVisualStyleBackColor = true;
            this.optionsBtn.Click += new System.EventHandler(this.optionsBtn_Click);
            // 
            // helpBtn
            // 
            this.helpBtn.AllowDrop = true;
            this.helpBtn.Location = new System.Drawing.Point(516, 70);
            this.helpBtn.Name = "helpBtn";
            this.helpBtn.Size = new System.Drawing.Size(75, 23);
            this.helpBtn.TabIndex = 3;
            this.helpBtn.Text = "&Help";
            this.helpBtn.UseVisualStyleBackColor = true;
            this.helpBtn.Click += new System.EventHandler(this.helpBtn_Click);
            // 
            // exitBtn
            // 
            this.exitBtn.AllowDrop = true;
            this.exitBtn.Location = new System.Drawing.Point(516, 99);
            this.exitBtn.Name = "exitBtn";
            this.exitBtn.Size = new System.Drawing.Size(75, 23);
            this.exitBtn.TabIndex = 4;
            this.exitBtn.Text = "E&xit";
            this.exitBtn.UseVisualStyleBackColor = true;
            this.exitBtn.Click += new System.EventHandler(this.exitBtn_Click);
            // 
            // MsgLbl
            // 
            this.MsgLbl.AutoSize = true;
            this.MsgLbl.Location = new System.Drawing.Point(12, 508);
            this.MsgLbl.Name = "MsgLbl";
            this.MsgLbl.Size = new System.Drawing.Size(13, 13);
            this.MsgLbl.TabIndex = 5;
            this.MsgLbl.Text = "()";
            this.MsgLbl.UseMnemonic = false;
            // 
            // TakeBackBtn
            // 
            this.TakeBackBtn.AllowDrop = true;
            this.TakeBackBtn.Location = new System.Drawing.Point(516, 230);
            this.TakeBackBtn.Name = "TakeBackBtn";
            this.TakeBackBtn.Size = new System.Drawing.Size(75, 23);
            this.TakeBackBtn.TabIndex = 6;
            this.TakeBackBtn.Text = "&Take Back";
            this.TakeBackBtn.UseVisualStyleBackColor = true;
            this.TakeBackBtn.Visible = false;
            this.TakeBackBtn.Click += new System.EventHandler(this.TakeBackBtn_Click);
            // 
            // EndMoveBtn
            // 
            this.EndMoveBtn.AllowDrop = true;
            this.EndMoveBtn.Location = new System.Drawing.Point(516, 260);
            this.EndMoveBtn.Name = "EndMoveBtn";
            this.EndMoveBtn.Size = new System.Drawing.Size(75, 23);
            this.EndMoveBtn.TabIndex = 7;
            this.EndMoveBtn.Text = "&End Move";
            this.EndMoveBtn.UseVisualStyleBackColor = true;
            this.EndMoveBtn.Visible = false;
            this.EndMoveBtn.Click += new System.EventHandler(this.EndMoveBtn_Click);
            // 
            // checkerBoard
            // 
            this.checkerBoard.AllowDrop = true;
            this.checkerBoard.AutoMove = false;
            this.checkerBoard.BorderColor = System.Drawing.Color.Yellow;
            this.checkerBoard.CanDragTo = null;
            this.checkerBoard.DragDropped = null;
            this.checkerBoard.DragMoveComplete = null;
            this.checkerBoard.Location = new System.Drawing.Point(12, 12);
            this.checkerBoard.Name = "checkerBoard";
            this.checkerBoard.Size = new System.Drawing.Size(489, 489);
            this.checkerBoard.SquareColor1 = System.Drawing.Color.Red;
            this.checkerBoard.SquareColor2 = System.Drawing.Color.Black;
            this.checkerBoard.TabIndex = 0;
            // 
            // PauseBtn
            // 
            this.PauseBtn.Location = new System.Drawing.Point(516, 201);
            this.PauseBtn.Name = "PauseBtn";
            this.PauseBtn.Size = new System.Drawing.Size(75, 23);
            this.PauseBtn.TabIndex = 8;
            this.PauseBtn.Text = "&Pause";
            this.PauseBtn.UseVisualStyleBackColor = true;
            this.PauseBtn.Visible = false;
            this.PauseBtn.Click += new System.EventHandler(this.PauseBtn_Click);
            // 
            // MainWin
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(606, 530);
            this.Controls.Add(this.PauseBtn);
            this.Controls.Add(this.EndMoveBtn);
            this.Controls.Add(this.TakeBackBtn);
            this.Controls.Add(this.MsgLbl);
            this.Controls.Add(this.exitBtn);
            this.Controls.Add(this.helpBtn);
            this.Controls.Add(this.optionsBtn);
            this.Controls.Add(this.newBtn);
            this.Controls.Add(this.checkerBoard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainWin";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Checkers";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainWin_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainWin_FormClosed);
            this.Load += new System.EventHandler(this.MainWin_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CheckerBoard checkerBoard;
        private System.Windows.Forms.Button newBtn;
        private System.Windows.Forms.Button optionsBtn;
        private System.Windows.Forms.Button helpBtn;
        private System.Windows.Forms.Button exitBtn;
        private System.Windows.Forms.Label MsgLbl;
        private System.Windows.Forms.Button TakeBackBtn;
        private System.Windows.Forms.Button EndMoveBtn;
        private System.Windows.Forms.Button PauseBtn;



    }
}

