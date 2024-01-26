using UnityEngine;

public class EnterDoorController : MonoBehaviour
{
    [SerializeField] EnterDoorController otherDoor;
    [SerializeField] bool isInsideDoor;
    Status status;
    public bool canInteract = true;

    private void Start()
    {
        status = Status.instance;
        canInteract = true;
    }


    private void OnTriggerEnter(Collider other)
    {

        if (canInteract == false) return;

        else
        {
            canInteract = false;
            otherDoor.canInteract = true;

            if (isInsideDoor)
            {
                status.uiController.SetInsideHouseCamera();
            }
            else
            {
                status.uiController.SetOutsideHouseCamera();
            }
        }
    }
}
