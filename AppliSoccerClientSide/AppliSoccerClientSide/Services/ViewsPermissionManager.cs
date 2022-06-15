using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services
{
    /// <summary>
    /// This class reponsible to determine which views the user can see according to his
    /// identity.
    /// </summary>  
    class ViewsPermissionManager
    {

        public bool IsPermissionedForNewOrderButton { get; private set; }

        private static ViewsPermissionManager _viewsPermissionManager;
        private ViewsPermissionManager()
        {
        }

        public static ViewsPermissionManager CreateManager()
        {
            if(_viewsPermissionManager == null)
            {
                _viewsPermissionManager = new ViewsPermissionManager();
            }
            return _viewsPermissionManager;
        }


        public void  UpdateUserPermissions(TeamMember teamMember, AppShell appShell)
        {
            if(MemberTypeRecognizer.IsAdminMember(teamMember))
            {
                OnAdminLogin(appShell);
            }
            else
            {
                OnUnadminLogin(appShell, teamMember);
            }
        }

        private void OnUnadminLogin(AppShell appShell, TeamMember teamMember)
        {
            appShell.IsSchedulePageAllowed = true;
            appShell.IsOrdersPageAllowed = true;

            if (MemberTypeRecognizer.IsCoachMember(teamMember))
            {
                OnCoachLogin(appShell);
            }
            else if (MemberTypeRecognizer.IsStaff(teamMember))
            {
                OnStaffLogin(appShell);
            }
            
            else if (MemberTypeRecognizer.IsPlayer(teamMember))
            {
                OnPlayerLogin(appShell);
            }
        }

        private void OnPlayerLogin(AppShell appShell)
        {
            appShell.IsReceivedOrdersPageAllowed = true;
            appShell.IsSentOrdersPageAllowed = false;
            IsPermissionedForNewOrderButton = false;
        }

        private void OnStaffLogin(AppShell appShell)
        {
            appShell.IsSentOrdersPageAllowed = true;
            appShell.IsReceivedOrdersPageAllowed = true;
            IsPermissionedForNewOrderButton = true;
        }

        private void OnCoachLogin(AppShell appShell)
        {
            appShell.IsSentOrdersPageAllowed = true;
            appShell.IsReceivedOrdersPageAllowed = false;
            IsPermissionedForNewOrderButton = true;
        }

        private void OnAdminLogin(AppShell appShell)
        {
            appShell.IsSchedulePageAllowed = false;
            appShell.IsOrdersPageAllowed = false;
        }

    }
}
