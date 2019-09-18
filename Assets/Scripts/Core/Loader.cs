using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System;

public class Loader : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [Header("~ OBJECTS ~")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject  outerBlocks;

    [Header("~ IN$CRIBE ~")]
    [SerializeField] private float       initGameManagerDelay = 2.0f;

    public enum eGameSequence { PreLoad, Loading, Loaded, GameOver };

    // Private:
    private bool gameStarted = false;

    // Static:                            //0     //1     //2        //3     //4    //5    //6        //7     //8    //9    //10    //11    //12
    public  static string[]      NAMES = { "NONE", "John", "Charlie", "Jane", "Roy", "Dan", "Chester", "Mary", "Bob", "Sam", "Bill", "Mark", "Fisher" };
    private static bool          canInput;
    private static eGameSequence gameState;

    // Delegates & Events:
    public delegate void  LoaderDelegate ();
    public static   event LoaderDelegate StartLoadingMain;
    public static   event LoaderDelegate StartGame;

    #endregion


    #region ~ Methods | MonoBehaviour ~
    private void Start  ()
    {
        gameState = eGameSequence.PreLoad;
        canInput = true;
    }
    private void Update ()
    {
        if (GameState == eGameSequence.PreLoad  && CanInput)
        {
            if (Input.GetButtonDown("Jump"))
            {
                GameState = eGameSequence.Loading;
                StartLoadingMain();
            }
            else return;
        }

        if (GameState == eGameSequence.Loaded   && CanInput && !gameStarted)
        { AwaitingForLaunch(); }

        if (GameState == eGameSequence.GameOver && CanInput)
        {
            if (Input.GetButtonDown("Jump"))
            {
                //DestroyImmediate(gameManager, true);
                StartCoroutine(Crt_Reload());
            }
        }
    }

    

    #endregion
    
    #region ~ Methods | Primary ~

    private void        AwaitingForLaunch ()
    {
    if (Input.GetButtonDown("Jump"))
        {
            gameStarted = true;
            StartGame();

            Invoke("InitGameManager", initGameManagerDelay);
        }
    }
    private void        InitGameManager   ()
    {
        if (GameManager.instance == null) Instantiate(gameManager);
    }
    private IEnumerator Crt_Reload        ()
    {
        /*
        foreach (GameObject o in GameObject.FindObjectsOfType<GameObject>())
        {
            if (!o.CompareTag("MainCamera")) Destroy(o.gameObject);
        }
        //GC.Collect();
        SceneManager.UnloadSceneAsync("MainScene");
        yield return new WaitForSeconds(5.0f);
        SceneManager.LoadScene("MainScene");
        */

        Application.LoadLevel(Application.loadedLevel);
        yield return null;
    }

    #endregion


    #region ~ Properties ~

    public static bool          CanInput
    {
        get { return canInput;  }
        set { canInput = value; }
    }
    public static eGameSequence GameState
    {
        get { return gameState; }
        set { gameState = value; }
    }

    #endregion
}