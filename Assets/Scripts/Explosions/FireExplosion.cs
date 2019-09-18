using UnityEngine;

public class FireExplosion : MonoBehaviour
{
    #region ~ Fields ~

    [SerializeField] private AudioClip[] explosionSfx;

    private Animator anim;
    private SpriteRenderer sr;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sr   = GetComponent<SpriteRenderer>();
    }
    private void Start()
    {
        transform.localScale += Vector3.one * Random.Range(.0f, .5f);
        sr.color = ColorCollection.colors[Random.Range(0, ColorCollection.colors.Length)];

        anim.SetTrigger("FireExplosion");

        SoundManager.instance.PlaySingle(explosionSfx[Random.Range(0, explosionSfx.Length)]);

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
