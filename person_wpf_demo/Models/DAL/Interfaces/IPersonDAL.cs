using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Models;

namespace person_wpf_demo.Models.DAL.Interfaces
{
    public interface IPersonDAL
    {
        public IList<Person> GetAll();
        public void Save(Person person);
        public void Update(Person person);
        public void Delete(Person person);
    }
}
