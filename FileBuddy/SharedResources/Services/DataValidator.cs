using System;
using System.Net.Mail;

namespace SharedRessources.Services
{
    /// <summary>
    /// Contains methods to validate the correctness of the given data. 
    /// </summary>
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
