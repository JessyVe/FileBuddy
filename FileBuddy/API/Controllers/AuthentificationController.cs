using Microsoft.AspNetCore.Mvc;
using SharedRessources.DataAccess.Authentification;
using SharedRessources.DataAccess.UserAccess;
using SharedRessources.Dtos;
using SharedRessources.Services.TokenLogic;
using System;

namespace API.Controllers
{
    /// <summary>
    /// Provides user-specific API methods.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthentificationController : ControllerBase
    {
        private static readonly log4net.ILog Log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly ITokenService _tokenService;
        private readonly IAuthentificationService _authentificationService;
        private readonly IUserAccess _userAccess;

        public AuthentificationController()
        {
            _tokenService = new TokenService();
            _authentificationService = new AuthentificationService();
            _userAccess = new UserAccess();
        }

        /// <summary>
        /// Returns the user object with the assigned 
        /// user id needed for further transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public ActionResult<AppUser> RegisterUser(AppUser user)
        {
            Log.Debug("RegisterUser()-Method was called.");
            try
            {
                _tokenService.GenerateTokensForUser(user);               
            }
            catch (Exception ex)
            {
                var errorText = "Unable to create access token for new user.";
                Log.ErrorFormat(errorText, ex);

                return BadRequest(errorText);
            }

            _authentificationService.RegisterUser(user);

            if (user.Id == 0)
            {
                var errorText = "Unable to register user.";
                Log.Error(errorText);

                return BadRequest(errorText);
            }         

            Log.Debug("User was registered successfully!");
            return Ok(user);
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        [HttpPost]
        [Route("login/macaddress/{macAddress}")]
        public ActionResult<AppUser> LoginWithMacAddress(string macAddress)
        {
            Log.Debug("LoginWithMacAddress()-Method was called.");
            var appUser = _authentificationService.LoginWithMacAddress(macAddress);

            return AssignTokens(appUser);
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login/mailaddress")]
        public ActionResult<AppUser> LoginWithMailAddress([FromBody] AppUser user)
        {
            Log.Debug("LoginWithMailAddress()-Method was called.");
            var appUser = _authentificationService.LoginWithMailAddress(user.MailAddress, user.Password);

            return AssignTokens(appUser);
        }       

        /// <summary>
        /// Returns the user object with assigned tokens. 
        /// </summary>
        /// <param name="appUser"></param>
        /// <returns></returns>
        private ActionResult<AppUser> AssignTokens(AppUser appUser)
        {           
            if (appUser == null)
            {
                var errorText = "Unable to login user.";
                Log.Error(errorText);

                return BadRequest(errorText);
            }
            _tokenService.GenerateTokensForUser(appUser);

            var success = _userAccess.UpdateUserInformation(appUser);
            if (success)
            {
                var errorText = "Unable to save access tokens to database";
                Log.Error(errorText);

                return BadRequest(errorText);
            }

            Log.Debug("User was logged in successfully!");
            return appUser;
        }
    }
}
