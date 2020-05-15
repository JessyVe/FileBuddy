using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using SharedRessources.DataAccess.FileDataAccess;
using SharedRessources.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private static readonly log4net.ILog Log =
             log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly IFileDataAccess _fileDataAccess;
        private const string folderName = "Resources";

        public FileController()
        {
            _fileDataAccess = new FileDataAccess();
        }

        /// <summary>
        /// Uploads given files and makes them available for
        /// definited users.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="userGroups"></param>
        /// <returns></returns>
        [Route("upload/{userId}/{receiverId}")]
        [HttpPost]
        public IActionResult Upload(int userId, int receiverId)
        {
            try
            {
                var file = Request.Form.Files[0];
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
                var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                var apiPath = Path.Combine(pathToSave, fileName);

                using (var stream = new FileStream(apiPath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                var sharedFile = new SharedFile()
                {
                    ApiPath = apiPath,
                    OwnerUserId = userId, 
                    SharedFileName = fileName, 
                    UploadDate = DateTime.Now
                };
                 _fileDataAccess.UploadFile(sharedFile, new List<int>() { receiverId });

                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Returns the file save under the given path. 
        /// </summary>
        /// <param name="apiPath"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("download/{apiPath}")]
        public IActionResult Download(string apiPath)
        {
            var fileProvider = new FileExtensionContentTypeProvider();

            if (!fileProvider.TryGetContentType(apiPath, out string contentType))
            {
                var errorText = $"Unable to find Content Type for file name {apiPath}.";

                Log.Error(errorText);
                return BadRequest(errorText);
            }

            _fileDataAccess.FileDownloaded(new DownloadTransaction()); // TODO: Create actual object

            return PhysicalFile(apiPath, contentType, Path.GetFileName(apiPath));
        }
    }
}
