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
            ClientSize = new Size(1249, 796);
            Name = "DataLibraryExpandedView";
            Text = "DataLibraryExpandedView";
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
            this.controlPanel.BorderStyle = BorderStyle.FixedSingle;
            this.Controls.Add(this.controlPanel);
            // 
            // exitButton
            // 
            exitButton = new Button();
            this.exitButton.Text = "Exit";
            this.exitButton.Size = new Size(100, 30);
            this.exitButton.Location = new Point(20, 15); // Adjust the location as needed
            this.exitButton.Click += new EventHandler(this.exitButton_Click);
            this.controlPanel.Controls.Add(this.exitButton);

            // 
            // printButton
            // 
            printButton = new Button();
            this.printButton.Text = "Print";
            this.printButton.Size = new Size(100, 30);
            this.printButton.Location = new Point(this.controlPanel.Width - 120, 15); // Adjust location to be on the right side
            this.printButton.Anchor = AnchorStyles.Right; // Ensure it stays on the right when resizing
            this.printButton.Click += new EventHandler(this.printButton_Click); // Placeholder, currently no function
            this.controlPanel.Controls.Add(this.printButton);



        }

        #endregion

        private Panel controlPanel;
        private Button exitButton;
        private Button printButton;
        private FlowLayoutPanel flowLayoutPanel;
    }
}