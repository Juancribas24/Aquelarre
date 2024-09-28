using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    // Load a scene by name
    public void LoadScene(string sceneName)
    {
        if (SceneExists(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogError($"Scene '{sceneName}' does not exist. Please check the scene name.");
        }
    }

    // Reload the current scene
    public void ReloadCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Load the next scene in the build settings
    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogError("No next scene found. Make sure you have more scenes added to the build settings.");
        }
    }

    // Quit the application
    public void QuitGame()
    {
#if UNITY_EDITOR
        // Application.Quit() does not work in the editor, so
        // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    // Check if the scene exists in build settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneFileName = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneFileName == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    public void LoadSceneCategory(string playerChoice)
    {
        if (playerChoice == "Noble")
        {
            SceneManager.LoadScene("Palacio(Pausa)"); // Asegúrate de que el nombre de la escena coincide con el nombre en los Build Settings
        }
        else if (playerChoice == "Plebeyo")
        {
            SceneManager.LoadScene("Taberna"); // Asegúrate de que el nombre de la escena coincide con el nombre en los Build Settings
        }
    }
}
