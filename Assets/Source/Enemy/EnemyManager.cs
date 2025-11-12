using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshSurface))]
public class EnemyManager : MonoBehaviour
{
    [Tooltip("Determines which enemies can spawn in each round of the game")]
    [SerializeField] private Round[] rounds;
    [SerializeField] private float spawnStartDistance = 20f;
    [SerializeField] private float spawnStopDistance = 40f;
    [SerializeField] private float navMeshSampleRadius = 2f;
    [SerializeField] private int maxSpawnAttempts = 15;

    private NavMeshSurface navMesh;
    private List<Enemy> enemies = new();

    private float spawnTimer = 0f;
    private int enemyKillCount;

    public void Initialize()
    {
        navMesh = GetComponent<NavMeshSurface>();
    }

    public void BuildNavMesh()
    {
        navMesh.BuildNavMesh();
    }

    public void UpdateEnemies(Player player)
    {
        int roundId = SerializeManager.GetRound();
        roundId = Mathf.Clamp(roundId, 0, rounds.Length - 1);
        Round currentRound = rounds[roundId];

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= 1f / currentRound.enemySpawnRate)
        {
            spawnTimer = 0f;
            TrySpawnEnemy(player, currentRound);
        }

        foreach (var enemy in enemies)
        {
            if (enemy.IsDead)
                enemyKillCount++;
        }

        enemies.RemoveAll(enemy => enemy.IsDead);

        foreach (var enemy in enemies)
        {
            enemy.SetTargetPosition(player.transform.position);
        }
    }

    public int GetTargetKillCount()
    {
        int roundId = SerializeManager.GetRound();
        roundId = Mathf.Clamp(roundId, 0, rounds.Length - 1);
        return rounds[roundId].enemyKillCount;
    }

    public int GetKillCount()
    {
        return enemyKillCount;
    }

    public int GetRoundCount()
    {
        return rounds.Length;
    }

    public void DisableEnemies()
    {
        foreach (var enemy in enemies)
        {
            enemy.Disable();
        }
    }

    private void TrySpawnEnemy(Player player, Round round)
    {
        EnemyVariant variant = PickEnemyVariant(round.enemyVariants);
        if (variant.enemyPrefab == null)
            return;

        if (FindSpawnPosition(player.transform.position, out Vector3 spawnPos))
        {
            SpawnEnemy(variant.enemyPrefab, spawnPos);
        }
    }

    private bool FindSpawnPosition(Vector3 playerPos, out Vector3 result)
    {
        for (int i = 0; i < maxSpawnAttempts; i++)
        {
            // Pick a random direction and distance
            Vector2 dir2D = Random.insideUnitCircle.normalized;
            float dist = Random.Range(spawnStartDistance, spawnStopDistance);
            Vector3 candidate = playerPos + new Vector3(dir2D.x, 0f, dir2D.y) * dist;

            // Sample the NavMesh
            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit,
                navMeshSampleRadius, NavMesh.AllAreas))
            {
                result = hit.position;
                return true;
            }
        }

        result = Vector3.zero;
        return false;
    }

    private EnemyVariant PickEnemyVariant(EnemyVariant[] variants)
    {
        if (variants == null || variants.Length == 0)
            return default;

        float total = 0f;
        foreach (var v in variants)
            total += v.probability;

        float rand = Random.value * total;
        float sum = 0f;

        foreach (var v in variants)
        {
            sum += v.probability;
            if (rand <= sum)
                return v;
        }

        return variants[variants.Length - 1];
    }

    private void SpawnEnemy(Enemy enemy, Vector3 pos)
    {
        Enemy newEnemy = Instantiate(enemy.gameObject, pos,
            Quaternion.identity, (Transform)default).GetComponent<Enemy>();

        enemies.Add(newEnemy);
    }

    public Vector3 GetCentroidOfLargestRegion()
    {
        var triangulation = NavMesh.CalculateTriangulation();
        int triCount = triangulation.indices.Length / 3;

        bool[] visited = new bool[triCount];
        List<List<int>> regions = new();

        // --- Group triangles into connected regions ---
        for (int i = 0; i < triCount; i++)
        {
            if (visited[i]) continue;

            List<int> region = new();
            Stack<int> stack = new();
            stack.Push(i);
            visited[i] = true;

            while (stack.Count > 0)
            {
                int tri = stack.Pop();
                region.Add(tri);

                // Compare with all others (brute force but fine for small navmeshes)
                for (int j = 0; j < triCount; j++)
                {
                    if (visited[j]) continue;
                    if (TrianglesShareEdge(tri, j, triangulation))
                    {
                        visited[j] = true;
                        stack.Push(j);
                    }
                }
            }

            regions.Add(region);
        }

        // --- Find region with largest total area ---
        float largestArea = 0f;
        List<int> largestRegion = null;
        foreach (var region in regions)
        {
            float total = 0f;
            foreach (int tri in region)
            {
                Vector3 a = triangulation.vertices[triangulation.indices[tri * 3 + 0]];
                Vector3 b = triangulation.vertices[triangulation.indices[tri * 3 + 1]];
                Vector3 c = triangulation.vertices[triangulation.indices[tri * 3 + 2]];
                total += Vector3.Cross(b - a, c - a).magnitude * 0.5f;
            }

            if (total > largestArea)
            {
                largestArea = total;
                largestRegion = region;
            }
        }

        if (largestRegion == null || largestRegion.Count == 0)
            return Vector3.zero;

        // --- Compute centroid of largest region ---
        Vector3 centroid = Vector3.zero;
        int count = 0;
        foreach (int tri in largestRegion)
        {
            for (int k = 0; k < 3; k++)
            {
                centroid += triangulation.vertices[triangulation.indices[tri * 3 + k]];
                count++;
            }
        }
        centroid /= count;

        // --- Snap to actual NavMesh ---
        if (NavMesh.SamplePosition(centroid, out NavMeshHit hit, 1000f, NavMesh.AllAreas))
            return hit.position;

        return centroid;
    }

    private bool TrianglesShareEdge(int t1, int t2, NavMeshTriangulation t)
    {
        int[] i1 = { t.indices[t1 * 3], t.indices[t1 * 3 + 1], t.indices[t1 * 3 + 2] };
        int[] i2 = { t.indices[t2 * 3], t.indices[t2 * 3 + 1], t.indices[t2 * 3 + 2] };

        int shared = 0;
        for (int a = 0; a < 3; a++)
            for (int b = 0; b < 3; b++)
                if (Vector3.Distance(t.vertices[i1[a]], t.vertices[i2[b]]) < 0.001f)
                    shared++;

        return shared >= 2; // share at least one edge
    }
}



[System.Serializable]
public struct EnemyVariant
{
    public float probability;
    public Enemy enemyPrefab;
}

[System.Serializable]
public struct Round
{
    public float enemySpawnRate;
    public EnemyVariant[] enemyVariants;
    public int enemyKillCount;
}