using MudSharp.Server.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace MudSharp.Server.Core
{
    internal sealed class Game : IGame
    {
        private readonly IConfigProvider _configProvider;
        private readonly ILoggingProvider _loggingProvider;
        private bool _running;
        private DateTime _gameStart;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="configProvider">The injected config provider to use.</param>
        /// <param name="loggingProvider">The injected logging provider to use.</param>
        public Game(IConfigProvider configProvider, ILoggingProvider loggingProvider)
        {
            _configProvider = configProvider;
            _loggingProvider = loggingProvider;
        }

        /// <summary>
        /// The main game loop.
        /// </summary>
        public void Run()
        {
            _running = true;
            _gameStart = DateTime.UtcNow;
            _loggingProvider.LogMessage("Entering main game loop");

            while (_running)
            {
                Heartbeat();
            }

            _loggingProvider.LogMessage("Starting game shutdown");
            PerformShutdown();
        }

        /// <summary>
        /// Starts the game shutdown sequence.
        /// </summary>
        public void Shutdown()
        {
            _running = false;
        }

        /// <summary>
        /// Actually performs the game shutdown.
        /// </summary>
        private void PerformShutdown()
        {
            _loggingProvider.LogMessage("Shutdown complete");
            Environment.Exit(0);
        }

        /// <summary>
        /// Runs continuously and processes things like NPC actions, combat, and such.
        /// </summary>
        private void Heartbeat()
        {
            var timeSinceStart = DateTime.UtcNow - _gameStart;

            CleanupDescriptors();

            if (timeSinceStart.TotalSeconds % _configProvider.Core.ZonePulseRateInSeconds == 0)
                PulseZones();

            if (timeSinceStart.TotalSeconds % _configProvider.Core.NpcPulseRateInSeconds == 0)
                PulseNpcs();

            if (timeSinceStart.TotalSeconds % _configProvider.Core.CombatPulseRateInSeconds == 0)
                PulseCombat();

            if (timeSinceStart.TotalSeconds % _configProvider.Core.AutosavePulseRateInSeconds == 0)
                PulseAutosave();
        }

        /// <summary>
        /// Processes player autosave.
        /// </summary>
        private void PulseAutosave()
        {

        }

        /// <summary>
        /// Processes all zone-related actions such as weather, loading/extracting of items and NPCs, and so on.
        /// </summary>
        private void PulseZones()
        {
            
        }

        /// <summary>
        /// Processes all non-combat NPC-related actions such as movement.
        /// </summary>
        private void PulseNpcs()
        {

        }

        /// <summary>
        /// Processes all combat actions.
        /// </summary>
        private void PulseCombat()
        {

        }

        /// <summary>
        /// Cleans up any disconnected descriptors.
        /// </summary>
        private void CleanupDescriptors()
        {

        }
    }
}
