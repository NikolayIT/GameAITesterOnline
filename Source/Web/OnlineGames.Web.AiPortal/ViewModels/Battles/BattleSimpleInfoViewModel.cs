// <copyright file="BattleSimpleInfoViewModel.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.ViewModels.Battles
{
    using System;
    using System.Linq;

    using AutoMapper;

    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.Infrastructure.Mapping;

    public class BattleSimpleInfoViewModel : IMapFrom<Battle>, IHaveCustomMappings
    {
        public int Id { get; set; }

        public int FirstTeamId { get; set; }

        public string FirstTeamName { get; set; }

        public int FirstTeamWins { get; set; }

        public int SecondTeamId { get; set; }

        public string SecondTeamName { get; set; }

        public int SecondTeamWins { get; set; }

        public string Comment { get; set; }

        public bool IsFinished { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public void CreateMappings(IConfiguration configuration)
        {
            configuration.CreateMap<Battle, BattleSimpleInfoViewModel>()
                .ForMember(m => m.FirstTeamName, opt => opt.MapFrom(b => b.FirstTeam.Name))
                .ForMember(m => m.FirstTeamWins, opt => opt.MapFrom(b => b.BattleGameResults.Count(x => x.BattleGameWinner == BattleGameWinner.First)))
                .ForMember(m => m.SecondTeamName, opt => opt.MapFrom(b => b.SecondTeam.Name))
                .ForMember(m => m.SecondTeamWins, opt => opt.MapFrom(b => b.BattleGameResults.Count(x => x.BattleGameWinner == BattleGameWinner.Second)));
        }
    }
}
