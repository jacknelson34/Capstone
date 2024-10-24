using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrazeViewV1
{
    public class ConsistentForm : Form
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

        
    }

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
