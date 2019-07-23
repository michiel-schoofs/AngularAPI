using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TatsugotchiWebAPI.Model
{
    public class Image{
        //Auto generated
        public int ImageID { get; set; }
        //Base64 string data
        public string Content { get; set; }
        //type
        public string Type { get; set; }

        public override string ToString(){
            return $"data:{Type};base64,{Content}";
        }
    }
}
