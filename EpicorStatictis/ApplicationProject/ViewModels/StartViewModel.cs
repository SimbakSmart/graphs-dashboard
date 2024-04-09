

using ApplicationProject.UC;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using MenuItem = ApplicationProject.Utils.MenuItem;

namespace ApplicationProject.ViewModels
{
    public partial class StartViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool _isOpen;

        #region MENU LINKS 

        private ObservableCollection<MenuItem> _menuOptions;
        private MenuItem _selectedItem;
        private UserControl _selectedUserControl;
        #endregion

        private string _title;

        public ObservableCollection<MenuItem> MenuOptions
        {
            get { return _menuOptions; }
            set { SetProperty(ref _menuOptions, value); }
        }

        public MenuItem SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                SetProperty(ref _selectedItem, value);
                SelectedUserControl = (UserControl)Activator.CreateInstance(_selectedItem.UserControlType);
                Title = _selectedItem.Title;
                IsOpen = false;
            }
        }

        public string Title
        {
            get { return _title; }
            set { SetProperty(ref _title, value); }
        }


        public UserControl SelectedUserControl
        {
            get { return _selectedUserControl; }
            set { SetProperty(ref _selectedUserControl, value); }
        }


        public StartViewModel()
        {
            IsOpen = false;
            Title = "ANÁLISIS DE REPORTES";
            MenuLinks();
        }
        private void MenuLinks()
        {
            MenuOptions = new ObservableCollection<MenuItem>()
            {
               new MenuItem("QUEUES","ViewDashboard","ANÁLISIS DE REPORTES EN QUEUES",typeof(QueuesControl)),
               new MenuItem("USUARIOS","AccountGroup","ANÁLISIS DE REPORTES EN USUARIOS",typeof(UsersControl)),
            };
            SelectedItem = MenuOptions[0];
        }

        [RelayCommand]
        private void ToggleMenu()
        {
            IsOpen = IsOpen ? true : false;
        }
    }
}
