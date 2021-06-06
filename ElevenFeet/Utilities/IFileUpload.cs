using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Utilities
{
    public enum FileResult
    {
        Succeded = 1,
        Error = 2
    }
    public interface IFileUpload
    {
        public FileUploadResult Upload(IFormFile file);

    }

    public class FileUploadResult
    {
        public string FileUrl { get; set; }

        public string OriginalName { get; set; }

        public FileResult FileResult { get; set; }

        public string Message { get; set; }
    }
}
