using System;
using System.Reflection;
using GorillaGameModes;
using GorillaLibrary.GameModes.Utilities;
using HarmonyLib;

namespace Utilla
{
    public class Events
    {
        public static Events Instance = new();

        public Events()
        {
            MethodInfo subscribe = AccessTools.Method(AccessTools.TypeByName("MelonEvent"), "Subscribe");
            object OnGameInitialized = AccessTools.Field(GorillaLibrary.Events.Game.GetType(), "OnGameInitialized").GetValue(GorillaLibrary.Events.Game);
            object OnRoomJoined = AccessTools.Field(GorillaLibrary.Events.Room.GetType(), "OnRoomJoined").GetValue(GorillaLibrary.Events.Room);
            object OnRoomLeft = AccessTools.Field(GorillaLibrary.Events.Room.GetType(), "OnRoomLeft").GetValue(GorillaLibrary.Events.Room);
            
            subscribe.Invoke(OnGameInitialized, [() =>
            {
                GameInitialized?.Invoke(null, EventArgs.Empty);
            }]);
            
            subscribe.Invoke(OnRoomJoined, [() =>
            {
                RoomJoinedArgs args = new RoomJoinedArgs();
                args.isPrivate = NetworkSystem.Instance.SessionIsPrivate;
                args.Gamemode = GameModeUtility.GetGameModeName(GameMode.CurrentGameModeType);
                RoomJoined?.Invoke(null, args);
            }]);
            
            subscribe.Invoke(OnRoomLeft, [() =>
            {
                RoomJoinedArgs args = new RoomJoinedArgs();
                args.isPrivate = NetworkSystem.Instance.SessionIsPrivate;
                args.Gamemode = GameModeUtility.GetGameModeName(GameMode.CurrentGameModeType);
                RoomLeft?.Invoke(null, args);
            }]);
        }

        /// <summary>
        /// An event that gets called whenever a room is joined.
        /// </summary>
        public static event EventHandler<RoomJoinedArgs> RoomJoined;

        /// <summary>
        /// An event that gets called whenever a room is left.
        /// </summary>
        public static event EventHandler<RoomJoinedArgs> RoomLeft;

        /// <summary>
        /// An event that gets called whenever the game has finished initializing.
        /// </summary>
        public static event EventHandler GameInitialized;

        public class RoomJoinedArgs : EventArgs
        {
            /// <summary>
            /// Whether or not the room is private.
            /// </summary>
            public bool isPrivate { get; set; }

            /// <summary>
            /// The gamemode that the current lobby is 
            /// </summary>
            public string Gamemode { get; set; }
        }
    }
}