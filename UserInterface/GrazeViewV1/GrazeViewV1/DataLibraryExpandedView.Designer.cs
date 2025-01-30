namespace GrazeViewV1
{
    partial class DataLibraryExpandedView
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
            // DataLibraryExpandedView
            // 
            AutoScaleDimensions = new SizeF(13F, 32F);
            AutoScaleMode = AutoScaleMode.Font;
            StartPosition = FormStartPosition.Manual;
            MinimumSize = new Size(1280, 918);
            Name = "DataLibraryExpandedView";
            Text = "DataLibraryExpandedView";
            BackColor = Color.White;
            ResumeLayout(false);
            //
            // Flow Layout Panel
            //
            flowLayoutPanel = new FlowLayoutPanel();
            this.flowLayoutPanel.Dock = DockStyle.Fill;
            this.flowLayoutPanel.AutoScroll = true;
            this.flowLayoutPanel.FlowDirection = FlowDirection.TopDown;
            this.flowLayoutPanel.WrapContents = false;
            this.Controls.Add(flowLayoutPanel);
            //
            // Control Panel at the bottom of the page
            //
            controlPanel = new Panel();
            this.controlPanel.Dock = DockStyle.Bottom;
            this.controlPanel.Height = 60;
            this.controlPanel.BackColor = Color.LightBlue;
            this.controlPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.controlPanel);
            // 
            // exitButton
            // 
            exitButton = new roundButton();
            this.exitButton.Text = "Return to Data Library";
            this.exitButton.borderRadius = 20;
            this.exitButton.Size = new Size(200, 40);
            this.exitButton.Location = new Point(20, 7); // Adjust the location as needed
            this.exitButton.Anchor = AnchorStyles.Left;
            this.exitButton.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.exitButton.Click += new EventHandler(this.exitButton_Click);
            this.controlPanel.Controls.Add(this.exitButton);

            // 
            // printButton
            // 
            printButton = new roundButton();
            this.printButton.Text = "Print Page";
            this.printButton.borderRadius = 20;
            this.printButton.Size = new Size(100, 40);
            this.printButton.Location = new Point(controlPanel.Width - 120, 7); // Adjust location to be on the right side
            this.printButton.Anchor = AnchorStyles.Right; // Ensure it stays on the right when resizing
            this.printButton.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.printButton.Click += new EventHandler(this.printButton_Click); 
            this.controlPanel.Controls.Add(this.printButton);

            MinimumSize = new Size(1050, 550);


        }

        #endregion

        private Panel controlPanel;
        private roundButton exitButton;
        private roundButton printButton;
        private FlowLayoutPanel flowLayoutPanel;
    }
}