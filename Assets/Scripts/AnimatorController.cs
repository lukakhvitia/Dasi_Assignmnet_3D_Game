using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimatorController : MonoBehaviour
{
    private const string Sprint = "Sprint";
    
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayIdle()
    {
        _animator.Play("Idle");
    }

    public void PlayRun()
    {
        _animator.Play("RunForward");
    }
}
