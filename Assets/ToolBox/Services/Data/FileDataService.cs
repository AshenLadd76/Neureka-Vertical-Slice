using CodeBase.Services.Encryption;
using ToolBox.Helpers;
using System;
using System.IO;
using UnityEngine;
using Logger = ToolBox.Utils.Logger;

namespace ToolBox.Services.Data
{
    /// <summary>
    /// Provides a file storage service with optional encryption and serialization support.
    /// Handles saving, loading, deleting files, and directory management.
    /// </summary>
    
    public class FileDataService : IFileDataService
    {
        private readonly string _basePath;
        private readonly string _fileExtension;
        private readonly IEncryptionService _encryptionService;
        private readonly ISerializer _serializer;
        private const string DefaultFileExtension = ".json";
        
        
        /// <summary>
        /// Initializes a new instance of the <see cref="FileDataService"/> class.
        /// </summary>
        /// <param name="encryptionService">Service used to encrypt and decrypt file contents.</param>
        /// <param name="serializer">Service used to serialize and deserialize objects.</param>
        /// <param name="fileExtension">Default file extension used when saving files.</param>
        /// <param name="path">Base path where files are stored. Defaults to Application.persistentDataPath.</param>
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
        
        
        /// <summary>
        /// Builds a full file path from a folder and file name, optionally creating the directory.
        /// </summary>
        /// <param name="folder">Folder path relative to base path.</param>
        /// <param name="fileName">File name.</param>
        /// <param name="createDirectory">Whether to create the folder if it does not exist.</param>
        /// <returns>The full file path.</returns>
        /// <exception cref="ArgumentException">Thrown if folder or fileName is null or empty.</exception>
        
        private string BuildPath(string folder, string fileName, bool createDirectory = false)
        {
            if (string.IsNullOrWhiteSpace(folder) || string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("Folder and fileName must not be null or empty.");

            var fullPath = Path.Combine(_basePath, folder, $"{fileName}");
            
            var dir  = Path.GetDirectoryName(fullPath);
            
            if( createDirectory && !string.IsNullOrWhiteSpace(dir) )
                Directory.CreateDirectory(dir);
            
            return fullPath;
        }
        
        /// <summary>
        /// Saves an object to a file in the specified folder.
        /// </summary>
        /// <typeparam name="T">Type of the object to save.</typeparam>
        /// <param name="data">The object to serialize and save.</param>
        /// <param name="folder">Folder path relative to base path.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="encrypt">Whether to encrypt the file contents.</param>
        /// <param name="createFolder">Whether to create the folder if it does not exist.</param>
        /// <returns>A <see cref="Result"/> indicating success or failure.</returns>

        public Result Save<T>(T data, string folder, string fileName, bool encrypt = true, bool createFolder = true)
        {
            try
            {
                string path = BuildPath(folder, fileName, createFolder);
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
                string path = BuildPath(folder, fileName,false);

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
                
                string ext = extension ?? _fileExtension;
                if (!ext.StartsWith(".")) ext = "." + ext;

                var files = Directory.GetFiles(dir, $"*{ext}");
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
        
        public Result<bool> FileExists(string folder, string fileName)
        {
            try
            {
                string path = Path.Combine(_basePath, folder, fileName);
                
                return Result<bool>.Ok(File.Exists(path));
            }
            catch (Exception ex)
            {
                return Result<bool>.Fail($"Error checking file existence: {ex.Message}");
            }
        }
    }
}
