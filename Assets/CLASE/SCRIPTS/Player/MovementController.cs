using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fusion;

[RequireComponent(typeof(Rigidbody), typeof(GroundCheck))]
public class MovementController : NetworkBehaviour
{
    private InputManager inputManager;
    private Rigidbody rbPlayer;

    [SerializeField] private Animator _animator;
   
    
    private void Awake()
    {
        rbPlayer = GetComponent<Rigidbody>();
    }
    

    public override void FixedUpdateNetwork() //Esto se sincroniza con el servidor
    {
        if (Object.HasStateAuthority)
        {
            //Aqui debo de cersioarrme de estar recibiendo el input del servidor
            if (GetInput(out NetworkInputData input)) //este me consigue el input que me manda el sevidor
            {
                Movement(input);
                UpdateAnimator(input);
            }
        }
    }

    private void UpdateAnimator(NetworkInputData input)
    {
        _animator.SetBool("IsWalking", input.move != Vector2.zero);
        _animator.SetBool("IsRunning", input.isRunning);
        _animator.SetFloat("WalkingZ", input.move.y);
        _animator.SetFloat("WalkingX", input.move.x);
    }

    #region Movimiento

    [SerializeField] private float walkSpeed = 5.5f;
    [SerializeField] private float runSpeed = 7.7f;
    [SerializeField] private float crouchSpeed = 3.9f;

    private void Movement(NetworkInputData input)
    {
        rbPlayer.linearVelocity = transform.localRotation *
                            new Vector3(input.move.x, 0, input.move.y) *
                            (Time.deltaTime * Speed(input));
    }

    private float Speed(NetworkInputData input)
    {
        return input.move.y < 0 || input.move.x != 0 ? walkSpeed :
            input.isRunning ? runSpeed : walkSpeed;
    }


    #endregion

  
}