using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel; // Referencia al panel de inventario
    public GameObject inventoryHide;
    private bool isInventoryOpen = false; // Estado del inventario
    private bool isInventoryHideOpen = true; // Estado del inventario

    public static int healthPotionCount = 0;
    public static int staminaPotionCount = 0;

    public TMP_Text healthPotionText;
    public TMP_Text staminaPotionText;

    public GameObject healthPotionImage;
    public GameObject staminaPotionImage;

    void Update()
    {
        // Detectar cuando se presiona la tecla E
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void ToggleInventory()
    {
        isInventoryOpen = !isInventoryOpen; // Cambiar el estado del inventario
        isInventoryHideOpen = !isInventoryHideOpen; // Cambiar el estado del inventario
        inventoryPanel.SetActive(isInventoryOpen); // Mostrar u ocultar el panel
        inventoryHide.SetActive(isInventoryHideOpen); 
        
    }

    public void UpdateUI()
    {
        //Health potion update
        if (healthPotionCount == 0) 
        {
            SpriteRenderer spriteRenderer = healthPotionImage.GetComponent<SpriteRenderer>();
            Color color= new Color(0.5f, 0.5f, 0.5f, 1);
            color.a = 0.6f;
            spriteRenderer.color = color;
            healthPotionText.text = healthPotionCount.ToString();
        }
        else 
        {
            SpriteRenderer spriteRenderer = healthPotionImage.GetComponent<SpriteRenderer>();
            Color color = new Color(1f, 1f, 1f, 1);
            color.a = 1f;
            spriteRenderer.color = color;
            healthPotionText.text = healthPotionCount.ToString();
        }

        //Stamina potion update
        if (staminaPotionCount == 0)
        {
            SpriteRenderer spriteRenderer = staminaPotionImage.GetComponent<SpriteRenderer>();
            Color color = new Color(0.5f, 0.5f, 0.5f, 1);
            color.a = 0.6f;
            spriteRenderer.color = color;
            staminaPotionText.text = staminaPotionCount.ToString();
        }
        else
        {
            SpriteRenderer spriteRenderer = staminaPotionImage.GetComponent<SpriteRenderer>();
            Color color = new Color(1f, 1f, 1f, 1);
            color.a = 1f;
            spriteRenderer.color = color;
            staminaPotionText.text = staminaPotionCount.ToString();
        }
    }
}
