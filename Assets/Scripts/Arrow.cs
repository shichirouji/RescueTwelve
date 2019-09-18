using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private float rotateSpeed = 100.0f;
    [SerializeField] private float strobSpeed  = .1f;

    // Private:
    private SpriteRenderer sr;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Awake  ()
    {
        sr = GetComponentInChildren<SpriteRenderer>();
    }
    private void Start  ()
    {
        InvokeRepeating("Strob", 0, strobSpeed);
    }
    private void Update ()
    {
        transform.Rotate(Vector3.back * rotateSpeed * Time.deltaTime);
    }

    #endregion

    #region ~ Methods | Primary ~

    private void Strob()
    {
        if (sr.enabled) sr.enabled = false; else sr.enabled = true;
    }

    #endregion
}
