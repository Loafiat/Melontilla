using System.IO;
using System.Reflection;
using BepInEx;
using HarmonyLib;

namespace Utilla;

[BepInDependency("Lofiat.MelInEx")]
[BepInPlugin(Constants.GUID, Constants.Name, Constants.Version)]
internal class Plugin : BaseUnityPlugin
{
    public Plugin()
    {
        if (File.Exists(Path.GetFullPath("./winhttp.dll"))) // if on BepInEx
        {
            // force load early
            Assembly.LoadFile(Path.GetFullPath("./Mods/GorillaLibrary.dll"));
            Assembly.LoadFile(Path.GetFullPath("./Mods/GorillaLibrary.GameModes.dll"));
        }
        Harmony.CreateAndPatchAll(Assembly.GetExecutingAssembly(), "Lofiat.Melontilla");
    }
}