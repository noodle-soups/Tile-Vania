using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{

    void Awake()
    {
        ManageScenePersistOnAwake();
    }



    #region Manage Scene Persists
    // ensure there is only ever 1 Game Session
    void ManageScenePersistOnAwake()
    {
        int numScenePersists = FindObjectsOfType<GameSession>().Length;
        if (numScenePersists > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
    #endregion

    public void ResetScenePersist()
    {
        Destroy(gameObject);
    }

}
