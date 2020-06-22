using System;
using System.Net.Mail;

namespace SharedResources.Services
{
    /// <summary>
    /// Contains methods to validate the correctness of the given data. 
    /// </summary>
    public static class DataValidator
    {
        public static bool IsMailAddressValid(string emailAddress)
        {
            try
            {
                _ = new MailAddress(emailAddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}
