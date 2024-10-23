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

    public class UploadInfo  // Global Class to ensure all data uploaded is stored in library
    {
        public string UploadName { get; set; }     // Global variable for name of file upload
        public DateTime SampleDate { get; set; }   // Global variable for date sample was taken
        public DateTime SampleTime { get; set; }   // Global variable for time sample was taken
        public DateTime UploadTime { get; set; }   // Global variable for time sample was uploaded
        public string SampleLocation { get; set; } // Global variable for location sample is from
        public string SheepBreed { get; set; }     // Global variable for sheep breed
        public string Comments { get; set; }       // Global variable for user comments
        public Image ImageFile { get; set; }       // Global variable for image uploaded

    }

    public static class GlobalData  // Public class to store uploaded data
    {
        public static List<UploadInfo> Uploads { get; } = new List<UploadInfo>();   // Add all data to public list

    }
}
