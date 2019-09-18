using UnityEngine;

public class Gem : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private Sprite[] spriteCollection;

    // Private:
    private SpriteRenderer sr;

    // Const & Static:
    //

    // Delegates & Events:
    public delegate void  GemDelegate (Vector2 position);
    public static   event GemDelegate GemDestroy;

    #endregion
    

    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start ()
    {
        sr.sprite = spriteCollection[Random.Range(0, spriteCollection.Length)];
    }

    #endregion

    #region ~ Methods | Primary ~

    private void GetDamage()
    {
        GemDestroy((Vector2)transform.position);
        Destroy(gameObject);
    }

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

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

    // Custom Property...

    #endregion
}
