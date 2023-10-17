using TMPro;
using UnityEngine;

public class InfoText : MonoBehaviour
{
    public TextMeshProUGUI text;
    [SerializeField] CanvasGroup canvasGroup;

    private float position = 200f;

    private void Start()
    {
        var value = LeanTween.value(1, 0, 2.4f).setOnUpdate((value) => canvasGroup.alpha = value);//.setOnComplete(() => transform.localPosition = new Vector3(150, 65, 0));
        transform.LeanMoveLocalY(position, 2.5f).setOnComplete(() => Destroy(gameObject));
    }

    public void SetText(string _text, float _position)
    {
        text.text = _text;
        position = _position;
    }
}
