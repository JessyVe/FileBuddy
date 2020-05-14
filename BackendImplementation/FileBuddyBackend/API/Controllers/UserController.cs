using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using SharedRessources.DataAccess.UserAccess;
using SharedRessources.Dtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static readonly log4net.ILog Log =
           log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ERROR_FORMAT = "{0};{1}";
        private readonly IUserAccess _userAccess;

        public UserController()
        {
            _userAccess = new UserAccess();
        }

        /// <summary>
        /// Updates user-specific settings.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update/{user}")]
        public IActionResult UpdateUserInformation(AppUser user)
        {
            try
            {
                var success = _userAccess.UpdateUserInformation(user);
                if (success)
                {
                    return Accepted();
                }
            }
            catch (Exception ex)
            {
                var errorText = "Update of user information failed!";

                Log.ErrorFormat(errorText, ex);
                return BadRequest(string.Format(ERROR_FORMAT, errorText, ex));
            }
            return NotFound();
        }

        /// <summary>
        /// Returns a dictionary containing all available files for the user. 
        /// (Key=userId of sender; Value=list of file names)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fetchfiles/{userId}")]
        public IList<SharedFile> FetchAvailableFiles(int userId)
        {
            return _userAccess.FetchAvailableFiles(userId);
        }
    }
}