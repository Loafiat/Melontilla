using GorillaGameModes;
using System;
using GorillaLibrary.GameModes.Utilities;
using Utilla.Models;

namespace Utilla.Utils
{
    public static class GameModeUtils
    {
        public static Gamemode CurrentGamemode => (Gamemode)GameModeUtility.CurrentGamemode;

        public static Gamemode FindGamemodeInString(string gmString)
        {
            return (Gamemode)GameModeUtility.FindGamemodeInString(gmString);
        }

        public static Gamemode GetGamemodeFromId(string id) => GetGamemode(gamemode => gamemode.ID == id);

        public static Gamemode GetGamemode(Func<Gamemode, bool> predicate)
        {
            return (Gamemode)GameModeUtility.GetGamemode(game => predicate.Invoke((Gamemode)game));
        }

        public static string GetGameModeName(GameModeType gameModeType)
        {
            return GameModeUtility.GetGameModeName(gameModeType);
        }

        public static GorillaGameManager GetGameModeInstance(GameModeType gameModeType)
        {
            return GameModeUtility.GetGameModeInstance(gameModeType);
        }

        public static bool IsSuperGameMode(this GameModeType gameMode) => GameModeUtility.IsSuperGameMode(gameMode);
    }
}