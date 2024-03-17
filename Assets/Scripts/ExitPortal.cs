using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{

    float levelLoadDelay = 2f;

    // if player triggers exit then load next scene
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            // LoadNextLevel after delay
            StartCoroutine(LoadNextLevel());
        }
    }

    // load next scene after delay
    IEnumerator LoadNextLevel()
    {
        // wait for a few seconds
        yield return new WaitForSecondsRealtime(levelLoadDelay);

        // compute next scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        //
        FindObjectOfType<ScenePersist>().ResetScenePersist();

        // load next scene
        SceneManager.LoadScene(nextSceneIndex);
    }

}
