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
