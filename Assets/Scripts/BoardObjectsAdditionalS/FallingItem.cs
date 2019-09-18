using UnityEngine;

public class FallingItem : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private bool isRandomSpeed       = false;
    [SerializeField] private int  constSpeed          = 1;
    [SerializeField] private int  rangeRandomMinSpeed = 1;
    [SerializeField] private int  rangeRandomMaxSpeed = 3;

    // Private:
    private int randomSpeed;

    // Delegates & Events:
    public delegate void  FallingItemDelegate ();
    public static   event FallingItemDelegate AddItemToObjectsToDestroy;
    public static   event FallingItemDelegate RemoveItemToObjectsToDestroy;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Start     ()
    {
        AddItemToObjectsToDestroy();

        randomSpeed = Random.Range(rangeRandomMinSpeed, rangeRandomMaxSpeed + 1);
    }
    private void Update    ()
    {
        if   (isRandomSpeed) transform.Translate(Vector3.down * randomSpeed * Time.deltaTime);
        else                 transform.Translate(Vector3.down * constSpeed  * Time.deltaTime);
    }
    private void OnDestroy ()
    {
        try { RemoveItemToObjectsToDestroy(); } catch { Debug.Log("It's All Okay With Falling THINGS."); }
    }

    #endregion

}
