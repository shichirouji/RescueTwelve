using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class OuterBlocks : MonoBehaviour
{
    #region ~ Fields ~

    // Public & Serialize:
    [SerializeField] private GameObject[] upBlocks;
    [SerializeField] private GameObject[] leftBlocks;
    [SerializeField] private GameObject[] rightBlocks;

    [SerializeField] private float speedShowing = .1f;

    #endregion


    #region ~ Methods | MonoBehaviour ~

    private void Start     ()
    {
        for (var i = 0; i < upBlocks.Length; i++)
        {
            upBlocks[i].SetActive(false);
            if (i < 10)
            {
                leftBlocks[i].SetActive(false);
                rightBlocks[i].SetActive(false);
            }
        }
    }
    private void OnEnable  ()
    {
        GameManager.GameOver       += RemoveBlocks;
        BoardManager.LevelComplete += RemoveBlocks;
        BoardManager.GameComplete  += RemoveBlocks;
        CanvasScript.UIReady       += InitBlocks;
    }
    private void OnDisable ()
    {
        GameManager.GameOver       -= RemoveBlocks;
        BoardManager.LevelComplete -= RemoveBlocks;
        BoardManager.GameComplete  -= RemoveBlocks;
        CanvasScript.UIReady       -= InitBlocks;
    }

    #endregion

    #region ~ Methods | Primary ~

    private void        InitBlocks        ()
    {
        StartCoroutine(Crt_OperateBlocks(true));
    }
    private void        RemoveBlocks      ()
    {
        StartCoroutine(Crt_OperateBlocks(false));
    }
    private void        RemoveBlocks      (int level)
    {
        StartCoroutine(Crt_OperateBlocks(false));
    }
    private IEnumerator Crt_OperateBlocks (bool operate)
    {
        for (int i = 5, j = 6; i >= 0; i--, j++) {
            upBlocks[i].SetActive(operate);
            upBlocks[j].SetActive(operate);
            yield return new WaitForSeconds(speedShowing);
        }
        for (int i = 0; i < leftBlocks.Length; i++)
        {
            leftBlocks[i].SetActive(operate);
            rightBlocks[i].SetActive(operate);
            yield return new WaitForSeconds(speedShowing);
        }
    }

    #endregion
}
