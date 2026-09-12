// <copyright file="ILibraryValidator.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Services.AiPortal.Uploads.LibraryValidators
{
    using System.Reflection;

    public interface ILibraryValidator
    {
        LibraryValidatorResult Validate(Assembly assembly);
    }
}
