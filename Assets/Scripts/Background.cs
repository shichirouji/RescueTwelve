using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private float movingSpeed = 1.0f;

    // Private:
    private Vector2        horizontalDirection;
    private Vector2        verticalDirection;
    private bool           canDoVerticalMovement;
    private SpriteRenderer sr;
    private Color targetColor;
    private bool  fadeIn;
    private bool  fadeOut;
    

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake     ()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start     ()
    {
        sr.color = Color.black;

        canDoVerticalMovement = false;
        fadeIn                = false;
        fadeOut               = false;

        if (Random.Range(0, 2) == 0) horizontalDirection = Vector3.right; else horizontalDirection = Vector2.left;
        if (Random.Range(0, 2) == 0) verticalDirection   = Vector2.up;    else verticalDirection   = Vector2.down;
    }
    private void Update    ()
    {
        Movement();
        if (fadeIn)  Fading(Color.white, .1f);
        if (fadeOut) Fading(Color.black, .2f);
    }
    private void OnEnable  ()
    {
        Loader.StartGame           += AwaitingForStart;
        GameManager.StartLevel     += FadeIn;
        GameManager.GameOver       += FadeOut;
        BoardManager.LevelComplete += FadeOut;
    }
    private void OnDisable ()
    {
        Loader.StartGame           -= AwaitingForStart;
        GameManager.StartLevel     -= FadeIn;
        GameManager.GameOver       -= FadeOut;
        BoardManager.LevelComplete -= FadeOut;
    }

    #endregion

    #region ~ Methods | Primary ~

    private void        Movement            ()
    {
        transform.Translate(horizontalDirection * movingSpeed * Time.deltaTime);
        if (transform.position.x > 5.0f || transform.position.x < -5.0f) horizontalDirection *= -1;
        if (canDoVerticalMovement)
        {
            transform.Translate(verticalDirection * (movingSpeed * .5f) * Time.deltaTime);
            if (transform.position.y > 2.0f || transform.position.y < -2.0f) verticalDirection *= -1;
        }
    }
    private void        AwaitingForStart    ()
    {
        StartCoroutine(Crt_StartUpMovement(-0.6f, 1.0f));
    }
    private IEnumerator Crt_StartUpMovement (float targetYCoordinate, float speed)
    {
        while (transform.position.y != targetYCoordinate)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector2(transform.position.x, targetYCoordinate), speed * Time.deltaTime);

            yield return null;
        }

        canDoVerticalMovement = true;
    }

    #endregion

    #region ~ Methods | Secondary ~

    private void Fading  (Color targetColor, float changeSpeed)
    {
        if (sr.color != targetColor)
        {
            sr.color = Color.Lerp(sr.color, targetColor, Mathf.PingPong(Time.time, changeSpeed));
        }
        else
        {
            if (fadeIn) fadeIn = false;
            if (fadeOut) fadeOut = false;
        }

    }
    private void FadeIn  (int level, float levelStartDelay, int hearts, int magic)
    {
        fadeIn = true;
        fadeOut = false;
    }
    private void FadeIn  ()
    {
        fadeIn = true;
        fadeOut = false;
    }
    private void FadeOut (int level)
    {
        fadeOut = true;
        fadeIn = false;
    }
    private void FadeOut ()
    {
        fadeOut = true;
        fadeIn = false;
    }

    #endregion
}