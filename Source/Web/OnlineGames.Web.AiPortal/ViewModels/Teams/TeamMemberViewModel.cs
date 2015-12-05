// <copyright file="TeamMemberViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Teams
{
    using AutoMapper;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class TeamMemberViewModel : IMapFrom<TeamMember>, IHaveCustomMappings
    {
        public string UserName { get; set; }

        public string AvatarUrl { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<TeamMember, TeamMemberViewModel>()
                .ForMember(x => x.UserName, opt => opt.MapFrom(x => x.User.UserName))
                .ForMember(x => x.AvatarUrl, opt => opt.MapFrom(x => x.User.AvatarUrl));
        }
    }
}
