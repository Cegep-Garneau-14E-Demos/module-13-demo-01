using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Services.Interfaces;
using person_wpf_demo.ViewModels;

namespace person_wpf_demo.Utils.Services
{
    public class NavigationService : BaseViewModel, INavigationService
    {
        private BaseViewModel _currentView;
        private Func<Type, BaseViewModel> _viewModelFactory;

        public BaseViewModel CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public NavigationService(Func<Type, BaseViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
        }

        public void NavigateTo<TViewModel>(params object[] parameters) where TViewModel : BaseViewModel
        {
            BaseViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            if (viewModel is INavigationParameterReceiver receiver)
            {
                receiver.ApplyNavigationParameters(parameters);
            }
            CurrentView = viewModel;
        }
    }
}
