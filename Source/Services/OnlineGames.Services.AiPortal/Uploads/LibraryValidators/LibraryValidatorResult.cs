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
