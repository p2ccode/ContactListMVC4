using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ContactListMVC4.Models;
using ContactListMVC4.DAL;
using System.Web.Mvc;
using System.Web;

namespace ContactListMVC4.Controllers
{
    public class ContactController : ApiController
    {
        private IRepository<Contact> repository = null;

        public ContactController()
        {
            repository = new ContactRepository();
        }



        public IEnumerable<Contact> GetAllContacts()
        {
            return repository.FindAll();
        }

        public Contact GetContact(int id)
        {
            Contact item = repository.Get(id);
            if (item == null)
            {
                throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.NotFound));
            }
            return item;
        }

        public HttpResponseMessage PostContact(Contact item)
        {
            item = repository.Add(item);
            repository.Save();

            var response = Request.CreateResponse<Contact>(HttpStatusCode.Created, item);

            string uri = Url.Link("DefaultApi", new { id = item.Id });
            response.Headers.Location = new Uri(uri);
            return response;
        }


        public void PutContact(int id, Contact contact)
        {
            contact.Id = id;
            repository.Update(id, contact);
            repository.Save();
        }

        public HttpResponseMessage DeleteContact(int id)
        {
            Contact contact = repository.Get(id);
            if (contact != null)
            {
                repository.Delete(contact);
                repository.Save();

            }
            
            return new HttpResponseMessage(HttpStatusCode.NoContent);
        }





    }

}
