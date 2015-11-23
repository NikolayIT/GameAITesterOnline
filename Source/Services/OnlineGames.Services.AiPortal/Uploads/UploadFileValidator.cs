﻿namespace OnlineGames.Services.AiPortal.Uploads
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;

    using OnlineGames.Services.AiPortal.Uploads.LibraryValidators;

    public class UploadFileValidator : IUploadFileValidator
    {
        public UploadFileValidatorResult ValidateFile(
            string fileName,
            int contentLength,
            Stream inputStream,
            ILibraryValidator libraryValidator)
        {
            var errors = new List<string>();
            if (!fileName.EndsWith(".dll"))
            {
                return new UploadFileValidatorResult("Only .dll files are supported.");
            }

            if (((contentLength / 1024) / 1024) > 2)
            {
                return new UploadFileValidatorResult("Files should be less than 2MB.");
            }

            byte[] fileData;
            try
            {
                using (var binaryReader = new BinaryReader(inputStream))
                {
                    fileData = binaryReader.ReadBytes(contentLength);
                }
            }
            catch (Exception ex)
            {
                return new UploadFileValidatorResult($"File reading error: {ex.Message}");
            }

            Assembly assembly;
            try
            {
                assembly = Assembly.Load(fileData);
            }
            catch (Exception ex)
            {
                return new UploadFileValidatorResult($"Loading assembly failed: {ex.Message}");
            }

            if (libraryValidator != null)
            {
                var libraryValidatorResult = libraryValidator.Validate(assembly);
                if (!libraryValidatorResult.IsValid)
                {
                    return new UploadFileValidatorResult($"Library validation failed: {libraryValidatorResult.Error}");
                }
            }

            return new UploadFileValidatorResult(fileData);
        }
    }
}