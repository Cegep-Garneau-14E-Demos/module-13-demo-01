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
            Person person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            _viewModel.SaveCommand.Execute(null);

            _addressServiceMock.Verify(service => service.Add(
                person,
                It.Is<Address>(address =>
                    address.Street == "Candy Lane" &&
                    address.City == "North Pole" &&
                    address.PostalCode == "H0H0H0" &&
                    address.PersonId == 42)),
                Times.Once);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void Save_command_cannot_execute_when_required_fields_are_missing()
        {
            Person person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = string.Empty;
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = "H0H0H0";

            bool canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Apply_navigation_parameters_with_null_does_not_throw_exception()
        {
            Assert.DoesNotThrow(() => _viewModel.ApplyNavigationParameters(null));
        }

        [Test]
        public void Apply_navigation_parameters_with_empty_array_does_not_throw_exception()
        {
            Assert.DoesNotThrow(() => _viewModel.ApplyNavigationParameters());
        }

        [Test]
        public void Apply_navigation_parameters_with_wrong_type_does_not_crash()
        {
            Assert.DoesNotThrow(() => _viewModel.ApplyNavigationParameters("wrong type"));
        }

        [Test]
        public void Save_command_cannot_execute_when_city_is_empty()
        {
            Person person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = string.Empty;
            _viewModel.PostalCode = "H0H0H0";

            bool canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Save_command_cannot_execute_when_postal_code_is_empty()
        {
            Person person = new Person { Id = 42 };
            _viewModel.ApplyNavigationParameters(person);
            _viewModel.Street = "Candy Lane";
            _viewModel.City = "North Pole";
            _viewModel.PostalCode = string.Empty;

            bool canExecute = _viewModel.SaveCommand.CanExecute(null);

            Assert.That(canExecute, Is.False);
        }

        [Test]
        public void Setting_street_to_empty_adds_validation_error()
        {
            _viewModel.Street = "";

            Assert.That(_viewModel.HasErrors, Is.True);
        }

        [Test]
        public void Setting_city_to_empty_adds_validation_error()
        {
            _viewModel.City = "";

            Assert.That(_viewModel.HasErrors, Is.True);
        }

        [Test]
        public void Setting_postal_code_to_empty_adds_validation_error()
        {
            _viewModel.PostalCode = "";

            Assert.That(_viewModel.HasErrors, Is.True);
        }
    }
}
