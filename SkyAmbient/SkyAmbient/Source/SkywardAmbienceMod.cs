using RimWorld;
using UnityEngine;
using Verse;

namespace SkywardAmbience
{
    [StaticConstructorOnStartup]
    public class SkywardAmbienceMod : Mod
    {
        private SkywardAmbienceSettings settings;

        static SkywardAmbienceMod()
        {
            Log.Message("[Skyward Ambience] Mod loaded successfully.");
        }

        public SkywardAmbienceMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<SkywardAmbienceSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect);
            
            listingStandard.Label("Cloud Settings");
            listingStandard.GapLine(12f);
            
            // Cloud Noise Scale
            listingStandard.Label("Cloud Noise Scale: " + settings.CloudNoiseScale.ToString("F1"));
            settings.CloudNoiseScale = listingStandard.Slider(settings.CloudNoiseScale, 0.5f, 5.0f);
            listingStandard.Gap(6f);
            
            // Cloud Noise Threshold
            listingStandard.Label("Cloud Noise Threshold: " + settings.CloudNoiseThreshold.ToString("F2"));
            settings.CloudNoiseThreshold = listingStandard.Slider(settings.CloudNoiseThreshold, 0.0f, 1.0f);
            listingStandard.Gap(12f);
            
            listingStandard.Label("Bird Flock Settings");
            listingStandard.GapLine(12f);
            
            // Bird Flock Size
            listingStandard.Label("Bird Flock Size: " + (settings.BirdFlockSize * 100f).ToString("F0") + "%");
            settings.BirdFlockSize = listingStandard.Slider(settings.BirdFlockSize, 0.1f, 3.0f);
            listingStandard.Gap(12f);
            
            // Cloud Color settings disabled - hardcoded to 20% (0.2) for all channels
            // Cloud Color R
            // listingStandard.Label("Cloud Color - Red: " + (settings.CloudColorR * 100f).ToString("F0") + "%");
            // settings.CloudColorR = listingStandard.Slider(settings.CloudColorR, 0f, 1f);
            // listingStandard.Gap(6f);
            
            // Cloud Color G
            // listingStandard.Label("Cloud Color - Green: " + (settings.CloudColorG * 100f).ToString("F0") + "%");
            // settings.CloudColorG = listingStandard.Slider(settings.CloudColorG, 0f, 1f);
            // listingStandard.Gap(6f);
            
            // Cloud Color B
            // listingStandard.Label("Cloud Color - Blue: " + (settings.CloudColorB * 100f).ToString("F0") + "%");
            // settings.CloudColorB = listingStandard.Slider(settings.CloudColorB, 0f, 1f);
            // listingStandard.Gap(12f);
            
            // Cloud Transparency
            listingStandard.Label("Cloud Transparency: " + (settings.CloudTransparency * 100f).ToString("F0") + "%");
            settings.CloudTransparency = listingStandard.Slider(settings.CloudTransparency, 0.1f, 1.0f);
            listingStandard.Gap(12f);
            
            // Reset button
            if (listingStandard.ButtonText("Reset to Defaults"))
            {
                settings.CloudNoiseScale = 2.0f;
                settings.CloudNoiseThreshold = 0.7f;
                settings.CloudColorR = 0.2f;
                settings.CloudColorG = 0.2f;
                settings.CloudColorB = 0.25f;
                settings.CloudTransparency = 0.3f;
                settings.BirdFlockSize = 1.0f;
            }
            
            listingStandard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Skyward Ambience";
        }
    }
}
