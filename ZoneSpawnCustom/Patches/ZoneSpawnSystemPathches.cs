using Game;
using Game.Common;
using Game.Simulation;
using HarmonyLib;
using ZoneSpawnCustom.Systems;

namespace ZoneSpawnCustom.Patches
{
    [HarmonyPatch(typeof(ZoneSpawnSystem))]
    public class ZoneSpawnSystemPathches
    {
        [HarmonyPatch("OnCreate")]
        [HarmonyPrefix]
        private static bool OnCreatePrefix(ZoneSpawnSystem __instance)
        {
            return false;
        }

        [HarmonyPatch("OnCreateForCompiler")]
        [HarmonyPrefix]
        private static bool OnCreateForCompilerPrefix(ZoneSpawnSystem __instance)
        {
            return false;
        }

        [HarmonyPatch("OnUpdate")]
        [HarmonyPrefix]
        private static bool OnUpdatePrefix(ZoneSpawnSystem __instance)
        {
            //__instance.World.GetOrCreateSystemManaged<MyZoneSpawnSystem>().Update();
            return false;
        }
    }

    [HarmonyPatch(typeof(SystemOrder))]
    public static class SystemOrderPatch
    {
        [HarmonyPatch("Initialize")]
        [HarmonyPostfix]
        public static void Postfix(UpdateSystem updateSystem)
        {
            updateSystem.UpdateAt<GetSizesSystem>(SystemUpdatePhase.MainLoop);
            updateSystem.UpdateAt<MyZoneSpawnSystem>(SystemUpdatePhase.GameSimulation);
            updateSystem.UpdateAt<ZoneSpawnCustomUISystem>(SystemUpdatePhase.UIUpdate);
        }
    }
}
