using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ball : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] protected internal GameObject[] objectsToLoot;

    [SerializeField] protected internal GameObject   arrow;
    [SerializeField] protected internal float        ballSpeed         = 1.0f;
    [SerializeField] protected internal float        speedOfAppearance = 120.0f;

    [SerializeField] protected internal AudioClip[] hitsSfx;
    [SerializeField] protected internal AudioClip   launchBall;

    // Private & Protected:
    protected internal Rigidbody2D    rb;
    protected internal bool           ballMoving;
    protected internal BreathfulThing breathfulThingScript;

    // Delegates & Events:
    public delegate void  BallDelegate ();
    public static   event BallDelegate BallFellAway;
    public static   event BallDelegate BallReadyToLaunch;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    protected internal void         Awake       ()
    {
        rb = GetComponent<Rigidbody2D>();
        breathfulThingScript = GetComponent<BreathfulThing>();
    }
    protected internal virtual void Start       ()
    {
        arrow.SetActive(false);
        ballMoving = true;

        transform.localScale = new Vector3(.01f, .01f, .01f);
        StartCoroutine(Crt_SmoothAppearance());
    }
    protected internal virtual void Update      ()
    {
        if (!ballMoving) WaitingForLaunchBall();
        else
        {
            CheckOutOfScreen();
        }
    }
    protected internal virtual void FixedUpdate ()
    {
        CheckVelocity();
    }

    #endregion

    #region ~ Methods | Primary ~

    private            void WaitingForLaunchBall ()
    {
        if (Input.GetButtonDown("Jump") && !ballMoving)
        {
            Vector2 direction = arrow.transform.right;

            arrow.active = false;
            breathfulThingScript.enabled = false;

            //SoundManager.instance.PlaySingleBall(launchBall);
            rb.velocity = (direction).normalized * ballSpeed;
            ballMoving = true;
        }
    }
    protected internal void CheckVelocity        ()
    {
        if (!Mathf.Approximately(rb.velocity.magnitude, ballSpeed))
        {
            rb.velocity = rb.velocity.normalized * ballSpeed;
        }
    }
    protected internal void CheckOutOfScreen     ()
    {
        if (transform.position.y < -5.25f)
        {
            BallFellAway();
            Destroy(gameObject);
        }
    }

    #endregion

    #region ~ Methods | Secondary ~

    protected internal virtual IEnumerator Crt_SmoothAppearance ()
    {
        while (transform.localScale.x < 3.0f)
        {
            transform.localScale += new Vector3(.05f, .05f, 05f) * Time.deltaTime * speedOfAppearance;
            yield return null;
        }
        transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);
        arrow.SetActive(true);
        ballMoving = false;

        BallReadyToLaunch();
    }

    #endregion


    #region ~ Collisions ~

    protected internal void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.layer == 10 || collision.gameObject.layer == 11 || collision.gameObject.layer == 18)
        {
            int randomRoll = Random.Range(1, 21);
            if      (randomRoll == 01)                      Instantiate(objectsToLoot[0], transform.position, Quaternion.identity);
            else if (randomRoll >= 02  && randomRoll <= 04) Instantiate(objectsToLoot[1], transform.position, Quaternion.identity);
            else if (randomRoll >= 05  && randomRoll <= 05) Instantiate(objectsToLoot[2], transform.position, Quaternion.identity);
            else if (randomRoll >= 11  && randomRoll <= 20) Instantiate(objectsToLoot[3], transform.position, Quaternion.identity);
        }

        SoundManager.instance.PlaySingleBall(hitsSfx[Random.Range(0, hitsSfx.Length)]);
    }

    #endregion
}
