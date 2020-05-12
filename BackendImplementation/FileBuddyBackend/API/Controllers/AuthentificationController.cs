using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedRessources.DataAccess.Authentification;
using SharedRessources.Dtos;

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

        private readonly IAuthentificationService _authentificationService;

        public AuthentificationController()
        {
            _authentificationService = new AuthentificationService();
        }

        /// <summary>
        /// Returns the user object with the assigned 
        /// user id needed for further transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        [AllowAnonymous]
        public ActionResult<AppUser> RegisterUser(AppUser user)
        {
            Log.Debug("RegisterUser()-Method was called.");
            var registeredUser = _authentificationService.RegisterUser(user).Result;

            if (registeredUser == null)
            {
                var errorText = "Unable to register user.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return registeredUser;
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="password"></param>
        [HttpPost]
        [Route("login/macaddress/{macAddress}")]
        [AllowAnonymous]
        public ActionResult<AppUser> LoginWithMacAddress([FromBody] AppUser user, string macAddress)
        {
            Log.Debug("LoginWithMacAddress()-Method was called.");
            var loggedInUser = _authentificationService.LoginWithMacAddress(macAddress, user.Password).Result;

            if (loggedInUser == null)
            {
                var errorText = "Unable to login user with MAC-Address.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return loggedInUser;
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
        [AllowAnonymous]
        public ActionResult<AppUser> LoginWithMailAddress([FromBody] AppUser user)
        {
            Log.Debug("LoginWithMailAddress()-Method was called.");
            var loggedInUser = _authentificationService.LoginWithMailAddress(user.MailAddress, user.Password).Result;

            if (loggedInUser == null)
            {
                var errorText = "Unable to login user with Mail-Address.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return loggedInUser;
        }

        /// <summary>
        /// Returns a refreshed authentification token.
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("refresh")]
        public ActionResult<AuthentificationToken> RefreshToken(AuthentificationToken token)
        {
            var refreshToken = _authentificationService.RefreshToken(token);

            if (refreshToken == null)
            {
                var errorText = "Refresh given token.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return refreshToken;
        }

        /// <summary>
        /// Revokes to token of the user sending the request.
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [Route("revoketoken")]
        public IActionResult Revoke()
        {
            var userId = User.Identity.Name;
            _authentificationService.RevokeToken(userId);

            return NoContent();
        }
    }
}
