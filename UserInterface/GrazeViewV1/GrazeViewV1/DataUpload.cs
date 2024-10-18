using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GrazeViewV1
{
    public partial class DataUpload : Form
    {
        private Image uploadedImage;

        public DataUpload()  
        {
            InitializeComponent();

            this.Text = "Data Upload";


        }

        /*          MOVE INTO BUTTON PRESSED
         * 
           // Image Upload Code here
           using(OpenFileDialog openFileDialog = new OpenFileDialog())
           {

               openFileDialog.Filter = "Image Files|*.jpg;*jpeg;*.png;*.gif";  // Filter upload to only these types of image : .jpg, .jpeg, .png, .gif

               if(openFileDialog.ShowDialog() == DialogResult.OK)  // Make sure that file selected fits criteria for upload
               {
                   string ImageFilePath = openFileDialog.FileName;  // String to hold the image's file path
                   uploadedImage = Image.FromFile(ImageFilePath);  // pull the image from its file path
               }
           }

           if (uploadedImage != null)  // Check for upload image
           {
               // --------------- BACKEND TO ML HERE ---------------

               // Image --> ML --> Results

               // --------------- BACKEND TO ML HERE ---------------

               LoadingPage loadingPage = new LoadingPage(uploadedImage);  // Ready the next page with the image stored and this page's size
               loadingPage.Show();  // open loading page while image is analyzed
               this.Hide();  // hide main page
           }
           else
           {
               MessageBox.Show("Invalid Upload Type.");  // output if uploadedImage is null by this step
           }

           */

    }
}
