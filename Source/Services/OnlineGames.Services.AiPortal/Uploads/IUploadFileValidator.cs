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
