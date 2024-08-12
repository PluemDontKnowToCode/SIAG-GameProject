using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : Singleton<StageManager>
{
    [SerializeField] AudioSource currentBGM;
    [SerializeField] SpriteRenderer backgroundImage;
    Stage _cureentStage;
    public Stage currentStage
    {
        get
        {
            return _cureentStage;
        }
        set
        {
            _cureentStage = value;
            currentBGM.clip = value.BGM;
            backgroundImage.sprite = value.background;

        }
    }
    
}