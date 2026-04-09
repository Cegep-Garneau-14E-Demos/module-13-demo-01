using NUnit.Framework;
using Moq;
using person_wpf_demo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using person_wpf_demo.Models.DAL.Interfaces;
using person_wpf_demo.Utils.Services;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo_tests
{
    public class PersonServiceTests
    {
        private Mock<IPersonDAL> _personDAL_Mock;
        private IPersonService _personService;

        [SetUp]
        public void Setup()
        {
            _personDAL_Mock = new Mock<IPersonDAL>();
            _personService = new PersonService(_personDAL_Mock.Object);
        }

        [Test]
        public void Adding_a_valid_person_calls_save()
        {
            Person person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            _personService.Add(person);

            _personDAL_Mock.Verify(repo => repo.Save(person), Times.Once);
        }

        [Test]
        public void Adding_a_person_with_invalid_firstname_throws_exception()
        {
            Person person = new Person { FirstName = "J", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Adding_a_person_with_invalid_lastname_throws_exception()
        {
            Person person = new Person { FirstName = "John", LastName = "D", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Calculating_age_returns_correct_age()
        {
            DateTime birthDate = new DateTime(1990, 1, 1);
            int expectedAge = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-expectedAge)) expectedAge--;

            int age = _personService.CalculateAge(birthDate);

            Assert.That(age, Is.EqualTo(expectedAge));
        }

        [Test]
        public void Finding_all_persons_returns_all_persons()
        {
            List<Person> persons = new List<Person>
            {
                new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) },
                new Person { FirstName = "Jane", LastName = "Doe", BirthDate = new DateTime(1992, 2, 2) }
            };
            _personDAL_Mock.Setup(repo => repo.GetAll()).Returns(persons);

            IEnumerable<Person> result = _personService.FindAll();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().FirstName, Is.EqualTo("John"));
            Assert.That(result.Last().FirstName, Is.EqualTo("Jane"));
        }

        [Test]
        public void Removing_a_valid_person_calls_delete()
        {
            Person person = new Person { FirstName = "John", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            _personService.Remove(person);

            _personDAL_Mock.Verify(repo => repo.Delete(person), Times.Once);
        }

        [Test]
        public void Adding_a_person_with_exactly_two_character_first_name_calls_save()
        {
            Person person = new Person { FirstName = "Jo", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            _personService.Add(person);

            _personDAL_Mock.Verify(repo => repo.Save(person), Times.Once);
        }

        [Test]
        public void Adding_a_person_with_exactly_two_character_last_name_calls_save()
        {
            Person person = new Person { FirstName = "John", LastName = "Do", BirthDate = new DateTime(1990, 1, 1) };

            _personService.Add(person);

            _personDAL_Mock.Verify(repo => repo.Save(person), Times.Once);
        }

        [Test]
        public void Adding_a_person_with_numbers_in_first_name_throws_exception()
        {
            Person person = new Person { FirstName = "John123", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Adding_a_person_with_numbers_in_last_name_throws_exception()
        {
            Person person = new Person { FirstName = "John", LastName = "Doe123", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Adding_a_person_with_spaces_in_first_name_throws_exception()
        {
            Person person = new Person { FirstName = "Jean Paul", LastName = "Doe", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Adding_a_person_with_spaces_in_last_name_throws_exception()
        {
            Person person = new Person { FirstName = "John", LastName = "Von Doe", BirthDate = new DateTime(1990, 1, 1) };

            Assert.That(() => _personService.Add(person), Throws.ArgumentException);
        }

        [Test]
        public void Calculating_age_for_person_born_today_returns_zero()
        {
            DateTime birthDate = DateTime.Today;

            int age = _personService.CalculateAge(birthDate);

            Assert.That(age, Is.EqualTo(0));
        }

        [Test]
        public void Calculating_age_for_person_with_birthday_not_yet_this_year_returns_correct_age()
        {
            DateTime birthDate = DateTime.Today.AddYears(-30).AddDays(1);
            int expectedAge = 29;

            int age = _personService.CalculateAge(birthDate);

            Assert.That(age, Is.EqualTo(expectedAge));
        }
    }
}



