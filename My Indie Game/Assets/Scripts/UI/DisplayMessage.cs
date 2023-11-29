using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayMessage : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] CanvasGroup canvasGroup;
    public UIController controller;

    private void Start()
    {
        canvasGroup.LeanAlpha(0, 3.5f).setOnComplete(OnCompleteText);

        transform.LeanMoveLocalY(52, 3.5f).setEaseInOutElastic();
    }

    private void OnCompleteText()
    {
        controller.canDisplayMessage = true;
        Destroy(this.gameObject);
    }
}
