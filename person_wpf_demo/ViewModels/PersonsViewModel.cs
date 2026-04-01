using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using person_wpf_demo.Models;
using person_wpf_demo.Utils;
using person_wpf_demo.Utils.Commands;
using person_wpf_demo.Utils.Services.Interfaces;

namespace person_wpf_demo.ViewModels
{
    class PersonsViewModel : BaseViewModel
    {
        private readonly IPersonService _personService;
        private readonly INavigationService _navigationService;
        public ObservableCollection<Person> Persons
        {
            get => new ObservableCollection<Person>(_personService.FindAll());
            set;
        }

        private Person _selectedPerson;
        public Person SelectedPerson
        {
            get => _selectedPerson;
            set
            {
                _selectedPerson = value;
                OnPropertyChanged(nameof(SelectedPerson));
                OnPropertyChanged(nameof(AddressCount));
            }
        }

        public int AddressCount => SelectedPerson?.Adresses?.Count ?? 0;

        public ICommand DeleteCommand { get; set; }
        public ICommand NavigateToNewAddressViewCommand { get; set; }

        public PersonsViewModel(IPersonService personService, INavigationService navigationService)
        {
            _personService = personService;
            _navigationService = navigationService;
            DeleteCommand = new RelayCommand(Delete, CanDelete);
            NavigateToNewAddressViewCommand = new RelayCommand(NavigateToNewAddressView, CanNavigateToNewAddressView);
        }

        private void Delete()
        {
            _personService.Remove(SelectedPerson);
            OnPropertyChanged(nameof(Persons));
        }

        private bool CanDelete()
        {
            return SelectedPerson != null;
        }

        private void NavigateToNewAddressView()
        {
            if (CanNavigateToNewAddressView())
            {
                _navigationService.NavigateTo<NewAddressViewModel>(SelectedPerson);
            }
        }

        private bool CanNavigateToNewAddressView()
        {
            return SelectedPerson != null;
        }
    }
}
