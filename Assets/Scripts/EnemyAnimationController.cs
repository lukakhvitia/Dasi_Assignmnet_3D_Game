using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyAnimationController : MonoBehaviour
{
   [SerializeField] private Animator animator;

   public void PlayIdle()
   {
      animator.Play("Idle");
   }

   public void PlayWalk()
   {
      animator.Play("WalkForward");
   }

   public void PlayRun()
   {
      animator.Play("RunForward");
   }

   public void PlayAttack()
   {
      animator.Play("Attack3");
   }
}
