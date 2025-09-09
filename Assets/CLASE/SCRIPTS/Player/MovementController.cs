using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

[RequireComponent(typeof(Rigidbody), typeof(GroundCheck))]
public class MovementController : MonoBehaviour
{
    private InputManager inputManager;
    private Rigidbody rbPlayer;

    [SerializeField] private Animator _animator;
   
    
    private void Start()
    {
        inputManager = InputManager.Instance;
        rbPlayer = GetComponent<Rigidbody>();
    }
    
    private void FixedUpdate()
    {
        Movement();
        _animator.SetBool("IsWalking", inputManager.IsMoveInputPressed());
        _animator.SetBool("IsRunning", inputManager.WasRunInputPressed());
        _animator.SetFloat("WalkingZ", inputManager.GetMoveInput().y);
        _animator.SetFloat("WalkingX", inputManager.GetMoveInput().x);
    }


    #region Movimiento

    [SerializeField] private float walkSpeed = 5.5f;
    [SerializeField] private float runSpeed = 7.7f;
    [SerializeField] private float crouchSpeed = 3.9f;

    private void Movement()
    {
        rbPlayer.linearVelocity = transform.localRotation *
                            new Vector3(inputManager.GetMoveInput().x, 0, inputManager.GetMoveInput().y) *
                            (Time.deltaTime * Speed());
    }

    private float Speed()
    {
        return inputManager.IsMovingBackwards() || inputManager.IsMovingOnXAxis() ? walkSpeed :
            inputManager.WasRunInputPressed() ? runSpeed : walkSpeed;
    }


    #endregion

  
}