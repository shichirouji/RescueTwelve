using UnityEngine;

public class PushedBack : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private bool  xPush     = false;
    [SerializeField] private bool  yPush     = false;
    [SerializeField] private float pushSpeed = 1.0f;
    [SerializeField] private float xCoordinateOfPushing;
    [SerializeField] private float yCoordinateOfPushing;

    // Private:
    private bool  onePush = false;
    private float cashedXCoordinate;
    private float cashedYCoordinate;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Start  ()
    {
        cashedXCoordinate = transform.position.x;
        cashedYCoordinate = transform.position.y;
    }
    private void Update ()
    {
        if (xPush) xPushed();
        if (yPush) yPushed();
    }

    #endregion

    #region ~ Methods | Primary ~

    private void yPushed ()
    {
        if (onePush == true && !Mathf.Approximately(transform.position.y, yCoordinateOfPushing))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, yCoordinateOfPushing, transform.position.z), Time.deltaTime * pushSpeed);
        }
        else if (!Mathf.Approximately(transform.position.y, cashedYCoordinate))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(transform.position.x, cashedYCoordinate, transform.position.z), Time.deltaTime * pushSpeed);
            onePush = false;
        }
    }
    private void xPushed ()
    {
        if (onePush == true && !Mathf.Approximately(transform.position.x, xCoordinateOfPushing))
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(xCoordinateOfPushing, transform.position.y, transform.position.z), Time.deltaTime * pushSpeed);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(cashedXCoordinate, transform.position.y, transform.position.z), Time.deltaTime * pushSpeed);
            onePush = false;
        }
    }

    #endregion


    #region ~ Collisions ~

    private void OnCollisionEnter2D (Collision2D collision)
    {
        if (collision.gameObject.layer == 15) return;

        onePush = true;
    }

    #endregion
}
