using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ElevenFeet.Utilities
{

    public class FileSystemFileUploader : IFileUpload
    {
        private readonly string _filePath;

        //if user select different location
        public FileSystemFileUploader(string filePath)
        {
            _filePath = filePath;
        }

        //default image save location
        public FileSystemFileUploader()
        {
            this._filePath = "images";
        }

        public FileUploadResult Upload(IFormFile file)
        {
            FileUploadResult result = new FileUploadResult();
            result.FileResult = FileResult.Error;
            result.Message = "An error occurred during file upload !";

            if (file.Length > 0)
            {
                //.jpg /.png
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                result.OriginalName = file.FileName;
                var phsycalPath = Path.Combine(Directory.GetCurrentDirectory(), "Images", _filePath, fileName);

                if (File.Exists(phsycalPath))
                {
                    result.Message = "There is such a file in the directory.";
                }
                else
                {
                    result.FileUrl = Path.Combine(_filePath, fileName);
                    try
                    {
                        using var stream = new FileStream(phsycalPath, FileMode.Create);
                        file.CopyTo(stream);
                        result.Message = "File saved successfully";
                        result.FileResult = FileResult.Succeded;
                    }
                    catch (Exception ex)
                    {

                        result.Message = "An error occurred during file upload !";
                        result.FileResult = FileResult.Error;
                    }
                }
            }

            return result;
        }
    }
}
