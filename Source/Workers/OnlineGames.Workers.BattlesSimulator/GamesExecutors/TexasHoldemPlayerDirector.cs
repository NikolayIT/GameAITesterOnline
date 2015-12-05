// <copyright file="TexasHoldemPlayerDirector.cs" company="Nikolay Kostov (Nikolay.IT)">
// Copyright (c) Nikolay Kostov (Nikolay.IT). All Rights Reserved.
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>

namespace OnlineGames.Workers.BattlesSimulator.GamesExecutors
{
    using System;
    using System.Threading.Tasks;

    using TexasHoldem.Logic.Players;

    public class TexasHoldemPlayerDirector : PlayerDecorator
    {
        public TexasHoldemPlayerDirector(IPlayer player)
            : base(player)
        {
        }

        public int TimeOuts { get; private set; }

        public int Crashes { get; private set; }

        public string FirstCrash { get; private set; }

        public override void StartGame(StartGameContext context)
        {
            this.TimeOuts = 0;
            this.Crashes = 0;
            this.FirstCrash = null;
            base.StartGame(context);
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            try
            {
                PlayerAction playerAction = null;
                var completed = ExecuteWithTimeLimit(
                    TimeSpan.FromMilliseconds(50),
                    () => playerAction = this.Player.GetTurn(context));
                if (completed)
                {
                    return playerAction;
                }
                else
                {
                    this.TimeOuts++;
                    return PlayerAction.CheckOrCall();
                }
            }
            catch (Exception ex)
            {
                this.Crashes++;
                if (this.FirstCrash != null)
                {
                    this.FirstCrash = ex.ToString();
                }

                return PlayerAction.CheckOrCall();
            }
        }

        private static bool ExecuteWithTimeLimit(TimeSpan timeSpan, Action codeBlock)
        {
            // TODO: memory limit?
            try
            {
                var task = Task.Factory.StartNew(codeBlock);
                task.Wait(timeSpan);
                return task.IsCompleted;
            }
            catch (AggregateException ae)
            {
                throw ae.InnerExceptions[0];
            }
        }
    }
}
