using CodeBase.Services.Encryption;
using ToolBox.Helpers;
using System;
using System.IO;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Data
{
    public interface IFileDataService
    {
        Result Save<T>(T data, string folder, string fileName, bool encrypt = true, bool createFolder = false);
        Result<T> Load<T>(string folder, string fileName, bool encrypted = true);
        public Result Delete(string folder, string fileName);
        Result<string[]> GetAllFiles(string folder, string extension = null);
        Result<string[]> GetSubdirectories(string folder);
        bool FileExists(string folder, string fileName);
    }

    public class FileDataService : IFileDataService
    {
        private readonly string _basePath;
        private readonly string _fileExtension;
        private readonly IEncryptionService _encryptionService;
        private readonly ISerializer _serializer;
        private const string DefaultFileExtension = ".json";
        
        public FileDataService(
            IEncryptionService encryptionService,
            ISerializer serializer,
            string fileExtension, string path = null)
        {
            _fileExtension = string.IsNullOrWhiteSpace(fileExtension)
                ? DefaultFileExtension
                : (fileExtension.StartsWith(".") ? fileExtension : $".{fileExtension}");


            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _basePath = path ?? Application.persistentDataPath;
        }
        
        private string BuildPath(string folder, string fileName, bool createDirectory = false)
        {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Folder and fileName must not be null or empty.");

            var fullPath = Path.Combine(_basePath, folder, $"{fileName}");
            
            if( createDirectory )
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            
            return fullPath;
        }
        
        // --- Save Generic ---
        public Result Save<T>(T data, string folder, string fileName, bool encrypt = true, bool createFolder = false)
        {
            try
            {
                string path = BuildPath(folder, fileName, true);
                string serialized = _serializer.Serialize(data);
                
                if (encrypt)
                    serialized = _encryptionService.Encrypt(serialized);

                File.WriteAllText(path, serialized);
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to save '{fileName}': {ex.Message}");
            }
        }
        
        // --- Load Generic ---
        public Result<T> Load<T>(string folder, string fileName, bool encrypted = true)
        {
            try
            {
                string path = BuildPath(folder, fileName, false);
                if (!File.Exists(path))
                    return Result<T>.Fail($"File does not exist: {path}");

                string content = File.ReadAllText(path);
                
                if (encrypted)
                    content = _encryptionService.Decrypt(content);

                T value = _serializer.Deserialize<T>(content);
                return Result<T>.Ok(value);
            }
            catch (Exception ex)
            {
                return Result<T>.Fail($"Failed to load '{fileName}': {ex.Message}");
            }
        }
        
        public Result Delete(string folder, string fileName)
        {
            try
            {
                string path = BuildPath(folder, fileName, false);

                if (!File.Exists(path))
                    return Result.Fail($"File does not exist: {path}");

                File.Delete(path);
                Logger.Log($"Deleted file: {path}");
                return Result.Ok();
            }
            catch (Exception ex)
            {
                return Result.Fail($"Failed to delete '{fileName}': {ex.Message}");
            }
        }
        
        
        //Utility methods
        public Result<string[]> GetAllFiles(string folder, string extension = null)
        {
            try
            {
                string dir = Path.Combine(_basePath, folder);
                if (!Directory.Exists(dir))
                    return Result<string[]>.Fail($"Directory does not exist: {dir}");

                var files = Directory.GetFiles(dir, $"*{extension ?? _fileExtension}");
                return Result<string[]>.Ok(files);
            }
            catch (Exception ex)
            {
                return Result<string[]>.Fail($"Failed to get files in '{folder}': {ex.Message}");
            }
        }
        
        public Result<string[]> GetSubdirectories(string folder)
        {
            try
            {
                string dir = Path.Combine(_basePath,  folder);
                if (!Directory.Exists(dir))
                    return Result<string[]>.Fail($"Directory does not exist: {dir}");

                string[] subdirs = Directory.GetDirectories(dir);
                return Result<string[]>.Ok(subdirs);
            }
            catch (Exception ex)
            {
                return Result<string[]>.Fail($"Failed to get subdirectories in '{folder}': {ex.Message}");
            }
        }
        
        public bool FileExists(string folder, string fileName)
        {
            string path = Path.Combine(_basePath, folder, $"{fileName}");

            Logger.Log( $"Checking path {path}  " );
            
            return File.Exists(path);
        }
    }
}
