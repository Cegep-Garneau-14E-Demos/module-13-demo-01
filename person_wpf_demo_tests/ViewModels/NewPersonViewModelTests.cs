using NUnit.Framework;
using Moq;
using person_wpf_demo.ViewModels;
using person_wpf_demo.Models;
using System;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo_tests
{
    public class NewPersonViewModelTests
    {
        private Mock<IPersonService> _personServiceMock;
        private Mock<INavigationService> _navigationServiceMock;
        private NewPersonViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _personServiceMock = new Mock<IPersonService>();
            _navigationServiceMock = new Mock<INavigationService>();
            _viewModel = new NewPersonViewModel(_personServiceMock.Object, _navigationServiceMock.Object);
        }

        [Test]
        public void Saving_a_valid_person_calls_add()
        {
            _viewModel.FirstName = "Santa";
            _viewModel.LastName = "Claus";
            _viewModel.DateOfBirth = new DateTime(1690, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Once);
        }

        [Test]
        public void Saving_a_valid_person_navigates_to_persons_view()
        {
            _viewModel.FirstName = "Santa";
            _viewModel.LastName = "Claus";
            _viewModel.DateOfBirth = new DateTime(1690, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void Saving_a_person_with_invalid_first_name_does_not_call_add()
        {
            _viewModel.FirstName = "J";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_invalid_last_name_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "D";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_invalid_date_of_birth_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = DateTime.MinValue;
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_invalid_street_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_invalid_city_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_invalid_postal_code_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_an_invalid_person_does_not_navigate()
        {
            _viewModel.FirstName = "J";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Never);
        }

        [Test]
        public void Save_command_cannot_execute_when_first_name_has_validation_errors()
        {
            _viewModel.FirstName = "J";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            bool canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Saving_a_person_with_null_date_of_birth_does_not_call_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = null;
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Never);
        }

        [Test]
        public void Saving_a_person_with_exactly_two_character_first_name_calls_add()
        {
            _viewModel.FirstName = "Jo";
            _viewModel.LastName = "Doe";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Once);
        }

        [Test]
        public void Saving_a_person_with_exactly_two_character_last_name_calls_add()
        {
            _viewModel.FirstName = "John";
            _viewModel.LastName = "Do";
            _viewModel.DateOfBirth = new DateTime(1990, 1, 1);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _personServiceMock.Verify(service => service.Add(It.IsAny<Person>()), Times.Once);
        }

        [Test]
        public void Save_command_cannot_execute_when_multiple_fields_are_invalid()
        {
            _viewModel.FirstName = "J";
            _viewModel.LastName = "D";
            _viewModel.DateOfBirth = null;
            _viewModel.Street = "";
            _viewModel.City = "";
            _viewModel.PostalCode = "";

            bool canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Setting_first_name_to_empty_adds_validation_error()
        {
            _viewModel.FirstName = "";

            Assert.That(_viewModel.HasErrors, Is.True);
        }

        [Test]
        public void Setting_last_name_to_empty_adds_validation_error()
        {
            _viewModel.LastName = "";

            Assert.That(_viewModel.HasErrors, Is.True);
        }
    }
}




