using UnityEngine;

public class BreathfulThing : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private bool  isPulsing         = true;
    [SerializeField] private float pulsingSpeed      = 5.0f;
    [SerializeField] private float pulsingAmplitude  = .005f;

    [SerializeField] private bool  isFloating        = false;
    [SerializeField] private float floatingSpeed     = 1.0f;
    [SerializeField] private float floatingAmplitude = .01f;

    [SerializeField] private bool  isRotating        = false;
    [SerializeField] private float rotatingSpeed     = 1.0f;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Update ()
    {
        if (isPulsing)  transform.localScale += transform.localScale * Mathf.Sin(Time.time * pulsingSpeed) * pulsingAmplitude;

        if (isFloating) transform.position = new Vector3(transform.position.x, transform.position.y + transform.position.y * Mathf.Sin(Time.time * floatingSpeed) * floatingAmplitude, transform.position.z);

        if (isRotating) transform.Rotate(Vector2.up * rotatingSpeed * Time.deltaTime);
    }
    
    #endregion

    #region ~ Methods | Primary ~

    // Custom Primary Function...

    #endregion

}
