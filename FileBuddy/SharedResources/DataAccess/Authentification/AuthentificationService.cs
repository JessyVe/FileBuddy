using Microsoft.EntityFrameworkCore;
using SharedRessources.Database;
using SharedRessources.Dtos;
using SharedRessources.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SharedRessources.DataAccess.Authentification
{
    public class AuthentificationService : IAuthentificationService
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

        private AuthentificationToken CreateAuthentificationToken(string userId)
        {
            return TokenGenerator.GenerateAuthentificationToken();
        }

        public AuthentificationToken RefreshToken(AuthentificationToken authentificationToken)
        {
            Log.Debug("Attempting to refresh authentification token.");
            throw new NotImplementedException();
        }

        public void RevokeToken(string userId)
        {
            Log.Debug("Attempting to revoke authentification token.");
            throw new NotImplementedException();
        }
    }
}
