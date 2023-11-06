using UnityEngine;

public class Diamond : MonoBehaviour
{
    public GameObject buff;
    public Item item;
    public AudioClip clip;
    private void Start()
    {
        gameObject.LeanScale(new Vector3(.25f, .25f, .25f), 1f).setLoopPingPong();
    }

    public void Interact(Inventory inventory, Player player)
    {
        inventory.AddItem(item, 1);
        player.soundController.PlayClip(clip);
        Instantiate(buff, player.transform.position, Quaternion.identity, player.transform);
    }

}

