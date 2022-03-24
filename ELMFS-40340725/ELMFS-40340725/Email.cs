using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
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



namespace ELMFS_40340725
{
    class Email : IMessage
    {

        #region Variables and properties
        private string messageBody;
        private string centre;
        private string sirCombined;

        public string MessageType { get; set; }
        public string Sender { get; set; }
        public string MessageID { get; set; }
        
        public string Subject { get; set; }
        public string MessageBody {
            get { return messageBody; }
            set
            {
                if (value.Length < 1068) //validating data, length of Email
                {
                    messageBody = value;
                }
                else
                    throw new Exception("Message cannot exceed 1048 characters");
            }
        }
        #endregion

        #region Constructor

        public Email(string messageID, string messageBody)
        {
            
            MessageID = messageID;
            MessageBody = messageBody;
            FindSender(MessageBody);
            FindSubject();
            ProcessEmail(MessageBody);
            if (Subject.Contains("SIR"))
            {
                MessageType = "SIR Email";
                FindCentreCode(MessageBody);
                FindNatureIncident(MessageBody);
            }
            else
            {
                MessageType = "Standard Email";
            }
            MessageBox.Show("Email processed");
        }
        #endregion

        #region Method to find Sender
        private void FindSender(string messagebody)
        {
            // Finding the email of the sender and storing it
            try
            {
                string pattern = @"[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,6}";
                Regex emailRegex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection mentionMatches = emailRegex.Matches(messagebody);
                Sender = mentionMatches[0].Value;
                MessageBody = messagebody.Replace(Sender, "");
            }
            catch (Exception)
            {
                throw new Exception("You have to add a sender email address at the beggining of Message Body");
            }
               
            
            
        }
        #endregion

        #region Method to process email's body
        private void ProcessEmail(string messagebody)
        {
            // Quarentining URLs
            UrlProcessor processor = new UrlProcessor();
            MessageBody = processor.FindUrl(messagebody);
            

        }
        #endregion

        #region Method to find the Subject of the email

        private void FindSubject()
        {
            // Finding and storing the subject of the email
            try
            {
                Subject = MessageBody.Substring(0, 20);
                MessageBody = MessageBody.Replace(Subject, "");
            }
            catch (Exception)
            {
                throw new Exception("There must be a Subject of 20 characters after the sender's email in Message Body");
            }

        }
        #endregion

        #region Method to find the code of the Centre
        // Finding the Centre code using Regex to be able to display the SIR list

        private void FindCentreCode(string messagebody)
        {
            try
            {
                string pattern = @"\d{2}-\d{3}-\d{2}";
                Regex centreRegex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection centreMatches = centreRegex.Matches(messagebody);
                centre = "Sports centre Code: " + centreMatches[0].Value + "\n";
            }
            catch (Exception)
            {
                throw new Exception("The Centre Code is not correct");
            }
        }
        #endregion

        #region Method to find the Nature of incident
        // Finding the Nature of Incident using a list of incidents to be able to display the SIR list
        private void FindNatureIncident(string messagebody)
        {
            try
            {
            List<string> incidents = new List<string> { "Theft of Properties", "Staff Attack", "Device Damage", "Raid", "Customer Attack", "Staff Abuse", "Bomb Threat", "Terrorism", "Suspicious Incident", "Sport Injury", "Personal Info Leak" };
            var natureIncident = incidents.Where(s => messagebody.Contains(s)).ToList(); 
            //MessageBox.Show(natureIncident[0]);
            sirCombined = centre + "Nature of incident: " + natureIncident[0];
            MainWindow.sirList.Add(sirCombined);
            }
            catch (Exception)
            {
                throw new Exception("The Nature of Incident is not correct");
            }

}
        #endregion

        #region Method to be able to display the result in main page
        public string DisplayResult()
        {
            // Serializing object to Json and passing te result to be displayed
            string output = JsonConvert.SerializeObject(this, Formatting.Indented);
            return output;
        }
        #endregion
    }
}
