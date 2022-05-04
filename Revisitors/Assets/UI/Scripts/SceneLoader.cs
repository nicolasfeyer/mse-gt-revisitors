using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    
 
    IEnumerator passiveMe(int secs)
    {
        yield return new WaitForSeconds(secs);
        gameObject.SetActive(false);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public void LoadStartScene()
    {
        SceneManager.LoadScene(0);
        
    }

    public void LoadNextScene()
    {
        StartCoroutine(passiveMe(1));
        
    }

    public void quitGame()
    {
        Application.Quit();
    }
}