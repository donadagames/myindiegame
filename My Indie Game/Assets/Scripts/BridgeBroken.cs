using UnityEngine;

public class BridgeBroken : SavableEntity
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
        shouldSave = true;
        SaveSystem.instance.SaveEntities();
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

    public override void RestoreState(SerializableSavableEntity savable)
    {
        _collider.SetActive(true);
        shouldSave = true;
        foreach (var tabua in tabuas)
        { 
            tabua.gameObject.SetActive(true);
            tabua.transform.localPosition = new Vector3(0,0,0);
        }

        foreach (var tabua in leftWoods)
        {
            tabua.gameObject.SetActive(true);
            tabua.transform.localPosition = new Vector3(0, 0, 0);
        }

        foreach (var tabua in rightWoods)
        {
            tabua.gameObject.SetActive(true);
            tabua.transform.localPosition = new Vector3(0, 0, 0);
        }
    }
}

