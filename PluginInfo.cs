using BepInEx;
using System;
using System.Linq;
using GorillaLibrary.GameModes;
using Utilla.Models;

namespace Utilla
{
    // Wrapper
    public class PluginInfo
    {
        public ModInfo underlyingInfo;
        
        public BaseUnityPlugin Plugin { get; set; }
        public Gamemode[] Gamemodes => underlyingInfo.Gamemodes.Select(x => new Gamemode(x)).ToArray();
        public Action<string> OnGamemodeJoin => underlyingInfo.OnGamemodeJoin;
        public Action<string> OnGamemodeLeave => underlyingInfo.OnGamemodeJoin;

        public override string ToString()
        {
            return $"{Plugin.Info.Metadata.Name} [{string.Join(", ", Gamemodes.Select(x => x.DisplayName))}]";
        }

        public PluginInfo(ModInfo underlyingInfo)
        {
            this.underlyingInfo = underlyingInfo;
        }
    }
}