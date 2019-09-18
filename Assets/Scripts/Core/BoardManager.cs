using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [Header("~ Board Objects ~")]
    [SerializeField] private GameObject   platform;
    [SerializeField] private GameObject   brick;
    [SerializeField] private GameObject   ball;
    [SerializeField] private GameObject   ballGreen;
    [SerializeField] private GameObject   ballBlue;
    [SerializeField] private GameObject   gem;
    [SerializeField] private GameObject   person;
    [SerializeField] private GameObject[] ship;

    
    [Header("~ IN$CRIBED ~")]
    [SerializeField] private float      objectSpawnAndDestroySpeed = .1f;

    [Header("~ ADDITIONAL ~")]
    [SerializeField] private Sprite[] colorSpritePersons;

    [Header("~ $OUND ~")]
    [SerializeField] private AudioClip buildSfx;

    // Private:
    private int   level;
    private bool  levelComplete;

    private float matrixXEdge  = -4.0f;
    private float matrixYUp    =  4.0f;
    private float matrixYDown  = -1.5f;
    private float matrixOffset =   .5f;
    
    private Transform     boardHolder;
    private List<Vector2> matrixPositions = new List<Vector2>();

    private float bricksToSpawn;
    private int   nonVulBricks;
    private int   vulBricks;
    private int   gems;
    private int   objectsToDestroyToCompleteLevel;
    private int   itemsToPickUp;
    private bool  waitingForPickingUpItems;

    private bool  canSpawnShips;
    private int   randomSpawnShipTime;
    private float timer;

    // Static:
    public static BoardManager instance;

    // Delegates & Events:
    public delegate void  BoardManagerDelegate ();
    public static   event BoardManagerDelegate LevelComplete;
    public static   event BoardManagerDelegate GameComplete;
    public static   event BoardManagerDelegate LevelHasBeenLoaded;
    public static   event BoardManagerDelegate FreePerson;

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
        levelComplete = false;

        canSpawnShips = false;
        randomSpawnShipTime = Random.Range(6, 13);
    }
    private void Update    ()
    {
        if (!levelComplete) CheckLevelProgression();

        if (waitingForPickingUpItems)
        {
            if (itemsToPickUp == 0)
            {
                StartCoroutine(Crt_RemoveAllBlocksAndSendMessageGameManager(true));
                waitingForPickingUpItems = false;
            }
        }

        if (canSpawnShips)
        {
            timer += Time.deltaTime;
            if (timer >= randomSpawnShipTime)
            {
                canSpawnShips = false;
                SpawnShip();
            }
        }
    }
    private void OnEnable  ()
    {
        GameManager.StartLevel                   += SetupScene;
        GameManager.SpawnBall                    += SpawnBallWithDelay;
        GameManager.GameOver                     += RemoveAfterGameOver;
        CanvasScript.UIReady                     += FillSceneWithObjects;
        Platform.BonusBalls                      += SpawnTwoBonusBalls;
        Brick.BrickDestroy                       += ObjectDestroyed;
        Gem.GemDestroy                           += ObjectDestroyed;
        Person.PersonRescued                     += PickFallingItemsAndGoToTheNextLevel;
        FallingItem.AddItemToObjectsToDestroy    += PlusItemToPickUp;
        FallingItem.RemoveItemToObjectsToDestroy += MinusItemToPickUp;
    }
    private void OnDisable ()
    {
        GameManager.StartLevel                   -= SetupScene;
        GameManager.SpawnBall                    -= SpawnBallWithDelay;
        GameManager.GameOver                     -= RemoveAfterGameOver;
        CanvasScript.UIReady                     -= FillSceneWithObjects;
        Platform.BonusBalls                      -= SpawnTwoBonusBalls;
        Brick.BrickDestroy                       -= ObjectDestroyed;
        Gem.GemDestroy                           -= ObjectDestroyed;
        Person.PersonRescued                     -= PickFallingItemsAndGoToTheNextLevel;
        FallingItem.AddItemToObjectsToDestroy    -= PlusItemToPickUp;
        FallingItem.RemoveItemToObjectsToDestroy -= MinusItemToPickUp;
    }

    #endregion

    #region ~ Methods | Primary ~

    private void        SetupScene                                    (int level, float levelStartDelay, int hearts, int magic)
    {
        boardHolder = new GameObject("Board").transform;

        this.level = level;
        levelComplete = false;
        objectsToDestroyToCompleteLevel = 0;
        itemsToPickUp = 0;
        waitingForPickingUpItems = false;

        bricksToSpawn = level * 2;

        nonVulBricks  = (int) Mathf.Round(bricksToSpawn * .4f);
        vulBricks     = (int) Mathf.Round(bricksToSpawn * .6f);

        gems = level * 2;

        objectsToDestroyToCompleteLevel = vulBricks + gems;

        InitialiseMatrix();
    }
    private void        FillSceneWithObjects                          ()
    {
        StartCoroutine(Crt_SpawnGameObjects(nonVulBricks, vulBricks, gems, 1.0f));

        canSpawnShips = true;
    }

    private IEnumerator Crt_SpawnGameObjects                          (int nonVulBricks, int vulBricks, int gems, float levelStartDelay)
    {
        yield return new WaitForSeconds(levelStartDelay);

        for (var i = 0; i < nonVulBricks; i++)
        {
            GameObject newBrick = LayoutObjectAtRandom(brick, true);
            SoundManager.instance.PlaySingle(buildSfx);
            yield return new WaitForSeconds(objectSpawnAndDestroySpeed);
        }

        for (var i = 0; i < vulBricks; i++)
        {
            GameObject newBrick = LayoutObjectAtRandom(brick, true);
            SoundManager.instance.PlaySingle(buildSfx);
            newBrick.GetComponent<Brick>().SetVulnerable = true;
            yield return new WaitForSeconds(objectSpawnAndDestroySpeed);
        }

        for (var i = 0; i < gems; i++)
        {
            GameObject newGem = LayoutObjectAtRandom(gem, true);
            SoundManager.instance.PlaySingle(buildSfx);
            yield return new WaitForSeconds(objectSpawnAndDestroySpeed);
        }

        LayoutObjectAtRandom(person, true);
        SoundManager.instance.PlaySingle(buildSfx);

        SpawnBall();

        Instantiate(platform, new Vector2(.0f, -4.65f), Quaternion.identity).transform.SetParent(boardHolder);

        LevelHasBeenLoaded();
    }
    private IEnumerator Crt_SpawnSingleObjectWithDelay                (GameObject go, bool removePositionFromMatrix, float delay)
    {
        yield return new WaitForSeconds(delay);
        LayoutObjectAtRandom(go, removePositionFromMatrix);
    }
    private void        SpawnBall                                     ()
    {
        LayoutObjectAtRandom(ball, false);
    }
    private void        SpawnBallWithDelay                            ()
    {
        StartCoroutine(Crt_SpawnSingleObjectWithDelay(ball, false, 2.0f));
    }
    private void        SpawnTwoBonusBalls                            ()
    {
        LayoutObjectAtRandom(ballGreen, false);
        LayoutObjectAtRandom(ballBlue, false);
    }

    private IEnumerator Crt_RemoveAllBlocksAndSendMessageToFreePerson (bool sendMessage)
    {
        canSpawnShips = false;
        List<Transform> objectsToRemove = new List<Transform>();

        for (var i = 0; i < boardHolder.childCount; i++)
        {
            objectsToRemove.Add(boardHolder.GetChild(i));
        }

        for (var i = objectsToRemove.Count - 1; i >= 0; i--)
        {
            try { GameObject tryIt = objectsToRemove[i].gameObject; } catch { --i; Debug.Log("Snap! Catched!!"); }

            int protectedLayer = objectsToRemove[i].gameObject.layer;
            if (protectedLayer != 16 && protectedLayer != 8 && protectedLayer != 9) Destroy(objectsToRemove[i].gameObject);

            yield return new WaitForSeconds(objectSpawnAndDestroySpeed / 2);
        }

        objectsToRemove.Clear();

        if (sendMessage)
        {
            yield return new WaitForSeconds(1.0f);

            FreePerson();
        }
    }
    private IEnumerator Crt_RemoveAllBlocksAndSendMessageGameManager  (bool sendMessage)
    {
        matrixPositions.Clear();
        try { Destroy(boardHolder.gameObject); } catch { Debug.Log("Snap! Catched! It's Okay."); }

        if (sendMessage)
        {
            if (GameManager.Level != 12)
                LevelComplete();
            else
                GameComplete();
            yield return null;
        }
    }
    private void        RemoveAfterGameOver                           (int level)
    {
        canSpawnShips = false;

        Destroy(boardHolder.gameObject);
    }
    private void        RemoveGameObjects                             (int level)
    {
        StartCoroutine(Crt_RemoveAllBlocksAndSendMessageGameManager(false));
    }

    private void        CheckLevelProgression                         ()
    {
        if (objectsToDestroyToCompleteLevel == 0)
        {
            levelComplete = true;
            canSpawnShips = false;

            objectsToDestroyToCompleteLevel = 1;

            StartCoroutine(Crt_RemoveAllBlocksAndSendMessageToFreePerson(true));
        }
    }
    private void        PickFallingItemsAndGoToTheNextLevel           ()
    {
        objectsToDestroyToCompleteLevel = 0;

        if   (itemsToPickUp != 0) waitingForPickingUpItems = true;
        else                      StartCoroutine(Crt_RemoveAllBlocksAndSendMessageGameManager(true));

    }

    #endregion

    #region ~ Methods | Secondary ~

    private void       InitialiseMatrix     ()
    {
        for (var x = matrixXEdge; x <= -matrixXEdge; x += matrixOffset)
        {
            for (var y = matrixYUp; y >= matrixYDown; y -= matrixOffset)
            {
                matrixPositions.Add(new Vector2(x, y));
            }
        }
    }
    private GameObject LayoutObjectAtRandom (GameObject go, bool removePositionFromMatrix)
    {
        Vector2 randomPosition = RandomPosition(removePositionFromMatrix);
        GameObject gameObject = Instantiate(go, randomPosition, Quaternion.identity);
        gameObject.transform.SetParent(boardHolder);

        if (gameObject.layer == 16) gameObject.GetComponent<SpriteRenderer>().sprite = colorSpritePersons[level - 1];

        return gameObject;
    }
    private Vector2    RandomPosition       (bool removePositionFromList)
    {
        int randomIndex = Random.Range(0, matrixPositions.Count);
        Vector2 randomPosition = matrixPositions[randomIndex];
        if (removePositionFromList) matrixPositions.RemoveAt(randomIndex);
        return randomPosition;
    }

    private void       ObjectDestroyed      (Vector2 positionOfDestroyedObject)
    {
        --objectsToDestroyToCompleteLevel;
        ReturnPosition(positionOfDestroyedObject);
    }
    private void       ReturnPosition       (Vector2 position)
    {
        matrixPositions.Add(position);
    }
    private void       PlusItemToPickUp     ()
    {
        itemsToPickUp++;
    }
    private void       MinusItemToPickUp    ()
    {
        itemsToPickUp--;
    }

    private void       EndSpawnShips        ()
    {
        canSpawnShips = false;
    }
    private void       SpawnShip           ()
    {
        Instantiate(ship[Random.Range(0, ship.Length)], new Vector2(-8.0f, Random.Range(-3.5f, 3.5f)), Quaternion.identity);
        timer = .0f;
        randomSpawnShipTime = Random.Range(6, 13);
        canSpawnShips = true;
    }

    #endregion

    #region ~ Properties ~

    public int ObjectsToDestroyToCompleteLevel
    {
        get { return objectsToDestroyToCompleteLevel; }
    }

    #endregion

}