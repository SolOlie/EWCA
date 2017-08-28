using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public interface IRepository<T>
    {
        T Create(T t); //Creates an instance of the object T
        T Read(int id); // Reads an instane of the object T 
        List<T> ReadAll(); // Reads a list of the instances
        bool Delete(T t); // Deletes an instance of the object T 
        bool Update(T t); // Updates an instance of the object T
        List<T> ReadAllWithFk(int id);// can read all assets with customer id * and all changelogs with asset id * etc.
    }
}
