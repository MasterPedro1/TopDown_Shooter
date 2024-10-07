using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Camera cam;

    Vector3 movement;
    Vector3 mousePos;

    void Update()
    {
        // Movimiento básico con WASD
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.z = Input.GetAxisRaw("Vertical");

        // Obtener posición del mouse
        mousePos = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.transform.position.y - transform.position.y));
    }

    void FixedUpdate()
    {
        // Mover al jugador
        transform.position += movement * moveSpeed * Time.fixedDeltaTime;

        // Rotar al jugador hacia el mouse
        Vector3 lookDir = mousePos - transform.position;
        lookDir.y = 0; // Mantener el eje Y constante
        transform.forward = lookDir;
    }
}
