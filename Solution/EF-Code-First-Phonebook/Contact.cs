

namespace EF_Code_First_Phonebook
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Data.Entity;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Contact
    {
        public Contact()
        {
            this.Phones = new HashSet<Phone>();
            this.Emails = new HashSet<Email>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(50)]
        public string Name { get; set; }

        public string Position { get; set; }

        public string Company { get; set; }

        public string Url { get; set; }

        public string Notes { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public ICollection<Email> Emails { get; set; }
    }
}
