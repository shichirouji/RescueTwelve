using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallBonus : Ball
{
    #region ~ Methods | MonoBehaviour ~

    protected internal override void Start       ()
    {
        base.arrow.SetActive(false);

        ballMoving = true;

        transform.localScale = new Vector3(.01f, .01f, .01f);
        StartCoroutine(Crt_SmoothAppearance());
    }
    protected internal override void Update      ()
    {
        CheckOutOfScreen();
    }
    protected internal override void FixedUpdate ()
    {
        CheckVelocity();
    }

    #endregion

    #region ~ Methods | Secondary ~

    protected internal override IEnumerator Crt_SmoothAppearance()
    {
        while (transform.localScale.x < 3.0f)
        {
            transform.localScale += new Vector3(.05f, .05f, 05f) * Time.deltaTime * speedOfAppearance;
            yield return null;
        }
        transform.localScale = new Vector3(3.0f, 3.0f, 3.0f);

        rb.velocity = (Random.insideUnitCircle).normalized * ballSpeed;
        ballMoving = true;
    }

    #endregion
}
