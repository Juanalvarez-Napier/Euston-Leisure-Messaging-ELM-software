using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows;

namespace ELMFS_40340725
{
    public class AbreviationHandling
    {
        #region Uploading the list of abreviations from a text file
        // getting the list of abreviations into a dictionary
        // this means we can edit, and or remove abbreviation easily just editing the file
        public static string textWordsPath = System.IO.Path.GetFullPath(Directory.GetCurrentDirectory() + "\\textwords.csv");
        private Dictionary<string, string> abreviations = File.ReadLines(textWordsPath).Select(line => line.Split(',')).ToDictionary(line => line[0], line => line[1]);
        #endregion

        #region Method to find and edit abreviations
        public string ChangeAbreviations(string messageBody)
        {
            // Using the created dictionary we go over the text of messagebody to find
            // and expand abreviations
            foreach (var entry in abreviations)
            {
                messageBody = messageBody.Replace(" " + entry.Key, " " + entry.Key + " <" + entry.Value + ">");
            }
            return messageBody;

        }
        #endregion

    }
}
