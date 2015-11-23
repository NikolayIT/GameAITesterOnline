namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using System;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class UploadViewModel : IMapFrom<Upload>
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string FileName { get; set; }
    }
}