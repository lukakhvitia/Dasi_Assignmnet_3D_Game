using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class EnemyChase : MonoBehaviour
{
    public static EnemyChase Instance;

    [Header("Enemy Stats")]
    [SerializeField] private NavMeshAgent enemyNavMeshAgent;
    [SerializeField] private float chaseDistance = 3f;
    [SerializeField] private Animator _animator;
    [SerializeField] private int damageToGivePlayer = 100;
    
    [Header("Target Stats")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;

    private bool _isMoving = false;
    private bool _isAttacking = false;
    
    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        _isMoving = enemyNavMeshAgent.velocity.magnitude > 0.01f;

        if (_isMoving)
        {
            _animator.SetBool("Move", _isMoving);
            _isAttacking = false;
        }
        else
        {
            _animator.SetBool("Move", false);
        }

        if (distanceToPlayer <= chaseDistance)
        {
            enemyNavMeshAgent.SetDestination(player.position);

            if (distanceToPlayer <= enemyNavMeshAgent.stoppingDistance)
            {
                _isAttacking = true;
                FaceTarget();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && !_isMoving)
        {
            playerHealth.TakeDamage(damageToGivePlayer);
            _animator.SetTrigger("Attack");
        }
    }

    private void FaceTarget()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, chaseDistance);
    }
}
