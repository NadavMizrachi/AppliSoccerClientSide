using AppliSoccerObjects.Modeling;
using Xamarin.Forms;
using System.Threading.Tasks;
using AppliSoccerClientSide.Services;

namespace AppliSoccerClientSide.Views.ViewsUtil
{
    public class UITeamMemberValidator
    {
        public static async Task<bool> ValidateNewUser(User userToValidate, Page page)
        {
            RegistrationDetailsValidator validator =
                new RegistrationDetailsValidator(userToValidate.Username, userToValidate.Password, null);
            if (!validator.IsValidUsername())
            {
                await page.DisplayAlert("Details Error", "Username is not valid!", "cancel");
                return false;
            }
            if (!validator.IsValidPassword())
            {
                await page.DisplayAlert("Details Error", "Password is not valid!", "cancel");
                return false;
            }

            TeamMemberValidator teamMemberValidator = GetConcreteValidator(userToValidate.TeamMember);

            if (!teamMemberValidator.isValidFirstName())
            {
                await page.DisplayAlert("Details Error", "First Name is not valid!", "cancel");
                return false;
            }

            if (!teamMemberValidator.isValidLastName())
            {
                await page.DisplayAlert("Details Error", "Last Name is not valid!", "cancel");
                return false;
            }

            if (!teamMemberValidator.isValidPhoneNumber())
            {
                await page.DisplayAlert("Details Error", "Phone number is not valid!", "cancel");
                return false;
            }

            if (!teamMemberValidator.IsValidAdditionalInfo())
            {
                await page.DisplayAlert("Details Error", "Additional info error", "cancel");
                return false;
            }

            return true;
        }

        private static TeamMemberValidator GetConcreteValidator(TeamMember teamMember)
        {
            if(teamMember.MemberType == MemberType.Staff)
            {
                return new StaffMemberValidator(teamMember);
            }
            if(teamMember.MemberType == MemberType.Player)
            {
                return new PlayerMemberValidator(teamMember);
            }
            return null;
        }
    }
}
