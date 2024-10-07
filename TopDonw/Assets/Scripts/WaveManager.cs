using System.Collections;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int baseEnemiesPerWave = 1; // Enemigos iniciales por oleada
    public float timeBetweenWaves = 5f;
    private int waveIndex = 1;
    private int enemiesAlive = 0;
    private int currentEnemiesInWave; // Número actual de enemigos en esta oleada

    void Start()
    {
        // Iniciar la primera oleada de inmediato
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        while (true)
        {
            // Esperar un tiempo entre oleadas si no es la primera oleada
            if (waveIndex > 1)
            {
                yield return new WaitForSeconds(timeBetweenWaves);
            }

            Debug.Log($"Iniciando oleada {waveIndex}");

            // Calcular el número de enemigos para la oleada actual
            currentEnemiesInWave = baseEnemiesPerWave + waveIndex - 1;
            enemiesAlive = currentEnemiesInWave;

            // Generar enemigos
            for (int i = 0; i < currentEnemiesInWave; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(1f); // Espacio entre la generación de enemigos
            }

            // Esperar a que todos los enemigos sean eliminados antes de iniciar la próxima oleada
            while (enemiesAlive > 0)
            {
                yield return null;
            }

            Debug.Log($"Oleada {waveIndex} completada.");
            waveIndex++; // Incrementar la oleada
        }
    }

    void SpawnEnemy()
    {
        int spawnIndex = Random.Range(0, spawnPoints.Length);
        Instantiate(enemyPrefab, spawnPoints[spawnIndex].position, Quaternion.identity);
    }

    // Este método se llama cuando un enemigo muere
    public void EnemyKilled()
    {
        enemiesAlive--;

        // La oleada se considera completada cuando no quedan enemigos vivos
        if (enemiesAlive <= 0)
        {
            Debug.Log("Todos los enemigos han sido eliminados.");
        }
    }
}
