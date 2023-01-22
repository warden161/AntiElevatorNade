using System.Linq;
using Exiled.API.Features;
using Exiled.API.Enums;
using Exiled.Events.EventArgs;
using Exiled.Events.EventArgs.Map;

namespace AntiElevatorNade.Handlers
{
    class Map
    {
        public bool IsFrag(ProjectileType projectile)
            => projectile switch
            {
                ProjectileType.FragGrenade => true,
                ProjectileType.Scp018 => true,
                _ => false,
            };

        public void OnExplodingGrenade(ExplodingGrenadeEventArgs ev)
        {
            // Checks if grenade harms players
            if (!IsFrag(ev.Projectile.ProjectileType)) return;

            // Checks if all affected players and thrower are of same side
            if (!(ev.TargetsToAffect.Count != 0 && ev.TargetsToAffect.Count >= AntiElevatorNade.Instance.Config.MinimumPlayersGrenade && ev.TargetsToAffect.All(player => ev.Player.Role.Side == player.Role.Side))) return; 

            foreach (Lift lift in Lift.List)
            {
                // Checks if affected players are in the elevator
                if (!((ev.TargetsToAffect.First().Position - lift.Position).sqrMagnitude < 13)) continue; 

                ev.IsAllowed = false; // Disables damage dealt to players

                // Checks config to see if thrower should be damaged by their grenade and is affected by explosion
                if (AntiElevatorNade.Instance.Config.KillThrower && ev.TargetsToAffect.Contains(ev.Player))
                {
                    ev.Player.ShowHint(AntiElevatorNade.Instance.Config.ThrowerHint, 5); // Gives hint to thrower
                    ev.Player.Hurt(-1, DamageType.Explosion); // Kills the thrower
                }
                return;
            }
        }
    }
}
