using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public CharacterStats playerStats;
    public TextMeshProUGUI healthText;
    public Slider healthBar;
    public Animator animator; // Referencia al Animator
    public Transform attackPoint; // Punto desde donde se inicia el ataque
    public GameObject magicAttackPrefab; // Prefab del ataque mágico específico del jugador

    public AudioClip physicalAttackSound; // Sonido del ataque físico
    public AudioClip magicAttackSound; // Sonido del ataque mágico
    private AudioSource audioSource; // Referencia al componente AudioSource

    private int currentHealth;

    void Start()
    {
        // Aquí no hacemos nada ya que la inicialización será manejada por TurnBasedCombatSystem
    }
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void InitializePlayer()
    {
        if (playerStats != null)
        {
            currentHealth = playerStats.health;
            Debug.Log(gameObject.name + " initialized with health: " + currentHealth);
            UpdateHealthUI();
        }
        else
        {
            Debug.LogError("PlayerStats not assigned in " + gameObject.name);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            currentHealth = 0;
        }
        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        healthText.text = "HP: " + currentHealth + "/" + playerStats.health;
        healthBar.value = (float)currentHealth / playerStats.health;
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public void PlayPhysicalAttackAnimation()
    {
        animator.SetBool("isPhysicalAttacking", true);
        PlaySound(physicalAttackSound);
    }

    public void StopPhysicalAttackAnimation()
    {
        animator.SetBool("isPhysicalAttacking", false);
    }

    public void PlayMagicAttackAnimation()
    {
        animator.SetBool("isMagicAttacking", true);
        PlaySound(magicAttackSound);
    }

    public void StopMagicAttackAnimation()
    {
        animator.SetBool("isMagicAttacking", false);
    }

    public void ExecuteMagicAttack(Vector3 targetPosition)
    {
        GameObject magicAttack = Instantiate(magicAttackPrefab, attackPoint.position, Quaternion.identity);
        MagicAttack magicAttackScript = magicAttack.GetComponent<MagicAttack>();
        if (magicAttackScript != null)
        {
            magicAttackScript.SetTarget(targetPosition);
        }
    }

    void Die()
    {
        // Lógica para manejar la muerte del jugador
        Debug.Log(playerStats.characterName + " ha muerto.");
        TurnBasedCombatSystem combatSystem = FindObjectOfType<TurnBasedCombatSystem>();
        combatSystem.playerControllers.Remove(this); // Eliminar el jugador de la lista
        Destroy(gameObject); // Destruir el objeto del jugador
    }
    void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
