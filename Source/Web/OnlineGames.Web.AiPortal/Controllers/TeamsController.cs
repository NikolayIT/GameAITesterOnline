// <copyright file="TeamsController.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Web.AiPortal.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;

    using AutoMapper.QueryableExtensions;

    using OnlineGames.Data.Common;
    using OnlineGames.Data.Models;
    using OnlineGames.Web.AiPortal.ViewModels.Teams;

    public class TeamsController : BaseController
    {
        private readonly IDbRepository<Team> teamsRepository;

        private readonly IDbRepository<Competition> competitionsRepository;

        private readonly IDbRepository<User> usersRepository;

        public TeamsController(
            IDbRepository<Team> teamsRepository,
            IDbRepository<Competition> competitionsRepository,
            IDbRepository<User> usersRepository)
        {
            this.teamsRepository = teamsRepository;
            this.competitionsRepository = competitionsRepository;
            this.usersRepository = usersRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult Create()
        {
            this.PrepareDataForTheView();
            return this.View(new CreateTeamViewModel());
        }

        [HttpPost]
        [Authorize]
        public ActionResult Create(CreateTeamViewModel model)
        {
            var teamMembers = model.TeamMembers.ToList();
            teamMembers[0] = this.User.Identity.Name;
            var competition = this.competitionsRepository.All().FirstOrDefault(x => x.Id == model.CompetitionId && x.IsActive);
            var realUsers = new HashSet<int>();
            if (competition != null)
            {
                foreach (var username in teamMembers.Where(x => !string.IsNullOrWhiteSpace(x)))
                {
                    var user = this.usersRepository.All().FirstOrDefault(x => x.UserName == username);
                    if (user != null)
                    {
                        realUsers.Add(user.Id);
                    }
                    else
                    {
                        this.ModelState.AddModelError(string.Empty, $"Username \"{username}\" could not be found!");
                    }
                }

                if (realUsers.Count < competition.MinimumParticipants
                    || realUsers.Count > competition.MaximumParticipants)
                {
                    this.ModelState.AddModelError(
                        string.Empty,
                        $"Expected between {competition.MinimumParticipants} and {competition.MaximumParticipants} team members!");
                }
            }
            else
            {
                this.ModelState.AddModelError(
                    nameof(CreateTeamViewModel.CompetitionId),
                    "Invalid competition selected!");
            }

            if (this.teamsRepository.All().Any(x => x.Name == model.Name && x.CompetitionId == model.CompetitionId))
            {
                this.ModelState.AddModelError(nameof(model.Name), "Team with the same name exists in this competition!");
            }

            if (this.ModelState.IsValid)
            {
                var team = new Team { CompetitionId = model.CompetitionId, Name = model.Name };
                foreach (var userId in realUsers)
                {
                    team.TeamMembers.Add(new TeamMember { UserId = userId });
                }

                this.teamsRepository.Add(team);
                this.teamsRepository.Save();
                return this.RedirectToAction(nameof(this.Info), new { id = team.Id });
            }
            else
            {
                this.PrepareDataForTheView();
                return this.View(model);
            }
        }

        // TODO: List file uploads
        public ActionResult Info(int id)
        {
            var teamInfo =
                this.teamsRepository.All().Where(x => x.Id == id).ProjectTo<TeamInfoViewModel>().FirstOrDefault();
            return this.View(teamInfo);
        }

        private void PrepareDataForTheView()
        {
            this.ViewBag.MaxTeamMembers =
                this.competitionsRepository.All().Where(x => x.IsActive).Max(x => x.MaximumParticipants);
            this.ViewBag.Competitions =
                this.competitionsRepository.All()
                    .Where(x => x.IsActive)
                    .Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() })
                    .ToList();
        }
    }
}
