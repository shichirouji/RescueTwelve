using UnityEngine;
using UnityEngine.UI;

public class StrobText : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private float strobRate = .5f;

    // Private:
    private Text   text;
    private string cashed;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake ()
    {
        text = GetComponent<Text>();
    }
    private void Start ()
    {
        InvokeRepeating("Strob", .0f, strobRate);
    }

    #endregion

    #region ~ Methods | Primary ~

    private void Strob ()
    {
        if (text.text != "") cashed = text.text;
        if (text.text == cashed) text.text = ""; else text.text = cashed;
    }

    #endregion
}
