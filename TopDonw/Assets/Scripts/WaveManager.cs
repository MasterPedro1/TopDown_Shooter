using System.Collections;
using UnityEngine;
using TMPro;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesPerWave = 5;
    public float timeBetweenWaves = 5f; // Tiempo entre oleadas
    private int waveIndex = 1; // Índice de la oleada actual
    private int enemiesAlive = 0; // Contador de enemigos vivos

    public TextMeshProUGUI waveText; // Referencia al texto de la oleada

    void Start()
    {
        waveText.text = "Wave: " + waveIndex; // Mostrar el número de la primera oleada
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        // Generar enemigos
        for (int i = 0; i < enemiesPerWave; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(1f); // Espacio entre la generación de enemigos
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
        enemiesAlive++;
    }

    // Este método se llamará cuando un enemigo muera
    public void EnemyKilled()
    {
        enemiesAlive--;

        // Cuando no quedan más enemigos vivos, la oleada ha terminado
        if (enemiesAlive == 0)
        {
            Debug.Log("Oleada completada. Esperando 4 segundos para la siguiente oleada.");
            StartCoroutine(WaitAndStartNextWave());
        }
    }

    IEnumerator WaitAndStartNextWave()
    {
        yield return new WaitForSeconds(4f); // Esperar 4 segundos antes de comenzar la nueva oleada

        // Incrementar el número de enemigos para la próxima oleada
        enemiesPerWave += waveIndex;
        waveIndex++; // Avanzar a la siguiente oleada

        waveText.text = "Wave: " + waveIndex; // Actualizar el texto de la oleada
        StartCoroutine(SpawnWave()); // Comenzar la nueva oleada
    }
}

