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
   public Transform[] wayPoints;
   private Vector3 target;
   private int currentWaypointIndex;
   public int patrollingSpeed = 4;
   
   [Header("Attacking")] //Attacking
   public float timeBetweenAttacks;
   private bool _alreadyAttacked;
   public int chaseSpeed = 9;

   [Header("States")] //States 
   public float sightRange;
   public float attackRange;
   public bool playerInSightRange;
   public bool playerInAttackRange;



   public float remainingDistanceToWayPoint = 3f;
   private void Start()
   {
      SetDestinationToWaypoint(currentWaypointIndex);
   }

   private void Update()
   {
      //Check for sight and attack range
      playerInSightRange = Physics.CheckSphere(transform.position, sightRange, whatIsPlayer);
      playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, whatIsPlayer);

      if (!playerInSightRange && !playerInAttackRange)
      {
         if (!agent.pathPending && agent.remainingDistance < remainingDistanceToWayPoint)
         {
            // Move to the next waypoint in a loop
            currentWaypointIndex = (currentWaypointIndex + 1) % wayPoints.Length;
            SetDestinationToWaypoint(currentWaypointIndex);
         }
         
         agent.speed = patrollingSpeed;
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
      
      
   }
   
   private void SetDestinationToWaypoint(int waypointIndex)
   {
      // Set the agent's destination to the specified waypoint
      agent.SetDestination(wayPoints[waypointIndex].position);
   }
   
   
   private void ChasePlayer()
   {
      agent.speed = chaseSpeed;
      animationController.PlayRun();
      agent.SetDestination(player.position); 
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
