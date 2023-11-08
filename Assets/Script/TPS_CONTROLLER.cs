using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_CONTROLLER : MonoBehaviour

{
    private CharacterController _controller;
    private Animator _animator;
    private Transform _camera;
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
    private bool _isGrounded;

    //rayo damage
    public int shootDamage = 2;

    void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponentInChildren<Animator>();
        _camera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");
        if(Input.GetButton("Fire2"))
        {
            AiMovement();
        }
        else
        {
            Movement();
        }

        //Movement();
        Jump();
        
        if(Input.GetKeyDown(KeyCode.K))
        {
        RayTest();
        }
    }

    void Movement()
    {
        Vector3 direction = new Vector3 (_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", 0);
        _animator.SetFloat("VelZ", direction.magnitude); //magnitude nos devuelve el tamaño del vector

        if(direction != Vector3.zero)
        {
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        _controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
    }
    void AiMovement()
    {
        Vector3 direction = new Vector3 (_horizontal, 0, _vertical);

        _animator.SetFloat("VelX", _horizontal);
        _animator.SetFloat("VelZ", _vertical);

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _camera.eulerAngles.y;
        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _camera.eulerAngles.y, ref turnSmoothVelocity, turnSmoothTime);
        
        transform.rotation = Quaternion.Euler(0, smoothAngle, 0);

        if(direction != Vector3.zero)
        {
        Vector3 moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;
        _controller.Move(moveDirection.normalized * playerSpeed * Time.deltaTime);
        }
    }

    void Jump()
    {
        _isGrounded = Physics.CheckSphere(_sensorPosition.position,_sensorRadius, _groundLayer);

        /*_isGrounded = Physics.Raycast(_sensorPosition.position, Vector3.down, _sensorRadius, _groundLayer);
        Debug.DrawRay(_sensorPosition.position, Vector3.down * _sensorRadius, Color.red);*/
        //Raycast ground sensor

        if(_isGrounded && _playerGravity.y <0)
        {
            _playerGravity.y = 0; //-2 en raycast
        }
            if(_isGrounded && Input.GetButtonDown ("Jump"))
        {
            _playerGravity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
        }

        _playerGravity.y += _gravity * Time.deltaTime;
        _controller.Move(_playerGravity * Time.deltaTime);

    }

    void RayTest()
    {
        /* Reycast simpre para el rayo
        if(Physics.Raycast(transform.position, transform.forward, 10))
        //si quisiesemos que el rayo fuese infinito seria Math.infinity en vez de 10//
        {
            Debug.Log("Hit");
            Debug.DrawRay(transform.position, transform.forward * 10, Color.green);
        }

        else
        {
            Debug.DrawRay(transform.position, transform.forward * 10, Color.red);
        }
        */

        RaycastHit hit;
        if(Physics.Raycast(transform.position, transform.forward, out hit, 10))
        {
            Debug.Log(hit.transform.name);
            Debug.Log(hit.transform.position);
            //Destroy(hit.transform.gameObject); pa destruir los objetos que se cruzan con el rayo

            Box caja = hit.transform.GetComponent <Box>(); //para que destruya solo las cajas
            if(caja != null)
            {
                caja.TakeDamage(shootDamage); //hace referencia al damage que le indicamos al principio
            }
        }
    }
}
