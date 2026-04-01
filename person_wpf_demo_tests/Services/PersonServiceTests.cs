using NUnit.Framework;
using Moq;
using person_wpf_demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using person_wpf_demo.Models.DAL.Interfaces;
using person_wpf_demo.Utils.Services;

namespace person_wpf_demo_tests
{
    public class PersonServiceTests
    {
        private Mock<IPersonDAL> _personDAL_Mock;
        private PersonService _personService;

        [SetUp]
        public void Setup()
        {
            _personDAL_Mock = new Mock<IPersonDAL>();
            _personService = new PersonService(_personDAL_Mock.Object);
        }

        [Test]
        public void Adding_a_valid_person_calls_save()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };

            _personService.Add(person);

            _personDAL_Mock.Verify(repo => repo.Save(person), Times.Once);
        }

        [Test]
        public void Adding_a_person_with_invalid_firstname_throws_exception()
        {
            var person = new Person { Prenom = "J", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Adding_a_person_with_invalid_lastname_throws_exception()
        {
            var person = new Person { Prenom = "John", Nom = "D", DateNaissance = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Calculating_age_returns_correct_age()
        {
            var birthDate = new DateTime(1990, 1, 1);
            var expectedAge = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-expectedAge)) expectedAge--;

            var age = _personService.CalculateAge(birthDate);

            Assert.That(age, Is.EqualTo(expectedAge));
        }

        [Test]
        public void Finding_all_persons_returns_all_persons()
        {
            var persons = new List<Person>
            {
                new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) },
                new Person { Prenom = "Jane", Nom = "Doe", DateNaissance = new DateTime(1992, 2, 2) }
            };
            _personDAL_Mock.Setup(repo => repo.GetAll()).Returns(persons);

            var result = _personService.FindAll();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().Prenom, Is.EqualTo("John"));
            Assert.That(result.Last().Prenom, Is.EqualTo("Jane"));
        }

        [Test]
        public void Removing_a_valid_person_calls_delete()
        {
            var person = new Person { Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1) };

            _personService.Remove(person);

            _personDAL_Mock.Verify(repo => repo.Delete(person), Times.Once);
        }
    }
}



