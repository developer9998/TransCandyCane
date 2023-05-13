using BepInEx;
using HarmonyLib;
using System.Reflection;
using TransCandyCane.Scripts;

namespace TransCandyCane
{
    [BepInPlugin(PluginInfo.GUID, PluginInfo.Name, PluginInfo.Version)]
    public class Plugin : BaseUnityPlugin
    {
        private Harmony Harmony;

        public void Awake()
        {
            // LoadTexture uses config, so load the config first
            Main.LoadConfig();
            Main.LoadTexture();

            Harmony = new Harmony(PluginInfo.GUID);
            Harmony.PatchAll(Assembly.GetExecutingAssembly());
        }
    }
}