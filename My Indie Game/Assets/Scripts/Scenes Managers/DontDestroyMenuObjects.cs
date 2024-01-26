using UnityEngine;

public class DontDestroyMenuObjects : MonoBehaviour
{
    public static DontDestroyMenuObjects instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(this);
    }
}
