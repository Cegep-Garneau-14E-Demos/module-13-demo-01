using Moq;
using NUnit.Framework;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.ViewModels;

namespace person_wpf_demo_tests
{
    public class MainViewModelTests
    {
        private Mock<INavigationService> _navigationServiceMock;

        [SetUp]
        public void Setup()
        {
            _navigationServiceMock = new Mock<INavigationService>();
        }

        [Test]
        public void Creating_main_view_model_navigates_to_persons_view()
        {
            _ = new MainViewModel(_navigationServiceMock.Object);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void Navigate_to_persons_command_calls_navigation_service()
        {
            var viewModel = new MainViewModel(_navigationServiceMock.Object);

            _navigationServiceMock.Reset();
            viewModel.NavigateToPersonsViewCommand.Execute(null);

            _navigationServiceMock.Verify(service => service.NavigateTo<PersonsViewModel>(It.IsAny<object[]>()), Times.Once);
        }

        [Test]
        public void Navigate_to_new_person_command_calls_navigation_service()
        {
            var viewModel = new MainViewModel(_navigationServiceMock.Object);

            _navigationServiceMock.Reset();
            viewModel.NavigateToNewPersonViewCommand.Execute(null);

            _navigationServiceMock.Verify(service => service.NavigateTo<NewPersonViewModel>(It.IsAny<object[]>()), Times.Once);
        }
    }
}
