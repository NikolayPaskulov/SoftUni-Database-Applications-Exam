using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace EF_Code_First_Phonebook
{
    internal sealed class MountainsMigration : DropCreateDatabaseIfModelChanges<PhonebookContext>
    {
        protected override void Seed(PhonebookContext context)
        {
            var PeterIvanovContact = new Contact()
            {
                Name = "Peter Ivanov",
                Position = "CTO",
                Company = "Smart Ideas",
                Url = "http://blog.peter.com",
                Notes = "Friend from school" 
            };

            var PeterIvanovFirstEmail = new Email() {EmailAddress = "peter@gmail.com"};
            var PeterIvanovSecondEmail = new Email() {EmailAddress = "peter_ivanov@yahoo.com"};

            var PeterIvanovFirstPhone = new Phone() { PhoneNumber = "+359 2 22 22 22" };
            var PeterIvanovSecondPhone = new Phone() { PhoneNumber = "+359 88 77 88 99" };

            PeterIvanovContact.Phones.Add(PeterIvanovFirstPhone);
            PeterIvanovContact.Phones.Add(PeterIvanovSecondPhone);

            PeterIvanovContact.Emails.Add(PeterIvanovFirstEmail);
            PeterIvanovContact.Emails.Add(PeterIvanovSecondEmail);

            context.Contacts.Add(PeterIvanovContact);

            var MariaContact = new Contact() {Name = "Maria"};
            var MariaPhone = new Phone() {PhoneNumber = "+359 22 33 44 55"};

            MariaContact.Phones.Add(MariaPhone);

            context.Contacts.Add(MariaContact);

            var AngieStantonContact = new Contact() { Name = "Angie Stanton", Url = "http://angiestanton.com" };
            var AngieStantonEmail = new Email() {EmailAddress = "info@angiestanton.com"};

            AngieStantonContact.Emails.Add(AngieStantonEmail);

            context.Contacts.Add(AngieStantonContact);

            context.SaveChanges();
        }
    }
}
