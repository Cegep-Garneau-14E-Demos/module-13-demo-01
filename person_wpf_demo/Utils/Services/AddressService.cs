using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Models;
using person_wpf_demo.Models.DAL.Interfaces;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.Utils.Services
{
    public class AddressService : IAddressService
    {
        private readonly IPersonDAL _personDAL;

        public AddressService(IPersonDAL personDAL)
        {
            _personDAL = personDAL;
        }

        public void Add(Person person, Address newAddress)
        {
            person.Adresses.Add(newAddress);
            _personDAL.Update(person);
        }
    }
}
