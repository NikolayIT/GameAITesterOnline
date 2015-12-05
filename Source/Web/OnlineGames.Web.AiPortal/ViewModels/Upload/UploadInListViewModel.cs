// <copyright file="UploadInListViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Upload
{
    using System;

    using AutoMapper;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class UploadInListViewModel : IMapFrom<Upload>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CompetitionName { get; set; }

        public string TeamName { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Upload, UploadInListViewModel>()
                .ForMember(x => x.TeamName, opt => opt.MapFrom(x => x.Team.Name))
                .ForMember(x => x.CompetitionName, opt => opt.MapFrom(x => x.Team.Competition.Name));
        }
    }
}
