// <copyright file="BattleInfoViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Battles
{
    using System;
    using System.Collections.Generic;

    using AutoMapper;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class BattleInfoViewModel : IMapFrom<Battle>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public string FirstTeamName { get; set; }

        public string SecondTeamName { get; set; }

        public string Comment { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public IEnumerable<BattleGameResultViewModel> BattleGameResults { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Battle, BattleInfoViewModel>()
                .ForMember(m => m.FirstTeamName, opt => opt.MapFrom(b => b.FirstTeam.Name))
                .ForMember(m => m.SecondTeamName, opt => opt.MapFrom(b => b.SecondTeam.Name));
        }
    }
}
