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
        if (Input.GetKeyDown(KeyCode.F)) // Press D to take damage
        {
            currentHealth -= 10;
            UpdateHealthBar();
            Debug.Log(currentHealth);

        }

        if (Input.GetKeyDown(KeyCode.H)) // Press H to heal
        {
            currentHealth += 10;
            UpdateHealthBar();
            Debug.Log(currentHealth);

        }
    }
    

    // Update the health bar UI
    private void UpdateHealthBar()
    {
        if (healthBar != null)
        {
            healthBar.fillAmount = currentHealth / maxHealth;  // Update the fill amount based on current health
        }
    }
}
