using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageControll : MonoBehaviour
{
    #region Singleton
    public static LanguageControll instance;
    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(this);
    }
    #endregion

    public bool isPortuguese = false;

    public void OnEnglishButtonPressed()
    {
        isPortuguese = false;
        OnLanguageUpdate?.Invoke(this, new OnLanguageUpdateEventHandler { _isPortuguese = isPortuguese });

    }

    public void OnPortugueseButtonPressed()
    {
        isPortuguese = true;
        OnLanguageUpdate?.Invoke(this, new OnLanguageUpdateEventHandler { _isPortuguese = isPortuguese});
    }

    public event EventHandler<OnLanguageUpdateEventHandler> OnLanguageUpdate;

    public class OnLanguageUpdateEventHandler : EventArgs
    {
        public bool _isPortuguese;
    }
}
