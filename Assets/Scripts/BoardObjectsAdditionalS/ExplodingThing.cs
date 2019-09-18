using UnityEngine;
using UnityEngine.UI;

public class ExplodingThing : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private GameObject  explosionPrefab;

    #endregion
    

    #region ~ Methods | MonoBehaviour ~

    private void Update    () { /* GET HAPPY */ }
    private void OnDestroy ()
    {
        if (gameObject.layer == 17) Instantiate(explosionPrefab, transform.position + new Vector3(.0f, 1.0f, .0f), Quaternion.identity);

        else if (explosionPrefab != null) Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }

    #endregion

    #region ~ Properties ~

    public GameObject SetExplosionPrefab
    {
        set { explosionPrefab = value; }
    }

    #endregion
}
