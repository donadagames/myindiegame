using System;
using UnityEngine;

public class Status : MonoBehaviour
{
    public static Status instance;
    public bool isSafeZone = true;

    public float currentHealth;
    public float currentEnergy;
    public float currentExperience;

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

    public event EventHandler<OnSafeZoneChangeEventHandler> OnSafeZoneChange;

    public class OnSafeZoneChangeEventHandler : EventArgs
    {
        public bool _isSafeZone;
    }

    public void ChangeSafeZone(bool _isSafeZone)
    { 
        isSafeZone = _isSafeZone;
        OnSafeZoneChange?.Invoke(this, new OnSafeZoneChangeEventHandler { _isSafeZone = isSafeZone});
    }
}
