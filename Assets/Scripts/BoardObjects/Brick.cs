using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class Brick : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private Sprite[] spriteCollection;
    [SerializeField] private bool     vulnerable;

    [SerializeField] private float    shakingDuration = .75f;

    // Private:
    private int            lives;

    private SpriteRenderer sr;

    private Color          currentColor;
    private Color          targetColor;
    private bool           interpolatingColour;

    private bool           shaking;
    private float          shakingTimer;
    private Vector2        shakingDirection;
    private Vector3        cashedPosition;

                                                         //Green    //Orange   //Red
    private List<string>   colours = new List<string>() { "#00FF00", "#FFA500", "#FF0000" };

    // Delegates & Events:
    public delegate void  BrickDelegate (Vector2 position);
    public static   event BrickDelegate BrickDestroy;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake  ()
    {
        sr = GetComponent<SpriteRenderer>();

        sr.sprite   = spriteCollection[Random.Range(0, spriteCollection.Length)];
        sr.color = Color.white;

        vulnerable = false;
    }
    private void Start  ()
    {
        cashedPosition = transform.position;
        interpolatingColour = false;
    }
    private void Update ()
    {
        if (interpolatingColour) sr.color = InterpolateColor(sr.color, ParseColor(colours[lives]), .12f);

        if      (shaking)                              CollisionShaking(shakingDuration, shakingDirection);
        else if (transform.position != cashedPosition) BackToCashedPosition();
    }

    #endregion

    #region ~ Methods | Primary ~

    private Color InterpolateColor     (Color initColor, Color targetColor, float speedChange)
    {
        if (initColor != targetColor) initColor = Color.Lerp(initColor, targetColor, Mathf.PingPong(Time.time, speedChange)); else interpolatingColour = false;
        return initColor;
    }
    private void  CollisionShaking     (float shakingDuration, Vector2 directionShaking)
    {
        shakingTimer += Time.deltaTime;

        if (shakingTimer < shakingDuration)
        {
            transform.Translate(directionShaking * Mathf.Sin(Time.time * 20.0f) * .005f);
            return;
        }

        shakingTimer = .0f;
        shaking = false;
    }
    private void  BackToCashedPosition ()
    {
        transform.position = Vector3.MoveTowards(transform.position, cashedPosition, Time.deltaTime * .1f);
    }
    private void  GetDamage            ()
    {
        shaking = true;
        shakingDirection = GetRandomDirection();

        if (!vulnerable) return;

        if (lives == 0)
        {
            BrickDestroy((Vector2)transform.position);
            Destroy(gameObject);
        }

        --lives;
        interpolatingColour = true;
    }

    #endregion

    #region ~ Methods | Secondary ~

    private Color   ParseColor         (string color)
    {
        Color parseColor;

        if (ColorUtility.TryParseHtmlString(color, out parseColor)) return parseColor;

        Debug.LogError("Couldn't parse that string. Returning black color.");
        return Color.black;
    }
    private Vector2 GetRandomDirection ()
    {
        int randomNumber = Random.Range(0, 4);

        switch (randomNumber)
        {
            case 0:
                return Vector2.up;
            case 1:
                return Vector2.right;
            case 2:
                return Vector2.up + Vector2.right;
            case 3:
                return Vector2.down + Vector2.right;
        }
        return Vector2.zero;
    }

    #endregion


    #region ~ Collisions ~

    private void OnCollisionEnter2D (Collision2D collision)
    {
        GetDamage();
    }
    private void OnTriggerEnter2D   (Collider2D collision)
    {
        if (collision.gameObject.layer != 17) return;
        GetDamage();
        //Destroy(collision.gameObject);
    }

    #endregion


    #region ~ Properties ~

    public bool SetVulnerable
    {
        set
        {
            vulnerable = value;

            lives = Random.Range(0, 3);
            currentColor = ParseColor(colours[lives]);
            sr.color = currentColor;
        }
    }

    #endregion
}
