using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using person_wpf_demo.Models;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.ViewModels;

namespace person_wpf_demo_tests
{
    public class PersonsViewModelTests
    {
        private Mock<IPersonService> _personServiceMock;
        private Mock<INavigationService> _navigationServiceMock;
        private PersonsViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _personServiceMock = new Mock<IPersonService>();
            _navigationServiceMock = new Mock<INavigationService>();

            _personServiceMock
                .Setup(service => service.FindAll())
                .Returns(new List<Person>());

            _viewModel = new PersonsViewModel(_personServiceMock.Object, _navigationServiceMock.Object);
        }

        [Test]
        public void Persons_returns_items_from_person_service()
        {
            var persons = new List<Person>
            {
                new Person { Id = 1, Prenom = "John", Nom = "Doe", DateNaissance = new DateTime(1990, 1, 1), Adresses = new List<Address>() },
                new Person { Id = 2, Prenom = "Jane", Nom = "Doe", DateNaissance = new DateTime(1992, 2, 2), Adresses = new List<Address>() }
            };

            _personServiceMock
                .Setup(service => service.FindAll())
                .Returns(persons);

            var result = _viewModel.Persons;

            Assert.That(result.Count, Is.EqualTo(2));
            Assert.That(result.First().Id, Is.EqualTo(1));
            Assert.That(result.Last().Id, Is.EqualTo(2));
        }

        [Test]
        public void Delete_with_selected_person_calls_remove()
        {
            var selectedPerson = new Person { Id = 10, Prenom = "John", Nom = "Doe", Adresses = new List<Address>() };
            _viewModel.SelectedPerson = selectedPerson;

            _viewModel.DeleteCommand.Execute(null);

            _personServiceMock.Verify(service => service.Remove(selectedPerson), Times.Once);
        }

        [Test]
        public void Delete_command_cannot_execute_when_selected_person_is_null()
        {
            _viewModel.SelectedPerson = null!;

            var canExecute = _viewModel.DeleteCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Navigate_to_new_address_with_selected_person_calls_navigation_service_with_person_parameter()
        {
            var selectedPerson = new Person { Id = 20, Prenom = "John", Nom = "Doe", Adresses = new List<Address>() };
            _viewModel.SelectedPerson = selectedPerson;

            _viewModel.NavigateToNewAddressViewCommand.Execute(null);

            _navigationServiceMock.Verify(
                service => service.NavigateTo<NewAddressViewModel>(It.Is<object[]>(parameters =>
                    parameters.Length == 1 && ReferenceEquals(parameters[0], selectedPerson))),
                Times.Once);
        }

        [Test]
        public void Navigate_to_new_address_without_selected_person_does_not_call_navigation_service()
        {
            _viewModel.SelectedPerson = null!;

            _viewModel.NavigateToNewAddressViewCommand.Execute(null);

            _navigationServiceMock.Verify(service => service.NavigateTo<NewAddressViewModel>(It.IsAny<object[]>()), Times.Never);
        }
    }
}
