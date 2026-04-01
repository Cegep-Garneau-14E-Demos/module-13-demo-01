using Moq;
using NUnit.Framework;
using person_wpf_demo.Models;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.ViewModels;

namespace person_wpf_demo_tests
{
    public class NewAddressViewModelTests
    {
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<IAddressService> _addressServiceMock;
        private NewAddressViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _navigationServiceMock = new Mock<INavigationService>();
            _addressServiceMock = new Mock<IAddressService>();
            _viewModel = new NewAddressViewModel(_navigationServiceMock.Object, _addressServiceMock.Object);
        }

        [Test]
        public void Save_with_valid_data_calls_add_and_navigates_to_persons_view()
        {
            var person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _addressServiceMock.Verify(service => service.Add(
                person,
                It.Is<Address>(address =>
                    address.Rue == "Candy Lane" &&
                    address.Ville == "North Pole" &&
                    address.CodePostal == "H0H0H0" &&
                    address.PersonId == 42)),
                Times.Once);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void Save_command_cannot_execute_when_required_fields_are_missing()
        {
            var person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = string.Empty;
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            var canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }
    }
}
