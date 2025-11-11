using UnityEngine;

public class ChunkManager : MonoBehaviour
{
    [Tooltip("Center of the world square")]
    [SerializeField] private Vector3 worldCenter;
    [Tooltip("Size in chunks of the world square")]
    [SerializeField] private int worldSize;
    [Tooltip("Height of the world bounds")]
    [SerializeField] private float worldHeight;
    [Tooltip("Base chunk prefab")]
    [SerializeField] private Chunk baseChunk;
    [Tooltip("Chunk terrain generator")]
    [SerializeField] private TileGenerator generator;

    private Chunk[] chunks;

    public void GenerateChunks()
    {
        generator.GenerateNoiseSeed();

        Bounds bounds = GetBounds();
        float chunkWorldSize = bounds.size.x / worldSize;
        float halfWorldSize = bounds.extents.x;

        chunks = new Chunk[worldSize * worldSize];

        for (int i = 0; i < chunks.Length; i++)
        {
            int chunkY = i / worldSize;
            int chunkX = i % worldSize;

            Vector3 localPos = new (
                chunkX * chunkWorldSize,
                0f,
                chunkY * chunkWorldSize);

            Vector3 worldPos = worldCenter + localPos -
                new Vector3(halfWorldSize, 0f, halfWorldSize);

            chunks[i] =
                Instantiate(baseChunk.gameObject, worldPos,
                Quaternion.identity, transform)
                .GetComponent<Chunk>();

            chunks[i].SetChunkPos(new Vector2Int(chunkX, chunkY));
            chunks[i].GenerateTiles(generator);
        }
    }

    public Bounds GetBounds()
    {
        float size = worldSize * generator.ChunkSize * generator.TileSize;

        Vector3 boundsSize = new (size, worldHeight, size);

        return new Bounds(worldCenter + 0.5f * worldHeight * Vector3.up,
            boundsSize);
    }
}