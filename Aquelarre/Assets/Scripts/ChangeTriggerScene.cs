using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeTriggerScene : MonoBehaviour
{
    public string sceneName; // Nombre de la escena a la que se cambiará

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Carga la escena especificada cuando el jugador entra en el área de colisión
            SceneManager.LoadScene(sceneName);
        }
    }
}
