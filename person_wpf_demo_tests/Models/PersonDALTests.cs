using NUnit.Framework;
using person_wpf_demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using person_wpf_demo.Models.DAL;

namespace person_wpf_demo_tests
{
    public class PersonDALTests
    {
        private ApplicationDbContext _dbContext;
        private PersonDAL _personDAL;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _dbContext = new ApplicationDbContext(options);
            _dbContext.Database.EnsureCreated();
            _personDAL = new PersonDAL(_dbContext);
        }

        [TearDown]
        public void TearDown()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

        [Test]
        public void Saving_a_valid_person_adds_person_to_database()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };

            _personDAL.Save(person);

            var savedPerson = _dbContext.Persons.FirstOrDefault(
                p => p.Prenom == "John" && p.Nom == "Doe");
            Assert.That(savedPerson, Is.Not.Null);
        }

        [Test]
        public void Getting_all_persons_returns_all_persons_from_database()
        {
            var persons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) },
                new Person { Prenom = "Jane", Nom = "Doe", DateNaissance = new DateTime(1992, 2, 2) }
            };
            _dbContext.Persons.AddRange(persons);
            _dbContext.SaveChanges();

            var result = _personDAL.GetAll();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].Prenom, Is.EqualTo("John"));
            Assert.That(result[1].Prenom, Is.EqualTo("Jane"));
        }

        [Test]
        public void Updating_a_valid_person_updates_person_in_database()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };
            _personDAL.Save(person);
            _dbContext.SaveChanges();

            person.Prenom = "Johnny";
            _personDAL.Update(person);
            _dbContext.SaveChanges();

            var updatedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(updatedPerson.Prenom, Is.EqualTo("Johnny"));
        }

        [Test]
        public void Deleting_a_valid_person_removes_person_from_database()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };
            _dbContext.Persons.Add(person);
            _dbContext.SaveChanges();

            _personDAL.Delete(person);
            _dbContext.SaveChanges();

            var deletedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(deletedPerson, Is.Null);
        }
    }
}




