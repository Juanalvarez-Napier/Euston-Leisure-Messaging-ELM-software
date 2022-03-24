using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Newtonsoft.Json;
namespace ELMFS_40340725
{
    class Sms : IMessage
    {
        #region Variables and properties
        // variables and properties
        private string phoneNumber;
        
        private string messageBody;
        private string messageId;

        public string MessageType { get; set; }
        public string PhoneNumber
        {
            get { return phoneNumber; }
            set { phoneNumber = value; }
        }
        public string MessageID
        {
            get { return messageId; }
            set { messageId = value; }
        }
        

        public string MessageBody
        {
            get { return messageBody; }
            set {

                if (value.Length < 152) //validating data, length of SMS
                    messageBody = value;
                else
                    throw new Exception("Message cannot exceed 140 characters");
            }
        }
        #endregion

        #region Constructor
        public Sms (string messageId, string messageBody)
        {
            MessageType = "SMS";
            ProcesSms(messageBody);
            PhoneNumber = FindPhoneNumber(MessageBody);
            MessageID = messageId;
            MessageBody = MessageBody.Replace(PhoneNumber, " ");
            MessageBox.Show("SMS processed");

        }
        #endregion

        #region Function to process SMS messages
        private void ProcesSms(string messageBody)
        {
            // Sending MessageBody to the abreviation handling class to find abbreviations and
            // and edit them as per the file provided with the coursework
            AbreviationHandling abreviations = new AbreviationHandling();
            MessageBody = abreviations.ChangeAbreviations(messageBody);
            
        }
        #endregion

        #region Finding Phone number method
        private string FindPhoneNumber(string messagebody)
        {
            try
            {
                // Regex for UK phone numbers, finding phone number and adding it to property
                string pattern = @"\(?\d{4}\)?-? *\d{3}-? *-?\d{4}";
                Regex phoneRegex = new Regex(pattern);
                MatchCollection phonematches = phoneRegex.Matches(messagebody);

                PhoneNumber = phonematches[0].Value;
                //MessageBox.Show(PhoneNumber.ToString());


                return PhoneNumber;
            }
            catch (Exception)
            {
                throw new Exception("You have to add a 11 digits Phone Number at the beggining of the Message Body");
            }
        }
        #endregion

        #region Display Result Method
        public string DisplayResult()
        {
            // Serializing object to Json and passing te result to be displayed
            string output = JsonConvert.SerializeObject(this,Formatting.Indented);
            return output;

        }
        #endregion
    }
}
