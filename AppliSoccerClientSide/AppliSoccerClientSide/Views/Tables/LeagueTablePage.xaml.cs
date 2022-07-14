using AppliSoccerClientSide.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Tables
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LeagueTablePage : ContentPage
    {
        public LeagueTableViewModel Table { get; set; }
        public LeagueTablePage(LeagueTableViewModel tableViewModel)
        {
            InitializeComponent();
            Table = tableViewModel;
            Title = tableViewModel.SubLeagueDescription;
            BindingContext = this;
        }

        private void listViewm_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private void listViewm_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }
    }
}