using Project_C.Models.UserModels;
using System.ComponentModel.DataAnnotations;

namespace Project_C.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        public string NameOfSender { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "veld is verplicht")]
        public string MessageBody { get; set; }

        [Required(ErrorMessage = "veld is verplicht")]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        public string PhoneNumber { get; set; }

        public bool IsAnswered { get; set; }

        //each employee can answer multiple messages 
        public Guid? EmployeeId { get; set; }
        public Employee? Employee { get; set; }
    }

}
