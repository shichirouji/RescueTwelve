using UnityEngine;

public class PlatformTilt : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [Header("~ iN$SCRIBED ~")]
    [SerializeField] private int   degrees = 10;
    [SerializeField] private float speed   = 40.0f;

    // Private:
    private float   input;
    private Vector3 getBack;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Update ()
    {
        SimpleTilt();
    }

    #endregion

    #region ~ Methods | Primary ~

    private void SimpleTilt ()
    {
        input = Input.GetAxisRaw("Horizontal");

        if (input > 0)
        {
            if (transform.rotation.z > -degrees * Mathf.Deg2Rad)
            {
                transform.Rotate(-Vector3.forward, Time.deltaTime * speed);
                getBack = Vector3.forward;
                return;
            }

        }
        else
        if (input < 0)
        {
            if (transform.rotation.z < degrees * Mathf.Deg2Rad)
            {
                transform.Rotate(Vector3.forward, Time.deltaTime * speed);
                getBack = -Vector3.forward;
                return;
            }

        }
        else
        if (input == 0)
        {
            if (getBack == Vector3.forward) if (transform.rotation.z > .0f) return;
            if (getBack == -Vector3.forward) if (transform.rotation.z < .0f) return;
            transform.Rotate(getBack, Time.deltaTime * speed);
        }
    }

    #endregion

    #region ~ Methods | Secondary ~

    // Custom Secondary Function...

    #endregion
}
