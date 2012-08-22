using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ContactListMVC4.DAL
{
    public class ContactRepository : IRepository<Contact>
    {
        private ContactListEntities db = new ContactListEntities();

        public IQueryable<Contact> FindAll()
        {
            return db.Contacts;
        }

        public Contact Get(int id)
        {
            return db.Contacts.FirstOrDefault(c => c.Id == id);
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public Contact Add(Contact contact)
        {
            db.Contacts.AddObject(contact);
            return contact;
        }

        public void Delete(Contact contact)
        {
            db.Contacts.DeleteObject(contact);
        }

        public Contact Update(int id, Contact newContact)
        {
            Contact contact = db.Contacts.FirstOrDefault(c => c.Id == id);

            db.Contacts.DeleteObject(contact);

            db.SaveChanges();

            db.Contacts.AddObject(newContact);
            return newContact;
        }
        
    }
}