using UnityEngine;

public class ItemExplosion : MonoBehaviour
{
    #region ~ Fields ~

    public SpriteRenderer sr;

    private Animator anim;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        anim = GetComponent<Animator>();
        sr   = GetComponent<SpriteRenderer>();
    }
    private void Start ()
    {
        int colorIndex = 999;

        Coin     coinScript     = GetComponentInChildren<Coin>();
        Surprise surpriseScript = GetComponentInChildren<Surprise>();
        Magic    magicScript    = GetComponentInChildren<Magic>();
        Heart    heartScript    = GetComponentInChildren<Heart>();

        if (coinScript     != null) colorIndex = coinScript.scoreMultiplier;
        if (surpriseScript != null) colorIndex = 1;
        if (magicScript    != null) colorIndex = 3;
        if (heartScript    != null) colorIndex = 3;

        switch (colorIndex)
        {
            case 1:
                sr.color = Color.yellow;
                break;
            case 2:
                sr.color = Color.blue;
                break;
            case 3:
                sr.color = Color.red;
                break;
            case 4:
                sr.color = Color.magenta;
                break;
        }

        anim.SetTrigger("ItemExplosion");
        Destroy(gameObject, 5.0f);
    }
    

    #endregion

    #region ~ Methods | Primary ~

    // Custom Primary Function...

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

    #endregion


    #region ~ Collisions ~

    /// OnCollisionEnter()
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
