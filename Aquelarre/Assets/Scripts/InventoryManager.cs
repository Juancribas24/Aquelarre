using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel; // Referencia al panel de inventario
    public GameObject inventoryHide;
    private bool isInventoryOpen = false; // Estado del inventario
    private bool isInventoryHideOpen = true; // Estado del inventario

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
}
