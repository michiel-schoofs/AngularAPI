
using System.ComponentModel.DataAnnotations;

namespace TatsugotchiWebAPI.DTO
{
    public class ImageDTO
    {
        [Required(AllowEmptyStrings =false,ErrorMessage ="You need to enter a string")]
        [RegularExpression(@"^data:image/[A-Z|a-z]+;base64,[\w/+=]+$",ErrorMessage ="Malformed base64 string")]
        public string Data { get; set; }
    }
}
