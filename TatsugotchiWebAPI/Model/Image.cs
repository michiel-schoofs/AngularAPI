using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TatsugotchiWebAPI.DTO;

namespace TatsugotchiWebAPI.Model
{
    public class Image{
        //Auto generated
        public int ImageID { get; set; }
        //Base64 string data
        public string Content { get; set; }
        //type
        public string Type { get; set; }

        public Image(ImageDTO imageDTO){
            string unfiltered = imageDTO.Data;
            Type = unfiltered.Split(":")[1].Split(";")[0];

            string[] notcontent = unfiltered.Split(",");
            int length = notcontent.Length - 1;

            string[] content = new string[length];
            Array.Copy(notcontent, 1, content, 0, length);

            Content = string.Join("",content);
        }

        //EF being retarded
        protected Image(){

        }

        public override string ToString(){
            return $"data:{Type};base64,{Content}";
        }
    }
}
