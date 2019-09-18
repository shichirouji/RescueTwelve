using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    // 

    // Private:


    // Const & Static:
    private static int score;

    // Delegates & Events:
    public delegate void   ScoreManagerDelegate (int scoreNumber);
    public static   event  ScoreManagerDelegate UpdateStatistic;


    #endregion


    #region ~ Methods | MonoBehaviour ~

    /// Awake()
    private void Start()
    {
        score = 0;
    }
    /// Update()
    /// FixedUpdate()
    /// LateUpdate()
    private void OnEnable()
    {
        PlayerGotScore.AddScore += IncreaseScore;
    }
    /// OnEnable()

    #endregion

    #region ~ Methods | Primary ~

    private void IncreaseScore (int scoreAmount)
    {
        score += scoreAmount;
        UpdateStatistic(score);
    }

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

    public static int Score
    {
        get         { return score; }
        private set { score = value; }
    }

    #endregion
}
