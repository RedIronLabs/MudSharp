using MudSharp.Data.Models.World.Actors;
using MudSharp.Data.Models.World.Environment.Rooms;
using MudSharp.Data.Models.World.Items;
using System;
using System.Collections.Concurrent;

namespace MudSharp.World
{
    /// <summary>
    /// Singleton for managing all world data throughout the game.
    /// </summary>
    public sealed class WorldManager
    {
        private static WorldManager _instance = null;
        private static readonly object _lockObject = new object();

        private WorldManager()
        {
            Rooms = new ConcurrentDictionary<Guid, Room>();
        }

        /// <summary>
        /// Gets the <see cref="WorldManager"/> instance.
        /// </summary>
        public static WorldManager Instance
        {
            get
            {
                lock (_lockObject)
                {
                    if (_instance == null)
                        _instance = new WorldManager();

                    return _instance;
                }
            }
        }

        #region Properties

        /// <summary>
        /// All instances of <see cref="Room"/> objects in the game world. Key is the instance id, value is the object reference.
        /// </summary>
        public ConcurrentDictionary<Guid, Room> Rooms { get; set; }

        /// <summary>
        /// All instances of <see cref="Item"/> objects in the game world. Key is the instance id, value is the object reference.
        /// </summary>
        public ConcurrentDictionary<Guid, Item> Items { get; set; }

        /// <summary>
        /// All instances of <see cref="NonPlayer"/> objects in the game world. Key is the instance id, value is the object reference.
        /// </summary>
        public ConcurrentDictionary<Guid, NonPlayer> NPCs { get; set; }

        #endregion
    }
}
