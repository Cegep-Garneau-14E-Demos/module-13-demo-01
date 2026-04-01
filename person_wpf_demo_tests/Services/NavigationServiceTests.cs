using System;
using Moq;
using NUnit.Framework;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Services;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo_tests
{
    public class NavigationServiceTests
    {
        [Test]
        public void Navigate_to_sets_current_view_with_view_model_from_factory()
        {
            var expectedViewModel = new TestViewModel();
            var factoryMock = new Mock<Func<Type, BaseViewModel>>();
            factoryMock
                .Setup(factory => factory.Invoke(typeof(TestViewModel)))
                .Returns(expectedViewModel);

            var navigationService = new NavigationService(factoryMock.Object);

            navigationService.NavigateTo<TestViewModel>();

            Assert.That(navigationService.CurrentView, Is.SameAs(expectedViewModel));
        }

        [Test]
        public void Navigate_to_passes_parameters_to_receiver_view_model()
        {
            var expectedParameter = "param";
            var receiverViewModel = new ReceiverViewModel();
            var factoryMock = new Mock<Func<Type, BaseViewModel>>();
            factoryMock
                .Setup(factory => factory.Invoke(typeof(ReceiverViewModel)))
                .Returns(receiverViewModel);

            var navigationService = new NavigationService(factoryMock.Object);

            navigationService.NavigateTo<ReceiverViewModel>(expectedParameter);

            Assert.That(receiverViewModel.LastParameters, Is.Not.Null);
            Assert.That(receiverViewModel.LastParameters.Length, Is.EqualTo(1));
            Assert.That(receiverViewModel.LastParameters[0], Is.EqualTo(expectedParameter));
            Assert.That(navigationService.CurrentView, Is.SameAs(receiverViewModel));
        }

        private class TestViewModel : BaseViewModel
        {
        }

        private class ReceiverViewModel : BaseViewModel, INavigationParameterReceiver
        {
            public object[]? LastParameters { get; private set; }

            public void ApplyNavigationParameters(params object[] parameters)
            {
                LastParameters = parameters;
            }
        }
    }
}
