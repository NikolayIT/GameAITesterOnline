// <copyright file="LibraryValidatorResult.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License, Version 2.0. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Uploads.LibraryValidators
{
    public class LibraryValidatorResult
    {
        public LibraryValidatorResult(string error)
        {
            this.IsValid = false;
            this.Error = error;
        }

        public LibraryValidatorResult()
        {
            this.IsValid = true;
        }

        public bool IsValid { get; }

        public string Error { get; }
    }
}
