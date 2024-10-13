using UnityEngine;

public class LadderClimb : MonoBehaviour
{
    public Transform teleportTarget;  // The position where the player should be teleported to
    private bool isTeleporting = false;
    private PlayerMovement playerMovement;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player detected, attempting to teleport...");
            
            if (!isTeleporting)
            {
                playerMovement = other.GetComponent<PlayerMovement>();

                if (playerMovement != null)
                {
                    Debug.Log("Disabling player movement...");
                    playerMovement.enabled = false; // Disable the regular movement
                }

                TeleportPlayer(other.transform); // Teleport the player
            }
        }
    }

    void TeleportPlayer(Transform player)
    {
        isTeleporting = true;

        if (teleportTarget != null)
        {
            Debug.Log("Teleporting player to: " + teleportTarget.position);
            // Teleport the player to the teleportTarget position
            player.position = teleportTarget.position;
        }
        else
        {
            Debug.LogWarning("Teleport target is not set.");
        }

        // Optionally, re-enable the player's movement after teleporting
        if (playerMovement != null)
        {
            Debug.Log("Re-enabling player movement...");
            playerMovement.enabled = true;
        }

        isTeleporting = false;  // Reset teleport flag
    }
}
