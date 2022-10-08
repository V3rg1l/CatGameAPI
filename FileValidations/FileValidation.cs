using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using Danilo.CatGame.API.Model;
using Microsoft.VisualBasic;

namespace Danilo.CatGame.API.FileValidations
{
    public class FileValidation
    {
        public FileValidation()
        {
        }

        public ObjectResponse ValidateDll(string pDir)
        {
            var rObject = new ObjectResponse("dll", true);
            var vDirectorys = Directory.GetFiles(pDir, "*", SearchOption.AllDirectories);
            var requiredFile = "RootFolder.dll";


            rObject.IsValid = vDirectorys.Where(d => Path.GetFileName(d) == requiredFile).Count() > 0;

            if (!rObject.IsValid)
            {
                rObject.IsValid = false;
                rObject.ErrorMsg = "Must contain RootFolder.dll file. Can have other files, other filetypes besides .dll are allowed";
            }
                

            return rObject;
        }

        public ObjectResponse ValidateImages(string pDir)
        {
            var rObject = new ObjectResponse("images", true);
            var vDirectorys = Directory.GetFiles(pDir, "*", SearchOption.AllDirectories);

            if (vDirectorys.Length <= 0)
            {
                rObject.IsValid = false;
                rObject.ErrorMsg = "Must contain at least 1 image of filetype .jpg or .png.";
            }
                

            foreach (var item in vDirectorys)
            {
                if (!Path.GetExtension(Path.GetFileName(item)).Equals(".jpg") && !Path.GetExtension(Path.GetFileName(item)).Equals(".png"))
                {
                    rObject.IsValid = false;
                    rObject.ErrorMsg = "Just .jpg or .png filetypes are allowed in images folder";
                    break;
                }
                    
            }

            return rObject;
        }

        public ObjectResponse ValidateLanguages(string pDir)
        {
            var rObject = new ObjectResponse("languages", true);
            var vDirectorys = Directory.GetFiles(pDir, "*", SearchOption.AllDirectories);
            var requiredFile = "RootFolder_en.xml";
            Regex regex = new Regex("RootFolder_\\[a-zA-Z]\\{2\\}\\.xml", RegexOptions.IgnoreCase);
            

            //*Must contain RootFolder_en.xml file. Folder can have only .xml files, but file name must follow the RootFolder_xx.xml naming convention where xx is 2 letter language code.

            foreach (var item in vDirectorys)
            { 
                if (!Path.GetExtension(Path.GetFileName(item)).Equals(".xml"))
                {
                    rObject.IsValid = false;
                    rObject.ErrorMsg = "Just .xml filetypes are allowed in languages folder. ";
                    break;
                }

                if (!regex.IsMatch(Path.GetFileName(item)))
                {
                    rObject.IsValid = false;
                    rObject.ErrorMsg = "File name must follow the RootFolder_xx.xml naming convention where xx is 2 letter language code at languages folder. ";
                    break;
                }

            }

            if (vDirectorys.Where(d => Path.GetFileName(d) == requiredFile).Count() <= 0)
            {
                rObject.IsValid = false;
                rObject.ErrorMsg += "Must contain RootFolder_en.xml file. ";
            }


            return rObject;
        }
    }
}

