using AppliSoccerClientSide.Models;
using AppliSoccerClientSide.Services;
using AppliSoccerClientSide.ViewModel;
using AppliSoccerClientSide.Views.ViewsUtil;
using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppliSoccerClientSide.Views.Orders
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class NewOrderPage : ContentPage
    {
        private HashSet<string> _reciversIds = new HashSet<string>();
        private HashSet<Role> _roleReceivers = new HashSet<Role>();
        private List<TeamMember> _myTeamMembers;

        public TeamMember MyMember { get; set; }
        public Order Order { get; set; }
        public ObservableCollection<ReceiverViewModel> StaffReceiversViewModel { get; set; }
        public ObservableCollection<ReceiverRoleViewModel> ReceiverRolesViewModel { get; set; }
        public ObservableCollection<ReceiverViewModel> PlayersReceiversViewModel { get; set; }


        public NewOrderPage()
        {
            InitializeComponent();
            InitMyMember();
            InitEmptyOrder();
            InitMyTeamStaffMembers();
            InitPlayersRoles();
            InitMyTeamPlayerMembers();
            BindingContext = this;
        }


        private void InitEmptyOrder()
        {
            Order = new Order();
        }
        private void InitMyMember()
        {
            MyMember = ApplicationGlobalData.GetMyTeamMember();
            if (!MemberTypeRecognizer.IsCoachMember(MyMember))
            {
                HideStaffReceiversExpander();
            }
        }

        private void HideStaffReceiversExpander()
        {
            ((Expander)staffExpander).IsVisible = false;
        }

        private void InitMyTeamStaffMembers()
        {
            StaffReceiversViewModel = new ObservableCollection<ReceiverViewModel>();
        }

        private void InitPlayersRoles()
        {
            ReceiverRolesViewModel = new ObservableCollection<ReceiverRoleViewModel>(); ;
            List<String> roleNames = GetRoleNames();
            roleNames.ForEach(roleName =>
            {
                ReceiverRolesViewModel.Add(new ReceiverRoleViewModel() { RoleName = roleName, IsShouldReceive = false });
            });
            playerRolesListView.ItemsSource = ReceiverRolesViewModel;
        }

        private List<string> GetRoleNames()
        {
            if (!MemberTypeRecognizer.IsCoachMember(MyMember))
            {
                // Filter only roles that managed bu this staff member
                return ((StaffAdditionalInfo)MyMember.AdditionalInfo).ManagedRoles.Select(role => role.ToString()).ToList();
            }
            else
            {
                // Is Coach, return all roles
                return Enums.RoleNames;
            }
        }

        private void InitMyTeamPlayerMembers()
        {
            PlayersReceiversViewModel = new ObservableCollection<ReceiverViewModel>();
        }

        private bool _wasAppeared = false;
        protected override async void OnAppearing()
        {
            base.OnAppearing();
            if (_wasAppeared)
            {
                return;
            }
            _wasAppeared = true;
            await PullMyTeamMembersFromServer();
            if (MemberTypeRecognizer.IsCoachMember(MyMember))
            {
                AddTeamStaffMembersToStaffReceivers();
            }
            AddTeamPlayerMembersToPlayerReceivers();
        }
        
        private async Task PullMyTeamMembersFromServer()
        {
            _myTeamMembers =  await AppliSoccerServerService.AppServer.PullTeamMembers(MyMember.TeamId);
        }

        private void AddTeamStaffMembersToStaffReceivers()
        {
            _myTeamMembers.ForEach(member =>
            {
               if (MemberTypeRecognizer.IsStaff(member) && !MemberTypeRecognizer.IsCoachMember(member))
               {
                    StaffReceiversViewModel.Add(ReceiverViewModel.FromTeamMember(member));
               };
            });
        }

        private void AddTeamPlayerMembersToPlayerReceivers()
        {
            if (MemberTypeRecognizer.IsCoachMember(MyMember))
            {
                AddTeamPlayersToPlayerReceiversForCoach();
            }
            else 
            {
                AddTeamPlayersToPlayerReceiversForStaff();
            }
        }

        private void AddTeamPlayersToPlayerReceiversForCoach()
        {
            _myTeamMembers.ForEach(member =>
            {
                if (MemberTypeRecognizer.IsPlayer(member))
                {
                    PlayersReceiversViewModel.Add(ReceiverViewModel.FromTeamMember(member));
                };
            });
        }
        private void AddTeamPlayersToPlayerReceiversForStaff()
        {
            _myTeamMembers.ForEach(member =>
            {
                if (MemberTypeRecognizer.IsPlayer(member))
                {
                    var staffManagedRoles = ((StaffAdditionalInfo)MyMember.AdditionalInfo).ManagedRoles;
                    var roleOfPlayer = ((PlayerAdditionalInfo)member.AdditionalInfo).Role;
                    if (staffManagedRoles.Contains(roleOfPlayer))
                    {
                        PlayersReceiversViewModel.Add(ReceiverViewModel.FromTeamMember(member));
                    }
                };
            });
        }


        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView lv) lv.SelectedItem = null;
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            PrepareOrderForSending();
            bool isValid = await ValidateOrderDetails(Order);
            if (isValid)
            {
                await SendOrderToServer(Order);
            }
        }

        private void PrepareOrderForSending()
        {
            // Title & Content binded through user UI
            Order.MemberIdsReceivers = GetReceiversIdList();
            Order.MembersIdsAlreadyRead = new List<string>();
            Order.RolesReceivers = GetRolesReceiversList();
            Order.SenderId = MyMember.ID;
            Order.SendingDate = DateTime.Now;
            Order.TeamId = MyMember.TeamId;
        }
        private List<string> GetReceiversIdList()
        {
            List<string> output = new List<string>();
            List<ReceiverViewModel> allReceiverModels = new List<ReceiverViewModel>();
            allReceiverModels.AddRange(StaffReceiversViewModel.ToList());
            allReceiverModels.AddRange(PlayersReceiversViewModel.ToList());
            allReceiverModels.ToList().ForEach(staffModel =>
           {
               if (staffModel.IsShouldReceive)
               {
                   output.Add(staffModel.MemberId);
               }
           });
            return output;
        }
        private List<Role> GetRolesReceiversList()
        {
            List<Role> output = new List<Role>();
            ReceiverRolesViewModel.ToList().ForEach(roleModel =>
            {
                if (roleModel.IsShouldReceive)
                {
                    Role role = (Role)Enum.Parse(typeof(Role), roleModel.RoleName);
                    output.Add(role);
                }
            });
            return output;
        }

        

        private Task<bool> ValidateOrderDetails(Order order)
        {
            return UIOrderValidator.Validate(order, this);
        }

        private async Task SendOrderToServer(Order order)
        {
            try
            {
                bool isSucceed = await AppliSoccerServerService.AppServer.CreateOrder(order);
                if (isSucceed)
                {
                    await DisplayAlert("Success!", "Order has been sent successfully!", "ok");
                    await Navigation.PopAsync();
                }
                else
                {
                    await DisplayAlert("Error!", "Order was not sent !", "ok");
                }
            }
            catch(Exception ex)
            {
                await DisplayAlert("Error!", "Order was not sent !", "ok");
            }
            return;
        }

        
        
    }
}