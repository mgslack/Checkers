namespace Checkers
{
    partial class CheckerBoard
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // CheckerBoard
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.DoubleBuffered = true;
            this.Name = "CheckerBoard";
            this.Size = new System.Drawing.Size(489, 489);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.CheckerBoard_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.CheckerBoard_DragEnter);
            this.GiveFeedback += new System.Windows.Forms.GiveFeedbackEventHandler(this.CheckerBoard_GiveFeedback);
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CheckerBoard_MouseDown);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
