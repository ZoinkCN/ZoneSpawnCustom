using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using HookUILib.Core;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Unity.Mathematics;
using UnityEngine.InputSystem.Utilities;

#if BEPINEX_V6
using BepInEx.Unity.Mono;
#endif


namespace ZoneSpawnCustom
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static ReadOnlyDictionary<string, int2> ResidentialSizes { get; private set; }
        public static ReadOnlyDictionary<string, int2> CommercialSizes { get; private set; }
        public static ReadOnlyDictionary<string, int2> IndustrialSizes { get; private set; }
        public static ReadOnlyDictionary<string, int2> OfficeSizes { get; private set; }

        public static bool initialized { get; private set; } = false;

        public static ReadOnlyArray<int> ResidentialIndexes { get; } = new ReadOnlyArray<int>(new int[] { 2, 3, 5, 6, 7, 8, 9, 10, 11, 19, 20 });
        public static ReadOnlyArray<int> CommercialIndexes { get; } = new ReadOnlyArray<int>(new int[] { 1, 4, 17, 18 });
        public static ReadOnlyArray<int> IndustrialIndexes { get; } = new ReadOnlyArray<int>(new int[] { 22 });
        public static ReadOnlyArray<int> OfficeIndexes { get; } = new ReadOnlyArray<int>(new int[] { 12, 21 });

        public static ConfigEntry<bool> m_rEnabled;
        public static ConfigEntry<bool> m_cEnabled;
        public static ConfigEntry<bool> m_iEnabled;
        public static ConfigEntry<bool> m_oEnabled;

        public static ConfigEntry<string> m_rMaxSize;
        public static ConfigEntry<string> m_cMaxSize;
        public static ConfigEntry<string> m_iMaxSize;
        public static ConfigEntry<string> m_oMaxSize;

        public static ConfigEntry<string> m_rMinSize;
        public static ConfigEntry<string> m_cMinSize;
        public static ConfigEntry<string> m_iMinSize;
        public static ConfigEntry<string> m_oMinSize;

        public static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);

        private void Awake()
        {
            var harmony = new Harmony(MyPluginInfo.PLUGIN_NAME);

            m_rEnabled = Config.Bind<bool>("Residential", "Enabled", false);
            m_rMinSize = Config.Bind<string>("Residential", "MinSize", "2*2");
            m_rMaxSize = Config.Bind<string>("Residential", "MaxSize", "6*6");
            m_cEnabled = Config.Bind<bool>("Commercial", "Enabled", false);
            m_cMinSize = Config.Bind<string>("Commercial", "MinSize", "2*2");
            m_cMaxSize = Config.Bind<string>("Commercial", "MaxSize", "6*6");
            m_iEnabled = Config.Bind<bool>("Industrial", "Enabled", false);
            m_iMinSize = Config.Bind<string>("Industrial", "MinSize", "2*2");
            m_iMaxSize = Config.Bind<string>("Industrial", "MaxSize", "6*6");
            m_oEnabled = Config.Bind<bool>("Office", "Enabled", false);
            m_oMinSize = Config.Bind<string>("Office", "MinSize", "2*2");
            m_oMaxSize = Config.Bind<string>("Office", "MaxSize", "6*6");

            harmony.PatchAll();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
        }

        public static void InitializeSizes(IDictionary<string, int2> rSizes, IDictionary<string, int2> cSizes, IDictionary<string, int2> iSizes, IDictionary<string, int2> oSizes)
        {
            if (initialized) return;
            ResidentialSizes = new ReadOnlyDictionary<string, int2>(rSizes);
            CommercialSizes = new ReadOnlyDictionary<string, int2>(cSizes);
            IndustrialSizes = new ReadOnlyDictionary<string, int2>(iSizes);
            OfficeSizes = new ReadOnlyDictionary<string, int2>(oSizes);

            initialized = true;
        }
    }

    public class PluginUI : UIExtension
    {
        public new readonly ExtensionType extensionType = ExtensionType.Panel;
        public new readonly string extensionID = "zoinkcn.zonespawncustom";
        public new readonly string extensionContent;
        public PluginUI()
        {
            extensionContent = LoadEmbeddedResource("ZoneSpawnCustom.UI.ZoneSpawnCustomUI.transpiled.js");
        }
    }
}
