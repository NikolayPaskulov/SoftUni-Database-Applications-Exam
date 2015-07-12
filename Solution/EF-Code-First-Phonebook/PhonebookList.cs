using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EF_Code_First_Phonebook
{
    class PhonebookList
    {
        static void Main()
        {
            var context = new PhonebookContext();

            var contacts = context.Contacts
                .Select(c => new
                {
                    c.Name,
                    c.Emails,
                    c.Phones
                });

            foreach (var contact in contacts)
            {
                Console.WriteLine("Contact name : {0}", contact.Name);

                Console.WriteLine("Emails:");
                foreach (var email in contact.Emails)
                {
                    Console.WriteLine("    {0}", email.EmailAddress);
                }

                Console.WriteLine("Phones:");
                foreach (var phone in contact.Phones)
                {
                    Console.WriteLine("    {0}", phone.PhoneNumber);
                }
            }
        }
    }
}
