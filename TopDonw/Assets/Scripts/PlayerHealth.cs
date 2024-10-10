using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float maxHealth = 100;
    public float maxShield = 50;
    public float shieldRegenRate = 5; // Regeneraci�n de escudo por segundo
    public float shieldRegenDelay = 7; // Tiempo de espera antes de comenzar la regeneraci�n
    private float currentHealth;
    private float currentShield;
    private float lastDamageTime; // Momento en que se recibi� el �ltimo da�o
    public Slider healthSlider;
    public Slider shieldSlider;
    public int damage = 5;
    public TextMeshProUGUI vida;
    public TextMeshProUGUI escudo;


    void Start()
    {
        currentHealth = maxHealth;
        currentShield = maxShield;
        shieldSlider.maxValue = maxShield;
        shieldSlider.value = currentShield;
        healthSlider.maxValue = maxHealth;
        healthSlider.value = currentHealth;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("balasenemigo"))
        {
            TakeDamage();
            other.gameObject.SetActive(false); // Desactivar la bala tras el impacto
            Debug.Log("El jugador ha recibido da�o.");
        }
    }

    void Update()
    {
        // Regenerar el escudo solo si ha pasado el tiempo de espera y el escudo no est� al m�ximo
        if (Time.time - lastDamageTime >= shieldRegenDelay && currentShield < maxShield)
        {
            RegenerateShield();
        }
        vida.text = healthSlider.value.ToString();
        escudo.text = shieldSlider.value.ToString();
    }

    // M�todo para aplicar el da�o al jugador
    public void TakeDamage()
    {
        // Si el escudo a�n tiene puntos, reducirlo primero
        if (currentShield > 0)
        {
            currentShield -= damage;
            if (currentShield < 0)
            {
                // Si el escudo no es suficiente, aplicar el da�o restante a la salud
                currentHealth += currentShield; // Como el escudo es negativo, esto resta de la salud
                currentShield = 0;
            }
            Debug.Log("Escudo actual: " + currentShield);
            shieldSlider.value = currentShield;
        }
        else
        {
            // Si no hay escudo, reducir la salud
            currentHealth -= damage;
            if (currentHealth <= 0)
            {
                Debug.Log("Game Over");
                Time.timeScale = 0f; // Pausar el juego en caso de muerte
            }
            Debug.Log("Salud actual: " + currentHealth);
            healthSlider.value = currentHealth;
        }

        // Reiniciar el tiempo de espera para la regeneraci�n del escudo
        lastDamageTime = Time.time;
    }

    // M�todo para regenerar el escudo
    void RegenerateShield()
    {
        currentShield += shieldRegenRate * Time.deltaTime;
        currentShield = Mathf.Clamp(currentShield, 0, maxShield); // Limitar entre 0 y el valor m�ximo
        shieldSlider.value = currentShield;
        Debug.Log("Escudo regener�ndose: " + currentShield);
    }
}
