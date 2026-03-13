using System;
using GorillaGameModes;

namespace Utilla.Models
{
    public class Gamemode : GorillaLibrary.GameModes.Models.Gamemode
    {
        public Gamemode(string id, string displayName, GameModeType? game_mode_type = null) : base(id, displayName, game_mode_type)
        {
        }

        public Gamemode(string id, string displayName, Type gameManager) : base(id, displayName, gameManager)
        {
        }
    }
}