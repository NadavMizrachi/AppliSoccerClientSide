using AppliSoccerClientSide.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace AppliSoccerClientSide
{
    public partial class AppShell : Xamarin.Forms.Shell
    {

        public AppShell()
        {
            BindingContext = this;
            InitializeComponent();
            InitRoutes();
        }

        private void InitRoutes()
        {
            Routing.RegisterRoute($"{nameof(StaffMembersPage)}", typeof(StaffMembersPage));
            Routing.RegisterRoute($"{nameof(PlayersPage)}", typeof(PlayersPage));
        }
        
        public bool IsSchedulePageAllowed
        {
            get => (bool)GetValue(IsSchedulePageAllowedProperty);
            set => SetValue(IsSchedulePageAllowedProperty, value);
        }

        public static readonly BindableProperty IsSchedulePageAllowedProperty =
            BindableProperty.Create("IsSchedulePageAllowed", typeof(bool), typeof(AppShell), false);

    }
}
