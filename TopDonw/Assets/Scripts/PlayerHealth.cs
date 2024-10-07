using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100f;
    public float maxShield = 50f;
    public float shieldRegenRate = 5f;
    public float shieldRegenDelay = 3f; // Tiempo de espera para regenerar escudo
    private float currentHealth;
    private float currentShield;
    private float lastDamageTime;
    public Slider healthSlider;
    public Slider shieldSlider;

    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        shieldSlider.value = currentShield;
        healthSlider.value = currentHealth;
    }

    void Update()
    {
        if (Time.time - lastDamageTime >= shieldRegenDelay && currentShield < maxShield)
        {
            currentShield += shieldRegenRate * Time.deltaTime;
            currentShield = Mathf.Clamp(currentShield, 0, maxShield);
        }
    }

    public void TakeDamage(float damage)
    {
        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                currentHealth += currentShield; // Resta el exceso de daño a la vida
                currentShield = 0;

            }
            Debug.Log(currentShield);
            shieldSlider.value = currentShield;
        }
        else
        {
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.Log("Game Over");
                Time.timeScale = 0f;
                

            }
            Debug.Log(currentHealth);
            healthSlider.value = currentHealth;
        }

        lastDamageTime = Time.time; // Reiniciar tiempo para regenerar escudo
    }
}
