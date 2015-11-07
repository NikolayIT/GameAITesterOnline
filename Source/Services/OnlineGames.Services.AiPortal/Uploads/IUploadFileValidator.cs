namespace OnlineGames.Services.AiPortal.Uploads
{
    using System.Collections.Generic;
    using System.IO;

    public interface IUploadFileValidator
    {
        UploadFileValidatorResult ValidateFile(string fileName, int contentLength, Stream inputStream);
    }
}
