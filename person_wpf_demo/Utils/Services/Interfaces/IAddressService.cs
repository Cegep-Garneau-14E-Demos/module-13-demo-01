using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Models;

namespace person_wpf_demo.Utils.Services.Interfaces
{
    public interface IAddressService
    {
        public void Add(Person person, Address newAddress);
    }
}
