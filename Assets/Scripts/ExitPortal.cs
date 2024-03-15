using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitPortal : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        // get current scene index
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        // load next scene
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

}
