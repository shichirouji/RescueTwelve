using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private GameObject firePrefab;

    [SerializeField] private float platformSpeed      = 6.0f;
    [SerializeField] private float speedOfScaleChange = 60.0f;

    [SerializeField] private GameObject itemPickedUpExplosion;
    [SerializeField] private AudioClip  itemPickedUpSfx;

    // Private:
    private Rigidbody2D rb;

    private int   magicNumber;
    private float input;
    private bool  platformReadyToFire;
    private bool  canMoveToLeft;
    private bool  canMoveToRight;

    private byte scalingFactor = 1;

    // Const:
    private const float PLATFORM_DEFAULT_SCALE = .4f;

    // Delegates & Events:
    public delegate void  PlatformDelegate1 ();
    public static   event PlatformDelegate1 MagicFired;
    public static   event PlatformDelegate1 BonusBalls;
    public delegate void  PlatformDelegate2 (string itemName);
    public static   event PlatformDelegate2 ItemPickedUp;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake       ()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start       ()
    {
        platformReadyToFire    = false;
        canMoveToLeft          = true;
        canMoveToRight         = true;

        transform.localScale = new Vector3(.1f, .5f, .5f);

        StartCoroutine(Crt_ScaleSmoothChange(.4f, false));
    }
    private void Update      ()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (!canMoveToLeft  && input < 0) input = 0;
        if (!canMoveToRight && input > 0) input = 0;

        transform.Translate(Vector2.up * Mathf.Cos(Time.time * 3f) * .0025f);

        if (Input.GetButtonDown("Jump"))
            if (magicNumber != 0)
            {

                if (platformReadyToFire)
                {
                    Fire();
                }
                else
                    platformReadyToFire = true;
            }
    }
    private void FixedUpdate ()
    {
        rb.velocity = Vector2.right * input * platformSpeed;
    }
    private void OnEnable    ()
    {
        GameManager.GetStatsAfterLoading += CashMagicNumber;
        Ball.BallReadyToLaunch           += BanPlatformToFire;
    }
    private void OnDisable   ()
    {
        GameManager.GetStatsAfterLoading -= CashMagicNumber;
        Ball.BallReadyToLaunch           -= BanPlatformToFire;
    }


    #endregion

    #region ~ Methods | Primary ~

    private IEnumerator Crt_ScaleSmoothChange (float targetScale, bool backToDefaultScale)
    {
        if   (targetScale > transform.localScale.x) scalingFactor = 2;
        else                                        scalingFactor = 0;

        if (transform.localScale.x < targetScale)
        {
            while (transform.localScale.x < targetScale)
            {
                transform.localScale += new Vector3(0.01f, 0, 0) * speedOfScaleChange * Time.deltaTime;
                yield return null;
            }
        }

        else
        {
            while (transform.localScale.x > targetScale)
            {
                transform.localScale -= new Vector3(0.01f, 0, 0) * speedOfScaleChange * Time.deltaTime;
                yield return null;
            }
        }

        if (backToDefaultScale)
        {
            yield return new WaitForSeconds(6.0f);
            targetScale = PLATFORM_DEFAULT_SCALE;

            if (transform.localScale.x < targetScale)
            {
                while (transform.localScale.x < targetScale)
                {
                    transform.localScale += new Vector3(0.01f, 0, 0) * speedOfScaleChange * Time.deltaTime;
                    yield return null;
                }
            }

            else
            {
                while (transform.localScale.x > targetScale)
                {
                    transform.localScale -= new Vector3(0.01f, 0, 0) * speedOfScaleChange * Time.deltaTime;
                    yield return null;
                }
            }
        }

        transform.localScale = new Vector3(targetScale, transform.localScale.y, transform.localScale.z);
        scalingFactor = 1;
    }
    private void        Fire                  ()
    {
        if (magicNumber == 0) return;

        magicNumber--;
        Instantiate(firePrefab, transform.position - new Vector3(.0f, .65f, .0f), Quaternion.identity);

        MagicFired();
    }
    private void        SurpriseRoll          ()
    {
        int randomPool = 4;

        if (GameManager.NumberOfBalls > 1) --randomPool;

        int randomNumber = Random.Range(0, randomPool);
        
        if (scalingFactor == 2 && randomNumber == 2)
        {
            do
            {
                randomNumber = Random.Range(0, randomPool);
            } while (randomNumber == 2);
        }

        if (scalingFactor == 0 && randomNumber == 0)
        {
            do
            {
                randomNumber = Random.Range(0, randomPool);
            } while (randomNumber == 0);
        }

        switch (randomNumber)
        {
            case 0:
                //Debug.Log("GetLittle!");
                StopAllCoroutines();
                StartCoroutine(Crt_ScaleSmoothChange(.2f, true));
                break;
            case 1:
                //Debug.Log("Emptiness detected");
                break;
            case 2:
                //Debug.Log("GetBig!");
                StopAllCoroutines();
                StartCoroutine(Crt_ScaleSmoothChange(.6f, true));
                break;
            case 3:
                //Debug.Log("GetBalls!");
                BonusBalls();
                break;
        }
    }

    #endregion

    #region ~ Methods | Secondary ~

    private void BanPlatformToFire ()
    {
        platformReadyToFire = false;
    }
    private void CashMagicNumber   (int level, float levelStartDelay, int hearts, int magic)
    {
        magicNumber = magic;
    }


    #endregion


    #region ~ Collisions ~

    private void OnTriggerEnter2D   (Collider2D  collision)
    {
        if (collision.gameObject.layer == 17) return;

        if (collision.gameObject.layer == 12)
        {
            ItemPickedUp("Coin");
        }
        if (collision.gameObject.layer == 13)
        {
            ItemPickedUp("Heart");
        }
        if (collision.gameObject.layer == 14)
        {
            if (magicNumber < 5) magicNumber++;
            ItemPickedUp("Magic");
        }
        if (collision.gameObject.layer == 19)
        {
            SurpriseRoll();
        }
        
        SoundManager.instance.PlaySingle(itemPickedUpSfx);
    }
    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        if (input > 0)  canMoveToRight = false;
        if (input < 0 ) canMoveToLeft  = false;
    }
    private void OnCollisionExit2D  (Collision2D collision)
    {
        if (collision.gameObject.layer == 8) return;

        canMoveToLeft  = true;
        canMoveToRight = true;
    }

    #endregion
}
