using UnityEngine;

public class PlayerGotScore : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    // 

    // Private:
    //

    // Const & Static:
    //

    // Delegates & Events:
    public delegate void  PlayerGotScoreDelegate (int scoreAmount);
    public static event   PlayerGotScoreDelegate AddScore;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    /// Awake()
    /// Start()
    /// Update()
    /// FixedUpdate()
    /// LateUpdate()
    /// OnDisable()
    /// OnEnable()

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
        if (gameObject.layer == 09) return;

        int maxScore = 0;
        switch (collision.gameObject.layer)
        {
            case 8:
                maxScore = 300;
                break;
            case 9:
                maxScore = 50;
                break;
            case 10:
                maxScore = 100;
                break;
            case 11:
                maxScore = 150;
                break;
            case 15:
                maxScore = 50;
                break;
            case 16:
                maxScore = 500;
                break;
            case 18:
                maxScore = 50;
                break;
            case 20:
                maxScore = 200;
                break;
        }
        AddScore(Random.Range(1, maxScore));
    }
    private void OnTriggerEnter2D   (Collider2D  collision)
    {
        if (gameObject.layer == 08) return;

        int maxScore = 0;
        switch (collision.gameObject.layer)
        {
            case 12:
                maxScore = 100;
                AddScore(Random.Range(1, maxScore) * collision.gameObject.GetComponent<Coin>().scoreMultiplier);
                break;
        }
    }

    #endregion


    #region ~ Properties ~

    // Custom Property...

    #endregion
}
