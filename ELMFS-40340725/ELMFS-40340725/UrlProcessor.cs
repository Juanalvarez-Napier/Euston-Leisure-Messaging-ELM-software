using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace ELMFS_40340725
{
    public class UrlProcessor
    {

        public string FindUrl(string MessageBody)
        {
            try
            {
                string pattern = @"(http|ftp|https)://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";
                Regex urlRegex = new Regex(pattern, RegexOptions.IgnoreCase);
                MatchCollection urlMatches = urlRegex.Matches(MessageBody);
                foreach (Match m in urlMatches)
                {
                    if (MainWindow.urls.Contains(m.Value))
                    { continue; }
                    else
                    {
                        MainWindow.urls.Add(m.Value);
                        
                    }
                }

                foreach(string address in MainWindow.urls)
                {
                    MessageBody = MessageBody.Replace(address, " <URL Quarantined> ");
                }


            }
            catch(Exception)
            { throw new Exception("No URL found"); }

            return MessageBody;
            
        }
    }
}
