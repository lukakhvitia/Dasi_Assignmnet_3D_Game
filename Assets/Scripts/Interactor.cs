using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{

    [SerializeField] private Transform interactionPoint;
    [SerializeField] private float interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask interactableLayerMask;

    [SerializeField] private Button chopButton;

    private readonly Collider[] _colliders = new Collider[3];
    [SerializeField] private int _numFound;

    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        _numFound = Physics.OverlapSphereNonAlloc(interactionPoint.position, interactionPointRadius, _colliders,
            interactableLayerMask);
    }

    public void CutTree()
    {
         if (_numFound > 0)
         {
             var interactable = _colliders[0].GetComponent<IInteractable>();

             interactable?.Interact(this);
             _animator.SetTrigger("Chop");
         }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
