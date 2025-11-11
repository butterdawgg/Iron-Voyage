using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileGenerator
{
    [Tooltip("Size (in tiles) of each chunk")]
    [SerializeField] private int chunkSize;
    [Tooltip("World size of each tile")]
    [SerializeField] private float tileSize;
    [Tooltip("Must be sorted from highest to lowest threshold")]
    [SerializeField] private TileVariant[] tileVariants;
    [Tooltip("Generate random seed?")]
    [SerializeField] private bool generateRandomSeed;
    [Tooltip("Seed used for perlin noise sampling")]
    [SerializeField][ConditionalHide("generateRandomSeed", false, true)]
    private int noiseSeed = 0;
    [Tooltip("Scale of perlin noise sampling")]
    [SerializeField] private float noiseScale = 0.1f;
    [Tooltip("Scale of resulting perlin noise height")]
    [SerializeField] private float noiseHeightMultiplier = 1f;

    private Vector2Int offset;

    public int ChunkSize { get { return chunkSize; } }
    public float TileSize { get { return tileSize; } }

    public void GenerateNoiseSeed()
    {
        int seed = generateRandomSeed ? Random.Range(int.MinValue, int.MaxValue)
            : noiseSeed;

        System.Random rand = new (seed);

        offset.x = rand.Next(-10000, 10000);
        offset.y = rand.Next(-10000, 10000);

        Debug.Log(seed);
    }

    public void GenerateTiles(Vector2Int chunkPos, Transform parent,
        ref Collider[] colliders, ref Renderer[] renderers)
    {
        List<Collider> colliderList = new ();
        List<Renderer> rendererList = new ();

        if (tileVariants == null || tileVariants.Length == 0)
        {
            Debug.LogWarning("No tiles assigned to ChunkGenerator!");

            return;
        }

        for (int y = 0; y < chunkSize; y++)
        {
            for (int x = 0; x < chunkSize; x++)
            {
                int worldX = chunkPos.x * chunkSize + x;
                int worldY = chunkPos.y * chunkSize + y;

                float noiseSample = Mathf.PerlinNoise(
                    (worldX + offset.x) * noiseScale,
                    (worldY + offset.y) * noiseScale) *
                    noiseHeightMultiplier;

                GameObject tilePrefab = null;
                for (int i = 0; i < tileVariants.Length; i++)
                {
                    if (noiseSample >= tileVariants[i].threshold)
                    {
                        tilePrefab = tileVariants[i].tilePrefab;
                        break;
                    }
                }

                if (tilePrefab == null)
                    continue;

                Vector3 worldPos = new (x * tileSize, 0f, y * tileSize);

                GameObject tileInstance =
                    Object.Instantiate(tilePrefab,
                    parent.position + worldPos, Quaternion.identity, parent);

                if (tileInstance.TryGetComponent(out Collider c))
                    colliderList.Add(c);

                if (tileInstance.TryGetComponent(out Renderer r))
                    rendererList.Add(r);
            }
        }

        colliders = colliderList.ToArray();
        renderers = rendererList.ToArray();
    }
}

[System.Serializable]
public struct TileVariant
{
    public float threshold;
    public GameObject tilePrefab;
}