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
        [Route("update/user/{user}")]
        public IActionResult UpdateUserInformation(FullUserData user)
        {
            try
            {
                var success = _userAccess.UpdateUserInformation(user).Result;
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
        /// Updates the group informations for an user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonGroupInformation"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("update/groups/{userId}/{userGroups}")]
        public IActionResult UpdateGroupInformationOfUser(string userId, IList<UserGroup> userGroups)
        {
            try
            {
                var success = _userAccess.UpdateGroupInformationOfUser(userId, userGroups).Result;
                if (success)
                {
                    return Accepted();
                }
            }
            catch (Exception ex)
            {
                var errorText = "Update of group information failed!";

                Log.ErrorFormat(errorText, ex);
                return BadRequest(string.Format(ERROR_FORMAT, errorText, ex));
            }
            return NotFound();
        }

        /// <summary>
        /// Returns a a collection containing user groups 
        /// created by the user. With this method user groups
        /// can be synchronized over all devices.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonGroupInformation"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fetch/groups/{userId}")]
        public ActionResult<IList<UserGroup>> GetGroupInformation(string userId)
        {
            var userGroups = _userAccess.GetGroupInformationOfUser(userId).Result;
            if (userGroups == null)
            {
                var errorText = "Unable to retrieve group information of user.";

                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return userGroups;
        }

        /// <summary>
        /// Returns a dictionary containing all available files for the user. 
        /// (Key=userId of sender; Value=list of file names)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fetch/files/{userId}")]
        public ActionResult<List<SharedFile>> FetchAvailableFiles(string userId)
        {
            var availableFiles = _userAccess.FetchAvailableFiles(userId).Result;
            if (availableFiles == null)
            {
                var errorText = "Unable to retrieve group information of user.";

                Log.Error(errorText);
                return BadRequest(errorText);
            }
            return availableFiles;
        }
    }
}