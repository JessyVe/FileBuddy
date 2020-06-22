using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http.Headers;
using SharedResources.DataAccess.FileDataAccess;
using SharedResources.Dtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
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
        /// defined users.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="receiverId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("upload/{userId}/{receiverId}")]
        public IActionResult Upload(int userId, int receiverId)
        {
            Log.Debug("Upload() - Method was called.");
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
                Log.Debug("File upload was completed. ");
                _fileDataAccess.UploadFile(sharedFile, new List<int>() { receiverId });

                return Ok();
            }
            catch (Exception ex)
            {
                Log.ErrorFormat("Error while uploading file.", ex);
                return StatusCode(500, $"Internal server error: {ex}");
            }
        }

        /// <summary>
        /// Returns the file save under the given path. 
        /// </summary>
        /// <param name="downloadRequest"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("download")]
        public IActionResult Download(DownloadRequest downloadRequest)
        {
            Log.Debug("Download() - Method was called.");

            var apiPath = _fileDataAccess.GetApiPathOfFile(downloadRequest.SharedFileId);

            if (string.IsNullOrEmpty(apiPath))
            {
                var errorMessage = "File with given Id was not found!";
                Log.Error(errorMessage);
                return NotFound(errorMessage);
            }

            var fileProvider = new FileExtensionContentTypeProvider();
            if (!fileProvider.TryGetContentType(apiPath, out string contentType))
            {
                var errorText = $"Unable to find Content Type for file name {apiPath}.";

                Log.Error(errorText);
                return BadRequest(errorText);
            }
            _fileDataAccess.FileDownloaded(new DownloadTransaction()
            {
                DownloadDate = DateTime.Now, 
                ReceiverUserId = downloadRequest.ReceiverId,
                SharedFileId = downloadRequest.SharedFileId
            }); 
            Log.Debug("Successfully initialized download. Requested file will be returned.");

            return PhysicalFile(apiPath, contentType, Path.GetFileName(apiPath));
        }
    }
}
