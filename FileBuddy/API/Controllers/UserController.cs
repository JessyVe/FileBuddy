using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedRessources.DataAccess.UserAccess;
using SharedRessources.Dtos;
using SharedRessources.Services;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        [Route("update")]
        public IActionResult UpdateUserInformation([FromBody]AppUser user)
        {
            Log.Debug("UpdateUserInformation() - Method was called.");
            try
            {
                _userAccess.UpdateUserInformation(user);
                Log.Debug("User data was updated successfully.");
                return Accepted();
            }
            catch (Exception ex)
            {
                var errorText = "Update of user information failed.";

                Log.ErrorFormat(errorText, ex);
                return BadRequest(string.Format(ERROR_FORMAT, errorText, ex));
            }
        }

        /// <summary>
        /// Deletes a certain user from the database.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("delete")]
        public IActionResult DeleteUser([FromBody]AppUser user)
        {
            Log.Debug("DeleteUser() - Method was called.");
            try
            {
                var success = _userAccess.DeleteUser(user.Id);
                if (success)
                {
                    Log.Debug("User was deleted.");
                    return Accepted();
                }
            }
            catch (Exception ex)
            {
                var errorText = "User could not be deleted.";

                Log.ErrorFormat(errorText, ex);
                return BadRequest(string.Format(ERROR_FORMAT, errorText, ex));
            }
            return NotFound();
        }

        /// <summary>
        /// Returns a list of available files.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("fetchfiles/{userId}")]
        public string FetchAvailableFiles(int userId)
        {
            Log.Debug("FetchAvailableFiles() - Method was called.");
            var files = _userAccess.FetchAvailableFiles(userId);

            Log.Debug($"{files.Count} file(s)  were fetched for user.");
            return JsonConverter.GetAsJson(files);
        }
    }
}