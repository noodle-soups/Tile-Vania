using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{

    float levelLoadDelay = 3f;

    void OnTriggerEnter2D(Collider2D other)
    {
        // load next level after delay
        StartCoroutine(LoadNextLevel());
    }


    IEnumerator LoadNextLevel()
    {
        // wait for few seconds
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // get current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // get next scene index
        int nextSceneIndex = currentSceneIndex + 1;

        // if no more levels, return to first
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        // load next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

}
