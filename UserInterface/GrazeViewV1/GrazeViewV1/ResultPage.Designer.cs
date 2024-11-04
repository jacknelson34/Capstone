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
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 600);  // Adjusted size
            this.Text = "Result Page";

            // Create Model Generated Panel
            MLOutputPanel = new Panel();
            MLOutputPanel.Size = new Size(250, 200);  // Adjusted size to be smaller
            MLOutputPanel.BorderStyle = BorderStyle.FixedSingle;
            MLOutputPanel.Location = new Point((this.ClientSize.Width / 2), this.ClientSize.Height - MLOutputPanel.Height - 100);  // Positioned next to the UserOutputPanel
            MLOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;  // Anchor to bottom-right
            this.Controls.Add(MLOutputPanel);

            Label modelTitle = CreatePanelTitle("Model Generated Data:");
            MLOutputPanel.Controls.Add(modelTitle);

            // Add ML data controls to Model Generated panel
            CreateMLDataControls(MLOutputPanel);

            // Create User Provided Data Panel
            UserOutputPanel = new Panel();
            UserOutputPanel.Size = new Size(250, 200);  // Adjusted size to be smaller
            UserOutputPanel.BorderStyle = BorderStyle.FixedSingle;
            UserOutputPanel.Location = new Point((this.ClientSize.Width / 2) - 250, this.ClientSize.Height - UserOutputPanel.Height - 100);  // Positioned to the left of MLOutputPanel
            UserOutputPanel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;  // Anchor to bottom-left
            this.Controls.Add(UserOutputPanel);

            Label userTitle = CreatePanelTitle("User Provided Data:");
            UserOutputPanel.Controls.Add(userTitle);

            // Add user data controls to User Provided Data panel
            CreateUserProvidedDataControls(UserOutputPanel);

            // Add Exit button
            Button exitButton = new Button();
            exitButton.Text = "Exit";
            exitButton.Size = new Size(80, 30);
            exitButton.Location = new Point((this.ClientSize.Width - exitButton.Width) / 2, this.ClientSize.Height - exitButton.Height - 20);
            exitButton.Anchor = AnchorStyles.Bottom;
            exitButton.Click += returnButton_Click;
            this.Controls.Add(exitButton);
        }

        // Helper method to create a title label for each panel
        private Label CreatePanelTitle(string titleText)
        {
            Label title = new Label();
            title.Text = titleText;
            title.Font = new Font("Times New Roman", 12, FontStyle.Bold);
            title.Location = new Point(10, 10);
            title.AutoSize = true;
            return title;
        }

        // Helper method to create ML Data controls using seamless textboxes
        private void CreateMLDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Starting below the title
            int labelWidth = 120;
            int textBoxWidth = 150;
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular);

            // Create and add controls for each ML data field using seamless textboxes
            qufuLabel = CreateLabel("Qufu (%):", new Point(10, currentYPosition));
            qufuTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(qufuLabel);
            panel.Controls.Add(qufuTextBox);

            currentYPosition += 30;
            erciLabel = CreateLabel("Erci (%):", new Point(10, currentYPosition));
            erciTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(erciLabel);
            panel.Controls.Add(erciTextBox);

            currentYPosition += 30;
            naleLabel = CreateLabel("Nale (%):", new Point(10, currentYPosition));
            naleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(naleLabel);
            panel.Controls.Add(naleTextBox);

            currentYPosition += 30;
            bubbleLabel = CreateLabel("Bubbles (%):", new Point(10, currentYPosition));
            bubbleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(bubbleLabel);
            panel.Controls.Add(bubbleTextBox);

            currentYPosition += 30;
            stemLabel = CreateLabel("Stem (%):", new Point(10, currentYPosition));
            stemTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(stemLabel);
            panel.Controls.Add(stemTextBox);
        }

        // Helper method to create User Provided Data controls using seamless textboxes
        private void CreateUserProvidedDataControls(Panel panel)
        {
            int currentYPosition = 40;  // Starting below the title
            int labelWidth = 120;
            int textBoxWidth = 150;
            Font commonFont = new Font("Times New Roman", 10, FontStyle.Regular);

            // Create and add controls for each user data field using seamless textboxes
            uploadNameLabel = CreateLabel("Upload Name:", new Point(10, currentYPosition));
            uploadNameTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(uploadNameLabel);
            panel.Controls.Add(uploadNameTextBox);

            currentYPosition += 30;
            dateUploadedLabel = CreateLabel("Date Uploaded:", new Point(10, currentYPosition));
            dateUploadedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(dateUploadedLabel);
            panel.Controls.Add(dateUploadedTextBox);

            currentYPosition += 30;
            dateOfSampleLabel = CreateLabel("Date of Sample:", new Point(10, currentYPosition));
            dateOfSampleTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(dateOfSampleLabel);
            panel.Controls.Add(dateOfSampleTextBox);

            currentYPosition += 30;
            sampleLocationLabel = CreateLabel("Sample Location:", new Point(10, currentYPosition));
            sampleLocationTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(sampleLocationLabel);
            panel.Controls.Add(sampleLocationTextBox);

            currentYPosition += 30;
            sheepBreedLabel = CreateLabel("Sheep Breed:", new Point(10, currentYPosition));
            sheepBreedTextBox = CreateSeamlessTextBox(commonFont, new Point(labelWidth, currentYPosition), textBoxWidth);
            panel.Controls.Add(sheepBreedLabel);
            panel.Controls.Add(sheepBreedTextBox);
        }

        // Helper method to create seamless textboxes
        private TextBox CreateSeamlessTextBox(Font font, Point location, int width)
        {
            TextBox textBox = new TextBox();
            textBox.Location = location;
            textBox.TabStop = false;
            textBox.Cursor = Cursors.Default;
            textBox.Size = new Size(width, 20);
            textBox.ReadOnly = true;  // Make the textboxes read-only
            textBox.BorderStyle = BorderStyle.None;  // Remove the border for a seamless look
            textBox.BackColor = this.BackColor;  // Match the background color for a seamless effect
            textBox.Font = font;  // Set the common font
            return textBox;
        }

        // Helper method to create labels
        private Label CreateLabel(string labelText, Point location)
        {
            Label label = new Label();
            label.Text = labelText;
            label.Location = location;
            label.AutoSize = true;
            return label;
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
        private Label stemLabel;
        private TextBox stemTextBox;

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

        // if there is any easier way of initializing these please tell me lol
    }
}