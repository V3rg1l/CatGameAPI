using System;
namespace Danilo.CatGame.API.Model
{
    public class ObjectResponse
    {
        public ObjectResponse(string folder, bool isValid = false)
        {
            Folder = folder;
            IsValid = isValid;
            ErrorMsg = String.Empty;
        }

        public string Folder { get; set; }
        public string ErrorMsg { get; set; }
        public bool IsValid { get; set; }
    }

    public class ApiResponse
    {
        public ApiResponse()
        {
            LstApiResponse = new List<ObjectResponse>();
        }

        public bool ValidStructureFiles { get; set; }
        public List<ObjectResponse> LstApiResponse { get; set; }

        public void IsValidItens()
        {
            if(LstApiResponse.Count() <= 0)
                ValidStructureFiles = false;
            else
                ValidStructureFiles = true;


            foreach (var item in LstApiResponse)
            {
                if (!item.IsValid)
                    ValidStructureFiles = false;
            }
        }
    }
}

