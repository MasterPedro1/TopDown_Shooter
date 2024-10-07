using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Enemy : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint; // Punto desde donde dispara el enemigo
    public float timeBetweenShots = 2f; // Tiempo entre disparos
    public float moveSpeed = 3f; // Velocidad de movimiento del enemigo
    public float stopDistance = 5f; // Distancia mínima para detenerse antes del jugador
    public float bulletSpeed = 20f;
    private Transform player;
    private float shotTimer;
    private bool isShooting = false; // Indica si el enemigo está disparando

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        shotTimer = timeBetweenShots;
    }

    void Update()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Si no está disparando, puede moverse
            if (!isShooting && distanceToPlayer > stopDistance)
            {
                Vector3 direction = (player.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
            }

            // Temporizador para disparar
            shotTimer -= Time.deltaTime;
            if (shotTimer <= 0 && distanceToPlayer <= stopDistance)
            {
                StartCoroutine(Shoot());
                shotTimer = timeBetweenShots; // Reiniciar el temporizador de disparo
            }
        }
    }

    // Coroutine para disparar y detener el movimiento mientras dispara
    IEnumerator Shoot()
    {
        isShooting = true; // Detener el movimiento

        if (player != null)
        {
            // Disparar la bala
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.velocity = firePoint.forward * bulletSpeed;

            // Tiempo que toma disparar (puedes ajustar este valor)
            yield return new WaitForSeconds(0.5f);
        }

        isShooting = false; // Permitir el movimiento de nuevo después de disparar
    }
}
