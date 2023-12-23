using Game;
using Game.Modding;

namespace ZoneSpawnCustom.Mods
{
    internal class MyMod : IMod
    {
        public void OnCreateWorld(UpdateSystem updateSystem)
        {
            var world = updateSystem.World;
        }

        public void OnDispose()
        {

        }

        public void OnLoad()
        {

        }
    }
}
