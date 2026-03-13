using System;
using GorillaGameModes;

namespace Utilla.Models
{
    /// <summary>
    /// The base gamemode for a gamemode to inherit.
    /// </summary>
    /// <remarks>
    /// None should not be used from an external program.
    /// </remarks>
    [Obsolete]
    public enum BaseGamemode
    {
        /// <summary>
        /// There is no gamemode manager to rely on, this should only be used by the Utilla mod when preparing modded gamemodes or gamemodes using a unique gamemode manager.
        /// </summary>
        None,
        /// <summary>
        /// Infection gamemode, requires at least four participating players for infection and under for tag.
        /// </summary>
        Infection,
        /// <summary>
        /// Casual gamemode, no players are affected by the gamemode, such as tagging or infecting.
        /// </summary>
        Casual,
        /// <summary>
        /// Hunt gamemode, requires at least four participating players.
        /// </summary>
        Hunt,
        /// <summary>
        /// Paintbrawl gamemode, requires at least two participating players.
        /// </summary>
        Paintbrawl
    }
    
    //Wrapper
    public class Gamemode
    {
        public GorillaLibrary.GameModes.Models.Gamemode underlyingGamemode;
        
        public string DisplayName => underlyingGamemode.DisplayName;

        public string ID => underlyingGamemode.ID;

        public GameModeType? BaseGamemode => underlyingGamemode.BaseGamemode;

        public Type GameManager => underlyingGamemode.GameManager;
        
        public Gamemode(GameModeType gameModeType)
        {
            underlyingGamemode = new GorillaLibrary.GameModes.Models.Gamemode(gameModeType);
        }

        public Gamemode(string id, string displayName, GameModeType? game_mode_type = null)
        {
            underlyingGamemode = new GorillaLibrary.GameModes.Models.Gamemode(id, displayName, game_mode_type);
        }

        public Gamemode(string id, string displayName, Type gameManager)
        {
            underlyingGamemode = new GorillaLibrary.GameModes.Models.Gamemode(id, displayName, gameManager);
        }

        public Gamemode(GorillaLibrary.GameModes.Models.Gamemode gamemode)
        {
            underlyingGamemode = gamemode;
        }
    }
}