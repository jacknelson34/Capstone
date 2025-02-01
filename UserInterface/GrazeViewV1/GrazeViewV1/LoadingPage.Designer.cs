namespace GrazeViewV1
{
    partial class LoadingPage
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
            SuspendLayout();
            // 
            // LoadingPage
            // 
            StartPosition = FormStartPosition.CenterScreen;
            BackgroundImage = Properties.Resources.BackgroundImage12;
            BackgroundImageLayout = ImageLayout.Stretch;
            AutoScaleDimensions = new SizeF(7F, 15F);
            BackColor = Color.FromArgb(116, 231, 247);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            MaximumSize = new Size(800, 450);
            MinimumSize = new Size(800, 450);
            Name = "LoadingPage";
            Text = "LoadingPage";
            ResumeLayout(false);
        }

        #endregion
    }
}