using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ContactListMVC4.DAL
{
    public interface IRepository<T>
    {
        IQueryable<T> FindAll();
        T Get(int id);
        void Save();
        T Add(T t);
        void Delete(T t);
        T Update(int id, T t);
    }
}
