using RimWorld;
using UnityEngine;
using Verse;

namespace SkywardAmbience
{
    [StaticConstructorOnStartup]
    public class CloudShadow
    {
        private Map map;
        private Vector2 noiseOffset;
        private Vector2 noiseVelocity;
        
        // Smooth transition variables
        private float currentTransparency = 0.3f;
        private Texture2D currentTexture;
        private Texture2D targetTexture;
        private float textureBlendFactor = 1f; // 0 = old texture, 1 = new texture
        private const float TransitionSpeed = 0.0005f; // How fast transitions occur (per frame)
        
        // Weather change detection and border roll-in effect
        private WeatherDef lastWeather;
        private bool isWeatherTransitioning = false;
        private Vector2 borderRollInOffset = Vector2.zero; // Offset to make texture start from border (in cloud movement direction)
        private Vector2 borderRollInDirection = Vector2.zero; // Direction of roll-in (matches cloud movement)
        private const float BorderRollInSpeed = 0.0005f; // How fast the border roll-in completes
        private const float MaxBorderOffset = 1.0f; // Maximum offset (starts from border)
        
        private const float BaseNoiseSpeed = 0.00005f;

        public CloudShadow(Map map)
        {
            this.map = map;
            
            // Random starting noise offset
            noiseOffset = new Vector2(Rand.Range(0f, 1000f), Rand.Range(0f, 1000f));
            
            // Random velocity for noise movement (creates drifting effect)
            float angle = Rand.Range(0f, 360f) * Mathf.Deg2Rad;
            float speed = BaseNoiseSpeed * Rand.Range(0.5f, 1.5f);
            noiseVelocity = new Vector2(Mathf.Cos(angle) * speed, Mathf.Sin(angle) * speed);
            
            // Initialize current texture and transparency
            currentTexture = noiseTexture;
            targetTexture = noiseTexture;
            currentTransparency = 0.3f;
            textureBlendFactor = 1f;
            
            // Initialize weather tracking
            lastWeather = map.weatherManager.curWeather;
            isWeatherTransitioning = false;
            borderRollInOffset = Vector2.zero;
            borderRollInDirection = Vector2.zero;
        }

        public void Tick()
        {
            // Check if torrential rain - if so, triple the cloud speed
            WeatherDef currentWeather = map.weatherManager.curWeather;
            string weatherDefName = currentWeather?.defName ?? "";
            bool isTorrentialRain = weatherDefName.Contains("Torrential") || weatherDefName.Contains("torrential");
            
            // Calculate current velocity (triple if torrential rain)
            Vector2 currentVelocity = isTorrentialRain ? noiseVelocity * 3f : noiseVelocity;
            
            // Update noise offset to create moving effect
            noiseOffset += currentVelocity;
        }

        public bool ShouldRemove()
        {
            // Cloud is always present, never remove
            return false;
        }

        private static Material cloudMaterial;
        private static Material cloudMaterialBlend;
        private static Texture2D noiseTexture;
        private static Texture2D rainNoiseTexture;
        private static SkywardAmbienceMod cachedMod;

        static CloudShadow()
        {
            cloudMaterial = new Material(ShaderDatabase.Transparent);
            cloudMaterialBlend = new Material(ShaderDatabase.Transparent);
            LoadNoiseTexture();
            LoadRainNoiseTexture();
            cachedMod = LoadedModManager.GetMod<SkywardAmbienceMod>();
        }
        
        private static SkywardAmbienceSettings GetSettings()
        {
            if (cachedMod == null)
            {
                cachedMod = LoadedModManager.GetMod<SkywardAmbienceMod>();
            }
            
            if (cachedMod != null)
            {
                return cachedMod.GetSettings<SkywardAmbienceSettings>();
            }
            
            return null;
        }

        private static void LoadNoiseTexture()
        {
            var mod = LoadedModManager.GetMod<SkywardAmbienceMod>();
            if (mod != null && mod.Content != null)
            {
                Texture2D loadedTexture = ContentFinder<Texture2D>.Get("Textures/CloudNoise", false);
                if (loadedTexture == null)
                {
                    loadedTexture = ContentFinder<Texture2D>.Get("CloudNoise", false);
                }
                
                if (loadedTexture != null)
                {
                    noiseTexture = loadedTexture;
                    noiseTexture.wrapMode = TextureWrapMode.Repeat;
                    noiseTexture.filterMode = FilterMode.Bilinear;
                    if (Prefs.DevMode)
                    {
                        Log.Message("[Skyward Ambience] Loaded cloud noise texture from file.");
                    }
                    return;
                }
            }
            
            if (Prefs.DevMode)
            {
                Log.Warning("[Skyward Ambience] Cloud noise texture file not found, generating programmatically.");
            }
            GenerateNoiseTexture();
        }
        
        private static void LoadRainNoiseTexture()
        {
            var mod = LoadedModManager.GetMod<SkywardAmbienceMod>();
            if (mod != null && mod.Content != null)
            {
                Texture2D loadedTexture = ContentFinder<Texture2D>.Get("Textures/CloudNoiseRain", false);
                if (loadedTexture == null)
                {
                    loadedTexture = ContentFinder<Texture2D>.Get("CloudNoiseRain", false);
                }
                
                if (loadedTexture != null)
                {
                    rainNoiseTexture = loadedTexture;
                    rainNoiseTexture.wrapMode = TextureWrapMode.Repeat;
                    rainNoiseTexture.filterMode = FilterMode.Bilinear;
                    if (Prefs.DevMode)
                    {
                        Log.Message("[Skyward Ambience] Loaded rain cloud noise texture from file.");
                    }
                    return;
                }
            }
            
            if (Prefs.DevMode)
            {
                Log.Warning("[Skyward Ambience] Rain cloud noise texture file not found, using regular cloud texture as fallback.");
            }
            rainNoiseTexture = noiseTexture;
        }
        
        private static void GenerateNoiseTexture()
        {
            var settings = GetSettings();
            float noiseThreshold = settings != null ? settings.CloudNoiseThreshold : 0.7f;
            
            int textureSize = 256;
            noiseTexture = new Texture2D(textureSize, textureSize, TextureFormat.RGBA32, false);
            noiseTexture.name = "CloudNoise";
            
            int borderPixels = Mathf.RoundToInt(textureSize * 0.02f);
            float fadeDistance = borderPixels * 2f;
            for (int x = 0; x < textureSize; x++)
            {
                for (int y = 0; y < textureSize; y++)
                {
                    float distFromLeft = x;
                    float distFromRight = textureSize - 1 - x;
                    float distFromTop = y;
                    float distFromBottom = textureSize - 1 - y;
                    float minDistToEdge = Mathf.Min(distFromLeft, distFromRight, distFromTop, distFromBottom);
                    
                    float fadeFactor = 1.0f;
                    if (minDistToEdge < fadeDistance)
                    {
                        float normalizedDist = minDistToEdge / fadeDistance;
                        fadeFactor = normalizedDist * normalizedDist * (3f - 2f * normalizedDist);
                    }
                    
                    float fx = (float)x / textureSize;
                    float fy = (float)y / textureSize;
                    
                    float noiseValue = Mathf.PerlinNoise(fx * 4f, fy * 4f) * 0.6f +
                                      Mathf.PerlinNoise(fx * 8f, fy * 8f) * 0.3f +
                                      Mathf.PerlinNoise(fx * 16f, fy * 16f) * 0.1f;
                    
                    noiseValue = Mathf.Clamp01(noiseValue);
                    noiseValue *= fadeFactor;
                    
                    float alpha = noiseValue < noiseThreshold ? 0f : noiseValue;
                    Color color = new Color(noiseValue, noiseValue, noiseValue, alpha);
                    noiseTexture.SetPixel(x, y, color);
                }
            }
            
            noiseTexture.Apply();
            noiseTexture.wrapMode = TextureWrapMode.Repeat;
            noiseTexture.filterMode = FilterMode.Bilinear;
        }
        
        public void Draw()
        {
            if (map.weatherManager.curWeather == WeatherDefOf.Clear)
            {
                return;
            }
            
            var settings = GetSettings();
            if (settings == null) return;
            
            if (noiseTexture == null)
            {
                LoadNoiseTexture();
            }
            if (rainNoiseTexture == null)
            {
                LoadRainNoiseTexture();
            }
            
            CellRect viewRect = Find.CameraDriver.CurrentViewRect;
            if (viewRect.Area == 0) return;
            
            DrawCloudLayer(settings, viewRect.ExpandedBy(2));
        }
        
        private void DrawCloudLayer(SkywardAmbienceSettings settings, CellRect unusedRect)
        {
            WeatherDef currentWeather = map.weatherManager.curWeather;
            string weatherDefName = currentWeather?.defName ?? "";
            
            if (currentWeather != lastWeather)
            {
                isWeatherTransitioning = true;
                bool isTorrentialRainForDirection = weatherDefName.Contains("Torrential") || weatherDefName.Contains("torrential");
                Vector2 currentVelocity = isTorrentialRainForDirection ? noiseVelocity * 3f : noiseVelocity;
                
                if (currentVelocity.magnitude > 0.0001f)
                {
                    borderRollInDirection = currentVelocity.normalized;
                }
                else
                {
                    borderRollInDirection = new Vector2(1f, 1f).normalized;
                }
                
                borderRollInOffset = -borderRollInDirection * MaxBorderOffset;
                lastWeather = currentWeather;
            }
            
            bool isCloudyWeather = weatherDefName == "Cloudy" || weatherDefName.Contains("Cloudy");
            bool isFogWeather = weatherDefName == "Fog" || weatherDefName == "Foggy";
            bool isRainWeather = weatherDefName == "Rain" || weatherDefName == "RainyThunderstorm" || weatherDefName.Contains("Rain");
            bool isThunderRain = weatherDefName == "RainyThunderstorm" || weatherDefName.Contains("Thunderstorm");
            bool isTorrentialRain = weatherDefName.Contains("Torrential") || weatherDefName.Contains("torrential");
            bool isBlindFog = weatherDefName == "BlindFog" || weatherDefName.Contains("BlindFog");
            bool isBlizzard = weatherDefName == "Blizzard" || weatherDefName.Contains("Blizzard");
            bool isDryThunderstorm = weatherDefName == "DryThunderstorm" || weatherDefName.Contains("DryThunderstorm");
            bool isHardSnow = weatherDefName == "HardSnow" || weatherDefName.Contains("HardSnow");
            bool isDeathPall = weatherDefName == "DeathPall" || weatherDefName.Contains("DeathPall");
            bool isGentleSnow = weatherDefName == "GentleSnow" || weatherDefName.Contains("GentleSnow");
            bool isBloodRain = weatherDefName == "BloodRain" || weatherDefName.Contains("BloodRain");
            bool isFoggyRain = weatherDefName == "FoggyRain" || weatherDefName.Contains("FoggyRain");
            bool isDLCWeather = currentWeather != null && currentWeather.modContentPack != null && currentWeather.modContentPack.IsCoreMod == false;
            
            float targetTransparency;
            Texture2D targetTextureToUse;
            bool useRedTint = false;
            
            if (isCloudyWeather)
            {
                targetTransparency = settings.CloudTransparency;
                targetTextureToUse = noiseTexture;
            }
            else if (isFogWeather)
            {
                targetTransparency = 0.06f;
                targetTextureToUse = noiseTexture;
            }
            else if (isRainWeather)
            {
                targetTransparency = 0.65f;
                bool useRainTexture = (isThunderRain || isTorrentialRain) && rainNoiseTexture != null;
                targetTextureToUse = useRainTexture ? rainNoiseTexture : noiseTexture;
            }
            else if (isBlindFog || isBlizzard || isDryThunderstorm || isHardSnow || isDeathPall)
            {
                targetTransparency = settings.CloudTransparency;
                targetTextureToUse = rainNoiseTexture != null ? rainNoiseTexture : noiseTexture;
            }
            else if (isGentleSnow || isFoggyRain)
            {
                targetTransparency = settings.CloudTransparency;
                targetTextureToUse = noiseTexture;
            }
            else if (isBloodRain)
            {
                targetTransparency = 0.65f;
                targetTextureToUse = noiseTexture;
                useRedTint = true;
            }
            else if (isDLCWeather)
            {
                targetTransparency = settings.CloudTransparency;
                targetTextureToUse = noiseTexture;
            }
            else
            {
                targetTransparency = settings.CloudTransparency;
                targetTextureToUse = noiseTexture;
            }
            
            if (Mathf.Abs(currentTransparency - targetTransparency) > 0.001f)
            {
                currentTransparency = Mathf.Lerp(currentTransparency, targetTransparency, TransitionSpeed);
            }
            else
            {
                currentTransparency = targetTransparency;
            }
            
            if (targetTextureToUse != targetTexture)
            {
                if (textureBlendFactor >= 1f)
                {
                    currentTexture = targetTexture;
                    targetTexture = targetTextureToUse;
                    textureBlendFactor = 0f;
                }
            }
            
            if (textureBlendFactor < 1f)
            {
                textureBlendFactor = Mathf.Min(1f, textureBlendFactor + TransitionSpeed);
            }
            
            if (isWeatherTransitioning)
            {
                bool isTorrentialRainForSpeed = weatherDefName.Contains("Torrential") || weatherDefName.Contains("torrential");
                Vector2 currentVelocity = isTorrentialRainForSpeed ? noiseVelocity * 3f : noiseVelocity;
                float currentSpeed = currentVelocity.magnitude;
                float rollInSpeed = Mathf.Max(BorderRollInSpeed, currentSpeed * 50f);
                Vector2 rollInStep = borderRollInDirection * rollInSpeed;
                borderRollInOffset += rollInStep;
                
                if (Vector2.Dot(borderRollInOffset, -borderRollInDirection) <= 0f)
                {
                    borderRollInOffset = Vector2.zero;
                    isWeatherTransitioning = false;
                }
            }
            
            Color baseCloudColor = useRedTint ? new Color(0.35f, 0.2f, 0.2f, 1f) : new Color(0.2f, 0.2f, 0.2f, 1f);
            
            if (textureBlendFactor < 1f && currentTexture != targetTexture)
            {
                cloudMaterial.mainTexture = currentTexture;
                cloudMaterialBlend.mainTexture = targetTexture;
                cloudMaterial.color = new Color(baseCloudColor.r, baseCloudColor.g, baseCloudColor.b, currentTransparency * (1f - textureBlendFactor));
                cloudMaterialBlend.color = new Color(baseCloudColor.r, baseCloudColor.g, baseCloudColor.b, currentTransparency * textureBlendFactor);
            }
            else
            {
                currentTexture = targetTextureToUse;
                targetTexture = targetTextureToUse;
                cloudMaterial.mainTexture = currentTexture;
                cloudMaterial.color = new Color(baseCloudColor.r, baseCloudColor.g, baseCloudColor.b, currentTransparency);
            }
            
            Vector2 baseTextureOffset = new Vector2(noiseOffset.x * settings.CloudNoiseScale, noiseOffset.y * settings.CloudNoiseScale);
            Vector2 rollInOffset = (isWeatherTransitioning && textureBlendFactor < 1f) ? borderRollInOffset * settings.CloudNoiseScale : Vector2.zero;
            
            cloudMaterial.mainTextureScale = new Vector2(settings.CloudNoiseScale, settings.CloudNoiseScale);
            cloudMaterialBlend.mainTextureScale = new Vector2(settings.CloudNoiseScale, settings.CloudNoiseScale);
            cloudMaterial.mainTextureOffset = baseTextureOffset;
            cloudMaterialBlend.mainTextureOffset = baseTextureOffset + rollInOffset;
            
            float skyY = AltitudeLayer.Skyfaller.AltitudeFor();
            Vector3 mapCenter = new Vector3(map.Size.x * 0.5f, skyY, map.Size.z * 0.5f);
            Vector3 mapScale = new Vector3(map.Size.x, 1f, map.Size.z);
            Matrix4x4 cloudMatrix = Matrix4x4.TRS(mapCenter, Quaternion.identity, mapScale);
            
            if (textureBlendFactor < 1f && currentTexture != targetTexture)
            {
                Graphics.DrawMesh(MeshPool.plane10, cloudMatrix, cloudMaterial, 0, null, 0);
                Graphics.DrawMesh(MeshPool.plane10, cloudMatrix, cloudMaterialBlend, 0, null, 0);
            }
            else
            {
                Graphics.DrawMesh(MeshPool.plane10, cloudMatrix, cloudMaterial, 0, null, 0);
            }
        }
        
        
    }
}

