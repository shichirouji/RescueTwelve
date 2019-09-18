using UnityEngine;

public class Ship : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:

    // Private:
    private float shipSpeed;

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion


    #region ~ Methods | MonoBehaviour ~

    /// Awake()
    private void Start()
    {
        shipSpeed = Random.Range(.5f, 2.5f);
    }
    private void Update()
    {
        transform.Translate((Vector3.right + Vector3.up * Mathf.Sin(Time.time * 6) * (.1f * Random.Range(1, 6))) * shipSpeed * Time.deltaTime);

        if (transform.position.x > 8.0f)
        {
            gameObject.GetComponent<ExplodingThing>().SetExplosionPrefab = null;

            Destroy(gameObject);
        }

    }
    /// FixedUpdate()
    /// LateUpdate()
    private void OnEnable()
    {
        BoardManager.LevelComplete += Destroy;
        GameManager.GameOver       += Destroy;
    }
    private void OnDisable()
    {
        BoardManager.LevelComplete -= Destroy;
        GameManager.GameOver       -= Destroy;
    }

    #endregion

    #region ~ Methods | Primary ~

    private void Destroy ()
    {
        Destroy(gameObject);
    }
    private void Destroy (int level)
    {
        Destroy(gameObject);
    }

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

    #endregion


    #region ~ Collisions ~

    private void OnCollisionEnter2D (Collision2D collision)
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D   (Collider2D  collision)
    {
        if (collision.gameObject.layer == 17)
        {
            Destroy(gameObject);
        }
    }

    #endregion


    #region ~ Properties ~

    // Custom Property...

    #endregion
}
