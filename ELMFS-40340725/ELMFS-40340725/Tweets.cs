using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace ELMFS_40340725
{
    class Tweets : IMessage
    {
        #region Variables and properties
        private string messageid;
        private string messageBody;
        private string tweeter;

        public string MessageType { get; set; }
        public string Tweeter
        {
            get
            { return tweeter; }
            set
            {

                tweeter = value;
            }
        }
        public string MessageID { get { return messageid; } set { messageid = value; } }


        
        public string MessageBody
        {
            get { return messageBody; }
            set
            {
                if (value.Length < 155) //validating data, length of Tweet
                {
                    messageBody = value;
                }
                else
                    throw new Exception("Message cannot exceed 140 characters");
            }
        }
        #endregion

        #region Constructor
        public Tweets(string messageID, string messageBody)
        {
            MessageType = "Tweet";
            ProcessTweets(messageBody);
            MessageID = messageID;

            Tweeter = FindMentions(MessageBody);
            FindHashtags(MessageBody);
            MessageBody = MessageBody.Replace(Tweeter, " ");
            MessageBox.Show("Tweet processed");

        }
        #endregion

        #region Method to handle abbreviations
        private void ProcessTweets(string messageBody)
        {
            // Sending MessageBody to the abreviation handling class to find abbreviations and
            // and edit them as per the file provided with the coursework
            AbreviationHandling abreviations = new AbreviationHandling();
            MessageBody = abreviations.ChangeAbreviations(messageBody);
            
        }
        #endregion


        #region Method to find Mentions so it can be displayed in Result panel
        private string FindMentions(string messagebody)
        {
            try
            {

                string pattern = @"@\w+";
                Regex tweetRegex = new Regex(pattern);
                MatchCollection mentionMatches = tweetRegex.Matches(messagebody);
                Tweeter = mentionMatches[0].Value;
                for (int i = 1; i < mentionMatches.Count; i++)
                {
                    //MainWindow mainWindow = new MainWindow();
                    MainWindow.mentions.Add(mentionMatches[i].Value);
                    //MessageBox.Show("Mention = " + mentionMatches[i].Value);
                }
                return Tweeter;
            }
            catch(Exception)
            {
                throw new Exception("You have to add a sender in Message Body");
            }

        }
        #endregion

        #region Method to find Hashtags
        private void FindHashtags(string messagebody)
        {
            string pattern = @"#\w+";
            Regex hashRegex = new Regex(pattern);
            MatchCollection mentionMatches = hashRegex.Matches(messagebody);
            foreach(Match m in mentionMatches)
            {
                MainWindow.hashtags.Add(m.Value);
                //MessageBox.Show("Hashtags = " + m.Value);
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
