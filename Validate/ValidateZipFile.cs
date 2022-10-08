using System;
using System.IO;
using System.IO.Compression;
using Danilo.CatGame.API.Model;
using Danilo.CatGame.API.FileValidations;

namespace Danilo.CatGame.API.Validate
{
    public static class ValidateZipFile
    {
        private static readonly List<string> _folderStructure = new List<string>() { "RootFolder", "dlls", "images", "languages" };

        public static List<ObjectResponse> Validate(IFormFile file, string path)
        {
            var filePath = path + @"tempFiles/";
            var zipFilePath = path + @"validateFiles/";
            CreateDirectorys(filePath);
            CreateDirectorys(zipFilePath);

            using (var stream = new FileStream(filePath + file.FileName, FileMode.Create))
            {
                file.CopyTo(stream);
            };

            string zipPath = Path.GetFileName(filePath + file.FileName);
            ZipFile.ExtractToDirectory(filePath+file.FileName, zipFilePath, true);

            var rValidate = ValidateFoldersFiles(zipFilePath);
            Directory.Delete(filePath, true);
            Directory.Delete(zipFilePath, true);

            return rValidate;
        }

        private static void CreateDirectorys(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private static List<ObjectResponse> ValidateFoldersFiles(string unzipFilesPath)
        {
            var objResponse = new List<ObjectResponse>();
            var _fileValidation = new FileValidation();

            var vDirectorys = Directory.GetDirectories(unzipFilesPath, "*", SearchOption.AllDirectories);

            foreach (var dir in vDirectorys)
            {

                string lastFolderName = new DirectoryInfo(dir).Name;

                if (!_folderStructure.Contains(lastFolderName) || vDirectorys.Length != 4)
                {
                    objResponse.Add(new ObjectResponse("structure", false)
                    {
                        ErrorMsg = "Some is missing or incorret in folders structure inside at .zip file"
                    }); 
                }

                if (lastFolderName == "dlls")
                    objResponse.Add(_fileValidation.ValidateDll(dir));

                if (lastFolderName == "images")
                    objResponse.Add(_fileValidation.ValidateImages(dir));

                if (lastFolderName == "languages")
                    objResponse.Add(_fileValidation.ValidateLanguages(dir));
            }

            return objResponse;
        }
    }
}

