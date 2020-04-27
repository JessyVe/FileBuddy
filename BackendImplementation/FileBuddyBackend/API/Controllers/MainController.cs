using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SharedRessources.DataAccess;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace API.Controllers
{
    [Route("api/filebuddy")]
    [ApiController]
    public class MainController : ControllerBase
    {
        private static readonly log4net.ILog Log =
            log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly string ERROR_FORMAT = "{0};{1}";

        private readonly IDatabaseAccess _dataAccess;
        private readonly IFileAccess _fileAccess;

        public MainController()
        {
            _dataAccess = new FirebaseDatabaseAccess();
            _fileAccess = new FtpAccess();
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="macAddress"></param>
        /// <param name="password"></param>
        [Route("login/macaddress/{macAddress}/{password}")]
        public User LoginWithMacAddress(string macAddress, string password)
        {
            var result = _dataAccess.LoginWithMacAddress(macAddress, password).Result;
            return result;
        }

        /// <summary>
        /// Returns the current user object to 
        /// ensure that it is synchronised over all devices. 
        /// </summary>
        /// <param name="mailAddress"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        [Route("login/mailaddress/{mailAddress}/{password}")]
        public User LoginWithMailAddress(string mailAddress, string password)
        {
            var result = _dataAccess.LoginWithMailAddress(mailAddress, password).Result;
            return result;
        }

        /// <summary>
        /// Returns the user object with the assigned 
        /// user id needed for further transactions.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [HttpPost("register"), DisableRequestSizeLimit]
        public User RegisterUser(User user)
        {
            Log.Debug("RegisterUser()-Method was called.");
            var result = _dataAccess.RegisterUser(user).Result;
            return result;
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        [Route("upload")]//{userId}/{userGroups}")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload()// string userId, string filename, IList<UserGroup> userGroups)
        {
            try
            {
                var file = Request.Form.Files[0];
                var folderName = "Resources";
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var tmpFullPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(tmpFullPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    _fileAccess.UploadFile(tmpFullPath, Path.Combine("test", Path.GetFileName(fileName)));

                    return Ok(new { dbPath });
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        [HttpGet("download/{filename}/")]//{userId}/{filehashes}")]
        public IActionResult Download(string filename)//string userId, IList<string> filehashes)
        {
            // Retrieve some file from some service
            var folderName = "Resources";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), folderName, filename);//.Replace(new char[] { '}', '{'}, ''));

            var fileProvider = new FileExtensionContentTypeProvider();

            if (!fileProvider.TryGetContentType(filePath, out string contentType))
            {
                throw new ArgumentOutOfRangeException($"Unable to find Content Type for file name {filePath}.");
            }

            var bytes = System.IO.File.ReadAllBytesAsync(filePath);
            return PhysicalFile(filePath, contentType, Path.GetFileName(filePath));
        }

        /// <summary>
        /// Returns a dictionary containing all available files for the user. 
        /// (Key=userId of sender; Value=list of file names)
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Route("fetch/files/{userId}")]
        public IList<SharedFile> FetchAvailableFiles(string userId)
        {
            var result = _dataAccess.FetchAvailableFiles(userId).Result;
            return result;
        }

        /// <summary>
        /// Updates user-specific settings.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Route("update/user/{user}")]
        public IActionResult UpdateUserInformation(User user)
        {
            try
            {
                var success = _dataAccess.UpdateUserInformation(user).Result;
                if (success)
                {
                    return Accepted();
                }
            }
            catch (Exception ex)
            {
                var errorText = "Update of user information failed!";
                Log.ErrorFormat(errorText, ex);
                BadRequest(string.Format(ERROR_FORMAT, errorText, ex));
            }
            return NotFound();
        }

        /// <summary>
        /// Updates the group informations for an user.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="jsonGroupInformation"></param>
        /// <returns></returns>
        [Route("update/groups/{userId}/{userGroups}")]
        public IActionResult UpdateGroupInformationOfUser(string userId, IList<UserGroup> userGroups)
        {
            try
            {
                var success = _dataAccess.UpdateGroupInformationOfUser(userId, userGroups).Result;
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
        [Route("fetch/groups/{userId}")]
        public IList<UserGroup> GetGroupInformation(string userId)
        {
            var result = _dataAccess.GetGroupInformationOfUser(userId).Result;
            return result;
        }
    }
}
