using AppliSoccerClientSide.Views;
using AppliSoccerClientSide.Views.Orders;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
            Routing.RegisterRoute($"{nameof(SentOrdersPage)}", typeof(SentOrdersPage));
            Routing.RegisterRoute($"{nameof(ReceivedOrdersPage)}", typeof(ReceivedOrdersPage));
        }
        
        public bool IsSchedulePageAllowed
        {
            get => (bool)GetValue(IsSchedulePageAllowedProperty);
            set => SetValue(IsSchedulePageAllowedProperty, value);
        }

        public static readonly BindableProperty IsSchedulePageAllowedProperty =
            BindableProperty.Create("IsSchedulePageAllowed", typeof(bool), typeof(AppShell), false);

        public bool IsOrdersPageAllowed
        {
            get => (bool)GetValue(IsOrdersPageAllowedProperty);
            set => SetValue(IsOrdersPageAllowedProperty, value);
        }

        public static readonly BindableProperty IsOrdersPageAllowedProperty =
            BindableProperty.Create("IsOrdersPageAllowed", typeof(bool), typeof(AppShell), false);


        public bool IsSentOrdersPageAllowed
        {
            get => (bool)GetValue(IsSentOrdersPageAllowedProperty);
            set => SetValue(IsSentOrdersPageAllowedProperty, value);
        }

        public static readonly BindableProperty IsSentOrdersPageAllowedProperty =
            BindableProperty.Create("IsSentOrdersPageAllowed", typeof(bool), typeof(AppShell), false);


        public bool IsReceivedOrdersPageAllowed
        {
            get => (bool)GetValue(IsReceivedOrdersPageAllowedProperty);
            set => SetValue(IsReceivedOrdersPageAllowedProperty, value);
        }

        public static readonly BindableProperty IsReceivedOrdersPageAllowedProperty =
            BindableProperty.Create("IsReceivedOrdersPageAllowed", typeof(bool), typeof(AppShell), false);
        

        private async void LogoutClicked(object sender, EventArgs e)
        {
            ApplicationGlobalData.Insert(null);
            
            // Clear all app data:
            Application.Current.MainPage = new AppShell();
            
            await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
        }
    }
}
