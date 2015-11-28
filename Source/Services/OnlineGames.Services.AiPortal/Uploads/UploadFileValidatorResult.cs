// <copyright file="UploadFileValidatorResult.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Uploads
{
    public class UploadFileValidatorResult
    {
        public UploadFileValidatorResult(string error)
        {
            this.IsValid = false;
            this.Error = error;
        }

        public UploadFileValidatorResult(byte[] fileContent)
        {
            this.IsValid = true;
            this.FileContent = fileContent;
        }

        public bool IsValid { get; }

        public string Error { get; }

        public byte[] FileContent { get; }
    }
}
