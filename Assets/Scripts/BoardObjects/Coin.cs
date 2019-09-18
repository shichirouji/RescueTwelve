using UnityEngine;

public class Coin : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private Sprite[] spriteCollection;
    [SerializeField] private GameObject itemExplosion;
    public int scoreMultiplier;

    // Private:
    private SpriteRenderer sr;

    #endregion



    #region ~ Methods | MonoBehaviour ~

    private void Awake  ()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start  ()
    {
        int spriteIndex;
        int randomNumber = Random.Range(1, 11);

        if      (randomNumber == 1) spriteIndex = 2;
        else if (randomNumber <= 4) spriteIndex = 1;
        else                        spriteIndex = 0;

        scoreMultiplier = spriteIndex + 1;

        sr.sprite = spriteCollection[spriteIndex];
    }
    private void Update ()
    {
        if (transform.position.y < -5.25f) Destroy(gameObject);
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
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != 09) return;

        GameObject explosion = Instantiate(itemExplosion, transform.position, Quaternion.identity);
        gameObject.transform.SetParent(explosion.transform);

        Destroy(gameObject);
    }
    /// OnTriggerStay()
    /// OnTriggerExit()

    #endregion



    #region ~ Properties ~

    // Custom Property...

    #endregion
}
