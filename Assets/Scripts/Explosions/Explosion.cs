using UnityEngine;


public class Explosion : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private AudioClip[] explosionSfx;

    // Private:
    private Animator anim;

    // Const & Static:
    //

    // Delegates & Events:
    //

    #endregion



    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        anim = GetComponent<Animator>();
    }
    private void Start ()
    {
        InitRandomExplosion();

        SoundManager.instance.PlaySingle(explosionSfx[Random.Range(0, explosionSfx.Length)]);

        Destroy(gameObject, 5.0f);
    }


    #endregion

    #region ~ Methods | Primary ~

    private void InitRandomExplosion ()
    {
        float randomExplosionSize = Random.Range(2.0f, 4.0f);
        transform.localScale *= randomExplosionSize;

        int randomExplosion = Random.Range(1, 6);

        switch (randomExplosion)
        {
            case 1:
                anim.SetTrigger("Explosion01");
                break;
            case 2:
                anim.SetTrigger("Explosion02");
                break;
            case 3:
                anim.SetTrigger("Explosion03");
                break;
            case 4:
                anim.SetTrigger("Explosion04");
                break;
            case 5:
                anim.SetTrigger("Explosion05");
                break;
        }
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

    // Custom Property...

    #endregion
}
