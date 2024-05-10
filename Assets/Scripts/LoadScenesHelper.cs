using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScenesHelper : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadScene("UIScene"));
    }
    IEnumerator LoadScene(string sceneName)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadScene(sceneName);
    }
}
