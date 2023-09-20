using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Tree : MonoBehaviour, IInteractable
{
    [SerializeField] private string prompt;
    [SerializeField] private GameObject tree;
    [SerializeField] private GameObject log1, log2;

    public string InteractionPrompt => prompt;
    public bool Interact(Interactor interactor)
    {
        DeactivateTree();
        ActivateLogs();
        Debug.Log("Cut");
        return true;
    }
    private void ActivateLogs()
    {
        log1.SetActive(true);
        log2.SetActive(true);
    }

    private void DeactivateTree()
    {
        tree.SetActive(false);
    }
}
