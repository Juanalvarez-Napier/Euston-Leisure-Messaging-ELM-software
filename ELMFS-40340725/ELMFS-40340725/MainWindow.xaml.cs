using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.Win32;


namespace ELMFS_40340725
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    /// 

        // Juan Alvarez
        // Student ID: 40340725
        // Edinburgh Napier University 2019/2020
    public partial class MainWindow : Window
    {
        #region Lists and variables
        public List<string> jsonList = new List<string>(); // list of json converted messages
        public static List<string> mentions = new List<string>(); // list of mentions
        public static List<string> hashtags = new List<string>(); // list of hashtags
        public static List<string> urls = new List<string>(); // list of quarentined urls
        public static List<string> sirList = new List<string>(); // list of SIRs
        public Dictionary<string, string> fileMessages = new Dictionary<string, string>();
        private int xcount = 0;
        private int ycount = 0;
        #endregion

        #region Constructor
        public MainWindow()
        {
            InitializeComponent();

        }
        #endregion

        #region Processing text boxes button
        // Processing text boxes content
        private void BtnProcess_Click(object sender, RoutedEventArgs e)
        {
            ProcessTxtBox textProcessor = new ProcessTxtBox();
            // Thaking the content of the text boxes and assign it to strings
            string messageId = txtBoxMessageId.Text;
            string messageBody = txtBoxNessageBody.Text;
            try
            {
                    if (messageId.Length == 10) // Validating that the ID of messages is minimun of 10 characters
                    {
                        // sending messageId and messageBody to be process and taking the return
                        // and assigning it to messageBody
                        messageBody = textProcessor.procesTxtBox(messageId, messageBody);


                        foreach (IMessage f in textProcessor.messageList)
                        {
                            // Sending messages to the Display method and adding them to the list to print to file later
                            txtBlockResult.Text += f.DisplayResult();
                            jsonList.Add(f.DisplayResult());

                        }
                    }
                    else
                    {
                        // Error message if Message ID is not long enough
                        // Data validation to ensure that it is not too long is done in xaml
                        MessageBox.Show("Message ID has to contain a letter and 9 digits");
                    }

            }
            catch (Exception f)
            {
                    MessageBox.Show(f.Message);
            }

            //}

        }
        #endregion

        #region Botton to save to file
        // Adds each message to a new line in the file, does not delete the previous content of the file
        // Sessions are timed so when checking the log we know when the messages were created
        private void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Creating a folder where to save the file, folder will be in root directory
                // and will be called ELMLogs
                if (!Directory.Exists(@"\ELMLogs"))
                {
                    Directory.CreateDirectory(@"\ELMLogs");
                }
                using (StreamWriter file = new StreamWriter(@"\ELMLogs\output.json", true))
                {
                    file.WriteLine("Messages from: " + DateTime.Now);
                    foreach (string s in jsonList)
                    {
                        file.WriteLine(s);
                    }

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please create folder ELMLogs in the root directory");
            }
            MessageBox.Show("Messages have been added to a json file in folder ELMLogs in the root directory");
        }
        #endregion

        #region Button to output List of Mentions, List of Hashtags and List of SIRs
        private void btnOutput_Click(object sender, RoutedEventArgs e)
        {
            txtBlockResult.Text = ""; // Clearing text box

            string result = "List of Mentions: \n";
            foreach (string s in mentions)
            {
                result += s + "\n"; // Adding Mentions to a string

            }
            result += "List of Hashtags: \n";
            var g = hashtags.GroupBy(i => i);
            foreach (var grp in g)
            {
                result += grp.Key + ": " + grp.Count() + "\n"; // Adding hashtags to the same string

            }
            result += "List of SIR: \n";
            foreach (string s in sirList)
            {
                result += s + "\n"; // Adding SIRs to the same string
            }
            // Displaying the lists of Mentions, Hashtags and SIRs stored in string results
            txtBlockResult.Text += result;
            // Writting the 3 lists to a file
            try
            {
                // This code is obsolete but could be used if we want to hard code a specific path where to save the file
                //using (StreamWriter file = new StreamWriter(@"D:\Napier2019-20\SoftwareEng\Code\ELMFS-40340725\MentionsHastagsandEmail.txt", true))
                // Creating a folder where to save the file, folder will be in root directory
                // and will be called ELMLogs
                if (!Directory.Exists(@"\ELMLogs"))
                {
                    Directory.CreateDirectory(@"\ELMLogs");
                }
                
                // This code saves file in previously created directory
                using (StreamWriter file = new StreamWriter(@"\ELMLogs\MentionsHastagsandEmail.txt", true))
                {
                    file.WriteLine("Messages from: " + DateTime.Now);
                    file.WriteLine("Mentions: ");
                    foreach (string s in mentions)
                        file.WriteLine(s);
                    file.WriteLine("Hashtags: ");
                    foreach (var grp in g)
                        file.WriteLine(grp.Key + ": " + grp.Count() + "\n");
                    file.WriteLine("SIRs: ");
                    foreach (string s in sirList)
                        file.WriteLine(s);

                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please create folder ELMLogs in the root directory");
            }
            MessageBox.Show("Mentions, Hashtags and SIRs have been added to a text file in folder ELMLogs in the root directory");
        }
        #endregion

        #region Button to find the file we want to upload
        // Method to open the explorer and allow the user to choose the file they want to upload
        private void btnFindPath_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == true)
                txtBoxFilePath.Text = openFile.FileName;

        }
        #endregion

        #region Button to upload the file into the system
        private void btnProcessFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Reading the file line by line and separating MessageId and MessageBody by an asterisc
                // and adding it to a dictionary
                fileMessages = File.ReadLines(txtBoxFilePath.Text).Select(line => line.Split('*')).ToDictionary(line => line[0], line => line[1]);
                // Populating the text boxes with the content of the first message
                txtBoxMessageId.Text = fileMessages.Keys.ElementAt(xcount);
                txtBoxNessageBody.Text = fileMessages.Values.ElementAt(ycount);
                MessageBox.Show("File has been uploaded");
            }
            catch(Exception)
            {
                MessageBox.Show("The file has to be a txt file. MessageId and Message Body have to be separated by * and messages have to be in different lines. File has not been uploaded");
               
            }
           
        }
        #endregion

        #region Next Button
        // button to see the next message in the loaded file
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
             try
             {
                    // Clearing text boxes
                    txtBoxMessageId.Text = "";
                    txtBoxNessageBody.Text = "";
                    // Displaying content of messages, already stored in a dictionary, in the text boxes
                    // and adding 1 to the counter
                    txtBoxMessageId.Text = fileMessages.Keys.ElementAt(xcount += 1);
                    txtBoxNessageBody.Text = fileMessages.Values.ElementAt(ycount += 1);

             }
             catch (Exception)
             {
                    ycount += 1;
                    txtBoxMessageId.Text = "";
                    txtBoxNessageBody.Text = "";
                    MessageBox.Show("There are no messages left");
             }
           
                        
        }
        #endregion

        #region Previous button
        // button to see the previous message in the loaded file
        private void btnPrevious_Click(object sender, RoutedEventArgs e)
        {
            if (xcount >= 0)
            {
                try
                {
                    // Clearing text boxes
                    txtBoxMessageId.Text = "";
                    txtBoxNessageBody.Text = "";
                    // Displaying content of messages, already stored in a dictionary, in the text boxes
                    // and substracting one from the counter
                    txtBoxMessageId.Text = fileMessages.Keys.ElementAt(xcount -= 1);
                    txtBoxNessageBody.Text = fileMessages.Values.ElementAt(ycount -= 1);

                }
                catch (Exception)
                {
                    ycount -= 1;
                    txtBoxMessageId.Text = "";
                    txtBoxNessageBody.Text = "";
                    MessageBox.Show("There are no more messages left");

                }
            }
            else
            {
                MessageBox.Show("There are no more messages left");
            }
                        
        }
        #endregion

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtBlockResult.Text = "";
        }
    }
}
