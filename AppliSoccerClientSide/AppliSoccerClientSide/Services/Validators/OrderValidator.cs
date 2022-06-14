using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Text;

namespace AppliSoccerClientSide.Services.Validators
{
    public class OrderValidator
    {
        private const int MIN_TITLE_LENGTH = 1;
        private const int MIN_CONTENT_LENGTH = 1;
        //// TODO unify to one place to logic of ID and user name
        //private const int MIN_SENDER_ID_LENGTH = 1;
        private Order _order;

        public OrderValidator(Order orderToValidate)
        {
            _order = orderToValidate;
        }

        private bool IsNullOrder()
        {
            return _order == null;
        }

        public bool IsValidTitle()
        {
            if (IsNullOrder())
            {
                return false;
            }
            return _order.Title != null && _order.Title.Length >= MIN_TITLE_LENGTH;
        }

        public bool IsValidContent()
        {
            if (IsNullOrder())
            {
                return false;
            }
            return _order.Content != null && _order.Content.Length >= MIN_CONTENT_LENGTH;
        }

        public bool IsValidSendingDate()
        {
            if (IsNullOrder())
            {
                return false;
            }
            return _order.SendingDate != null;
        }

        public bool IsValidSenderId()
        {
            if (IsNullOrder())
            {
                return false;
            }
            return _order.SenderId != null && _order.SenderId.Length > 0;
        }

        public bool IsValidTeamId()
        {
            if (IsNullOrder())
            {
                return false;
            }
            return _order.TeamId != null && _order.TeamId.Length > 0;
        }

        public bool IsValidGameId()
        {
            // TODO - right now there is no support in game attachment to order
            return true;
        }

        public bool IsValidReceivers()
        {
            if (IsNullOrder())
            {
                return false;
            }
            bool isExistReceiver = _order.MemberIdsReceivers != null && _order.MemberIdsReceivers.Count > 0;
            bool isExistPlayerRoleReceiver = _order.RolesReceivers != null && _order.RolesReceivers.Count > 0;

            return isExistReceiver || isExistPlayerRoleReceiver;
        }
    }
}
