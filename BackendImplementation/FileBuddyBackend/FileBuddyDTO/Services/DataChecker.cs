using System;
using System.Net.Mail;

namespace SharedRessources.Services
{
    public static class DataChecker
    {
        public static bool IsValid(string emailaddress)
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
