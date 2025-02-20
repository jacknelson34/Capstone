namespace GrazeViewV1
{
    partial class ResultPage
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
            helpButton = new PictureBox();
            resultsPagePanel = new Panel();
            //buttonPanel = new Panel();
            MLOutputPanel = new Panel();
            UserOutputPanel = new Panel();
            exitButton = new roundButton();
            dataViewButton = new roundButton();
            returnToUploadButton = new roundButton();
            resultsPagePanel.SuspendLayout();
            SuspendLayout();
            resultsPagePanel.Visible = false;
            ClientSize = new Size(1280, 918);
            MinimumSize = new Size(1280, 918);
            //
            // buttonPanel
            //
            /*buttonPanel.Dock = DockStyle.Bottom;
            buttonPanel.Location = new Point(0, 1159);
            buttonPanel.Padding = new Padding(5);
            buttonPanel.Size = new Size(this.ClientSize.Width, 120);
            buttonPanel.BackColor = Color.LightBlue;
            buttonPanel.BorderStyle = BorderStyle.FixedSingle;
            Controls.Add(buttonPanel);*/
            // 
            // resultsPagePanel
            // 
            resultsPagePanel.Anchor = AnchorStyles.None;
            resultsPagePanel.BorderStyle = BorderStyle.Fixed3D;
            resultsPagePanel.BackColor = Color.White;
            resultsPagePanel.Name = "resultsPagePanel";
            resultsPagePanel.Size = new Size((int)(this.ClientSize.Width * 0.637), (int)(this.ClientSize.Height * .708));
            resultsPagePanel.Location = new Point((this.ClientSize.Width / 2)-(resultsPagePanel.Width /2), (this.ClientSize.Height / 2)-(resultsPagePanel.Height / 2) - 100);
            resultsPagePanel.TabIndex = 1;
            // 
            // MLOutputPanel
            // 
            MLOutputPanel.BorderStyle = BorderStyle.None;
            MLOutputPanel.BackColor = Color.Transparent;
            MLOutputPanel.Location = new Point(500, 400);
            MLOutputPanel.Name = "MLOutputPanel";
            MLOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            MLOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.375), 200);
            MLOutputPanel.TabIndex = 0;
            // 
            // UserOutputPanel
            // 
            UserOutputPanel.BorderStyle = BorderStyle.None;
            UserOutputPanel.BackColor = Color.Transparent;
            UserOutputPanel.Location = new Point(0, 400);
            UserOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            UserOutputPanel.Name = "UserOutputPanel";
            UserOutputPanel.Size = new Size((int)(resultsPagePanel.Width * 0.625), 200);
            UserOutputPanel.TabIndex = 1;
            // 
            // exitButton
            //  
            exitButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            exitButton.borderRadius = 20;
            exitButton.FlatStyle = FlatStyle.Flat;
            exitButton.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            exitButton.Name = "exitButton";
            exitButton.Size = new Size(225, 60);
            exitButton.Location = new Point(ClientSize.Width - 250, ClientSize.Height - 190);
            exitButton.TabIndex = 0;
            exitButton.Text = "Return to Home";
            exitButton.UseVisualStyleBackColor = false;
            exitButton.Click += returnButton_Click;
            Controls.Add(exitButton);
            // 
            // returnToUploadButton
            // 
            returnToUploadButton.FlatAppearance.BorderSize = 0;
            returnToUploadButton.borderRadius = 20;
            returnToUploadButton.FlatStyle = FlatStyle.Flat;
            returnToUploadButton.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            returnToUploadButton.Anchor = AnchorStyles.Bottom;
            returnToUploadButton.Name = "returnToUploadButton";
            returnToUploadButton.Size = new Size(300, 75);
            returnToUploadButton.Location = new Point((this.ClientSize.Width / 2) - 340, this.ClientSize.Height - 200);
            returnToUploadButton.TabIndex = 2;
            returnToUploadButton.Text = "Upload New Data";
            returnToUploadButton.UseVisualStyleBackColor = false;
            returnToUploadButton.Click += returnToUploadButton_Click;
            Controls.Add(returnToUploadButton);
            //
            // dataViewButton
            //
            //buttonPanel.Controls.Add(dataViewButton);
            dataViewButton.FlatAppearance.BorderSize = 0;
            dataViewButton.borderRadius = 20;
            dataViewButton.FlatStyle = FlatStyle.Flat;
            dataViewButton.Font = new Font("Times New Roman", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataViewButton.Anchor = AnchorStyles.Bottom;
            dataViewButton.Name = "dataViewButton";
            dataViewButton.Size = new Size(300, 75);
            dataViewButton.Location = new Point((this.ClientSize.Width / 2) + 40, this.ClientSize.Height - 200);
            dataViewButton.TabIndex = 2;
            dataViewButton.Text = "View in Library";
            dataViewButton.UseVisualStyleBackColor = false;
            dataViewButton.Click += dataViewButton_Click;
            Controls.Add(dataViewButton);
            // 
            // helpButton
            // 
            helpButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            helpButton.Cursor = Cursors.Hand;
            helpButton.Image = Properties.Resources.Help_Icon;
            helpButton.BackColor = Color.Transparent;
            helpButton.Location = new Point(1218, 12);
            helpButton.Name = "helpButton";
            helpButton.Size = new Size(50, 50);
            helpButton.SizeMode = PictureBoxSizeMode.StretchImage;
            helpButton.TabIndex = 3;
            helpButton.TabStop = false;
            helpButton.Click += helpButton_Click;
            Controls.Add(helpButton);
            // 
            // ResultPage
            // 
            AutoScaleDimensions = new SizeF(200F, 200F);
            FormBorderStyle = FormBorderStyle.Sizable;
            AutoScaleMode = AutoScaleMode.Dpi;
            StartPosition = FormStartPosition.Manual;
            BackColor = Color.FromArgb(116, 231, 247);
            BackgroundImage = Properties.Resources.BackgroundImage12;
            BackgroundImageLayout = ImageLayout.Stretch;
            CreateUserProvidedDataControls(UserOutputPanel);
            CreateMLDataControls(MLOutputPanel);
            resultsPagePanel.Controls.Add(UserOutputPanel);
            resultsPagePanel.Controls.Add(MLOutputPanel);
            Controls.Add(resultsPagePanel);
            MinimumSize = new Size(1280, 918);
            Name = "ResultPage";
            Text = "Result Page";
            resultsPagePanel.ResumeLayout(false);
            resultsPagePanel.Visible = true;
            ResumeLayout(false);
        }

        // Helper method to create a title label for each panel
        private Label CreatePanelTitle(string titleText)
        {
            Label title = new Label(); // Create a new label instance
            title.Text = titleText; // Set the text of the label to the provided title
            title.Font = new Font("Times New Roman", 10, FontStyle.Bold); // Set the font style and size for the label
            title.Location = new Point(10, 10); // Position the label at coordinates (10, 10)
            title.Anchor = AnchorStyles.Top | AnchorStyles.Left; // Anchor the label to the top-left of its container
            title.AutoSize = true; // Automatically adjust the size of the label to fit the text
            return title; // Return the configured label
        }

        // Helper method to create ML Data controls using seamless textboxes
        private void CreateMLDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Define the starting vertical position for controls
            int labelWidth = 120; // Define the width for labels
            int textBoxWidth = 150; // Define the width for textboxes
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular); // Define the common font for labels and textboxes

            // Create panel title
            Label title = CreatePanelTitle("Model Generated Data:"); // Create a title label for the panel
            panel.Controls.Add(title); // Add the title label to the panel

            // Create and add controls for each ML data field using seamless textboxes
            qufuLabel = CreateLabel(commonFont, "Qufu (%):", new Point(10, currentYPosition)); // Create a label for "Qufu"
            qufuTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 80, currentYPosition), textBoxWidth); // Create a seamless textbox for "Qufu"
            panel.Controls.Add(qufuLabel); // Add the label to the panel
            panel.Controls.Add(qufuTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            erciLabel = CreateLabel(commonFont, "Erci (%):", new Point(10, currentYPosition)); // Create a label for "Erci"
            erciTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 80, currentYPosition), textBoxWidth); // Create a seamless textbox for "Erci"
            panel.Controls.Add(erciLabel); // Add the label to the panel
            panel.Controls.Add(erciTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            naleLabel = CreateLabel(commonFont, "Nale (%):", new Point(10, currentYPosition)); // Create a label for "Nale"
            naleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 80, currentYPosition), textBoxWidth); // Create a seamless textbox for "Nale"
            panel.Controls.Add(naleLabel); // Add the label to the panel
            panel.Controls.Add(naleTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            bubbleLabel = CreateLabel(commonFont, "Bubbles (%):", new Point(10, currentYPosition)); // Create a label for "Bubbles"
            bubbleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 80, currentYPosition), textBoxWidth); // Create a seamless textbox for "Bubbles"
            panel.Controls.Add(bubbleLabel); // Add the label to the panel
            panel.Controls.Add(bubbleTextBox); // Add the textbox to the panel

            /*currentYPosition += 30; // Increment vertical position for the next control
            stemLabel = CreateLabel(commonFont, "Stem (%):", new Point(10, currentYPosition)); // Create a label for "Stem"
            stemTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 80, currentYPosition), textBoxWidth); // Create a seamless textbox for "Stem"
            panel.Controls.Add(stemLabel); // Add the label to the panel
            panel.Controls.Add(stemTextBox); // Add the textbox to the panel*/
        }

        // Helper method to create User Provided Data controls using seamless textboxes
        private void CreateUserProvidedDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Define the starting vertical position for controls
            int labelWidth = 120; // Define the width for labels
            int textBoxWidth = 300; // Define the width for textboxes
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular); // Define the common font for labels and textboxes

            // Create panel title
            Label title = CreatePanelTitle("User Provided Data:"); // Create a title label for the panel
            panel.Controls.Add(title); // Add the title label to the panel

            // Create and add controls for each user data field using seamless textboxes
            uploadNameLabel = CreateLabel(commonFont, "Upload Name:", new Point(10, currentYPosition)); // Create a label for "Upload Name"
            uploadNameTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 100, currentYPosition), textBoxWidth); // Create a seamless textbox for "Upload Name"
            panel.Controls.Add(uploadNameLabel); // Add the label to the panel
            panel.Controls.Add(uploadNameTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            dateUploadedLabel = CreateLabel(commonFont, "Date Uploaded:", new Point(10, currentYPosition)); // Create a label for "Date Uploaded"
            dateUploadedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 100, currentYPosition), textBoxWidth); // Create a seamless textbox for "Date Uploaded"
            panel.Controls.Add(dateUploadedLabel); // Add the label to the panel
            panel.Controls.Add(dateUploadedTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            dateOfSampleLabel = CreateLabel(commonFont, "Date of Sample:", new Point(10, currentYPosition)); // Create a label for "Date of Sample"
            dateOfSampleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 100, currentYPosition), textBoxWidth); // Create a seamless textbox for "Date of Sample"
            panel.Controls.Add(dateOfSampleLabel); // Add the label to the panel
            panel.Controls.Add(dateOfSampleTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            sampleLocationLabel = CreateLabel(commonFont, "Sample Location:", new Point(10, currentYPosition)); // Create a label for "Sample Location"
            sampleLocationTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 110, currentYPosition), textBoxWidth); // Create a seamless textbox for "Sample Location"
            panel.Controls.Add(sampleLocationLabel); // Add the label to the panel
            panel.Controls.Add(sampleLocationTextBox); // Add the textbox to the panel

            currentYPosition += 30; // Increment vertical position for the next control
            sheepBreedLabel = CreateLabel(commonFont, "Sheep Breed:", new Point(10, currentYPosition)); // Create a label for "Sheep Breed"
            sheepBreedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth + 100, currentYPosition), textBoxWidth); // Create a seamless textbox for "Sheep Breed"
            panel.Controls.Add(sheepBreedLabel); // Add the label to the panel
            panel.Controls.Add(sheepBreedTextBox); // Add the textbox to the panel
        }

        // Helper method to create seamless textboxes
        private TextBox CreateSeamlessTextBox(Font font, Point location, int width)
        {
            TextBox textBox = new TextBox(); // Create a new textbox instance
            textBox.Location = location; // Set the location of the textbox
            textBox.TabStop = false; // Disable tab stop for the textbox
            textBox.Cursor = Cursors.Default; // Set the default cursor for the textbox
            textBox.Size = new Size(width, 20); // Set the size of the textbox
            textBox.ReadOnly = true;  // Make the textbox read-only
            textBox.BorderStyle = BorderStyle.None;  // Remove the border for a seamless look
            textBox.BackColor = Color.White;  // Set the background color to white for consistency
            textBox.Font = font;  // Apply the specified font to the textbox
            return textBox; // Return the configured textbox
        }

        // Helper method to create labels
        private Label CreateLabel(Font font, string labelText, Point location)
        {
            Label label = new Label(); // Create a new label instance
            label.Font = font; // Apply the specified font to the label
            label.Text = labelText; // Set the text of the label
            label.Location = location; // Set the location of the label
            label.AutoSize = true; // Automatically adjust the size of the label to fit the text
            return label; // Return the configured label
        }


        #endregion

        // Panel 1 - ML Data
        private Panel MLOutputPanel;
        private Label qufuLabel;
        private TextBox qufuTextBox;
        private Label erciLabel;
        private TextBox erciTextBox;
        private Label bubbleLabel;
        private TextBox bubbleTextBox;
        private Label naleLabel;
        private TextBox naleTextBox;
        //private Label stemLabel;
        //private TextBox stemTextBox;

        // Panel 2 - User provided Data
        private Panel UserOutputPanel;
        private Label uploadNameLabel;
        private TextBox uploadNameTextBox;
        private Label dateUploadedLabel;
        private TextBox dateUploadedTextBox;
        private Label dateOfSampleLabel;
        private TextBox dateOfSampleTextBox;
        private Label sampleLocationLabel;
        private TextBox sampleLocationTextBox;
        private Label sheepBreedLabel;
        private TextBox sheepBreedTextBox;

        // Panel 3 - For resizing purposes
        private Panel resultsPagePanel;
        //private Panel buttonPanel;
        private roundButton exitButton;
        private roundButton returnToUploadButton;
        private roundButton dataViewButton;
        private PictureBox helpButton;

        // if there is any easier way of initializing these please tell me lol
    }
}