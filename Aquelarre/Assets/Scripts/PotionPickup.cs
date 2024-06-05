using UnityEngine;

public class PotionPickup : MonoBehaviour
{
    public string potionType; // Set this in the Inspector to "Health" or "Stamina"
    private bool isPlayerInRange = false;
    public InventoryManager invManager;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
    public void AddPotion()
    {
        if (potionType == "Health")
        {
            InventoryManager.healthPotionCount++;
            invManager.UpdateUI();
        }
        else if (potionType == "Stamina")
        {
            InventoryManager.staminaPotionCount++;
            invManager.UpdateUI();
        }
    }

    private void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            AddPotion();
            Destroy(gameObject);
        }
    }
}