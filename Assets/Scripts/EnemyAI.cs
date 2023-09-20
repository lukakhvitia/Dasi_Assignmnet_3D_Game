using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class EnemyAI : MonoBehaviour
{
   [SerializeField] private NavMeshAgent agent;
   [SerializeField] private Transform player;
   [SerializeField] private LayerMask whatIsGround;
   [SerializeField] private LayerMask whatIsPlayer;

   [SerializeField] private float health;
   
   [Header("Animation Controller")]
   [SerializeField] EnemyAnimationController animationController;
   
   [Header("Player Health")] //Player Health Options
   [SerializeField] private PlayerHealth playerHealth;
   [SerializeField] private int damageToTake = 10;
   
   [Header("Patrolling")] //Patrolling
   public Vector3 walkPoint;
   private bool _walkPointSet;
   public float walkPointRange;
   
   [Header("Attacking")] //Attacking
   public float timeBetweenAttacks;
   private bool _alreadyAttacked;

   [Header("States")] //States 
   public float sightRange;
   public float attackRange;
   public bool playerInSightRange;
   public bool playerInAttackRange;

   private void Update()
   {
      //Check for sight and attack range
      playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
      playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

      if (!playerInSightRange && !playerInSightRange)
      {
         Patrolling();
      }

      if (playerInSightRange && !playerInAttackRange)
      {
         ChasePlayer();
      }

      if (playerInSightRange && playerInAttackRange)
      {
         AttackPlayer();
      }
   }


   private void Patrolling()
   {
      if (!_walkPointSet) SearchWalkPoint();
      
      animationController.PlayWalk();
      
      if (_walkPointSet)
      {
         agent.SetDestination(walkPoint);
      }

      Vector3 distanceToWalkPoint = transform.position - walkPoint;
      
      //Walk point Reached
      if (distanceToWalkPoint.magnitude < 1f)
      {
         _walkPointSet = false;
      }
   }
   
   private void ChasePlayer()
   {
      animationController.PlayRun();
      agent.SetDestination(player.position); 
   }

   private void SearchWalkPoint()
   {
      //Calculate random point in range 
      float randomZ = Random.Range(-walkPointRange, walkPointRange);
      float randomX = Random.Range(-walkPointRange, walkPointRange);

      
      walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
      if (Physics.Raycast(walkPoint, -transform.up, 2f, whatIsGround))
      {
         _walkPointSet = true;
      }
   }

   private void AttackPlayer()
   {
      //We have to make sure enemy doesn't move
      agent.SetDestination(transform.position);
      
      //Smoothly looking at Target(player)
      FaceTarget();
      
         if (!_alreadyAttacked)
         {
            //Start of Attack
            
            animationController.PlayAttack();
            TakeDamageToPlayer();
            //End of Attack

            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
         }
         
   }
   
   private void TakeDamageToPlayer()
   {
      playerHealth.TakeDamage(damageToTake);
   }

   private void ResetAttack()
   {
      _alreadyAttacked = false;
   }

   private void FaceTarget()
   {
      Vector3 direction = (player.position - transform.position).normalized;
      Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
      transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
   }

   //Visual Stuff
   private void OnDrawGizmosSelected()
   {
      //Draws wire of attackRange
      Gizmos.color = Color.red;
      Gizmos.DrawWireSphere(transform.position, attackRange);

      //Draws wire of sightRange
      Gizmos.color = Color.yellow;
      Gizmos.DrawWireSphere(transform.position, sightRange);
   }
}
