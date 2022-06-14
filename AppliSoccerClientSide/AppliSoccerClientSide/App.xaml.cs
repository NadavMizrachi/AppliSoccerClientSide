using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.Views;
using System;
using System.Diagnostics;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide
{
    public partial class App : Application
    {

        // TODO - Create reusable xaml controls/contents
        public App()
        {
            InitializeComponent();

            //DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            Debug.WriteLine("On Start!!!");
        }

        protected override void OnSleep()
        {
            Debug.WriteLine("On Sleep!!!");
        }

        protected override void OnResume()
        {
            Debug.WriteLine("On Resume!!!");
        }
    }
}
