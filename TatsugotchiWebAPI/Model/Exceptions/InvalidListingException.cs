using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model.Exceptions
{
    public class InvalidListingException:Exception{
        public InvalidListingException():base("This listing is not valid"){}
        public InvalidListingException(string msg):base(msg){}
    }
}
