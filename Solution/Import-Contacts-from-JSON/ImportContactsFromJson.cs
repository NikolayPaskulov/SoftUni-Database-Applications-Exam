

namespace Import_Contacts_from_JSON
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using EF_Code_First_Phonebook;
    using Newtonsoft.Json.Linq;

    class ImportContactsFromJson
    {
        static void Main()
        {
            string contactsJson = File.ReadAllText("../../contacts.json");
            var contacts = JArray.Parse(contactsJson);
            var context = new PhonebookContext();

            foreach (var contact in contacts)
            {
                if (contact["name"] != null)
                {
                    var currentContact = new Contact();
                    currentContact.Name = contact["name"].Value<string>();

                    if (contact["company"] != null)
                    {
                        currentContact.Company = contact["company"].Value<string>();
                    }

                    if (contact["position"] != null)
                    {
                        currentContact.Position = contact["position"].Value<string>();
                    }

                    if (contact["url"] != null)
                    {
                        currentContact.Url = contact["url"].Value<string>();
                    }

                    if (contact["notes"] != null)
                    {
                        currentContact.Notes = contact["notes"].Value<string>();
                    }

                    if (contact["phones"] != null)
                    {
                        foreach (var phone in contact["phones"])
                        {
                            var currentPhone = new Phone() { PhoneNumber = phone.Value<string>() };
                            currentContact.Phones.Add(currentPhone);
                        }
                    }

                    if (contact["emails"] != null)
                    {
                        foreach (var email in contact["emails"])
                        {
                            var currentEmail = new Email() { EmailAddress = email.Value<string>() };
                            currentContact.Emails.Add(currentEmail);
                        }
                    }
                    context.Contacts.Add(currentContact);
                    context.SaveChanges();
                    Console.WriteLine("Contact {0} imported", contact["name"].Value<string>());
                }
                else
                {
                    Console.WriteLine("Error: Name is required");
                }
            }
        }
    }
}
