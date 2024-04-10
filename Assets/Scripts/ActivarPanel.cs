using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivarPanel : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject panel;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerUI;

    private void ActivarDesactivarPanel()
    {
        panel.SetActive(!panel.activeSelf);
        player.SetActive(!player.activeSelf);
        playerUI.SetActive(!playerUI.activeSelf);
    }

    public void Interact(Transform interactorTransform)
    {
        ActivarDesactivarPanel();
    }

    public string GetInteractText()
    {
        return "Activar Sistema de Medicion";
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
