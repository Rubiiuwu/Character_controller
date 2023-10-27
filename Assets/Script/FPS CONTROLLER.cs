using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCONTROLLER : MonoBehaviour
{
    CharacterController _controller;
    Transform _fpsCamera;

    [SerializeField] private float _speed;
    [SerializeField] private float _sensivility = 200;

    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer; 
    private bool _isGrounded;

    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;

    float _XRotation = 0;
    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _fpsCamera = Camera.main.transform;
    }

    void Update()
    {
        Movement();
        Jump();
    }

    void Movement()
    {
        float mouseX = Input.GetAxis("Mouse X") * _sensivility * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * _sensivility * Time.deltaTime;

        _XRotation -= mouseY;
        _XRotation = Mathf.Clamp(_XRotation, -90, 90);

        _fpsCamera.localRotation = Quaternion.Euler(_XRotation, 0, 0);
        transform.Rotate(Vector3.up * mouseX);

        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        _controller.Move(move.normalized * _speed * Time.deltaTime);
    }
    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position,_sensorRadius, _groundLayer);

        if(_isGrounded && _playerGravity.y <0)
        {
            _playerGravity.y = 0;
        }
            if(_isGrounded && Input.GetButtonDown ("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);

    }

}
