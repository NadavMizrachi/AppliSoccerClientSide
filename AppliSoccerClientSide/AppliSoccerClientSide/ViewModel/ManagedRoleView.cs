using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace AppliSoccerClientSide.ViewModel
{
    public class ManagedRoleView : INotifyPropertyChanged
    {
        private bool _isManager;
        private bool _isVisible;
        private bool _isCheckBoxVisible;
        public String RoleName { get; set; }
        public bool IsManaged
        {
            get { return _isManager; }
            set
            {
                if(value == _isManager) { return; }
                _isManager = value;
                OnPropertyChanged("IsManaged");
            }
        }
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (value == _isVisible) { return; }
                _isVisible= value;
                OnPropertyChanged("IsVisible");
            }
        }
        public bool IsCheckBoxVisible
        {
            get { return _isCheckBoxVisible; }
            set
            {
                if (value == _isCheckBoxVisible) { return; }
                _isCheckBoxVisible = value;
                OnPropertyChanged("IsCheckBoxVisible");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public static List<ManagedRoleView> CreateManagedRoleViews(List<Role> managedRoles)
        {
            List<ManagedRoleView> res = new List<ManagedRoleView>();
            foreach (var role in Enum.GetValues(typeof(Role)).Cast<Role>())
            {
                bool isManagedRole = managedRoles.Contains(role);
                
                res.Add(
                    new ManagedRoleView()
                    {
                        RoleName = role.ToString(),
                        IsManaged = isManagedRole,
                        IsVisible = isManagedRole,
                        IsCheckBoxVisible = false

                    }
                );
            }
            return res;
        }

        public static void MarkAllAsVisibleForEditing(ObservableCollection<ManagedRoleView> roleViews)
        {
            foreach (var roleView in roleViews)
            {
                roleView.IsVisible = true;
                roleView.IsCheckBoxVisible = true;
            }
        }

    }
}
