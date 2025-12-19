using RimWorld;
using UnityEngine;
using Verse;

namespace SkywardAmbience
{
    [StaticConstructorOnStartup]
    public class BirdFlock
    {
        private Map map;
        private Vector2 position;
        private Vector2 velocity;
        private float size;
        
        private const float BaseSpeed = 0.15f;
        private const float MinSize = 128f; // 4x larger (32 * 4)
        private const float MaxSize = 240f; // 4x larger (60 * 4)

        public BirdFlock(Map map, Vector2? targetPosition = null)
        {
            this.map = map;
            
            Vector2 target = targetPosition ?? new Vector2(map.Size.x * 0.5f, map.Size.z * 0.5f);
            
            // Spawn from a random edge of the map
            int spawnEdge = Rand.RangeInclusive(0, 3);
            float spawnX, spawnZ;
            
            switch (spawnEdge)
            {
                case 0: // West edge
                    spawnX = map.Size.x * -0.1f;
                    spawnZ = Rand.Range(0f, map.Size.z);
                    break;
                case 1: // East edge
                    spawnX = map.Size.x * 1.1f;
                    spawnZ = Rand.Range(0f, map.Size.z);
                    break;
                case 2: // South edge
                    spawnX = Rand.Range(0f, map.Size.x);
                    spawnZ = map.Size.z * -0.1f;
                    break;
                default: // North edge
                    spawnX = Rand.Range(0f, map.Size.x);
                    spawnZ = map.Size.z * 1.1f;
                    break;
            }
            
            position = new Vector2(spawnX, spawnZ);
            Vector2 directionToTarget = (target - position).normalized;
            velocity = directionToTarget * BaseSpeed * Rand.Range(0.8f, 1.2f);
            size = Rand.Range(MinSize, MaxSize);
        }

        public void Tick()
        {
            position += velocity;
        }

        public bool ShouldRemove()
        {
            // Remove when left the map area
            return position.x < map.Size.x * -0.2f || position.x > map.Size.x * 1.2f ||
                   position.y < map.Size.z * -0.2f || position.y > map.Size.z * 1.2f;
        }

        private static Material shadowMaterial;
        private static Texture2D shadowTexture;
        private static SkywardAmbienceMod modInstance;

        static BirdFlock()
        {
            shadowMaterial = new Material(ShaderDatabase.Transparent);
            shadowMaterial.color = new Color(0f, 0f, 0f, 0.03f);
            
            LoadShadowTexture();
            modInstance = LoadedModManager.GetMod<SkywardAmbienceMod>();
        }
        
        private static SkywardAmbienceSettings GetSettings()
        {
            if (modInstance == null)
            {
                modInstance = LoadedModManager.GetMod<SkywardAmbienceMod>();
            }
            
            return modInstance?.GetSettings<SkywardAmbienceSettings>();
        }
        
        private static void LoadShadowTexture()
        {
            var mod = LoadedModManager.GetMod<SkywardAmbienceMod>();
            if (mod?.Content == null)
                return;
                
            Texture2D loadedTexture = ContentFinder<Texture2D>.Get("Textures/BirdFlockShadow", false) 
                                   ?? ContentFinder<Texture2D>.Get("BirdFlockShadow", false);
            
            if (loadedTexture != null)
            {
                shadowTexture = loadedTexture;
                shadowTexture.wrapMode = TextureWrapMode.Clamp;
                shadowTexture.filterMode = FilterMode.Bilinear;
                
                if (Prefs.DevMode)
                {
                    Log.Message("[Skyward Ambience] Loaded bird flock shadow texture.");
                }
            }
            else if (Prefs.DevMode)
            {
                Log.Warning("[Skyward Ambience] Bird flock shadow texture not found.");
            }
        }

        public void Draw()
        {
            IntVec3 mapPos = new IntVec3((int)position.x, 0, (int)position.y);
            if (!Find.CameraDriver.CurrentViewRect.ExpandedBy(10).Contains(mapPos))
                return;

            Vector3 shadowPosition = new Vector3(position.x, AltitudeLayer.Shadows.AltitudeFor(), position.y);
            
            const float shadowAlpha = 0.4f;
            
            // Calculate rotation to face movement direction
            float movementAngle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
            float rotationY = 90f - movementAngle; // Convert to Unity Y rotation (texture faces +X = 90°)
            
            var settings = GetSettings();
            float sizeMultiplier = settings?.BirdFlockSize ?? 1.0f;
            
            // Set shadow material properties
            if (shadowTexture != null)
            {
                shadowMaterial.mainTexture = shadowTexture;
            }
            shadowMaterial.color = new Color(0f, 0f, 0f, shadowAlpha);
            
            // Scale shadow (wider than long) and apply size multiplier
            Vector3 shadowScale = new Vector3(size * 0.5f * sizeMultiplier, 1f, size * 0.25f * sizeMultiplier) * 0.9f;
            Matrix4x4 shadowMatrix = Matrix4x4.TRS(shadowPosition, Quaternion.Euler(0f, rotationY, 0f), shadowScale);
            Graphics.DrawMesh(MeshPool.plane10, shadowMatrix, shadowMaterial, 0, null, 0);
        }
    }
}
