using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float timeBetweenShots = 2f;
    public float moveSpeed = 3f;
    public float stopDistance = 5f;
    public float bulletSpeed = 20f;
    public float minDistanceFromOtherEnemies = 2f;
    public float detectionRange = 10f;
    public float wanderRadius = 5f;
    public float wanderSpeed = 1.5f;
    public float followDuration = 3f; // Tiempo que sigue al jugador después de detectarlo
    public float health = 10;
    public LayerMask groundLayer; // Layer "Suelo"

    private Transform player;
    private float shotTimer;
    private Vector3 wanderTarget;
    private float followTimer = 0f; // Temporizador para seguir al jugador
    private bool isFollowingPlayer = false;

    private enum EnemyState { Wander, ChasingPlayer }
    private EnemyState currentState;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shotTimer = timeBetweenShots;
        currentState = EnemyState.Wander;
        SetNewWanderTarget();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case EnemyState.Wander:
                Wander();

                // Detectar si el jugador está en rango
                if (distanceToPlayer <= detectionRange)
                {
                    currentState = EnemyState.ChasingPlayer;
                    followTimer = followDuration; // Inicia el temporizador de seguimiento
                }
                break;

            case EnemyState.ChasingPlayer:
                LookAtPlayer();

                // Seguir al jugador mientras esté en rango o mientras el temporizador no haya terminado
                if (followTimer > 0)
                {
                    followTimer -= Time.deltaTime;
                    MoveTowardsPlayer();
                }
                else if (distanceToPlayer > detectionRange) // Si el jugador está fuera de rango y el temporizador terminó
                {
                    currentState = EnemyState.Wander;
                    SetNewWanderTarget();
                }

                // Permitir que el enemigo dispare mientras se mueve
                shotTimer -= Time.deltaTime;
                if (shotTimer <= 0 && distanceToPlayer <= stopDistance)
                {
                    Shoot(); // Disparar sin detener el movimiento
                    shotTimer = timeBetweenShots;
                }
                break;
        }

        if (health <= 0)
        {
            FindObjectOfType<WaveManager>().EnemyKilled();
            Destroy(this.gameObject);
        }
    }

    // Método para mover al enemigo hacia el jugador, ajustado al layer "Suelo"
    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Mantener al enemigo en el plano del suelo (XZ) ajustando su altura con un Raycast
        AdjustToGround(ref newPosition);
        transform.position = newPosition;
    }

    // Wander: Movimiento aleatorio ajustado al layer "Suelo"
    void Wander()
    {
        if (Vector3.Distance(transform.position, wanderTarget) < 1f) // Si ha llegado al destino
        {
            SetNewWanderTarget();
        }
        else
        {
            Vector3 direction = (wanderTarget - transform.position).normalized;
            Vector3 newPosition = transform.position + direction * wanderSpeed * Time.deltaTime;

            // Mantener al enemigo en el plano del suelo (XZ) ajustando su altura con un Raycast
            AdjustToGround(ref newPosition);
            transform.position = newPosition;
        }
    }

    // Método para ajustar la altura del enemigo en base al layer "Suelo" usando un Raycast
    void AdjustToGround(ref Vector3 position)
    {
        RaycastHit hit;

        // Hacemos un raycast hacia abajo para encontrar la posición del "Suelo"
        if (Physics.Raycast(position + Vector3.up * 5f, Vector3.down, out hit, 10f, groundLayer))
        {
            position.y = hit.point.y; // Ajustamos la posición Y para que esté sobre el "Suelo"
        }
    }

    // Establece un nuevo destino aleatorio para el wander
    void SetNewWanderTarget()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        randomDirection.y = transform.position.y; // Mantener en el mismo plano

        wanderTarget = randomDirection;
    }

    // Método para que el enemigo siempre mire al jugador
    void LookAtPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }

    // Método para disparar sin detenerse
    void Shoot()
    {
        if (player != null)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * bulletSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Balas"))
        {
            health -= 5;
            other.gameObject.SetActive(false);
        }

        if (other.CompareTag("Misil"))
        {
            health -= 10;
            other.gameObject.SetActive(false);
        }
    }
}
