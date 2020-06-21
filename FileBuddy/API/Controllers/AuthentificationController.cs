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
        public ActionResult<AppUser> RegisterUser(AppUser user)
        {
            Log.Debug("RegisterUser()-Method was called.");
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
            var loggedInUser = _authentificationService.LoginWithMacAddress(macAddress);

            if (loggedInUser == null)
            {
                var errorText = "Unable to login user with MAC-Address.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            Log.Debug("User was logged in successfully!");
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
        public ActionResult<AppUser> LoginWithMailAddress([FromBody] AppUser user)
        {
            Log.Debug("LoginWithMailAddress()-Method was called.");
            var loggedInUser = _authentificationService.LoginWithMailAddress(user.MailAddress, user.Password);

            if (loggedInUser == null)
            {
                var errorText = "Unable to login user with Mail-Address.";
                Log.Error(errorText);
                return BadRequest(errorText);
            }
            Log.Debug("User was logged in successfully!");
            return loggedInUser;
        }       
    }
}
