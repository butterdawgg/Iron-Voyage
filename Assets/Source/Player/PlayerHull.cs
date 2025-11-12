using UnityEngine;

public class PlayerHull : MonoBehaviour
{
    [Tooltip("Maximum HP")]
    [SerializeField] private float maxHealth;
    [Tooltip("Percentage of damage absorbed")]
    [SerializeField] private float damageAbsorption;

    private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    private float CalculateDamage(float damage)
    {
        float factor = (100f - Mathf.Clamp(damageAbsorption, 0f, 100f)) / 100f;

        return damage * factor;
    }

    public void TakeDamage(float damage)
    {
        health -= CalculateDamage(damage);
        health = Mathf.Clamp(health, 0f, maxHealth);
    }

    public float GetHealth()
    {
        return health;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }
}