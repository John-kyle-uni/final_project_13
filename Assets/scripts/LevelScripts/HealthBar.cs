using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health
    private float currentHealth;      // Current health
    public Image healthBar;           // Reference to the health bar UI

    void Start()
    {
        currentHealth = maxHealth;    // Set current health to max health at the start
        UpdateHealthBar();
    }

    void Update() {
        
    }
    

    // Update the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;  // Update the fill amount based on current health
        }
    }

    public void ApplyDamage(int damage) {
        currentHealth -= damage;
        UpdateHealthBar();
    }
}
