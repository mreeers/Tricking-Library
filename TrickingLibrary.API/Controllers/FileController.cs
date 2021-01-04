using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TrickingLibrary.API.BackgroundServices.VideoEditing;
using TrickingLibrary.API.Settings;

namespace TrickingLibrary.API.Controllers
{
    [Route("api/files")]
    public class FileController : ControllerBase
    {
        private readonly IFileManager fileManagerLocal;

        public FileController(IFileManager fileManagerLocal)
        {
            this.fileManagerLocal = fileManagerLocal;
        }

        [HttpGet("{type}/{file}")]
        public IActionResult GetVideo(string type, string file)
        {
            var mime = type.Equals(nameof(FileType.Image), StringComparison.InvariantCultureIgnoreCase)
                ? "image/jpg" : type.Equals(nameof(FileType.Video), StringComparison.InvariantCultureIgnoreCase)
                ? "video/mp4" : null;

            if (mime == null)
                return BadRequest();

            var savePath = fileManagerLocal.GetSavePath(file);

            if (string.IsNullOrEmpty(savePath))
                return BadRequest();

            return new FileStreamResult(new FileStream(savePath, FileMode.Open, FileAccess.Read), mime);
        }

        [HttpPost]
        public Task<string> UploadVideo(IFormFile video)
        {
            return fileManagerLocal.SaveTemporaryFile(video);
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteTemporaryVideo(string fileName)
        {
            if (fileManagerLocal.Temporary(fileName))
            {
                return BadRequest();
            }

            if (!fileManagerLocal.Temporary(fileName))
            {
                return NoContent();
            }

            fileManagerLocal.DeleteTemporaryFile(fileName);

            return Ok();
        }
    }
}
