using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
   [SerializeField] private FloatingJoystick _joystick;
   [SerializeField] private AnimatorController _animatorController;

   [SerializeField] private PlayerHealth _playerHealth;
   private CameraMovement _cameraMovement;

   public float _moveSpeed;
   public float _rotateSpeed;

   private Rigidbody _rigidbody;
   private Vector3 _moveVector;

   private void Awake()
   {
      _rigidbody = GetComponent<Rigidbody>();
   }

   private void Update()
   {
      Debug.Log(_rigidbody.velocity);
      _rigidbody.velocity = Vector3.zero;
      Move();

      if (!_playerHealth.isAlive)
      {
         _animatorController.PlayDeath();
      }
   }

   private void Move()
   {
      _moveVector = Vector3.zero;

      _moveVector.x = _joystick.Horizontal * _moveSpeed * Time.deltaTime;
      _moveVector.z = _joystick.Vertical * _moveSpeed * Time.deltaTime;

      if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
      {
         Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
         transform.rotation = Quaternion.LookRotation(direction);

         _animatorController.PlayRun();
      }
      
      if (_joystick.Horizontal > 1 || _joystick.Vertical > 1)
      {
         Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, _rotateSpeed * Time.deltaTime, 0.0f);
         transform.rotation = Quaternion.LookRotation(direction);

         _animatorController.PlaySprint();
      }
      
      else if (_joystick.Horizontal == 0 && _joystick.Vertical == 0)
      {
         _animatorController.PlayIdle();
      }
      
      _rigidbody.MovePosition(_rigidbody.position + _moveVector);
   }
}
