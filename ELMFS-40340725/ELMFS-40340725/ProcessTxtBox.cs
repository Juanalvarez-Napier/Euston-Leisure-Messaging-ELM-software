using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.IO;
using System.Windows;


namespace ELMFS_40340725
{
    public class ProcessTxtBox
    {

        private int count = -1;
     
        public List<IMessage> messageList = new List<IMessage>();
        public string procesTxtBox(string messageId, string messageBody)
        {
          
                
            // We check if we have either SMS, Email or Tweet and deal with the result

            if (messageId.Trim().StartsWith("S"))
            {
                //MessageBox.Show("This is a SMS");
                //messageBody = Sms.ProcesSms(messageBody);
                //smsList.Add(new Sms(messageBody));
                //Sms newSms = new Sms(messageBody);
                //smsList.Add(new Sms(newSms.MessageBody));
                count += 1;
                messageList.Add(new Sms(messageId, messageBody));

                return messageList[count].MessageBody;
                

            }
            else if (messageId.Trim().StartsWith("E"))
            {
                count += 1;
                messageList.Add(new Email(messageId, messageBody));
                return messageList[count].MessageBody;
                //ISms.ProcesSms(messageId, messageBody);
            }
            else if (messageId.Trim().StartsWith("T"))
            {
                count += 1;
                messageList.Add(new Tweets(messageId, messageBody));

                return messageList[count].MessageBody;
                //MessageBox.Show("This is a Tweet");
               // messageBody = Tweets.ProcessTweets(messageBody);
                
            }
            else
            {
                MessageBox.Show("The first letter of Message Id has to be either S, E or T in capital");
                return "";
            }
            
        }
    }
}
