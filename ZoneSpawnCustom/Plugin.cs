using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using Colossal.UI.Binding;
using Game.Zones;
using HarmonyLib;
using HookUILib.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine.InputSystem.Utilities;
using ZoneSpawnCustom.Classes;

#if BEPINEX_V6
using BepInEx.Unity.Mono;
#endif


namespace ZoneSpawnCustom
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        public static WritableList<string> rSizes;
        public static WritableList<string> r1EuSizes;
        public static WritableList<string> r2EuSizes;
        public static WritableList<string> r3EuSizes;
        public static WritableList<string> r4EuSizes;
        public static WritableList<string> r6EuSizes;
        public static WritableList<string> r1NaSizes;
        public static WritableList<string> r2NaSizes;
        public static WritableList<string> r3NaSizes;
        public static WritableList<string> r4NaSizes;
        public static WritableList<string> r6NaSizes;
        public static WritableList<string> r5Sizes;

        public static WritableList<string> cSizes;
        public static WritableList<string> c1EuSizes;
        public static WritableList<string> c2EuSizes;
        public static WritableList<string> c1NaSizes;
        public static WritableList<string> c2NaSizes;

        public static WritableList<string> iSizes;

        public static WritableList<string> oSizes;
        public static WritableList<string> o1Sizes;
        public static WritableList<string> o2Sizes;

        public static bool initialized { get; private set; } = false;

        public static ConfigEntry<bool> m_rEnabled;
        public static ConfigEntry<bool> m_rDetailed;
        public static ConfigEntry<bool> m_cEnabled;
        public static ConfigEntry<bool> m_cDetailed;
        public static ConfigEntry<bool> m_iEnabled;
        public static ConfigEntry<bool> m_oEnabled;
        public static ConfigEntry<bool> m_oDetailed;

        public static ConfigEntry<string> m_rMaxSize;
        public static ConfigEntry<string> m_r1EuMaxSize;
        public static ConfigEntry<string> m_r2EuMaxSize;
        public static ConfigEntry<string> m_r3EuMaxSize;
        public static ConfigEntry<string> m_r4EuMaxSize;
        public static ConfigEntry<string> m_r6EuMaxSize;
        public static ConfigEntry<string> m_r1NaMaxSize;
        public static ConfigEntry<string> m_r2NaMaxSize;
        public static ConfigEntry<string> m_r3NaMaxSize;
        public static ConfigEntry<string> m_r4NaMaxSize;
        public static ConfigEntry<string> m_r6NaMaxSize;
        public static ConfigEntry<string> m_r5MaxSize;

        public static ConfigEntry<string> m_cMaxSize;
        public static ConfigEntry<string> m_c1EuMaxSize;
        public static ConfigEntry<string> m_c2EuMaxSize;
        public static ConfigEntry<string> m_c1NaMaxSize;
        public static ConfigEntry<string> m_c2NaMaxSize;

        public static ConfigEntry<string> m_iMaxSize;

        public static ConfigEntry<string> m_oMaxSize;
        public static ConfigEntry<string> m_o1MaxSize;
        public static ConfigEntry<string> m_o2MaxSize;

        public static ConfigEntry<string> m_rMinSize;
        public static ConfigEntry<string> m_r1EuMinSize;
        public static ConfigEntry<string> m_r2EuMinSize;
        public static ConfigEntry<string> m_r3EuMinSize;
        public static ConfigEntry<string> m_r4EuMinSize;
        public static ConfigEntry<string> m_r6EuMinSize;
        public static ConfigEntry<string> m_r1NaMinSize;
        public static ConfigEntry<string> m_r2NaMinSize;
        public static ConfigEntry<string> m_r3NaMinSize;
        public static ConfigEntry<string> m_r4NaMinSize;
        public static ConfigEntry<string> m_r6NaMinSize;
        public static ConfigEntry<string> m_r5MinSize;

        public static ConfigEntry<string> m_cMinSize;
        public static ConfigEntry<string> m_c1EuMinSize;
        public static ConfigEntry<string> m_c2EuMinSize;
        public static ConfigEntry<string> m_c1NaMinSize;
        public static ConfigEntry<string> m_c2NaMinSize;

        public static ConfigEntry<string> m_iMinSize;

        public static ConfigEntry<string> m_oMinSize;
        public static ConfigEntry<string> m_o1MinSize;
        public static ConfigEntry<string> m_o2MinSize;

        public static ManualLogSource Log { get; } = BepInEx.Logging.Logger.CreateLogSource(MyPluginInfo.PLUGIN_NAME);

        public static Plugin Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(MyPluginInfo.PLUGIN_NAME);

            harmony.PatchAll();
            Logger.LogInfo($"Plugin {MyPluginInfo.PLUGIN_NAME} is loaded!");
        }

        public void InitializeSizes(List<BuildingInfo> buildingInfos)
        {
            if (initialized) return;
            //BuildingInfos = buildingInfos;
            rSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & BuildingInfoFlag.R) != 0).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r1_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level1;
            r1EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r1_eu) == r1_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r2_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level2;
            r2EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r2_eu) == r2_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r3_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level3;
            r3EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r3_eu) == r3_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r4_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level4;
            r4EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r4_eu) == r4_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r6_eu = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level6;
            r6EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r6_eu) == r6_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r1_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level1;
            r1NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r1_na) == r1_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r2_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level2;
            r2NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r2_na) == r2_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r3_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level3;
            r3NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r3_na) == r3_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r4_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level4;
            r4NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r4_na) == r4_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r6_na = BuildingInfoFlag.R | BuildingInfoFlag.NA | BuildingInfoFlag.Level6;
            r6NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r6_na) == r6_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var r5 = BuildingInfoFlag.R | BuildingInfoFlag.EU | BuildingInfoFlag.Level5;
            r5Sizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & r5) == r5).Select(s => s.SizeString).Distinct().OrderBy(s => s));

            cSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & BuildingInfoFlag.C) != 0).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var c1_eu = BuildingInfoFlag.C | BuildingInfoFlag.EU | BuildingInfoFlag.Level1;
            c1EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & c1_eu) == c1_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var c2_eu = BuildingInfoFlag.C | BuildingInfoFlag.EU | BuildingInfoFlag.Level2;
            c2EuSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & c2_eu) == c2_eu).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var c1_na = BuildingInfoFlag.C | BuildingInfoFlag.NA | BuildingInfoFlag.Level1;
            c1NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & c1_na) == c1_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var c2_na = BuildingInfoFlag.C | BuildingInfoFlag.NA | BuildingInfoFlag.Level2;
            c2NaSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & c2_na) == c2_na).Select(s => s.SizeString).Distinct().OrderBy(s => s));

            iSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & BuildingInfoFlag.I) != 0).Select(s => s.SizeString).Distinct().OrderBy(s => s));

            oSizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & BuildingInfoFlag.O) != 0).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var o1 = BuildingInfoFlag.O | BuildingInfoFlag.Level1;
            o1Sizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & o1) == o1).Select(s => s.SizeString).Distinct().OrderBy(s => s));
            var o2 = BuildingInfoFlag.O | BuildingInfoFlag.Level2;
            o2Sizes = new WritableList<string>(buildingInfos.Where(s => (s.Flags & o2) == o2).Select(s => s.SizeString).Distinct().OrderBy(s => s));

            m_rEnabled = Config.Bind("Residential", "Enabled", false);
            m_rDetailed = Config.Bind("Residential", "Detailed", false);
            m_rMinSize = Config.Bind("Residential", "MinSize", rSizes.First());
            m_rMaxSize = Config.Bind("Residential", "MaxSize", rSizes.Last());
            m_r1EuMinSize = Config.Bind("Residential", "EULowMinSize", r1EuSizes.First());
            m_r1EuMaxSize = Config.Bind("Residential", "EULowMaxSize", r1EuSizes.Last());
            m_r2EuMinSize = Config.Bind("Residential", "EUMediumRowMinSize", r2EuSizes.First());
            m_r2EuMaxSize = Config.Bind("Residential", "EUMediumRowMaxSize", r2EuSizes.Last());
            m_r3EuMinSize = Config.Bind("Residential", "EUMediumMinSize", r3EuSizes.First());
            m_r3EuMaxSize = Config.Bind("Residential", "EUMediumMaxSize", r3EuSizes.Last());
            m_r4EuMinSize = Config.Bind("Residential", "EUMixedMinSize", r4EuSizes.First());
            m_r4EuMaxSize = Config.Bind("Residential", "EUMixedMaxSize", r4EuSizes.Last());
            m_r6EuMinSize = Config.Bind("Residential", "EUHighMinSize", r6EuSizes.First());
            m_r6EuMaxSize = Config.Bind("Residential", "EUHighMaxSize", r6EuSizes.Last());
            m_r1NaMinSize = Config.Bind("Residential", "NALowMinSize", r1NaSizes.First());
            m_r1NaMaxSize = Config.Bind("Residential", "NALowMaxSize", r1NaSizes.Last());
            m_r2NaMinSize = Config.Bind("Residential", "NAMediumRowMinSize", r2NaSizes.First());
            m_r2NaMaxSize = Config.Bind("Residential", "NAMediumRowMaxSize", r2NaSizes.Last());
            m_r3NaMinSize = Config.Bind("Residential", "NAMediumMinSize", r3NaSizes.First());
            m_r3NaMaxSize = Config.Bind("Residential", "NAMediumMaxSize", r3NaSizes.Last());
            m_r4NaMinSize = Config.Bind("Residential", "NAMixedMinSize", r4NaSizes.First());
            m_r4NaMaxSize = Config.Bind("Residential", "NAMixedMaxSize", r4NaSizes.Last());
            m_r6NaMinSize = Config.Bind("Residential", "NAHighMinSize", r6NaSizes.First());
            m_r6NaMaxSize = Config.Bind("Residential", "NAHighMaxSize", r6NaSizes.Last());
            m_r5MinSize = Config.Bind("Residential", "LowRentMinSize", r5Sizes.First());
            m_r5MaxSize = Config.Bind("Residential", "LowRentMaxSize", r5Sizes.Last());

            m_cEnabled = Config.Bind("Commercial", "Enabled", false);
            m_cDetailed = Config.Bind("Commercial", "Detailed", false);
            m_cMinSize = Config.Bind("Commercial", "MinSize", cSizes.First());
            m_cMaxSize = Config.Bind("Commercial", "MaxSize", cSizes.Last());
            m_c1EuMinSize = Config.Bind("Commercial", "EULowMinSize", c1EuSizes.First());
            m_c1EuMaxSize = Config.Bind("Commercial", "EULowMaxSize", c1EuSizes.Last());
            m_c2EuMinSize = Config.Bind("Commercial", "EUHighMinSize", c2EuSizes.First());
            m_c2EuMaxSize = Config.Bind("Commercial", "EUHighMaxSize", c2EuSizes.Last());
            m_c1NaMinSize = Config.Bind("Commercial", "NALowMinSize", c1NaSizes.First());
            m_c1NaMaxSize = Config.Bind("Commercial", "NALowMaxSize", c1NaSizes.Last());
            m_c2NaMinSize = Config.Bind("Commercial", "NAHighMinSize", c2NaSizes.First());
            m_c2NaMaxSize = Config.Bind("Commercial", "NAHighMaxSize", c2NaSizes.Last());

            m_iEnabled = Config.Bind("Industrial", "Enabled", false);
            m_iMinSize = Config.Bind("Industrial", "MinSize", iSizes.First());
            m_iMaxSize = Config.Bind("Industrial", "MaxSize", iSizes.Last());

            m_oEnabled = Config.Bind("Office", "Enabled", false);
            m_oDetailed = Config.Bind("Office", "Detailed", false);
            m_oMinSize = Config.Bind("Office", "MinSize", oSizes.First());
            m_oMaxSize = Config.Bind("Office", "MaxSize", oSizes.Last());
            m_o1MinSize = Config.Bind("Office", "LowMinSize", o1Sizes.First());
            m_o1MaxSize = Config.Bind("Office", "LowMaxSize", o1Sizes.Last());
            m_o2MinSize = Config.Bind("Office", "HighMinSize", o2Sizes.First());
            m_o2MaxSize = Config.Bind("Office", "HighMaxSize", o2Sizes.Last());

            initialized = true;
        }

        public static int2 GetSizeFromString(string s)
        {
            var temp = s.Split('*');
            if (temp.Length != 2) throw new ArgumentException();
            int x = Convert.ToInt32(temp[0]);
            int y = Convert.ToInt32(temp[1]);
            return new int2(x, y);
        }

        public static bool JudgeBuilding(int index, int2 size)
        {
            bool result = true;
            BuildingInfoFlag flags = BuildingInfo.GetBuildingInfoFlag(index);
            if ((flags & BuildingInfoFlag.R) != 0)
            {
                if (m_rEnabled.Value)
                {
                    if (m_rDetailed.Value)
                    {
                        if ((flags & BuildingInfoFlag.Level1) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_r1EuMinSize.Value);
                                int2 max = GetSizeFromString(m_r1EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_r1NaMinSize.Value);
                                int2 max = GetSizeFromString(m_r1NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                        if ((flags & BuildingInfoFlag.Level2) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_r2EuMinSize.Value);
                                int2 max = GetSizeFromString(m_r2EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_r2NaMinSize.Value);
                                int2 max = GetSizeFromString(m_r2NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                        if ((flags & BuildingInfoFlag.Level3) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_r3EuMinSize.Value);
                                int2 max = GetSizeFromString(m_r3EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_r3NaMinSize.Value);
                                int2 max = GetSizeFromString(m_r3NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                        if ((flags & BuildingInfoFlag.Level4) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_r4EuMinSize.Value);
                                int2 max = GetSizeFromString(m_r4EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_r4NaMinSize.Value);
                                int2 max = GetSizeFromString(m_r4NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                        if ((flags & BuildingInfoFlag.Level5) != 0)
                        {
                            int2 min = GetSizeFromString(m_r5MinSize.Value);
                            int2 max = GetSizeFromString(m_r5MaxSize.Value);
                            result = math.all(size >= min) && math.all(size <= max);
                        }
                        if ((flags & BuildingInfoFlag.Level6) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_r6EuMinSize.Value);
                                int2 max = GetSizeFromString(m_r6EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_r6NaMinSize.Value);
                                int2 max = GetSizeFromString(m_r6NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                    }
                    else
                    {
                        int2 min = GetSizeFromString(m_rMinSize.Value);
                        int2 max = GetSizeFromString(m_rMaxSize.Value);
                        result = math.all(size >= min) && math.all(size <= max);
                    }
                }
            }
            if ((flags & BuildingInfoFlag.C) != 0)
            {
                if (m_cEnabled.Value)
                {
                    if (m_cDetailed.Value)
                    {
                        if ((flags & BuildingInfoFlag.Level1) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_c1EuMinSize.Value);
                                int2 max = GetSizeFromString(m_c1EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_c1NaMinSize.Value);
                                int2 max = GetSizeFromString(m_c1NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                        if ((flags & BuildingInfoFlag.Level2) != 0)
                        {
                            if ((flags & BuildingInfoFlag.EU) != 0)
                            {
                                int2 min = GetSizeFromString(m_c2EuMinSize.Value);
                                int2 max = GetSizeFromString(m_c2EuMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                            if ((flags & BuildingInfoFlag.NA) != 0)
                            {
                                int2 min = GetSizeFromString(m_c2NaMinSize.Value);
                                int2 max = GetSizeFromString(m_c2NaMaxSize.Value);
                                result = math.all(size >= min) && math.all(size <= max);
                            }
                        }
                    }
                    else
                    {
                        int2 min = GetSizeFromString(m_cMinSize.Value);
                        int2 max = GetSizeFromString(m_cMaxSize.Value);
                        result = math.all(size >= min) && math.all(size <= max);
                    }
                }
            }
            if ((flags & BuildingInfoFlag.I) != 0)
            {
                if (m_iEnabled.Value)
                {
                    int2 min = GetSizeFromString(m_iMinSize.Value);
                    int2 max = GetSizeFromString(m_iMaxSize.Value);
                    result = math.all(size >= min) && math.all(size <= max);
                }
            }
            if ((flags & BuildingInfoFlag.O) != 0)
            {
                if (m_oEnabled.Value)
                {
                    if (m_oDetailed.Value)
                    {
                        if ((flags & BuildingInfoFlag.Level1) != 0)
                        {
                            int2 min = GetSizeFromString(m_o1MinSize.Value);
                            int2 max = GetSizeFromString(m_o1MaxSize.Value);
                            result = math.all(size >= min) && math.all(size <= max);
                        }
                        if ((flags & BuildingInfoFlag.Level2) != 0)
                        {
                            int2 min = GetSizeFromString(m_o2MinSize.Value);
                            int2 max = GetSizeFromString(m_o2MaxSize.Value);
                            result = math.all(size >= min) && math.all(size <= max);
                        }
                    }
                    else
                    {
                        int2 min = GetSizeFromString(m_oMinSize.Value);
                        int2 max = GetSizeFromString(m_oMaxSize.Value);
                        result = math.all(size >= min) && math.all(size <= max);
                    }
                }
            }

            return result;
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
