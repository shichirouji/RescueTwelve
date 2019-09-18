using UnityEngine;

public class CollisionFxInterpolatingColour : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private Color targetColor;
    [SerializeField] private float colorChangeSpeed = 1.0f;

    // Private:
    private SpriteRenderer sr;

    private bool interpolatingColour;
    private bool interpolatingColourToTarget;
    private bool interpolatingColourToWhite;

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start ()
    {
        interpolatingColourToTarget = false;
        interpolatingColourToWhite = false;
    }
    private void Update()
    {
        if (interpolatingColour)
        {
            if (interpolatingColourToTarget)
                if (sr.color != targetColor) sr.color = InterpolateColor(sr.color, targetColor, colorChangeSpeed);
                else { interpolatingColourToTarget = false; interpolatingColourToWhite = true; }
            if (interpolatingColourToWhite)
                if (sr.color != Color.white) sr.color = InterpolateColor(sr.color, Color.white, colorChangeSpeed / 2);
                else { interpolatingColourToWhite = false; interpolatingColour = false; }
        }
    }
    /// FixedUpdate()
    /// LateUpdate()
    /// OnDisable()
    /// OnEnable()

    #endregion

    #region ~ Methods | Primary ~

    private Color InterpolateColor(Color initColor, Color targetColor, float speedChange)
    {
        if (initColor != targetColor) initColor = Color.Lerp(initColor, targetColor, Mathf.PingPong(Time.deltaTime * speedChange, speedChange)); //else interpolatingColour = false;
        return initColor;
    }

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

    #endregion


    #region ~ Collisions ~

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (interpolatingColour) return;
        interpolatingColour = true;
        interpolatingColourToTarget = true;
    }
    /// OnCollisionStay()
    /// OnCollisionExit()
    /// 
    /// OnTriggerEnter()
    /// OnTriggerStay()
    /// OnTriggerExit()

    #endregion


    #region ~ Properties ~

    // Custom Property...

    #endregion
}
