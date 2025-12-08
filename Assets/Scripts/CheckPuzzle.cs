using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;
using Oculus.Interaction;
using Oculus.Interaction.Collections;

public class CheckPuzzle : MonoBehaviour
{
    [Header("Setup")]
    public List<Transform> targetOptions; 

    [HideInInspector]
    public Transform snappedTarget;
    public bool isSolved;

    private AudioSource audioSource;
    private Rigidbody rigidbody;
    private BoxCollider boxCollider;
    private GrabInteractable grabInteractable;
    private VisualMemoryManager memoryManager;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        grabInteractable = transform.GetChild(0).GetComponent<GrabInteractable>();
        memoryManager = FindObjectOfType<VisualMemoryManager>();
    }

    void Update()
    {
        if (isSolved || grabInteractable.State == InteractableState.Select) return; 

        foreach (Transform potentialTarget in targetOptions)
        {
            // Wir prüfen jetzt auf den COLLIDER, nicht mehr auf ActiveSelf,
            // weil das Objekt ja sichtbar bleiben muss für das Feedback.
            if (potentialTarget.GetComponent<Collider>().enabled == true)
            {
                if (Vector3.Distance(transform.position, potentialTarget.position) < 0.15f)
                {
                    SnapToTarget(potentialTarget);
                    break;
                }
            }
        }
    }

    void SnapToTarget(Transform target)
    {
        IEnumerable<GrabInteractor> setInteractors = grabInteractable.Interactors;
        foreach (GrabInteractor interactor in setInteractors) 
        { 
            interactor.Unselect(); 
        }

        transform.SetPositionAndRotation(target.position, target.rotation);
        
        // ÄNDERUNG: Wir schalten den Block NICHT aus, sondern deaktivieren nur den Collider.
        // So kann kein anderer Block mehr hier einrasten, aber wir können ihn noch grün färben.
        if(target.GetComponent<Collider>() != null)
            target.GetComponent<Collider>().enabled = false;

        snappedTarget = target;
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        boxCollider.enabled = false;

        if (audioSource != null) audioSource.Play();
        isSolved = true;
        
        // Jetzt den Manager rufen für das Farb-Feedback
        if (memoryManager != null)
        {
            memoryManager.CheckPlayerInput(target.gameObject);
        }
    }
}