// <copyright file="IUploadFileValidator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Uploads
{
    using System.IO;

    using OnlineGames.Services.AiPortal.Uploads.LibraryValidators;

    public interface IUploadFileValidator
    {
        UploadFileValidatorResult ValidateFile(
            string fileName,
            int contentLength,
            Stream inputStream,
            ILibraryValidator libraryValidator);
    }
}
