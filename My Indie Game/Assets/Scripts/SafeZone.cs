using UnityEngine;

public class SafeZone : MonoBehaviour
{
    Status status;
    bool check;

    private void Start()
    {
        status = Status.instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>() && check == true)
        {
            check = false;
            status.ChangeSafeZone(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>() && check == false)
        {
            check = true;
            status.ChangeSafeZone(false);
        }
    }
}
