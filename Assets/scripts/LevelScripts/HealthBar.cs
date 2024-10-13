using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class HealthManager : MonoBehaviour
{
    public float maxHealth = 100f;  // Maximum health
    private float currentHealth;      // Current health
    public Image healthBar;           // Reference to the health bar UI
    public CoinsCollected coinsCollected;

    void Start()
    {
        currentHealth = maxHealth;    // Set current health to max health at the start
        UpdateHealthBar();
    }

    void Update() {
        if (currentHealth <= 0) {
            Cursor.visible = true;  // Show the cursor
            Cursor.lockState = CursorLockMode.None;  // Free the cursor from being locked

            Debug.Log(coinsCollected.coinsCollected);
            DataHolder.Coins += coinsCollected.coinsCollected;

            SceneManager.LoadScene("LossScene");
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

    public void ApplyDamage(int damage) {
        currentHealth -= damage;
        UpdateHealthBar();
    }
}
