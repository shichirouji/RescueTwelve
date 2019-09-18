using UnityEngine;

public class Heart : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private GameObject itemExplosion;

    // Private:
    //

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Update()
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
