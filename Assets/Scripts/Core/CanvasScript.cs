using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [Header("~ MAIN ~")]
    [SerializeField] private Text       rescueTwelve;
    [SerializeField] private Text       hooray;
    [SerializeField] private GameObject personsAnchor;
    [SerializeField] private Image[]    persons;
    [SerializeField] private Text       pressSpace;
    [SerializeField] private Text       rescued;
    [SerializeField] private Text       scorePost;

    [Header("~ INIT+LEVEL ~")]
    [SerializeField] private Text       levelText;
    [SerializeField] private Image[]    hearts;
    [SerializeField] private Image[]    magic;
    [SerializeField] private Text       scoreGameplay;
    [SerializeField] private Text       fxMessage;

    [Header("~ IN$CRIBED ~")]
    [SerializeField] private float      showingDelay = .1f;

    [Header("~ ADDITIONAL ~")]
    [SerializeField] private Sprite[]   colorSpritePersons;

    [Header("~ SOUND ~")]
    [SerializeField] private AudioClip blip01;
    [SerializeField] private AudioClip pressSpaceSfx;

    // Private:
    private int heartsNumber;
    private int magicNumber;
    private int cashedScore;

    private int personToRescue;

    // Delegates & Events:
    public delegate void  CanvasDelegate ();
    public static   event CanvasDelegate UIReady;
    public static event   CanvasDelegate PostLevelHasBeenShown;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Start     ()
    {
        personToRescue = 1;

        TurnOffAllUI();

        pressSpace.text = "press space";
        pressSpace.enabled = true;
    }
    private void OnEnable  ()
    {

        Loader.StartLoadingMain      += LoadingMainMenu;
        GameManager.StartLevel       += InitNewLevelUI;
        GameManager.GameOver         += GameOver;
        BoardManager.LevelComplete   += ShowPostLevelMenu;
        BoardManager.GameComplete    += GameComplete;
        ScoreManager.UpdateStatistic += UpdateScoreGameplay;
        Platform.MagicFired          += TurnOffOneMagic;
        Platform.ItemPickedUp        += UpdateUIAfterItemPickedUp;
        Ball.BallFellAway            += TurnOffOneHeart;
        Gem.GemDestroy               += MoveTextObjectToVector2Position;
    }
    private void OnDisable ()
    {
        Loader.StartLoadingMain      -= LoadingMainMenu;
        GameManager.StartLevel       -= InitNewLevelUI;
        GameManager.GameOver         -= GameOver;
        BoardManager.LevelComplete   -= ShowPostLevelMenu;
        BoardManager.GameComplete    -= GameComplete;
        ScoreManager.UpdateStatistic -= UpdateScoreGameplay;
        Platform.MagicFired          -= TurnOffOneMagic;
        Platform.ItemPickedUp        -= UpdateUIAfterItemPickedUp;
        Ball.BallFellAway            -= TurnOffOneHeart;
        Gem.GemDestroy               -= MoveTextObjectToVector2Position;
    }

    #endregion

    #region ~ Methods | Primary ~

    private void        LoadingMainMenu               ()
    {
        pressSpace.enabled = false;
        StartCoroutine(Crt_LoadingMainMenu());
    }
    private IEnumerator Crt_LoadingMainMenu           ()
    {
        Loader.CanInput = false;

        string rt = "rescue twelve";
        rescueTwelve.enabled = true;
        rescueTwelve.text = "";
        for (var i = 0; i < rt.Length; i++)
        {
            rescueTwelve.text += rt[i];
            if (rt[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        for (var i = 0; i < persons.Length; i++)
        {
            persons[i].enabled = true;
            SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        Loader.GameState = Loader.eGameSequence.Loaded;
        pressSpace.text = "press space to start";
        SoundManager.instance.PlaySingle(pressSpaceSfx);
        pressSpace.enabled = true;

        Loader.CanInput = true;
    }
    private void        ShowPostLevelMenu             ()
    {
        StartCoroutine(Crt_ShowPostLevelMenu());
    }
    private IEnumerator Crt_ShowPostLevelMenu         ()
    {
        Loader.CanInput = false;

        TurnOnMain();
        StartCoroutine(Crt_HideCurrentMagicAndHearts());
        StartCoroutine(Crt_HideScoreGameplay());
        hooray.enabled = true;
        string hr = "hooray! " + Loader.NAMES[personToRescue++] + " has been rescued!";
        hooray.text = "";
        for (var i = 0; i < hr.Length; i++)
        {
            hooray.text += hr[i];
            yield return new WaitForSeconds(showingDelay / 3);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        for (var i = 0; i < persons.Length; i++)
        {
            persons[i].enabled = true;
            yield return new WaitForSeconds(showingDelay / 3);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        persons[personToRescue - 2].sprite = colorSpritePersons[personToRescue - 2];
        persons[personToRescue - 2].transform.localScale *= 1.5f;
        yield return new WaitForSeconds(1.0f - showingDelay);

        rescued.enabled = true;
        rescued.text = "";
        string targetString = "Rescued:   " + (personToRescue - 1).ToString() + " / 12";
        for (var i = 0; i < targetString.Length; i++)
        {
            rescued.text += targetString[i];
            if (targetString[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay / 2);
        }

        scorePost.enabled = true;
        scorePost.text = "";
        targetString = "Score:      " + ScoreManager.Score.ToString();
        for (var i = 0; i < targetString.Length; i++)
        {
            scorePost.text += targetString[i];
            if (targetString[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay / 2);
        }

        yield return new WaitForSeconds(.5f);

        pressSpace.text = "press space to continue";
        SoundManager.instance.PlaySingle(pressSpaceSfx);
        pressSpace.enabled = true;
        Loader.CanInput = true;

        PostLevelHasBeenShown();
    }
    private IEnumerator Crt_ShowPostLevelMenu         (int level, string message, bool canInput)
    {
        Loader.CanInput = false;

        TurnOnMain();
        StartCoroutine(Crt_HideCurrentMagicAndHearts());
        StartCoroutine(Crt_HideScoreGameplay());
        rescueTwelve.enabled = true;
        string rt = message;
        rescueTwelve.enabled = true;
        rescueTwelve.alignment = TextAnchor.MiddleCenter;
        rescueTwelve.rectTransform.anchoredPosition = new Vector2(.0f, rescueTwelve.rectTransform.anchoredPosition.y);
        rescueTwelve.text = "";
        for (var i = 0; i < rt.Length; i++)
        {
            rescueTwelve.text += rt[i];
            if (rt[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        for (var i = 0; i < personToRescue - 1; i++)
        {
            persons[i].enabled = true;

            yield return new WaitForSeconds(showingDelay / 3);
        }
        yield return new WaitForSeconds(1.0f - showingDelay);

        if (level == 12)
        {
            persons[personToRescue - 1].enabled = true;
            persons[personToRescue - 1].sprite = colorSpritePersons[personToRescue - 1];
            persons[personToRescue - 1].transform.localScale *= 1.5f;
        }

        rescued.enabled = true;
        rescued.text = "";
        string targetString;
        if (level != 12) targetString = "Rescued:   " + (personToRescue - 1).ToString() + " / 12";
        else             targetString = "Rescued:   " + "12 / 12";

        for (var i = 0; i < targetString.Length; i++)
        {
            rescued.text += targetString[i];
            if (targetString[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay / 2);
        }

        scorePost.enabled = true;
        scorePost.text = "";
        targetString = "Score:      " + ScoreManager.Score.ToString();
        for (var i = 0; i < targetString.Length; i++)
        {
            scorePost.text += targetString[i];
            if (targetString[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(showingDelay / 2);
        }

        yield return new WaitForSeconds(3.3f);

        pressSpace.text = "press space to try again";
        SoundManager.instance.PlaySingle(pressSpaceSfx);
        pressSpace.enabled = canInput;

        Loader.CanInput = canInput;
        Loader.GameState = Loader.eGameSequence.GameOver;
    }

    private void        InitNewLevelUI                (int level, float levelStartDelay, int hearts, int magic)
    {
        TurnOffMain();

        heartsNumber = hearts;
        magicNumber  = magic;

        StartCoroutine(Crt_ShowLevelText(level, levelStartDelay));
    }
    private IEnumerator Crt_ShowLevelText             (int level, float levelStartDelay)
    {
        yield return new WaitForSeconds(levelStartDelay);

        levelText.text = "";
        string targetString1 = "Level " + level;
        string targetString2 =  Loader.NAMES[level];

        levelText.enabled = true;
        for (var i = 0; i < targetString1.Length; i++)
        {
            levelText.text += targetString1[i];
            if (targetString1[i] != ' ') SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(1.0f);
        levelText.text = "";

        for (var i = 0; i < targetString2.Length; i++)
        {
            levelText.text += targetString2[i];
            SoundManager.instance.PlaySingle(blip01);

            yield return new WaitForSeconds(.1f);
        }

        yield return new WaitForSeconds(levelStartDelay);

        levelText.enabled = false;

        StartCoroutine(Crt_ShowCurrentMagicAndHearts(level, heartsNumber, magicNumber));
        StartCoroutine(Crt_ShowScoreGameplay());

        UIReady();
    }
    private IEnumerator Crt_ShowCurrentMagicAndHearts (int level, int hearts, int magic)
    {
        for (var i = 0; i < magic; i++)
        {
            this.magic[i].enabled = true;
            yield return new WaitForSeconds(showingDelay);
        }

        for (var i = hearts-1; i >= 0; i--)
        {
            this.hearts[i].enabled = true;
            yield return new WaitForSeconds(showingDelay);
        }
    }
    private IEnumerator Crt_HideCurrentMagicAndHearts ()

    {
        for (var i = 0; i < magicNumber; i++)
        {
            this.magic[i].enabled = false;
            yield return new WaitForSeconds(showingDelay);
        }

        for (var i = heartsNumber - 1; i >= 0; i--)
        {
            this.hearts[i].enabled = false;
            yield return new WaitForSeconds(showingDelay);
        }
    }
    private IEnumerator Crt_ShowScoreGameplay         ()
    {
        scoreGameplay.enabled = true;
        scoreGameplay.text = "";

        string targetString = "Score: " + ScoreManager.Score.ToString();

        for (var i = 0; i < targetString.Length; i++)
        {
            scoreGameplay.text += targetString[i];
            yield return new WaitForSeconds(showingDelay);
        }
    }
    private IEnumerator Crt_HideScoreGameplay()
    {
        while (scoreGameplay.text != "")
        {
            scoreGameplay.text = scoreGameplay.text.Remove(scoreGameplay.text.Length - 1);
            yield return new WaitForSeconds(showingDelay);
        }
    }
    private void        GameOver                      (int level)
    {
        StartCoroutine(Crt_HideCurrentMagicAndHearts());
        StartCoroutine(Crt_HideScoreGameplay());
        StartCoroutine(Crt_ShowPostLevelMenu(level, "game over", true));
    }
    private void        GameComplete                  ()
    {
        StartCoroutine(Crt_HideCurrentMagicAndHearts());
        StartCoroutine(Crt_HideScoreGameplay());
        StartCoroutine(Crt_ShowPostLevelMenu(12, "well... you did it.", false));
    }

    private void        TurnOnMain                    ()
    {
        StartCoroutine(Crt_TurnOnMain());
    }
    private IEnumerator Crt_TurnOnMain                ()
    {
        while (pressSpace.color.a != 1)
        {
            //Debug.Log(rescueTwelve.color.a);
            //rescueTwelve.color += new Color(0, 0, 0, Time.deltaTime);
            for (var i = 0; i < persons.Length; i++) persons[i].color += new Color(0, 0, 0, Time.deltaTime);
            pressSpace.color += new Color(0, 0, 0, Time.deltaTime);
            yield return null;
        }
    }
    private void        TurnOffMain                   ()
    {
        StartCoroutine(Crt_TurnOffMain());
    }
    private IEnumerator Crt_TurnOffMain               ()
    {
        while (persons[0].color.a >= .0f)
        {
            if (rescueTwelve.enabled) rescueTwelve.color -= new Color(0, 0, 0, Time.deltaTime / 2);

            if (hooray.enabled) hooray.color -= new Color(0, 0, 0, Time.deltaTime / 2);
            for (var i = 0; i < persons.Length; i++) persons[i].color -= new Color(0, 0, 0, Time.deltaTime / 2);

            if (rescued.enabled) rescued.color -= new Color(0, 0, 0, Time.deltaTime / 2);
            if (scorePost.enabled) scorePost.color -= new Color(0, 0, 0, Time.deltaTime / 2);

            if (pressSpace.enabled) pressSpace.enabled = false;

            yield return null;
        }

        if (rescueTwelve.enabled)
        {
            rescueTwelve.enabled = false;
            rescueTwelve.color += new Color(0, 0, 0, 1);
        }

        if (hooray.enabled)
        {
            hooray.enabled = false;
            hooray.color += new Color(0, 0, 0, 1);
        }

        for (var i = 0; i < persons.Length; i++)
        {
            persons[i].enabled = false;
            persons[i].color += new Color(0, 0, 0, 1);
        }

        if (rescued.enabled)
        {
            rescued.enabled = false;
            rescued.color += new Color(0, 0, 0, 1);
        }
        if (scorePost.enabled)
        {
            scorePost.enabled = false;
            scorePost.color += new Color(0, 0, 0, 1);
        }
    }

    #endregion

    #region ~ Methods | Secondary ~
    private void        TurnOffAllUI                    ()
    {
        hooray.enabled = false;
        rescueTwelve.enabled = false;
        for (int i = 0; i < persons.Length; i++) persons[i].enabled = false;
        pressSpace.enabled = false;

        levelText.enabled = false;
        for (var i = 0; i < magic.Length; i++) magic[i].enabled = false;
        for (var i = 0; i < hearts.Length; i++) hearts[i].enabled = false;

        rescued.enabled = false;
        scorePost.enabled = false;

        scoreGameplay.enabled = false;
    }
    private void        TurnOffOneHeart                 ()
    {
        if (BoardManager.instance.ObjectsToDestroyToCompleteLevel == 0) return;
        if (GameManager.NumberOfBalls > 1) return;
        if (heartsNumber != 0) hearts[--heartsNumber].enabled = false;
    }
    private void        TurnOffOneMagic                 ()
    {
        if (magicNumber != 0) magic[--magicNumber].enabled = false;
    }
    private void        UpdateUIAfterItemPickedUp       (string itemName)
    {
        if (itemName == "Magic")
        {
            if (magicNumber == 5) return;
            magic[magicNumber++].enabled = true;
        }
        if (itemName == "Heart")
        {
            if (heartsNumber == 5) return;
            hearts[++heartsNumber - 1].enabled = true;
        }
    }
    private void        UpdateScoreGameplay             (int score)
    {
        scoreGameplay.GetComponent<BreathfulThing>().enabled = true;

        StopCoroutine  (Crt_UpdatingScore(score));
        StartCoroutine (Crt_UpdatingScore(score));
    }
    private IEnumerator Crt_UpdatingScore               (int score)
    {
        while (cashedScore <= score)
        {
            cashedScore += 10;
            yield return null;
            scoreGameplay.text = "Score: " + cashedScore;
        }
        cashedScore = score;
        scoreGameplay.text = "Score: " + cashedScore;

        scoreGameplay.GetComponent<BreathfulThing>().enabled = false;
        scoreGameplay.rectTransform.localScale = new Vector3(1, 1, 1);
    }
    private void        MoveTextObjectToVector2Position (Vector2 position)
    {
        Vector2 newPosition = Camera.main.WorldToScreenPoint(position);
        fxMessage.transform.position = newPosition;
    }

    #endregion
}
