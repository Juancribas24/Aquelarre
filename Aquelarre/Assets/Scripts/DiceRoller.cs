using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceRoller : MonoBehaviour
{
    public static DiceRoller Instance { get; private set; } // Singleton instance

    public Sprite[] diceFaces; // Array de sprites de cada cara del dado
    public float rotationTime = 1.0f; // Tiempo total de la animación
    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        // Ensure this is the only instance of DiceRoller
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional: Makes it persist across scenes
        }
        else
        {
            Destroy(gameObject); // Ensures there are not duplicate instances
        }
    }

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void RollDice()
    {
        StartCoroutine(RotateDice());
    }

    private IEnumerator RotateDice()
    {
        float timer = 0;
        int randomFace = 0;

        while (timer < rotationTime)
        {
            randomFace = Random.Range(0, diceFaces.Length);
            spriteRenderer.sprite = diceFaces[randomFace];
            timer += Time.deltaTime;
            yield return null;
        }

        // Decide el resultado final
        randomFace = Random.Range(0, diceFaces.Length);
        spriteRenderer.sprite = diceFaces[randomFace];
    }
}
