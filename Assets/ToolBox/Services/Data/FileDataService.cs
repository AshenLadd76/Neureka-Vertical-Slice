using CodeBase.Services.Encryption;
using ToolBox.Helpers;
using System;
using System.IO;
using UnityEngine;

namespace ToolBox.Services.Data
{
    public class FileDataService 
    {
        private readonly string _basePath;
        private readonly string _fileExtension;
        private readonly IEncryptionService _encryptionService;
        private readonly ISerializer _serializer;
        
        public FileDataService(
            IEncryptionService encryptionService,
            ISerializer serializer,
            string fileExtension = "json")
        {
            _basePath = Application.persistentDataPath;
            _fileExtension = fileExtension;
            _encryptionService = encryptionService ?? throw new ArgumentNullException(nameof(encryptionService));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }
        
        private string BuildPath(string folder, string fileName)
        {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Folder and fileName must not be null or empty.");

            string fullPath = Path.Combine(_basePath, folder, $"{fileName}.{_fileExtension}");
            
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            
            return fullPath;
        }
        
        // --- Save Generic ---
        public Result Save<T>(T data, string folder, string fileName, bool encrypt = true)
        {
            try
            {
                string path = BuildPath(folder, fileName);
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
                string path = BuildPath(folder, fileName);
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
        
        
        //Utility methods
        public Result<string[]> GetAllFiles(string folder, string extension = null)
        {
            try
            {
                string dir = Path.Combine(_basePath, "Data", folder);
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
                string dir = Path.Combine(_basePath, "Data", folder);
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
            string path = Path.Combine(_basePath, "Data", folder, $"{fileName}.{_fileExtension}");
            return File.Exists(path);
        }
        
    }
}
