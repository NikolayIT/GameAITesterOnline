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

        public override string Name { get; } = Guid.NewGuid().ToString();

        public override void StartGame(StartGameContext context)
        {
            this.TimeOuts = 0;
            this.Crashes = 0;
            this.FirstCrash = null;
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.StartGame(context));
        }

        public override void StartRound(StartRoundContext context)
        {
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.StartRound(context));
        }

        public override void StartHand(StartHandContext context)
        {
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.StartHand(context));
        }

        public override PlayerAction GetTurn(GetTurnContext context)
        {
            try
            {
                PlayerAction playerAction = null;
                var completed = ExecuteWithTimeLimit(
                    TimeSpan.FromMilliseconds(50),
                    () => playerAction = base.GetTurn(context));
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
                if (this.FirstCrash == null)
                {
                    this.FirstCrash = ex.ToString();
                }

                return PlayerAction.CheckOrCall();
            }
        }

        public override void EndHand(EndHandContext context)
        {
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.EndHand(context));
        }

        public override void EndRound(EndRoundContext context)
        {
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.EndRound(context));
        }

        public override void EndGame(EndGameContext context)
        {
            ExecuteWithTimeLimit(TimeSpan.FromMilliseconds(100), () => base.EndGame(context));
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
