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

    // ensure there is only ever 1 Game Session
    void Awake()
    {
        // how many Game-Session objects are there?
        int numGameSessions = FindObjectsOfType<GameSession>().Length;

        // if > 1 then destroy
        if (numGameSessions > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        // set tmp to read playerLives
        livesText.text = playerLives.ToString();
        
        // set tmp to read playerLives
        currencyText.text = playerCurrency.ToString();
    }

    // add to player currency
    public void CurrencyAdd(int currencyToAdd)
    {
        playerCurrency += currencyToAdd;
        currencyText.text = playerCurrency.ToString();
    }

    // methods triggered upon death (TakeLife or ResetGameSession)
    public void ProcessPlayerDeath()
    {
        if (playerLives > 1)
        {
            TakeLife();
        }
        else
        {
            ResetGameSession();
        }
    }

    // decrease lives by -1 & reload current scene
    void TakeLife()
    {
        // decrease current number of lives by 1
        playerLives--;

        // reload current scene
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);

        // set tmp to read playerLives
        livesText.text = playerLives.ToString();

        playerCurrency = 0;
        currencyText.text = playerCurrency.ToString();
    }

    // reload starting scene & destroy this game session
    void ResetGameSession()
    {
        // load first scene
        SceneManager.LoadScene(0);

        // destroy this game object
        Destroy(gameObject);
    }
}
