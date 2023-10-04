using System.Collections;
using System.Collections.Generic;
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
        parentSlot = GetComponentInParent<InventorySlot>();
        image = GetComponentInParent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        changebleParentTransform = parentTransform;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
        parentSlot.placeHolderIcon.enabled = true;
        image.raycastTarget = false;

        if (actionbarSlot != null)
        {
            actionbarSlot.quantity.text = string.Empty; 
        }
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

            if (actionbarSlot != null)
            {
                actionbarSlot.CleanSlot();
                actionbarSlot = null;
            }
        }
    }
}
