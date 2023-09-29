using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController _controller;
    private float _horizontal;
    private float _vertical;
    [SerializeField] private float playerSpeed = 5;
    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        Movement();
    }

    void Movement()
    {
        Vector3 direction = new Vector3 (_horizontal, 0, _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);
        //Estas dos líneas son la función para que el personaje gire

        _controller.Move(direction * playerSpeed * Time.deltaTime);
    }
}
