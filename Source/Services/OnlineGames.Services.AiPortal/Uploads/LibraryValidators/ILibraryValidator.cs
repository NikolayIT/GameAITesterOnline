namespace OnlineGames.Services.AiPortal.Uploads.LibraryValidators
{
    using System.Reflection;

    public interface ILibraryValidator
    {
        LibraryValidatorResult Validate(Assembly assembly);
    }
}
