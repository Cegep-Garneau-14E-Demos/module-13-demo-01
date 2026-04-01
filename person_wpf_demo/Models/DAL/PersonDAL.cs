using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using person_wpf_demo.Models;
using person_wpf_demo.Models.DAL.Interfaces;

namespace person_wpf_demo.Models.DAL
{
    public class PersonDAL : IPersonDAL
    {
        private readonly ApplicationDbContext _context;

        public PersonDAL(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Person> GetAll()
        {
            return _context.Persons.Include(p => p.Adresses).ToList();
        }

        public void Save(Person person)
        {
            _context.Persons.Add(person);
            _context.SaveChanges();
        }

        public void Update(Person person)
        {
            var existingPerson = _context.Persons.Include(p => p.Adresses).FirstOrDefault(p => p.Id == person.Id);
            if (existingPerson != null)
            {
                existingPerson.Prenom = person.Prenom;
                existingPerson.Nom = person.Nom;

                foreach (var address in person.Adresses)
                {
                    var existingAddress = existingPerson.Adresses.FirstOrDefault(a => a.Id == address.Id);
                    if (existingAddress != null)
                    {
                        existingAddress.Rue = address.Rue;
                        existingAddress.Ville = address.Ville;
                        existingAddress.CodePostal = address.CodePostal;
                    }
                    else
                    {
                        existingPerson.Adresses.Add(address);
                    }
                }

                foreach (var address in existingPerson.Adresses.ToList())
                {
                    if (!person.Adresses.Any(a => a.Id == address.Id))
                    {
                        _context.Addresses.Remove(address);
                    }
                }

                _context.Persons.Update(existingPerson);
                _context.SaveChanges();
            }
        }

        public void Delete(Person person)
        {
            _context.Persons.Remove(person);
            _context.SaveChanges();
        }
    }
}
