namespace OnlineGames.Web.AiPortal.ViewModels.Upload
{
    using System.ComponentModel.DataAnnotations;
    using System.Web;

    public class FileUploadInputModel
    {
        public int Id { get; set; }

        // TODO: Add URL validation
        [Required]
        public string SourceCodeRepository { get; set; }

        public HttpPostedFileBase AiFile { get; set; }
    }
}
