using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSession : MonoBehaviour
{

    [SerializeField] int playerLives = 3;
    [SerializeField] int playerCurrency = 0;

    [SerializeField] TextMeshProUGUI livesText;
    [SerializeField] TextMeshProUGUI currencyText;


    void Awake()
    {
        ManageGameSessionOnAwake();
    }


    void Start()
    {
        UIDefaultValues();
    }


    #region Manage Game Session
    // ensure there is only ever 1 Game Session
    void ManageGameSessionOnAwake()
    {
        int numGameSessions = FindObjectsOfType<GameSession>().Length;
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // destroy current Game Session
    void ResetGameSession()
    {
        Destroy(gameObject);
        //FindObjectOfType<ScenePersist>().ResetScenePersist();
    }
    #endregion


    #region UI Default Values
    // set default values for UI
    void UIDefaultValues()
    {
        livesText.text = playerLives.ToString();
        currencyText.text = playerCurrency.ToString();
    }
    #endregion


    #region Player Death Events
    // methods triggered upon death (see PlayerMovement.Die)
    public void ProcessPlayerDeath()
    {
        ResetPlayerCurrency();
        if (playerLives > 1)
        {
            ChangePlayerLives(-1);
            SceneLoadCurrent();
        }
        else
        {
            ResetGameSession();
            SceneLoadFirst();
        }
    }
    #endregion


    #region Player Lives
    // change the player's lives
    void ChangePlayerLives(int livesChangeValue)
    {
        playerLives += livesChangeValue;
        livesText.text = playerLives.ToString();
    }
    #endregion


    #region Scene Loads
    // load first scene
    void SceneLoadFirst()
    {
        SceneManager.LoadScene(0);
    }

    // load current scene
    void SceneLoadCurrent()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
    #endregion


    #region Player Currency
    // change the player's currency
    public void ChangePlayerCurrency(int currencyValue)
    {
        // called inside CoinPickup script
        playerCurrency += currencyValue;
        currencyText.text = playerCurrency.ToString();
    }

    // reset the player's currency
    void ResetPlayerCurrency()
    {
        playerCurrency = 0;
        currencyText.text = playerCurrency.ToString();
    }
    #endregion

}
