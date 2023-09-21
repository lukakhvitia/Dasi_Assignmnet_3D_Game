using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private FloatingJoystick joystick;

   [SerializeField] private PlayerHealth playerHealth;
   [SerializeField] private Animator animator;
   private CameraMovement _cameraMovement;

   public float moveSpeed;
   public float rotateSpeed;

   private Rigidbody _rigidbody;
   private Vector3 _moveVector;

   private void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }

   private void Start()
   {
      animator.SetBool("isAlive", true);
      animator.SetBool("Idle", true);
   }

   private void Update()
   {
      Debug.Log(_rigidbody.velocity);
      _rigidbody.velocity = Vector3.zero;
      Move();

      Death();
   }

   private void Move()
   {
      _moveVector = Vector3.zero;

      _moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
      _moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;

      if (joystick.Horizontal != 0 || joystick.Vertical != 0)
      {
         Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, rotateSpeed * Time.deltaTime, 0.0f);
         transform.rotation = Quaternion.LookRotation(direction);

         animator.SetBool("isRunning", true);
         animator.SetBool("Idle", false);
      }

      else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
      {
         animator.SetBool("isRunning", false);
         animator.SetBool("Idle", true);
      }
      
      _rigidbody.MovePosition(_rigidbody.position + _moveVector);
   }

   private void Death()
   {
      if (!playerHealth.isAlive)
      {
         animator.SetBool("isAlive", false);
         animator.SetBool("Idle", false);
         animator.SetBool("isRunning", false);
         
      }
   }
}
