using Exiled.API.Features;
using Map = Exiled.Events.Handlers.Map;
using System;

namespace AntiElevatorNade
{
    public class AntiElevatorNade : Plugin<Config>
    {
        public static AntiElevatorNade Instance { get; private set; }
        public override string Author => "TemmieGamerGuy";
        public override string Name => "AntiElevatorNade";
        public override Version Version => new Version(1, 1, 0);
        public override Version RequiredExiledVersion => new Version(6, 0, 0);

        private Handlers.Map map;

        public void RegisterEvents()
        {
            map = new Handlers.Map();
            Map.ExplodingGrenade += map.OnExplodingGrenade;
        }

        public void UnregisterEvents()
        {
            Map.ExplodingGrenade -= map.OnExplodingGrenade;
            map = null;
        }

        public override void OnEnabled()
        {
            Instance = this;
            RegisterEvents();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            UnregisterEvents();
            Instance = null;
            base.OnDisabled();
        }
    }
}
