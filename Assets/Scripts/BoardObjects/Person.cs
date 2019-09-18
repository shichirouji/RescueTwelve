using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Person : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private GameObject kago;
    [SerializeField] private float      colorChangeSpeed = 1.0f;
    [SerializeField] private float      speedOfAppearance = 120.0f;

    // Private:
    private bool isClosed;

    private SpriteRenderer srKago;
    private BoxCollider2D  boxCollider;

    private bool interpolatingColour;
    private bool interpolatingColourToRed;
    private bool interpolatingColourToWhite;

    // Delegates & Events:
    public delegate void  PersonDelegate ();
    public static   event PersonDelegate PersonRescued;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake     ()
    {
        isClosed = true;

        srKago = kago.GetComponent<SpriteRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }
    private void Start     ()
    {
        srKago.color = Color.white;

        interpolatingColourToRed = false;
        interpolatingColourToWhite = false;

        transform.localScale = new Vector3(.01f, .01f, .01f);
        StartCoroutine(SmoothAppearance());
    }
    private void Update    ()
    {
        if (interpolatingColour)
        {
            if (interpolatingColourToRed)
                if (srKago.color != Color.red) srKago.color = InterpolateColor(srKago.color, Color.red,   colorChangeSpeed);
            else { interpolatingColourToRed = false; interpolatingColourToWhite = true; }
            if (interpolatingColourToWhite)
                if (srKago.color != Color.white) srKago.color = InterpolateColor(srKago.color, Color.white, colorChangeSpeed / 2);
            else { interpolatingColourToWhite = false; interpolatingColour = false; }
        }

        if (isClosed) return;
        if (interpolatingColour) interpolatingColour = false;

        if (!Mathf.Approximately(srKago.color.a, .0f)) srKago.color = new Color(srKago.color.r, srKago.color.g, srKago.color.b, srKago.color.a - Time.deltaTime);
        if (transform.localScale.x <= 7.0f) transform.localScale += Vector3.one * Time.deltaTime;
        boxCollider.enabled = false;
    }
    private void OnEnable  ()
    {
        BoardManager.FreePerson += KagoFadingOut;
    }
    private void OnDisable ()
    {
        BoardManager.FreePerson -= KagoFadingOut;
    }

    #endregion

    #region ~ Methods | Primary ~

    // ^ _ ^ \\

    #endregion

    #region ~ Methods | Secondary ~

    private IEnumerator SmoothAppearance ()
    {
        while (transform.localScale.x < 2.0f)
        {
            transform.localScale += new Vector3(.05f, .05f, 05f) * Time.deltaTime * speedOfAppearance;
            yield return null;
        }
        transform.localScale = new Vector3(2.0f, 2.0f, 2.0f);
    }
    private Color       InterpolateColor (Color initColor, Color targetColor, float speedChange)
    {
        if (initColor != targetColor) initColor = Color.Lerp(initColor, targetColor, Mathf.PingPong(Time.deltaTime * speedChange, speedChange)); //else interpolatingColour = false;
        return initColor;
    }
    private void        KagoFadingOut    ()
    {
        isClosed = false;
    }

    #endregion


    #region ~ Collisions ~

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (!isClosed)
        {
            PersonRescued();

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (interpolatingColour) return;
        interpolatingColour = true;
        interpolatingColourToRed = true;
    }
    private void OnTriggerEnter2D   (Collider2D collision)
    {
        if (collision.gameObject.layer != 17) return;
        if (!isClosed)
        {
            PersonRescued();

            Destroy(collision.gameObject);
            Destroy(gameObject);
        }
        if (interpolatingColour) return;
        interpolatingColour = true;
        interpolatingColourToRed = true;
    }

    #endregion
    

    #region ~ Properties ~

    // Custom Property...

    #endregion
}
