
using System;
using System.Text.RegularExpressions;

namespace ServiceAppApi.Utilities
{
    public static class Utilities
    {
        public static readonly string EMAIL_REGX = @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-0-9a-z]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        public static readonly string PHONE_NUMBER_REGX = @"^[0-9]{10}$";

        #region IsValidEmail
        public static bool IsValidEmail(string email)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(email))
            {
                isValid = Regex.IsMatch(email, EMAIL_REGX, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(200));
            }
            return isValid;
        }
        #endregion

        #region IsValidPhoneNumber
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            bool isValid = false;
            if (!string.IsNullOrWhiteSpace(phoneNumber))
            {
                isValid = Regex.IsMatch(phoneNumber, PHONE_NUMBER_REGX, RegexOptions.None, TimeSpan.FromMilliseconds(200));
            }
            return isValid;
        }

        #endregion
    }
}
