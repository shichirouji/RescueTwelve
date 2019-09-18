using UnityEngine;

public class SoundManager : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioSource ballSource;

    public static SoundManager instance = null;

    [Header("Music")]
    [SerializeField] private AudioClip[] gamePlayMusic;
    [SerializeField] private AudioClip   levelComplete;
    [SerializeField] private AudioClip   gameComplete;
    [SerializeField] private AudioClip   gameOver;



    // Private:
    //

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        if (instance == null) instance = this;
        else
        {
            Debug.LogError("Attemp to instantiate second SoundManager");
            Debug.Break();
        }
    }
    /// Start()
    /// Update()
    /// FixedUpdate()
    /// LateUpdate()
    private void OnEnable()
    {
        GameManager.StartLevel     += StartLevel;
        GameManager.GameOver       += GameOver;
        BoardManager.LevelComplete += LevelComplete;
        BoardManager.GameComplete  += GameComplete;
    }
    private void OnDisable()
    {
        GameManager.StartLevel     -= StartLevel;
        GameManager.GameOver       -= GameOver;
        BoardManager.LevelComplete -= LevelComplete;
        BoardManager.GameComplete  -= GameComplete;
    }

    #endregion

    #region ~ Methods | Primary ~

    public  void PlaySingle     (AudioClip clip)
    {
        sfxSource.clip = clip;
        sfxSource.Play();
    }
    public  void PlaySingleBall (AudioClip clip)
    {
        ballSource.clip = clip;
        ballSource.pitch += Random.Range(-.05f, +.05f);
        ballSource.Play();
    }

    private void StartLevel     (int level, float levelStartDelay, int hearts, int magic)
    {
        PlayMusic(gamePlayMusic[Random.Range(0, gamePlayMusic.Length)], 1.0f, true);
    }
    private void LevelComplete  ()
    {
        PlayMusic(levelComplete, 1.0f, false);
    }
    private void GameOver       (int level)
    {
        PlayMusic(gameOver, 1.0f, false);
    }
    private void GameComplete   ()
    {
        PlayMusic(gameComplete, 1.0f, true);
    }

    #endregion

    #region ~ Methods | Secondary ~

    private void PlayMusic(AudioClip clip, float volume, bool isLoop)
    {
        musicSource.Stop();
        musicSource.clip = clip;
        musicSource.loop = isLoop;
        musicSource.volume = volume;
        musicSource.Play();
    }

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
