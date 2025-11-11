using UnityEngine;

public class Chunk : MonoBehaviour
{
    private Vector2Int chunkPos;

    private Collider[] colliders = System.Array.Empty<Collider>();
    private Renderer[] renderers = System.Array.Empty<Renderer>();

    public bool CollisionsEnabled { get; private set; }
    public bool RenderingEnabled { get; private set; }

    public void SetChunkPos(Vector2Int chunkPos)
    {
        this.chunkPos = chunkPos;
    }

    public void GenerateTiles(TileGenerator generator)
    {
        generator.GenerateTiles(chunkPos, transform, ref colliders, ref renderers);
    }

    public void SetCollisionsEnabled(bool enabled)
    {
        foreach (var collider in colliders)
        {
            collider.enabled = enabled;
        }

        CollisionsEnabled = enabled;
    }

    public void SetRenderingEnabled(bool enabled)
    {
        foreach(var renderer in renderers)
        {
            renderer.enabled = enabled;
        }

        RenderingEnabled = enabled;
    }
}