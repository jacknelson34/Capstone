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
using System.Text.Json.Serialization;

namespace GrazeViewV1
{
    /*--------------------Page Sizing-------------------------------------*/


    // NOTE : This class is now only used for the transition from DataUpload -> LoadingPage -> ResultsPage
    //        as LoadingPage does not follow the consistency of all other pages, the global variable is
    //        necessary.
    public static class ConsistentForm // Public class to store form resize for consistency
    {
        public static Size FormSize { get; set; } = new Size(1280, 720); // Default of 1280, 918
        public static Point FormLocation { get; set; } = new Point(Screen.PrimaryScreen.WorkingArea.Width / 4, 
                                                                   Screen.PrimaryScreen.WorkingArea.Height / 6); // Default of center screen-ish
        public static bool IsFullScreen { get; set; } = false;  // Default bool set to not full screen

    }


    /*---------------------Custom Controls--------------------------------*/

    // Public Class to create consistent button design with rounded edges - WORKS
    public class roundButton : Button
    {
        // Design Fields
        private int borderSize = 1;
        public int borderRadius = 20;
        private Color borderColor = Color.Gray;

        // Constructors
        public roundButton()
        {
            this.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point, 0);
            this.FlatStyle = FlatStyle.Flat;
            this.FlatAppearance.BorderSize = 0;
            this.FlatAppearance.BorderColor = Color.Gray;
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
            pevent.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            pevent.Graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

            float dpiScale = this.DeviceDpi / 96f; // Normalize DPI
            float scaledBorderRadius = borderRadius * dpiScale;

            // Define the button's surface and border rectangles
            RectangleF rectSurface = new RectangleF(0, 0, this.Width, this.Height);
            RectangleF rectBorder = new RectangleF(
                borderSize / 2f,
                borderSize / 2f,
                this.Width - borderSize,
                this.Height - borderSize
            );

            if (borderRadius > 2) // Rounded Button
            {
                using (GraphicsPath pathSurface = GetfigurePath(rectSurface, scaledBorderRadius))
                using (GraphicsPath pathBorder = GetfigurePath(rectBorder, scaledBorderRadius - borderSize / 2f))
                using (Pen penSurface = new Pen(this.Parent.BackColor, borderSize))
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


    // Custom MessageBox to hold images
    public class CustomMessageBox : Form
    {
        private PictureBox pictureBox;
        private Label messageLabel;
        private Button okButton;

        public CustomMessageBox(string message, Image image)
        {
            // Set form properties
            this.Text = message;
            this.Size = new Size(400, 300);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            // Initialize PictureBox
            pictureBox = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(10, 10),
                Size = new Size(100, 100)
            };
            this.Controls.Add(pictureBox);

            // Initialize Label
            messageLabel = new Label
            {
                Text = message,
                AutoSize = true,
                Location = new Point(120, 50),
                MaximumSize = new Size(250, 0),
            };
            this.Controls.Add(messageLabel);

            // Initialize OK Button
            okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(150, 200),
            };
            this.Controls.Add(okButton);

            // Set the Accept button
            this.AcceptButton = okButton;
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

    public class TransparentPanel : Panel
    {
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            // Check if the parent exists
            if (Parent != null)
            {
                // Get the parent's graphics object
                Graphics parentGraphics = Parent.CreateGraphics();

                // Draw the parent's background onto this control
                Bitmap bmp = new Bitmap(Width, Height, parentGraphics);
                Parent.DrawToBitmap(bmp, Bounds);

                // Draw the bitmap as the panel's background
                e.Graphics.DrawImage(bmp, 0, 0);
            }
            else
            {
                base.OnPaintBackground(e);
            }
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
        // Exclude ImageFile from serialization
        [JsonIgnore]
        public Image ImageFile { get; set; }

        // Serialize Image as Base64
        public string ImageBase64
        {
            get
            {
                if (ImageFile == null) return null;
                using (MemoryStream ms = new MemoryStream())
                {
                    ImageFile.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    ImageFile = null;
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(value);
                    using (MemoryStream ms = new MemoryStream(bytes))
                    {
                        ImageFile = Image.FromStream(ms);
                    }
                }
            }
        }
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
        //public string qufustemPercentage { get; set; }
        // No longer using
        
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
