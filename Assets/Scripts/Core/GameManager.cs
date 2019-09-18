using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [Header("~ IN$CRIBED ~")]
    
    [SerializeField] private int   hearts = 2;
    [SerializeField] private int   magic = 5;
    [SerializeField] private float levelStartDelay = 2.0f;

    // $INGLETON ++
    public static GameManager instance = null;

    // Private:
    private bool canLevelUp;


    // Static:
    private static int level = 1;
    private static int numberOfBalls;

    // Delegates & Events:
    public delegate void        GameManagerDelegate1  (int level, float levelStartDelay, int hearts, int magic);
    public static   event       GameManagerDelegate1  StartLevel;
    public static   event       GameManagerDelegate1  GetStatsAfterLoading;
    public delegate void        GameManagerDelegate2  ();
    public static   event       GameManagerDelegate2  SpawnBall;
    public delegate void        GameManagerDelegate3  (int level);
    public static   event       GameManagerDelegate3  GameOver;

    #endregion

    #region ~ Methods | MonoBehaviour ~

    private void Awake     ()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("Attemp to instantiate second GameManager");
            Debug.Break();
        }
    }

    private void Start     ()
    {
        level = 1;

        InitLevel(level);

        canLevelUp = false;
    }
    private void Update    ()
    {
        if (Input.GetButtonDown("Jump") && Loader.CanInput && canLevelUp)
        {
            canLevelUp = false;
            InitLevel(++level);
        }
    }
    private void OnEnable  ()
    {
        BoardManager.LevelHasBeenLoaded    += SendStatsAfterLoading;
        CanvasScript.PostLevelHasBeenShown += CanLevelUp;
        Platform.MagicFired                += LostMagic;
        Platform.ItemPickedUp              += UpdateStats;
        Platform.BonusBalls                += ThreeBalls;
        Ball.BallFellAway                  += LostHeart;
    }
    private void OnDisable ()
    {
        BoardManager.LevelHasBeenLoaded    -= SendStatsAfterLoading;
        CanvasScript.PostLevelHasBeenShown -= CanLevelUp;
        Platform.MagicFired                -= LostMagic;
        Platform.ItemPickedUp              -= UpdateStats;
        Platform.BonusBalls                -= ThreeBalls;
        Ball.BallFellAway                  -= LostHeart;
    }
    

    #endregion

    #region ~ Methods | Primary ~

    private void InitLevel  (int level)
    {
        numberOfBalls = 1;
        StartLevel(level, levelStartDelay, hearts, magic);
    }
    private void CanLevelUp ()
    {
        canLevelUp = true;
    }
    

    #endregion

    #region ~ Methods | Secondary ~

    private void LostHeart             ()
    {
        if (BoardManager.instance.ObjectsToDestroyToCompleteLevel == 0) return;

        if (numberOfBalls > 1)
        {
            numberOfBalls--;
            return;
        }

        if (hearts > 1)
        {
            --hearts;
            SpawnBall();
        }
        else GameOver(level);
    }
    private void LostMagic             ()
    {
        if (magic > 0)
        {
            --magic;
        }
    }
    private void SendStatsAfterLoading ()
    {
        GetStatsAfterLoading(level, levelStartDelay, hearts, magic);
    }
    private void UpdateStats           (string itemName)
    {
        if (itemName == "Coin") ;
        if (itemName == "Magic")
        {
            if (magic == 5) return;
            magic++;
        }
        if (itemName == "Heart")
        {
            if (hearts == 5) return;
            hearts++;
        }
    }
    private void ThreeBalls            ()
    {
        numberOfBalls = 3;
    }

    #endregion


    #region ~ Properties ~

    public static int NumberOfBalls
    {
        get { return numberOfBalls; }
    }
    public static int Level
    {
        get { return level; }
    }

    #endregion
}