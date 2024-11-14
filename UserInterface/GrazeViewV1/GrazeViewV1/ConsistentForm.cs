using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Configuration;
using System.ComponentModel;
using System.CodeDom;
using System.Drawing.Design;
using System.Net.Http.Headers;

namespace GrazeViewV1
{
    /*--------------------Page Sizing-------------------------------------*/

    // Class to create sizing consistencies
    /*public class ConsistentForm : Form
    {
        // Static variables to store the size and location of the last form
        private static Size? previousFormSize = null;
        private static Point? previousFormLocation = null;

        public ConsistentForm()
        {
            // If a previous form size is stored, apply it
            if (previousFormSize != null)
            {
                this.Size = previousFormSize.Value;
            }

            // If a previous form location is stored, apply it
            if (previousFormLocation != null)
            {
                this.StartPosition = FormStartPosition.Manual;  // Use manual start position
                this.Location = previousFormLocation.Value;     // Apply the previous location
            }
            else
            {
                // Only for the very first form, center the screen
                this.StartPosition = FormStartPosition.CenterScreen;
            }

            // Attach an event to store the form size and location when the form is closing
            this.FormClosing += (s, e) =>
            {
                // Store the size and location of the current form before closing it
                previousFormSize = this.Size;
                previousFormLocation = this.Location;
            };
        }

        
    }*/

    public static class ConsistentForm // Public class to store form resize for consistency
    {
        public static Size FormSize { get; set; } = new Size(1280, 918); // Default of 1280, 918
        public static Point FormLocation { get; set; } = new Point(100, 100); // Default of 100, 100 location
        public static bool IsFullScreen { get; set; } = false;  // Default bool set to not full screen

    }


    /*---------------------Custom Controls--------------------------------*/

    // Public Class to create consistent button design with rounded edges - WORKS
    public class roundButton : Button
    {
        // Design Fields
        private int borderSize = 0;
        public int borderRadius = 0;
        private Color borderColor = Color.Black;

        // Constructors
        public roundButton()
        {
            this.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.Size = new Size(150, 40);
            this.BackColor = Color.LightGreen;
            this.ForeColor = Color.Black;
        }

        // Methods
        private GraphicsPath GetfigurePath(RectangleF rect, float radius)
        {
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, radius, radius, 180, 90);
            path.AddArc(rect.Width - radius, rect.Y, radius, radius, 270, 90);
            path.AddArc(rect.Width - radius, rect.Height - radius, radius, radius, 0, 90);
            path.AddArc(rect.X, rect.Height - radius, radius, radius, 90, 90);
            path.CloseFigure();

            return path;

        }

        protected override void OnPaint(PaintEventArgs pevent)
        {
            base.OnPaint(pevent);
            pevent.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(1, 1, this.Width - 0.8F, this.Height - 1);

            if (borderRadius > 2) // Rounded Button
            {
                using (GraphicsPath pathSurface = GetfigurePath(rectSurface, borderRadius))
                using (GraphicsPath pathBorder = GetfigurePath(rectBorder, borderRadius - 1F))
                using (Pen penSurface = new Pen(this.Parent.BackColor, 2))
                using (Pen penBorder = new Pen(borderColor, borderSize))
                {
                    penBorder.Alignment = PenAlignment.Inset;
                    this.Region = new Region(pathSurface);
                    // Draw surface of the border for HD Result
                    pevent.Graphics.DrawPath(penSurface, pathSurface);
                    // Button Border
                    if (borderSize >= 1)
                    {
                        pevent.Graphics.DrawPath(penBorder, pathBorder);
                    }

                }
            }
            else  // Normal Button
            {
                // Button Surface
                this.Region = new Region(rectSurface);
                // Button Border
                if (borderSize >= 1)
                {
                    using (Pen penBorder = new Pen(borderColor, borderSize))
                    {
                        penBorder.Alignment = PenAlignment.Inset;
                        pevent.Graphics.DrawRectangle(penBorder, 0, 0, this.Width - 1, this.Height - 1);
                    }
                }

            }
        }


        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            this.Parent.BackColorChanged += new EventHandler(Container_BackColorChanged);
        }

        private void Container_BackColorChanged(object sender, EventArgs e) 
        {
            if (this.DesignMode)
            {
                this.Invalidate();
            }
        }


    }

    public class TransparentLabel : Label
    {
        protected override void OnPaint(PaintEventArgs e)
        {
            // Clear the background
            e.Graphics.Clear(this.BackColor);

            // Call the base OnPaint to render the text
            base.OnPaint(e);
        }
    }

    // Public Class to create custom comboBox
    [DefaultEvent("OnSelectedIndexChanged")]
    class customBox : ComboBox
    {
        // Fields
        private Color backColor = Color.WhiteSmoke;
        private Color iconColor = Color.LightGreen;
        private Color listBackColor = Color.LightGreen;
        private Color listTextColor = Color.Black;
        private Color borderColor = Color.LightGreen;
        private int borderSize = 1;

        // Properties
        // -> Appearance
        [Category("customBox - Appearance")]
        public new Color BackColor
        {
            get { return backColor; }
            set
            {
                backColor = value;
                lblText.BackColor = backColor;
                btnIcon.BackColor = backColor;
            }
        }

        [Category("customBox - Appearance")]
        public Color IconColor
        {
            get { return iconColor; }
            set
            {
                iconColor = value;
                btnIcon.Invalidate(); // Redraw icon
            }
        }

        [Category("customBox - Appearance")]
        public Color ListBackColor
        {
            get { return listBackColor; }
            set
            {
                listBackColor = value;
                cmbList.BackColor = listBackColor;
            }
        }

        [Category("customBox - Appearance")]
        public Color ListTextColor
        {
            get { return listTextColor; }
            set
            {
                listTextColor = value;
                cmbList.ForeColor = listTextColor;
            }
        }

        [Category("customBox - Appearance")]
        public Color BorderColor
        {
            get { return borderColor; }
            set
            {
                borderColor = value;
                base.BackColor = borderColor; // Border Color
            }
        }

        [Category("customBox - Appearance")]
        public int BorderSize
        {
            get { return borderSize; }
            set
            {
                borderSize = value;
                this.Padding = new Padding(borderSize); // Border Size 
                AdjustComboBoxDimensions();
            }
        }

        [Category("customBox - Appearance")]
        public override Color ForeColor
        {
            get { return base.ForeColor; }
            set
            {
                base.ForeColor = value;
                lblText.ForeColor = value;
            }
        }

        [Category("customBox - Appearance")]
        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                lblText.Font = value;
                cmbList.Font = value; // Optional
            }
        }

        [Category("customBox - Appearance")]
        public string Texts
        {
            get { return lblText.Text; }
            set { lblText.Text = value; }
        }

        [Category("customBox - Appearance")]
        public ComboBoxStyle DropDownStyle
        {
            get { return cmbList.DropDownStyle; }
            set
            {
                if (cmbList.DropDownStyle != ComboBoxStyle.Simple)
                    cmbList.DropDownStyle = value;
            }
        }

        // Properties
        // -> Data
        [Category("customBox - Data")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design", typeof(UITypeEditor))]
        [Localizable(true)]
        [MergableProperty(false)]
        public ComboBox.ObjectCollection Items
        {
            get { return cmbList.Items; }
        }

        [Category("customBox - Data")]
        [AttributeProvider(typeof(IListSource))]
        [DefaultValue(null)]
        public object DataSource
        {
            get { return cmbList.DataSource; }
            set { cmbList.DataSource = value; }
        }

        [Category("customBox - Data")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public AutoCompleteStringCollection AutoCompleteCustomSource
        {
            get { return cmbList.AutoCompleteCustomSource; }
            set { cmbList.AutoCompleteCustomSource = value; }
        }

        [Category("customBox - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteSource.None)]
        public AutoCompleteSource AutoCompleteSource
        {
            get { return cmbList.AutoCompleteSource; }
            set { cmbList.AutoCompleteSource = value; }
        }

        [Category("customBox - Data")]
        [Browsable(true)]
        [DefaultValue(AutoCompleteMode.None)]
        public AutoCompleteMode AutoCompleteMode
        {
            get { return cmbList.AutoCompleteMode; }
            set { cmbList.AutoCompleteMode = value; }
        }

        [Category("customBox - Data")]
        [Bindable(true)]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public object SelectedItem
        {
            get { return cmbList.SelectedItem; }
            set { cmbList.SelectedItem = value; }
        }

        [Category("customBox - Data")]
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int SelectedIndex
        {
            get { return cmbList.SelectedIndex; }
            set { cmbList.SelectedIndex = value; }
        }

        [Category("customBox - Data")]
        [DefaultValue("")]
        public string DisplayMember
        {
            get { return cmbList.DisplayMember; }
            set { cmbList.DisplayMember = value; }
        }

        [Category("customBox - Data")]
        [DefaultValue("")]
        public string ValueMember
        {
            get { return cmbList.ValueMember; }
            set { cmbList.ValueMember = value; }
        }

        // Fields for components
        private ComboBox cmbList;
        private TransparentLabel lblText;
        private Button btnIcon;

        // Events
        public event EventHandler OnSelectedIndexChanged;

        // Constructor
        public customBox()
        {
            this.DoubleBuffered = true;

            cmbList = new ComboBox();
            lblText = new TransparentLabel();
            btnIcon = new Button();
            this.SuspendLayout();

            // ComboBox setup
            cmbList.BackColor = listBackColor;
            cmbList.Font = new Font(this.Font.Name, 10F);
            cmbList.ForeColor = listTextColor;
            cmbList.SelectedIndexChanged += ComboBox_SelectedIndexChanged;
            cmbList.TextChanged += ComboBox_TextChanged;

            // Button setup
            btnIcon.Dock = DockStyle.Right;
            btnIcon.FlatStyle = FlatStyle.Flat;
            btnIcon.FlatAppearance.BorderSize = 0;
            btnIcon.BackColor = backColor;
            btnIcon.Size = new Size(30, 30);
            btnIcon.Cursor = Cursors.Hand;
            btnIcon.Click += Icon_Click;
            btnIcon.Paint += Icon_Paint;

            // Label setup
            lblText.Dock = DockStyle.Fill;
            lblText.AutoSize = false;
            lblText.BackColor = listBackColor;
            lblText.TextAlign = ContentAlignment.MiddleLeft;
            lblText.Padding = new Padding(8, 0, btnIcon.Width + 5, 0);
            lblText.Font = new Font(this.Font.Name, 10F);
            lblText.Click += Surface_Click;
            lblText.MouseEnter += Surface_MouseEnter;
            lblText.MouseLeave += Surface_MouseLeave;

            // User Control setup
            this.Controls.Add(lblText);
            this.Controls.Add(btnIcon);
            this.Controls.Add(cmbList);
            this.MinimumSize = new Size(200, 30);
            this.Size = new Size(200, 30);
            this.ForeColor = Color.Black;
            this.Padding = new Padding(borderSize);
            this.Font = new Font(this.Font.Name, 10F);
            base.BackColor = borderColor;

            this.ResumeLayout();
            AdjustComboBoxDimensions();
        }

        // Method to adjust dimensions
        private void AdjustComboBoxDimensions()
        {
            cmbList.Width = lblText.Width;
            cmbList.Location = new Point()
            {
                X = this.Width - this.Padding.Right - cmbList.Width,
                Y = lblText.Bottom - cmbList.Height
            };
        }

        // Event handlers
        private void ComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (OnSelectedIndexChanged != null)
                OnSelectedIndexChanged.Invoke(sender, e);

            // Refresh displayed text
            lblText.Text = cmbList.Text;

            // Update background color based on selection (optional customization)
            lblText.BackColor = listBackColor; // Ensure lblText back color matches custom back color
            btnIcon.BackColor = backColor;

            lblText.Invalidate();
            this.Invalidate();
        }

        private void Icon_Click(object sender, EventArgs e)
        {
            cmbList.Select();
            cmbList.DroppedDown = true;
        }

        private void Surface_Click(object sender, EventArgs e)
        {
            this.OnClick(e);
            cmbList.Select();
            cmbList.DroppedDown = true;

        }

        private void ComboBox_TextChanged(object sender, EventArgs e)
        {
            lblText.Text = cmbList.Text;
            lblText.BackColor = listBackColor;
        }

        private void Icon_Paint(object sender, PaintEventArgs e)
        {
            int iconWidth = 14;
            int iconHeight = 6;
            var rectIcon = new Rectangle((btnIcon.Width - iconWidth) / 2, (btnIcon.Height - iconHeight) / 2, iconWidth, iconHeight);
            Graphics graph = e.Graphics;

            using (GraphicsPath path = new GraphicsPath())
            using (Pen pen = new Pen(iconColor, 2))
            {
                graph.SmoothingMode = SmoothingMode.AntiAlias;
                path.AddLine(rectIcon.X, rectIcon.Y, rectIcon.X + (iconWidth / 2), rectIcon.Bottom);
                path.AddLine(rectIcon.X + (iconWidth / 2), rectIcon.Bottom, rectIcon.Right, rectIcon.Y);
                graph.DrawPath(pen, path);
            }
        }

        private void Surface_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void Surface_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        // Override OnPaint to clear any artifacts
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(this.BackColor); // Clear any background artifacts
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            AdjustComboBoxDimensions();
        }
    }


    /*------------------Data Storage---------------------*/

    // Class to store all the information related to an upload, including metadata like dates, sample information, and the uploaded image.
    public class UploadInfo
    {
        // The name of the file or upload, typically set by the user or generated automatically if left blank.
        public string UploadName { get; set; }

        // The date when the sample was taken, which represents when the data (e.g., a sample from the field) was collected.
        public DateTime SampleDate { get; set; }

        // The exact time when the sample was taken. It helps track the time of day the data was collected.
        public DateTime SampleTime { get; set; }

        // The date and time when the file was uploaded into the system, automatically set during the upload process.
        public DateTime UploadTime { get; set; }

        // The location where the sample was collected, which can be a geographical location or a specific field/site.
        public string SampleLocation { get; set; }

        // The breed of the sheep (or any other applicable animal/specimen), which is important for certain types of research or data analysis.
        public string SheepBreed { get; set; }

        // Additional notes or comments provided by the user during the upload process, which could include observations or special instructions.
        public string Comments { get; set; }

        // The image file uploaded by the user, typically a sample image or any visual data associated with the sample.
        public Image ImageFile { get; set; }

        // Thumbnail version of the image to be used by data library
        public Image ThumbNail { get; set; }
    }

    // Class to store all the data given from the ML model relating to each image upload
    public class MLData 
    {
        // Public string to store the percentage of nale grass in an image
        public string nalePercentage { get; set; }

        // Public string to store the percentage of qufu grass in an image
        public string qufuPercentage { get; set; }

        // Public string to store the percentage of erci grass in an image
        public string erciPercentage { get; set; }

        // Public string to store the percentage of bubbles in an image
        public string bubblePercentage { get; set; }

        // Public string to store the percentage of qufu stems in an image
        public string qufustemPercentage { get; set; }
        
    }


    // A static class that serves as a global storage for all uploads.
    // It contains a public static list to hold multiple instances of `UploadInfo`, representing each file and its associated metadata.
    public static class GlobalData
    {
        // A static list that stores all upload data. Each upload is an instance of `UploadInfo`, containing details like the file name, sample info, image, etc.
        public static List<UploadInfo> Uploads { get; } = new List<UploadInfo>();

        // A static list that stores all ML generated data.  Each upload will have its own data not provided by the user and will be stored here.
        public static List<MLData> machineLearningData { get; } = new List<MLData>();
    }
}
