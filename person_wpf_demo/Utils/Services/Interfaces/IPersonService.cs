using System;
using System.Collections.Generic;
using person_wpf_demo.Models;

namespace person_wpf_demo.Utils.Services.Interfaces
{
    public interface IPersonService
    {
        void Add(Person newPerson);
        IEnumerable<Person> FindAll();
        void Remove(Person person);
        int CalculateAge(DateTime birthDate);
    }
}
