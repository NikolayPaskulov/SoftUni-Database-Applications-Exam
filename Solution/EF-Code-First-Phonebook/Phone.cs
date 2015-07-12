using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Code_First_Phonebook
{
    using System.ComponentModel.DataAnnotations;

    public class Phone
    {

        [Key]
        public int Id { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

    }
}
