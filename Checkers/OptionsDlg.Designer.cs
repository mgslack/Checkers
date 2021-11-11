namespace Checkers
{
    partial class OptionsDlg
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
            this.label1 = new System.Windows.Forms.Label();
            this.cbCColor1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbCColor2 = new System.Windows.Forms.ComboBox();
            this.pbChecker1 = new System.Windows.Forms.PictureBox();
            this.pbChecker2 = new System.Windows.Forms.PictureBox();
            this.cbAlt = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbSqCol1 = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbSqCol2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbBCol = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cbPlayer1 = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cbPlayer2 = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cbCrown = new System.Windows.Forms.ComboBox();
            this.pbCrown = new System.Windows.Forms.PictureBox();
            this.OkBtn = new System.Windows.Forms.Button();
            this.CancelBtn = new System.Windows.Forms.Button();
            this.DefaultBtn = new System.Windows.Forms.Button();
            this.cbMoveAllowed = new System.Windows.Forms.CheckBox();
            this.bsBord = new Checkers.BoardSquare();
            this.bsSq2 = new Checkers.BoardSquare();
            this.bsSq1 = new Checkers.BoardSquare();
            ((System.ComponentModel.ISupportInitialize)(this.pbChecker1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChecker2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCrown)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Checker Color &1:";
            // 
            // cbCColor1
            // 
            this.cbCColor1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCColor1.FormattingEnabled = true;
            this.cbCColor1.Location = new System.Drawing.Point(12, 25);
            this.cbCColor1.Name = "cbCColor1";
            this.cbCColor1.Size = new System.Drawing.Size(130, 21);
            this.cbCColor1.TabIndex = 1;
            this.cbCColor1.SelectedIndexChanged += new System.EventHandler(this.cbCColor1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(209, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Checker Color &2:";
            // 
            // cbCColor2
            // 
            this.cbCColor2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCColor2.FormattingEnabled = true;
            this.cbCColor2.Location = new System.Drawing.Point(209, 25);
            this.cbCColor2.Name = "cbCColor2";
            this.cbCColor2.Size = new System.Drawing.Size(130, 21);
            this.cbCColor2.TabIndex = 3;
            this.cbCColor2.SelectedIndexChanged += new System.EventHandler(this.cbCColor2_SelectedIndexChanged);
            // 
            // pbChecker1
            // 
            this.pbChecker1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbChecker1.Location = new System.Drawing.Point(150, 9);
            this.pbChecker1.Name = "pbChecker1";
            this.pbChecker1.Size = new System.Drawing.Size(51, 51);
            this.pbChecker1.TabIndex = 4;
            this.pbChecker1.TabStop = false;
            // 
            // pbChecker2
            // 
            this.pbChecker2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbChecker2.Location = new System.Drawing.Point(347, 9);
            this.pbChecker2.Name = "pbChecker2";
            this.pbChecker2.Size = new System.Drawing.Size(51, 51);
            this.pbChecker2.TabIndex = 5;
            this.pbChecker2.TabStop = false;
            // 
            // cbAlt
            // 
            this.cbAlt.AutoSize = true;
            this.cbAlt.Location = new System.Drawing.Point(12, 75);
            this.cbAlt.Name = "cbAlt";
            this.cbAlt.Size = new System.Drawing.Size(276, 17);
            this.cbAlt.TabIndex = 4;
            this.cbAlt.Text = "&Use Alternate Starting Position (used next new game)";
            this.cbAlt.UseVisualStyleBackColor = true;
            this.cbAlt.CheckedChanged += new System.EventHandler(this.cbAlt_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(154, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Checker Board &Square Color 1:";
            // 
            // cbSqCol1
            // 
            this.cbSqCol1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSqCol1.FormattingEnabled = true;
            this.cbSqCol1.Location = new System.Drawing.Point(12, 120);
            this.cbSqCol1.Name = "cbSqCol1";
            this.cbSqCol1.Size = new System.Drawing.Size(150, 21);
            this.cbSqCol1.TabIndex = 6;
            this.cbSqCol1.SelectedIndexChanged += new System.EventHandler(this.cbSqCol1_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(154, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Checker &Board Square Color 2:";
            // 
            // cbSqCol2
            // 
            this.cbSqCol2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSqCol2.FormattingEnabled = true;
            this.cbSqCol2.Location = new System.Drawing.Point(209, 120);
            this.cbSqCol2.Name = "cbSqCol2";
            this.cbSqCol2.Size = new System.Drawing.Size(150, 21);
            this.cbSqCol2.TabIndex = 9;
            this.cbSqCol2.SelectedIndexChanged += new System.EventHandler(this.cbSqCol2_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 156);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Checker Board Borde&r Color:";
            // 
            // cbBCol
            // 
            this.cbBCol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbBCol.FormattingEnabled = true;
            this.cbBCol.Location = new System.Drawing.Point(12, 172);
            this.cbBCol.Name = "cbBCol";
            this.cbBCol.Size = new System.Drawing.Size(150, 21);
            this.cbBCol.TabIndex = 12;
            this.cbBCol.SelectedIndexChanged += new System.EventHandler(this.cbBCol_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 211);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "&Player 1:";
            // 
            // cbPlayer1
            // 
            this.cbPlayer1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlayer1.FormattingEnabled = true;
            this.cbPlayer1.Location = new System.Drawing.Point(66, 208);
            this.cbPlayer1.Name = "cbPlayer1";
            this.cbPlayer1.Size = new System.Drawing.Size(100, 21);
            this.cbPlayer1.TabIndex = 15;
            this.cbPlayer1.SelectedIndexChanged += new System.EventHandler(this.cbPlayer1_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(209, 211);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "P&layer 2:";
            // 
            // cbPlayer2
            // 
            this.cbPlayer2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPlayer2.FormattingEnabled = true;
            this.cbPlayer2.Location = new System.Drawing.Point(263, 208);
            this.cbPlayer2.Name = "cbPlayer2";
            this.cbPlayer2.Size = new System.Drawing.Size(100, 21);
            this.cbPlayer2.TabIndex = 17;
            this.cbPlayer2.SelectedIndexChanged += new System.EventHandler(this.cbPlayer2_SelectedIndexChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 242);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(66, 13);
            this.label8.TabIndex = 18;
            this.label8.Text = "Cro&wn Style:";
            // 
            // cbCrown
            // 
            this.cbCrown.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCrown.FormattingEnabled = true;
            this.cbCrown.Location = new System.Drawing.Point(12, 258);
            this.cbCrown.Name = "cbCrown";
            this.cbCrown.Size = new System.Drawing.Size(100, 21);
            this.cbCrown.TabIndex = 19;
            this.cbCrown.SelectedIndexChanged += new System.EventHandler(this.cbCrown_SelectedIndexChanged);
            // 
            // pbCrown
            // 
            this.pbCrown.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbCrown.Location = new System.Drawing.Point(150, 242);
            this.pbCrown.Name = "pbCrown";
            this.pbCrown.Size = new System.Drawing.Size(51, 51);
            this.pbCrown.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbCrown.TabIndex = 22;
            this.pbCrown.TabStop = false;
            // 
            // OkBtn
            // 
            this.OkBtn.Location = new System.Drawing.Point(12, 315);
            this.OkBtn.Name = "OkBtn";
            this.OkBtn.Size = new System.Drawing.Size(75, 23);
            this.OkBtn.TabIndex = 20;
            this.OkBtn.Text = "&OK";
            this.OkBtn.UseVisualStyleBackColor = true;
            this.OkBtn.Click += new System.EventHandler(this.OkBtn_Click);
            // 
            // CancelBtn
            // 
            this.CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.CancelBtn.Location = new System.Drawing.Point(93, 315);
            this.CancelBtn.Name = "CancelBtn";
            this.CancelBtn.Size = new System.Drawing.Size(75, 23);
            this.CancelBtn.TabIndex = 21;
            this.CancelBtn.Text = "&Cancel";
            this.CancelBtn.UseVisualStyleBackColor = true;
            // 
            // DefaultBtn
            // 
            this.DefaultBtn.Location = new System.Drawing.Point(323, 315);
            this.DefaultBtn.Name = "DefaultBtn";
            this.DefaultBtn.Size = new System.Drawing.Size(75, 23);
            this.DefaultBtn.TabIndex = 23;
            this.DefaultBtn.Text = "&Defaults";
            this.DefaultBtn.UseVisualStyleBackColor = true;
            this.DefaultBtn.Click += new System.EventHandler(this.DefaultBtn_Click);
            // 
            // cbMoveAllowed
            // 
            this.cbMoveAllowed.AutoSize = true;
            this.cbMoveAllowed.Location = new System.Drawing.Point(212, 260);
            this.cbMoveAllowed.Name = "cbMoveAllowed";
            this.cbMoveAllowed.Size = new System.Drawing.Size(160, 17);
            this.cbMoveAllowed.TabIndex = 24;
            this.cbMoveAllowed.Text = "&Jump Allowed Afer Crowning";
            this.cbMoveAllowed.UseVisualStyleBackColor = true;
            this.cbMoveAllowed.CheckedChanged += new System.EventHandler(this.cbMoveAllowed_CheckedChanged);
            // 
            // bsBord
            // 
            this.bsBord.BoardLocation = 0;
            this.bsBord.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bsBord.Image = null;
            this.bsBord.Location = new System.Drawing.Point(172, 172);
            this.bsBord.Name = "bsBord";
            this.bsBord.Size = new System.Drawing.Size(20, 20);
            this.bsBord.TabIndex = 13;
            // 
            // bsSq2
            // 
            this.bsSq2.BoardLocation = 0;
            this.bsSq2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bsSq2.Image = null;
            this.bsSq2.Location = new System.Drawing.Point(367, 121);
            this.bsSq2.Name = "bsSq2";
            this.bsSq2.Size = new System.Drawing.Size(20, 20);
            this.bsSq2.TabIndex = 10;
            // 
            // bsSq1
            // 
            this.bsSq1.BoardLocation = 0;
            this.bsSq1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.bsSq1.Image = null;
            this.bsSq1.Location = new System.Drawing.Point(172, 121);
            this.bsSq1.Name = "bsSq1";
            this.bsSq1.Size = new System.Drawing.Size(20, 20);
            this.bsSq1.TabIndex = 7;
            // 
            // OptionsDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 351);
            this.Controls.Add(this.cbMoveAllowed);
            this.Controls.Add(this.DefaultBtn);
            this.Controls.Add(this.CancelBtn);
            this.Controls.Add(this.OkBtn);
            this.Controls.Add(this.pbCrown);
            this.Controls.Add(this.cbCrown);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbPlayer2);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cbPlayer1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bsBord);
            this.Controls.Add(this.cbBCol);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bsSq2);
            this.Controls.Add(this.cbSqCol2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bsSq1);
            this.Controls.Add(this.cbSqCol1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbAlt);
            this.Controls.Add(this.pbChecker2);
            this.Controls.Add(this.pbChecker1);
            this.Controls.Add(this.cbCColor2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cbCColor1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "OptionsDlg";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.Load += new System.EventHandler(this.OptionsDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbChecker1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbChecker2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbCrown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbCColor1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbCColor2;
        private System.Windows.Forms.PictureBox pbChecker1;
        private System.Windows.Forms.PictureBox pbChecker2;
        private System.Windows.Forms.CheckBox cbAlt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbSqCol1;
        private BoardSquare bsSq1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbSqCol2;
        private BoardSquare bsSq2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbBCol;
        private BoardSquare bsBord;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbPlayer1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbPlayer2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cbCrown;
        private System.Windows.Forms.PictureBox pbCrown;
        private System.Windows.Forms.Button OkBtn;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.Button DefaultBtn;
        private System.Windows.Forms.CheckBox cbMoveAllowed;
    }
}