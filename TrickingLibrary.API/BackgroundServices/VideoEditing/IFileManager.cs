using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrickingLibrary.API.Settings;

namespace TrickingLibrary.API.BackgroundServices.VideoEditing
{
    public interface IFileManager
    {
        string GetFFMPEGPath();
        string GetFileUrl(string fileName, FileType fileType);
        string TemporarySavePath(string messageInput);
        bool TemporaryFileExists(string outputConvertedName);
        void DeleteTemporaryFile(string outputConvertedName);
        string GetSavePath(string fileName);
        Task<string> SaveTemporaryFile(IFormFile video);
        bool Temporary(string fileName);
    }
}
