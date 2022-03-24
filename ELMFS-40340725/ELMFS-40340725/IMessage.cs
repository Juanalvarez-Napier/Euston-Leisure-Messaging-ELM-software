using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ELMFS_40340725
{
    public interface IMessage
    {
        // properties that will have to be implemented by contract
        // Email.cs, Tweets.cs and SMS.cs inherint for this interface
        string MessageID { get; set; }

        string MessageBody { get; set; }

        string MessageType { get; set; }
        
        // method that will be used to Display results in the Result Panel
        string DisplayResult();


        
        

    }
}
