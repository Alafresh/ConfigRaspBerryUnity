using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCInteractable : MonoBehaviour, IInteractable {

    [SerializeField] private string interactText;

    private Animator animator;
    private NPCHeadLookAt npcHeadLookAt;

    private void Awake() {
        animator = GetComponent<Animator>();
        npcHeadLookAt = GetComponent<NPCHeadLookAt>();
    }

    public void Interact(Transform interactorTransform) {
        ChatBubble3D.Create(transform.transform, new Vector3(-.3f, 1.7f, 0f), ChatBubble3D.IconType.Angry, "Detente!");
    }

    public string GetInteractText() {
        return interactText;
    }

    public Transform GetTransform() {
        return transform;
    }

}