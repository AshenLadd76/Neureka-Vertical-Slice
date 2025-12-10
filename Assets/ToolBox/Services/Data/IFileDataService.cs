namespace ToolBox.Services.Data
{
    public interface IFileDataService
    {
       
        Result Save<T>(T data, string folder, string fileName, bool encrypt = true, bool createFolder = false);
        
        Result Save(string data, string folder, string fileName, bool encrypt = true, bool createFolder = false);
        
        Result<T> Load<T>(string folder, string fileName, bool encrypted = true);
        public Result Delete(string folder, string fileName);
        Result<string[]> GetAllFiles(string folder, string extension = null);
        Result<string[]> GetSubdirectories(string folder);
        Result<bool> FileExists(string folder, string fileName);
    }
}