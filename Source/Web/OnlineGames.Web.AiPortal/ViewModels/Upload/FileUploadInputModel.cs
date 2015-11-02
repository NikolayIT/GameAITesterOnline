namespace OnlineGames.Web.AiPortal.ViewModels.Upload
{
    using System.ComponentModel.DataAnnotations;

    public class FileUploadInputModel
    {
        // TODO: Add URL validation
        [Required]
        public string SourceCodeRepository { get; set; }

        public FileUploadInputModel File { get; set; }
    }
}
