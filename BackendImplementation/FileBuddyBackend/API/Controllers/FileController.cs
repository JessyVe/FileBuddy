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
        [Route("upload/{userId}/{userGroups}")]
        [HttpPost, DisableRequestSizeLimit]
        public IActionResult Upload(string userId, IList<UserGroup> userGroups)
        {
            try
            {
                var file = Request.Form.Files[0];
                var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

                if (file.Length > 0)
                {
                    var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var apiPath = Path.Combine(pathToSave, fileName);
                    var dbPath = Path.Combine(folderName, fileName);

                    using (var stream = new FileStream(apiPath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }
                    // TODO: Initialize new SharedFile object
                    // _fileDataAccess.UploadFile(apiPath, userId, userGroups);

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

        /// <summary>
        /// Returns the file assigned to given file hash. 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="fileHash"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("download/{userId}/{fileHash}")]
        public IActionResult Download(string userId, string fileHash)
        {
            var apiPath = _fileDataAccess.GetApiPathOfFile(fileHash).Result;
            var fileProvider = new FileExtensionContentTypeProvider();

            if (!fileProvider.TryGetContentType(apiPath, out string contentType))
            {
                var errorText = $"Unable to find Content Type for file name {apiPath}.";

                Log.Error(errorText);
                return BadRequest(errorText);
            }

            _fileDataAccess.FileDownloaded(new DownloadTransaction()
            {
                FileHashId = fileHash, 
                DowloadUserHashId = userId, 
                TransactionDate = DateTime.Now
            });
            return PhysicalFile(apiPath, contentType, Path.GetFileName(apiPath));
        }
    }
}
