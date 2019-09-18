using UnityEngine;

public class OuterBlock : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:

    // Private:
    private SpriteRenderer sr;
    private Vector3 randomDirection;

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        int randomNumber = Random.Range(1, 3);
        if (randomNumber == 1) randomDirection = Vector3.forward;
        else randomDirection = -Vector3.forward;
    }
    private void Update()
    {
        transform.RotateAroundLocal(randomDirection * Mathf.Cos(Time.time), .05f * Time.deltaTime);
    }
    /// FixedUpdate()
    /// LateUpdate()
    /// OnDisable()
    private void OnEnable()
    {
        sr.color = Color.white;
        gameObject.transform.rotation = Quaternion.identity;
    }

    #endregion

    #region ~ Methods | Primary ~

    // Custom Primary Function...

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

    #endregion

        
    #region ~ Collisions ~

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.layer == 15) return;

        sr.color = ColorCollection.colors[Random.Range(0, ColorCollection.colors.Length)];
    }
    private void OnTriggerEnter2D   (Collider2D collision)
    {
        //Destroy(collision.gameObject);
    }

    #endregion


    #region ~ Properties ~

    // Custom Property...

    #endregion
}
