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
        public string UploadDataText { get; set; }     // Global variable for name of upload
        public DateTime SampleTime { get; set; }   // Global variable for time sample was taken
        public DateTime UploadTime { get; set; }   // Global variable for time sample was uploaded
        public string SampleLocationText { get; set; } // Global variable for location sample is from
        public string SheepBreedText { get; set; }     // Global variable for sheep breed

    }

    public static class GlobalData  // Public class to store uploaded data - Temporary until SQ is used
    {
        public static List<UploadInfo> Uploads { get; } = new List<UploadInfo>();   // Add all data to public list

    }
}
