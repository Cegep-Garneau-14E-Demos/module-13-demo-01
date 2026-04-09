using NUnit.Framework;
using person_wpf_demo.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using person_wpf_demo.Models.DAL;
using person_wpf_demo.Models.DAL.Interfaces;

namespace person_wpf_demo_tests
{
    public class PersonDALTests
    {
        private ApplicationDbContext _dbContext;
        private IPersonDAL _personDAL;

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
            Person person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            _personDAL.Save(person);

            Person? savedPerson = _dbContext.Persons.FirstOrDefault(
                p => p.FirstName == "John" && p.LastName == "Doe");
            Assert.That(savedPerson, Is.Not.Null);
        }

        [Test]
        public void Getting_all_persons_returns_all_persons_from_database()
        {
            List<Person> persons = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) },
                new Person { FirstName = "Jane", LastName = "Doe", BirthDate = new DateTime(1992, 2, 2) }
            };
            _dbContext.Persons.AddRange(persons);
            _dbContext.SaveChanges();

            IList<Person> result = _personDAL.GetAll();

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result[0].FirstName, Is.EqualTo("John"));
            Assert.That(result[1].FirstName, Is.EqualTo("Jane"));
        }

        [Test]
        public void Updating_a_valid_person_updates_person_in_database()
        {
            Person person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };
            _personDAL.Save(person);
            _dbContext.SaveChanges();

            person.FirstName = "Johnny";
            _personDAL.Update(person);
            _dbContext.SaveChanges();

            Person? updatedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(updatedPerson.FirstName, Is.EqualTo("Johnny"));
        }

        [Test]
        public void Deleting_a_valid_person_removes_person_from_database()
        {
            Person person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };
            _dbContext.Persons.Add(person);
            _dbContext.SaveChanges();

            _personDAL.Delete(person);
            _dbContext.SaveChanges();

            Person? deletedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == person.Id);
            Assert.That(deletedPerson, Is.Null);
        }

        [Test]
        public void Saving_a_person_with_addresses_adds_person_and_addresses_to_database()
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                Addresses = new List<Address>
                {
                    new Address { Street = "Street 1", City = "City 1", PostalCode = "A1A1A1" },
                    new Address { Street = "Street 2", City = "City 2", PostalCode = "B2B2B2" }
                }
            };

            _personDAL.Save(person);
            _dbContext.SaveChanges();

            Person? savedPerson = _dbContext.Persons
                .Include(p => p.Addresses)
                .FirstOrDefault(p => p.FirstName == "John" && p.LastName == "Doe");
            Assert.That(savedPerson, Is.Not.Null);
            Assert.That(savedPerson.Addresses.Count, Is.EqualTo(2));
        }

        [Test]
        public void Deleting_a_person_with_addresses_removes_person_and_addresses_from_database()
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                Addresses = new List<Address>
                {
                    new Address { Street = "Street 1", City = "City 1", PostalCode = "A1A1A1" }
                }
            };
            _dbContext.Persons.Add(person);
            _dbContext.SaveChanges();
            int personId = person.Id;

            _personDAL.Delete(person);
            _dbContext.SaveChanges();

            Person? deletedPerson = _dbContext.Persons.FirstOrDefault(p => p.Id == personId);
            int addressCount = _dbContext.Addresses.Count(a => a.PersonId == personId);
            Assert.That(deletedPerson, Is.Null);
            Assert.That(addressCount, Is.EqualTo(0));
        }

        [Test]
        public void Updating_a_person_with_addresses_updates_person_in_database()
        {
            Person person = new Person
            {
                FirstName = "John",
                LastName = "Doe",
                BirthDate = new DateTime(1990, 1, 1),
                Addresses = new List<Address>
                {
                    new Address { Street = "Old Street", City = "Old City", PostalCode = "OLD" }
                }
            };
            _personDAL.Save(person);
            _dbContext.SaveChanges();

            person.FirstName = "Johnny";
            person.Addresses.First().Street = "New Street";
            _personDAL.Update(person);
            _dbContext.SaveChanges();

            Person? updatedPerson = _dbContext.Persons
                .Include(p => p.Addresses)
                .FirstOrDefault(p => p.Id == person.Id);
            Assert.That(updatedPerson!.FirstName, Is.EqualTo("Johnny"));
            Assert.That(updatedPerson.Addresses.First().Street, Is.EqualTo("New Street"));
        }

        [Test]
        public void Getting_all_persons_returns_empty_list_when_database_is_empty()
        {
            IList<Person> result = _personDAL.GetAll();

            Assert.That(result.Count, Is.EqualTo(0));
        }
    }
}




