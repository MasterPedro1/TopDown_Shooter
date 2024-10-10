using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Shooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public GameObject missilePrefab;
    public Transform firePoint;
    public float bulletSpeed = 20f;
    public float missileSpeed = 15f;
    public int maxMissiles = 2;
    private int currentMissiles;
    public TextMeshProUGUI misiles;

    void Start()
    {
        currentMissiles = maxMissiles;
    }

    void Update()
    {
        // Disparar balas con click izquierdo
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullet();
        }

        // Disparar misiles con click derecho, solo si tiene disponibles
        if (Input.GetMouseButtonDown(1) && currentMissiles > 0)
        {
            ShootMissile();
            currentMissiles--;
            Debug.Log(currentMissiles);
        }
        misiles.text = "Misiles: " + currentMissiles.ToString();
    }

    void ShootBullet()
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * bulletSpeed;
    }

    void ShootMissile()
    {
        GameObject missile = Instantiate(missilePrefab, firePoint.position, firePoint.rotation);
        Rigidbody rb = missile.GetComponent<Rigidbody>();
        rb.velocity = firePoint.forward * missileSpeed;
    }
}
