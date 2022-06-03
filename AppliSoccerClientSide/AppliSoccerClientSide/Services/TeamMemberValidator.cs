using AppliSoccerObjects.Modeling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AppliSoccerClientSide.Services
{
    public class TeamMemberValidator
    {
        protected TeamMember _teamMember;
        public TeamMemberValidator(TeamMember teamMember)
        {
            _teamMember = teamMember;
        }

        public bool isValidFirstName()
        {
            var firstName = _teamMember.FirstName;
            return firstName != null &&
                firstName.Length > 0 &&
                IsAllLetters(firstName);
        }

        public bool isValidLastName()
        {
            var lastName = _teamMember.LastName;
            return lastName != null &&
                lastName.Length > 0 &&
                IsAllLetters(lastName);
        }

        public bool isValidPhoneNumber()
        {
            var phoneNumber = _teamMember.PhoneNumber;
            return phoneNumber != null &&
                phoneNumber.Length > 6 &&
                (IsAllDigits(phoneNumber) || (phoneNumber.StartsWith("+") && IsAllDigits(phoneNumber.Substring(1))));
        }

        private bool IsAllDigits(string str)
        {
            return str.All(Char.IsDigit);
        }

        private bool IsAllLetters(string str)
        {
            return Regex.IsMatch(str, @"^[a-zA-Z]+$");
        }

        public bool IsValidBirthDate()
        {
            return _teamMember.BirthDate != null;
        }

        public virtual bool IsValidAdditionalInfo()
        {
            return _teamMember.AdditionalInfo != null;
        }

    }
}
