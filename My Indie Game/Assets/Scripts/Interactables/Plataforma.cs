using UnityEngine;

public class Plataforma : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.input.platform = transform;
            player.input.isOnPlatform = true;
            player.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponent<Player>();

        if (player != null)
        {
            player.input.platform = null;
            player.input.isOnPlatform = false;
            player.transform.SetParent(null);
        }
    }
}
