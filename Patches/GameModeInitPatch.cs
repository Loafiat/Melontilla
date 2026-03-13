using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using BepInEx;
using BepInEx.Bootstrap;
using GorillaLibrary.GameModes;
using GorillaLibrary.GameModes.Attributes;
using GorillaLibrary.GameModes.Behaviours;
using GorillaLibrary.GameModes.Models;
using HarmonyLib;
using UnityEngine;

namespace Utilla.Patches;

[HarmonyPatch(typeof(GameModeManager), "GetPluginInfos")]
public class GameModeInitPatch
{
    public static bool Prefix(GameModeManager __instance, ref List<ModInfo> __result)
    {
        bool gotFirstMelon = false;
        object gorillaLibMelon = null;
        
        List<ModInfo> infos = [];

        object regMels = AccessTools.Property(AccessTools.TypeByName("MelonBase"), "RegisteredMelons").GetValue(null);
        
        foreach (var melonBase in (IEnumerable)regMels)
        {
            
            Debug.Log(melonBase.GetType().FullName);

            if (melonBase.GetType().FullName == "GorillaLibrary.GameModes.Mod")
                gorillaLibMelon = melonBase;
            
            if (melonBase.GetType().BaseType != AccessTools.TypeByName("MelonMod"))
            {
                Debug.LogWarning("not melonmod");
                continue;
            }

            Type type = melonBase.GetType();

            IEnumerable<Gamemode> gamemodes = __instance.GetGamemodes(type);

            object gameJoin = AccessTools.Method(__instance.GetType(), "CreateJoinLeaveAction").Invoke(__instance, [melonBase, type, typeof(ModdedGamemodeJoinAttribute)]);
            object gameLeave = AccessTools.Method(__instance.GetType(), "CreateJoinLeaveAction").Invoke(__instance, [melonBase, type, typeof(ModdedGamemodeJoinAttribute)]);
            if (gamemodes.Any())
            {
                ModInfo info = new ModInfo
                {
                    Gamemodes = [.. gamemodes],
                    OnGamemodeJoin = (Action<string>)gameJoin,
                    OnGamemodeLeave = (Action<string>)gameLeave
                };
                AccessTools.Property(info.GetType(), "Mod").SetValue(info, melonBase);
                infos.Add(info);
            }
        }

        foreach (var pluginInfo in Chainloader.PluginInfos)
        {
            if (pluginInfo.Value is null) continue;
            BaseUnityPlugin plugin = pluginInfo.Value.Instance;
            if (plugin is null) continue;
            Type type = plugin.GetType();
            Debug.Log(type.FullName);

            IEnumerable<Gamemode> gamemodes = GetGamemodes(type, __instance);
            
            object gameJoin = AccessTools.Method(__instance.GetType(), "CreateJoinLeaveAction").Invoke(__instance, [gorillaLibMelon, type, typeof(Utilla.Attributes.ModdedGamemodeJoinAttribute)]);
            object gameLeave = AccessTools.Method(__instance.GetType(), "CreateJoinLeaveAction").Invoke(__instance, [gorillaLibMelon, type, typeof(Utilla.Attributes.ModdedGamemodeLeaveAttribute)]);
            
            if (gamemodes.Any())
            {
                ModInfo info = new ModInfo
                {
                    Gamemodes = [.. gamemodes],
                    OnGamemodeJoin = (Action<string>)gameJoin,
                    OnGamemodeLeave = (Action<string>)gameLeave
                };
                AccessTools.Property(info.GetType(), "Mod").SetValue(info, gorillaLibMelon);
                infos.Add(info);
            }
        }
        
        __result = infos;
        return false;
    }
    
    public static HashSet<Gamemode> GetGamemodes(Type type, GameModeManager __instance)
    {
        IEnumerable<Utilla.Attributes.ModdedGamemodeAttribute> customAttributes = type.GetCustomAttributes<Utilla.Attributes.ModdedGamemodeAttribute>();
        HashSet<Gamemode> gamemodes = new HashSet<Gamemode>();
        if (customAttributes != null)
        {
            foreach (Utilla.Attributes.ModdedGamemodeAttribute gamemodeAttribute in customAttributes)
            {
                if (gamemodeAttribute.gamemode != null)
                    gamemodes.Add(gamemodeAttribute.gamemode.underlyingGamemode);
                else
                    gamemodes.UnionWith(__instance.ModdedGamemodes);
            }
        }
        return gamemodes;
    }
}