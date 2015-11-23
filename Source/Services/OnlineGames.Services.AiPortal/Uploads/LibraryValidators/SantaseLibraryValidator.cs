namespace OnlineGames.Services.AiPortal.Uploads.LibraryValidators
{
    using System;
    using System.Linq;
    using System.Reflection;

    using Santase.Logic.GameMechanics;
    using Santase.Logic.Players;

    public class SantaseLibraryValidator : ILibraryValidator
    {
        public LibraryValidatorResult Validate(Assembly assembly)
        {
            var playerClasses =
                assembly.GetTypes()
                    .Where(x => x.IsPublic && x.IsClass && !x.IsAbstract && typeof(IPlayer).IsAssignableFrom(x)).ToList();
            if (playerClasses.Count > 1)
            {
                return new LibraryValidatorResult("More than one public inheritant of IPlayer found.");
            }

            if (playerClasses.Count == 0)
            {
                return new LibraryValidatorResult("No public types that inherit IPlayer (or BasePlayer) found!");
            }

            var playerClass = playerClasses[0];
            IPlayer firstInstance;
            IPlayer secondInstance;
            try
            {
                // TODO: var sandbox = Sandbox.CreateSandbox();
                // TODO: firstInstance = sandbox.CreateInstanceFromAndUnwrap(playerClass.Assembly.FullName, playerClass.FullName) as IPlayer;
                // TODO: secondInstance = sandbox.CreateInstanceFromAndUnwrap(playerClass.Assembly.FullName, playerClass.FullName) as IPlayer;
                firstInstance = Activator.CreateInstance(playerClass) as IPlayer;
                secondInstance = Activator.CreateInstance(playerClass) as IPlayer;
            }
            catch (Exception ex)
            {
                return new LibraryValidatorResult($"Creating instance of \"{playerClass.Name}\" failed: {ex.Message}");
            }

            if (firstInstance == null || secondInstance == null)
            {
                return new LibraryValidatorResult($"Instance of \"{playerClass.Name}\" is null.");
            }

            try
            {
                var santaseGame = new SantaseGame(firstInstance, secondInstance);
                santaseGame.Start();
                santaseGame = new SantaseGame(secondInstance, firstInstance);
                santaseGame.Start();
            }
            catch (Exception ex)
            {
                return new LibraryValidatorResult($"Playing a new game between two instances of your player failed: {ex.Message}");
            }

            return new LibraryValidatorResult();
        }
    }
}
