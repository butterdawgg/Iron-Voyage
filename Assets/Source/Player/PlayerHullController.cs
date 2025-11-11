using UnityEngine;

public class PlayerHullController : MonoBehaviour
{
    [SerializeField] private Transform hullPivot;

    private PlayerHull hull;

    public void Add(PlayerHull hullPrefab)
    {
        hull = Instantiate(hullPrefab.gameObject, hullPivot.position,
            hullPivot.rotation, hullPivot).GetComponent<PlayerHull>();
    }

    public void TakeDamage(float damage)
    {
        if (hull == null)
            return;

        hull.TakeDamage(damage);
    }

    public float GetHealth()
    {
        return hull.GetHealth();
    }
}