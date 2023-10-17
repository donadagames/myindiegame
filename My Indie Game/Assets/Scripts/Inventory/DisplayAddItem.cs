using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAddItem : MonoBehaviour
{
    public Transform cam;
    public CanvasGroup canvasGroup;
    public TextMeshProUGUI quantity;
    public Image icon;

    private void Start()
    {
        transform.LeanScale(new Vector3(.75f, .75f, .75f), .25f);
        transform.LeanMoveLocalY(1.75f, 1f).setEase(LeanTweenType.easeOutElastic).
            setOnComplete(() => canvasGroup.LeanAlpha(0, 1f).setOnComplete(() => Destroy(this.gameObject)));

    }

    private void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
