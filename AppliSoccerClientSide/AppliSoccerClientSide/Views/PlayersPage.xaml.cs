using AppliSoccerObjects.Modeling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayersPage : ContentPage
    {
        public TeamMember TeamMember { get; set; }
        public PlayersPage()
        {
            InitializeComponent();
            TeamMember = GetTeamMemberObject();
            BindingContext = this;
        }

        private TeamMember GetTeamMemberObject()
        {
            TeamMember teamMember = null;
            if (Application.Current.Properties.ContainsKey(AppPropertiesConsts.TeamMemberKey))
            {
                string teamMemberAsJson = Application.Current.Properties[AppPropertiesConsts.TeamMemberKey].ToString();
                Debug.WriteLine("Team member as json : " + teamMemberAsJson);
                teamMember = JsonConvert.DeserializeObject<TeamMember>(teamMemberAsJson);
            }
            else
            {
                Debug.WriteLine("Application.Current.Properties does not have " + AppPropertiesConsts.TeamMemberKey + " key");
            }
            return teamMember;
        }


    }
}