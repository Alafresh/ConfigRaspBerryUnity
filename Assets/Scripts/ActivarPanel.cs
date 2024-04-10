using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActivarPanel : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerUI;

 

    private void ActivarDesactivarPanel()
    {       
        //player.SetActive(!player.activeSelf);
        //playerUI.SetActive(!playerUI.activeSelf);
        SceneManager.LoadScene("LoadingScene");
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
