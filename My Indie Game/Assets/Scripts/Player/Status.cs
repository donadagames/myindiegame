using UnityEngine;

public class Status : MonoBehaviour
{
    public static Status instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }

    public Player player;
    public UIController uiController;

    private void Start()
    {
        uiController = UIController.instance;
    }
}
