using Microsoft.EntityFrameworkCore;
using SharedResources.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using SharedResources.Database;
using SharedResources.Dtos;

namespace SharedResources.DataAccess.Authentication
{
    public class AuthenticationService : IAuthenticationService
    {
        private static readonly log4net.ILog Log =
                log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public AppUser RegisterUser(AppUser user)
        {
            Log.Debug("Attempting to register user.");

            using (var context = new SQLiteDBContext())
            {
                try
                {
                    context.AppUser.Add(user);
                    context.Entry(user).State = EntityState.Added;
                    context.ChangeTracker.DetectChanges();
                    context.SaveChanges();
                } catch(Exception ex)
                {
                    Log.ErrorFormat("Unable to save user object to database. ", ex);
                }
            }
            return user;
        }

        public AppUser LoginWithMacAddress(string macAddress)
        {
            Log.Debug("Attempting to login user with mail address.");
            using (var context = new SQLiteDBContext())
            {
                foreach(var user in context.AppUser)
                {
                    if (JsonConverter.GetObjectFromJson<List<UserDevice>>(user.UserDevices)
                        .Any(device => device.MacAddress.Equals(macAddress)))
                        return user;
                }
            }
            var ex = new Exception("User with specified MAC address was not found or password is invalid!");
            Log.ErrorFormat("Login Failed.", ex);
            throw ex;
        }

        public AppUser LoginWithMailAddress(string mailAddress, string password)
        {
            Log.Debug("Attempting to login user with mail address.");
            using (var context = new SQLiteDBContext())
            {
                foreach (var user in context.AppUser)
                {
                    if (!user.Password.Equals(password) || !user.MailAddress.Equals(mailAddress))
                        continue;

                    return user;
                }
            }
            var ex = new Exception("User with specified mailAddress was not found or password is invalid!");
            Log.ErrorFormat("Login Failed.", ex);
            throw ex;
        }

        public bool MailAddressAlreadyInUse(string mailAddress)
        {
            Log.Debug("Checking if mail address is already in use.");

            using (var context = new SQLiteDBContext())
            {
                try
                {
                    return context.AppUser.Any(user => user.MailAddress.Equals(mailAddress));
                }
                catch (Exception ex)
                {
                    Log.ErrorFormat("Unable to access database. ", ex);
                }
            }
            return false;
        }
    }
}
