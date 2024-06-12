namespace SharpCraft.Client
{
    partial class CrashForm
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
            rtxtDetails = new System.Windows.Forms.RichTextBox();
            pbLogo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)pbLogo).BeginInit();
            SuspendLayout();
            // 
            // rtxtDetails
            // 
            rtxtDetails.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            rtxtDetails.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            rtxtDetails.Location = new System.Drawing.Point(80, 100);
            rtxtDetails.Name = "rtxtDetails";
            rtxtDetails.Size = new System.Drawing.Size(694, 280);
            rtxtDetails.TabIndex = 0;
            rtxtDetails.Text = "";
            // 
            // pbLogo
            // 
            pbLogo.Anchor = System.Windows.Forms.AnchorStyles.Top;
            pbLogo.BackColor = System.Drawing.Color.Transparent;
            pbLogo.Location = new System.Drawing.Point(299, 30);
            pbLogo.Name = "pbLogo";
            pbLogo.Size = new System.Drawing.Size(256, 336);
            pbLogo.TabIndex = 1;
            pbLogo.TabStop = false;
            // 
            // CrashForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.FromArgb(46, 52, 68);
            ClientSize = new System.Drawing.Size(854, 480);
            Controls.Add(rtxtDetails);
            Controls.Add(pbLogo);
            Name = "CrashForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "SharpCraft";
            FormClosing += CrashForm_FormClosing;
            ((System.ComponentModel.ISupportInitialize)pbLogo).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.RichTextBox rtxtDetails;
        private System.Windows.Forms.PictureBox pbLogo;
    }
}