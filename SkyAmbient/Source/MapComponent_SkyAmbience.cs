using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SkywardAmbience
{
    public class MapComponent_SkyAmbience : MapComponent
    {
        private List<BirdFlock> birdFlocks = new List<BirdFlock>();
        private CloudShadow cloudShadow;
        
        // Spawn every 20-45 minutes: average ~32.5 minutes = 32.5 * 60 * 60 ticks = 117,000 ticks
        // Chance per tick = 1 / 117,000 ≈ 0.0000085
        private const float BirdSpawnChancePerTick = 0.0000085f;
        private const int MaxConcurrentBirdFlocks = 1;

        public MapComponent_SkyAmbience(Map map) : base(map)
        {
            cloudShadow = new CloudShadow(map);
            
            if (Prefs.DevMode)
            {
                Log.Message("[Skyward Ambience] MapComponent initialized on map: " + map.uniqueID);
            }
        }

        public override void MapComponentTick()
        {
            base.MapComponentTick();

            // Update and remove expired bird flocks
            for (int i = birdFlocks.Count - 1; i >= 0; i--)
            {
                birdFlocks[i].Tick();
                if (birdFlocks[i].ShouldRemove())
                {
                    birdFlocks.RemoveAt(i);
                }
            }

            cloudShadow?.Tick();

            // Spawn new bird flock
            if (birdFlocks.Count < MaxConcurrentBirdFlocks && Rand.Value < BirdSpawnChancePerTick)
            {
                Vector2? targetPosition = null;
                if (Find.CameraDriver != null)
                {
                    CellRect viewRect = Find.CameraDriver.CurrentViewRect;
                    if (viewRect.Area > 0)
                    {
                        IntVec3 viewCenter = viewRect.CenterCell;
                        targetPosition = new Vector2(viewCenter.x, viewCenter.z);
                    }
                }
                
                birdFlocks.Add(new BirdFlock(map, targetPosition));
                
                if (Prefs.DevMode)
                {
                    Log.Message("[Skyward Ambience] Bird flock spawned. Total active: " + birdFlocks.Count);
                }
            }
        }

        public override void MapComponentUpdate()
        {
            base.MapComponentUpdate();
            
            foreach (var flock in birdFlocks)
            {
                flock.Draw();
            }

            cloudShadow?.Draw();
        }
    }
}
