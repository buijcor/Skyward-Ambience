using RimWorld;
using UnityEngine;
using Verse;

namespace SkywardAmbience
{
    public class SkywardAmbienceSettings : ModSettings
    {
        // Cloud noise settings
        public float CloudNoiseScale = 2.0f;
        public float CloudNoiseThreshold = 0.7f; // Noise values below this threshold are transparent
        
        // Cloud color settings (RGB values 0-1)
        public float CloudColorR = 0.2f;
        public float CloudColorG = 0.2f;
        public float CloudColorB = 0.25f;
        
        // Cloud transparency (alpha value 0-1)
        public float CloudTransparency = 0.3f;
        
        // Bird flock size multiplier
        public float BirdFlockSize = 1.0f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref CloudNoiseScale, "cloudNoiseScale", 2.0f);
            Scribe_Values.Look(ref CloudNoiseThreshold, "cloudNoiseThreshold", 0.7f);
            Scribe_Values.Look(ref CloudColorR, "cloudColorR", 0.2f);
            Scribe_Values.Look(ref CloudColorG, "cloudColorG", 0.2f);
            Scribe_Values.Look(ref CloudColorB, "cloudColorB", 0.25f);
            Scribe_Values.Look(ref CloudTransparency, "cloudTransparency", 0.3f);
            Scribe_Values.Look(ref BirdFlockSize, "birdFlockSize", 1.0f);
        }
    }
}

