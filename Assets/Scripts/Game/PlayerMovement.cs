using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController _characterController;

    private Vector3 _moveDirection;

    private float _speed = 5;
    private float _gravity = 40;

    [SerializeField]
    private float _jumpForce = 10;
    private float _verticalVelocity;

    private AudioSource _stepAudioSource;

    [SerializeField]
    private float STEP_SFX_DISTANCE = 0.25f;

    private float _continuousMove = 0;


    void Awake()
    {
        _characterController = GetComponent<CharacterController>();

        _stepAudioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        ProcessMove();
        ProcessJump();
    }

    private void ProcessJump()
    {
        if(_characterController.isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            _verticalVelocity = _jumpForce;
        }
    }

    private void ProcessMove()
    {
        // Get input from keyboard.
        _moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        // Transform input from keyboard to player world position.
        _moveDirection = transform.TransformDirection(_moveDirection);

        // Calculate input from keyboard by multiplying it to speed.
        _moveDirection *= _speed * Time.deltaTime;

        _stepAudioSource.volume = 0.5f;

        // Process crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            transform.GetChild(0).localPosition = new Vector3(0, 1, 0);
            _moveDirection /= 2;
            _stepAudioSource.volume = 0.25f;
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3(0, 1.6f, 0);
        }


        // Process sprint.
        if (Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.LeftControl))
        {
            _moveDirection *= 2;
            _stepAudioSource.volume = 1;
        }

        CalculateGravityImpact();

        // Calculate user continuous move duration.
        Vector3 movement2D = new Vector3(_moveDirection.x, 0, _moveDirection.z);
        if (movement2D.magnitude > 0)
        {
            _continuousMove += movement2D.magnitude;
        }
        else
        {
            _continuousMove = 0;
        }

        // Move character with our inputs.
        _characterController.Move(_moveDirection);

        // Step
        if (_continuousMove >= STEP_SFX_DISTANCE)
        {
            _continuousMove = 0;
            _stepAudioSource.Play();
        }
    }

    private void CalculateGravityImpact()
    {
        _verticalVelocity -= _gravity * Time.deltaTime;

        _moveDirection.y = _verticalVelocity * Time.deltaTime;
    }
}
 