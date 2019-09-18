using UnityEngine;

public class Fire : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private Vector2 direction = Vector2.up;
    [SerializeField] private float   fireSpeed = 6.0f;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Update ()
    {
        transform.Translate(direction * fireSpeed * Time.deltaTime);
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
        int[] returnLayers = { 08, 09, 12, 13, 14, 19 };
        foreach (var layer in returnLayers) if (layer == collision.gameObject.layer) return;

        Destroy(gameObject);
    }
    /// OnTriggerStay()
    /// OnTriggerExit()

    #endregion


    #region ~ Properties ~

    // Custom Property...

    #endregion
}
