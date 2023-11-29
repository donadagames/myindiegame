using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DragableItem : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    public Transform parentTransform;
    public Transform changebleParentTransform;
    InputHandler input;
    public InventorySlot parentSlot;
    public Image image;
    public ActionBarSlot actionbarSlot;

    private void Start()
    {
        input = InputHandler.instance;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        changebleParentTransform = parentTransform;
        transform.SetParent(parentSlot.inventory.draggingParent);
        transform.SetAsLastSibling();
        parentSlot.placeHolderIcon.enabled = true;
        image.raycastTarget = false;

        if (actionbarSlot != null)
        {
            actionbarSlot.quantity.text = string.Empty;
        }

        parentSlot.OnSlotPressed();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var dragPosition = input.playerInputActions.Player.ScreenPos.ReadValue<Vector2>();
        var newPosition = new Vector3(dragPosition.x, dragPosition.y, 0);

        transform.position = newPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (changebleParentTransform == parentTransform)
        {

            transform.SetParent(changebleParentTransform);
            transform.localPosition = new Vector3(0, 0, 0);
            image.raycastTarget = true;
            transform.localScale = new Vector3(0, 0, 0);
            transform.LeanScale(new Vector3(.75f, .75f, .1f), .15f);

            if (actionbarSlot != null)
            {
                actionbarSlot.CleanSlot();
                actionbarSlot = null;
            }
        }
    }

    public void RestoreOnLoad()
    {
        //changebleParentTransform = parentTransform;
        parentSlot.placeHolderIcon.enabled = true;
    }
}
