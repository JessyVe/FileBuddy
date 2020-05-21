using System;
using System.Net.Mail;

namespace SharedRessources.Services
{
    public static class DataValidator
    {
        public static bool IsMailAddressValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
