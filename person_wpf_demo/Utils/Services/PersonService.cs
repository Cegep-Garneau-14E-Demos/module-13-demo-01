using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using person_wpf_demo.Models;
using person_wpf_demo.Models.DAL.Interfaces;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.Utils.Services
{
    public class PersonService : IPersonService
    {
        private readonly IPersonDAL _personDAL;

        public PersonService(IPersonDAL personAL)
        {
            _personDAL = personAL;
        }

        public void Add(Person newPerson)
        {
            ValidatePerson(newPerson);
            _personDAL.Save(newPerson);
        }

        public IEnumerable<Person> FindAll()
        {
            return _personDAL.GetAll();
        }

        public void Remove(Person person)
        {
            _personDAL.Delete(person);
        }

        public int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate.Date > today.AddYears(-age)) age--;
            return age;
        }

        private void ValidatePerson(Person person)
        {
            var nameRegex = new Regex("^[a-zA-Z]{2,}$");
            if (!nameRegex.IsMatch(person.Prenom))
            {
                throw new ArgumentException("Le prénom doit contenir au moins 2 caractères alphabétiques.");
            }
            if (!nameRegex.IsMatch(person.Nom))
            {
                throw new ArgumentException("Le nom doit contenir au moins 2 caractères alphabétiques.");
            }
        }
    }
}
