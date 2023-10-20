using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class SHOULDER_CONTROLLER : MonoBehaviour

{
    private CharacterController _controller;
    private Transform _camera;
    private Transform _lookAtTransform;
    private float _horizontal;
    private float _vertical;

    //Variables para vel, salto y gravedad
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float _jumpHeight = 1;
    private float _gravity = -9.81f;
    private Vector3 _playerGravity;  

    //Variables de rotación
    private float turnSmoothVelocity;
    [SerializeField] float turnSmoothTime = 0.1f;

    //Variables para sensor
    [SerializeField] private Transform _sensorPosition;
    [SerializeField] private float _sensorRadius = 0.2f;
    [SerializeField] private LayerMask _groundLayer; 
    [SerializeField] private AxisState yAxis;
    [SerializeField] private AxisState xAxis;
    private bool _isGrounded;

    // Update is called once per frame
        void Awake()
    {
        _controller = GetComponent <CharacterController>();
        _camera = Camera.main.transform;
        _lookAtTransform = GameObject.Find("LookAt").transform;
    }
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        Jump();
        Movement();
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

    void Movement()
    {
        Vector3 move = new Vector3 (_horizontal, 0, _vertical) .normalized;

        yAxis.Update(Time.deltaTime);
        xAxis.Update(Time.deltaTime);

        transform.rotation = Quaternion. Euler(0, xAxis.Value, 0);
        _lookAtTransform.eulerAngles = new Vector3(yAxis.Value, xAxis.Value, 0);

        if(move != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
            Vector3 desiredDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
            _controller.Move(desiredDirection * playerSpeed * Time.deltaTime);
            
        }
    }
}