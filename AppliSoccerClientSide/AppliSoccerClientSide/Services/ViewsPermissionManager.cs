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
        private AppShell _appShell;
        private static ViewsPermissionManager _viewsPermissionManager;
        private ViewsPermissionManager(AppShell appShell)
        {
            _appShell = appShell;
        }

        public static ViewsPermissionManager CreateManager(AppShell appShell)
        {
            if(_viewsPermissionManager == null)
            {
                _viewsPermissionManager = new ViewsPermissionManager(appShell);
            }
            return _viewsPermissionManager;
        }


        public void  UpdateUserPermissions(TeamMember teamMember)
        {
            if(teamMember.MemberType == MemberType.Admin)
            {
                OnAdminLogin();
            }
            else
            {
                OnUnadminLogin();
            }
        }

        private void OnUnadminLogin()
        {
            _appShell.IsSchedulePageAllowed = true;
        }

        private void OnAdminLogin()
        {
            _appShell.IsSchedulePageAllowed = false;
        }

    }
}
