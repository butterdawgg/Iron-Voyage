using UnityEngine;

[RequireComponent(typeof(ChunkManager))]
[RequireComponent(typeof(EnemyManager))]
[RequireComponent(typeof(PlayerManager))]
public class WorldManager : MonoBehaviour
{
    [Tooltip("Base water plane of the world")]
    [SerializeField] private Border waterPlane;
    [Tooltip("North (+Z) border plane of the world")]
    [SerializeField] private Border northBorder;
    [Tooltip("East (+X) border plane of the world")]
    [SerializeField] private Border eastBorder;
    [Tooltip("South (-Z) border plane of the world")]
    [SerializeField] private Border southBorder;
    [Tooltip("West (-X) border plane of the world")]
    [SerializeField] private Border westBorder;

    private ChunkManager chunkManager;
    private EnemyManager enemyManager;
    private PlayerManager playerManager;

    private Player player;

    private void Awake()
    {
        chunkManager = GetComponent<ChunkManager>();
        enemyManager = GetComponent<EnemyManager>();
        playerManager = GetComponent<PlayerManager>();

        chunkManager.GenerateChunks();
        enemyManager.Initialize();

        PlaceBorders();

        enemyManager.BuildNavMesh();

        player = playerManager.InitializePlayer(
            enemyManager.GetCentroidOfLargestRegion());
    }

    private void Update()
    {
        enemyManager.UpdateEnemies(player);
    }

    private void PlaceBorders()
    {
        Bounds bounds = chunkManager.GetBounds();

        waterPlane.Place(bounds.center + Vector3.down * bounds.extents.y,
            Vector3.up, new Vector2(bounds.size.x, bounds.size.z));

        northBorder.Place(bounds.center + Vector3.forward * bounds.extents.z,
            Vector3.back, new Vector2(bounds.size.x, bounds.size.y));

        eastBorder.Place(bounds.center + Vector3.right * bounds.extents.x,
            Vector3.left, new Vector2(bounds.size.z, bounds.size.y));

        southBorder.Place(bounds.center + Vector3.back * bounds.extents.z,
            Vector3.forward, new Vector2(bounds.size.x, bounds.size.y));

        westBorder.Place(bounds.center + Vector3.left * bounds.extents.x,
            Vector3.right, new Vector2(bounds.size.z, bounds.size.y));
    }
}