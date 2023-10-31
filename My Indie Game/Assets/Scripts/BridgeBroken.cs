using UnityEngine;

public class BridgeBroken : MonoBehaviour
{
    public Transform[] tabuas;
    int tabuasCount = -1;
    int tabuasId;

    int tabuasIdleft;
    int tabuasIdright;

    int tabuasCountleft = 0;
    int tabuasCountright = 0;

    public Transform[] rightWoods;
    public Transform[] leftWoods;

    public GameObject _collider;

    public void BuildBridge()
    {
        DealWoods();
    }

    private void DealWoods()
    {
        tabuasCount++;
        if (tabuasCount >= tabuas.Length)
        {
            LeanTween.cancel(tabuasId);
            _collider.SetActive(true);
            leftWoods[tabuasCountleft].gameObject.SetActive(true);
            rightWoods[tabuasCountright].gameObject.SetActive(true);
            tabuasIdright = rightWoods[tabuasCountright].LeanMoveLocalZ(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete(DealRightWoods).id;
            tabuasIdleft = leftWoods[tabuasCountleft].LeanMoveLocalZ(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete(DealLeftWoods).id;

            return;
        }

        else
        {
            tabuas[tabuasCount].gameObject.SetActive(true);
            tabuas[tabuasCount].LeanMoveLocalZ(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete(DealWoods);
        }
    }

    private void DealLeftWoods()
    {
        tabuasCountleft++;
        if (tabuasCountleft >= leftWoods.Length)
        {
            LeanTween.cancel(tabuasIdleft);
            return;
        }

        else
        {
            leftWoods[tabuasCountleft].gameObject.SetActive(true);
            leftWoods[tabuasCountleft].LeanMoveLocalZ(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete(DealLeftWoods);
        }
    }

    private void DealRightWoods()
    {
        tabuasCountright++;
        if (tabuasCountright >= rightWoods.Length)
        {
            LeanTween.cancel(tabuasIdright);
            return;
        }

        else
        {
            rightWoods[tabuasCountright].gameObject.SetActive(true);
            rightWoods[tabuasCountright].LeanMoveLocalZ(0, .25f).setEase(LeanTweenType.easeOutBack).setOnComplete(DealRightWoods);
        }
    }
}
