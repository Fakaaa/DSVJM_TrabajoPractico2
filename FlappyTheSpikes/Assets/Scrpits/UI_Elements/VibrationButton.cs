using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VibrationButton : MonoBehaviour
{
    [SerializeField] Image vibrationImage;
    [SerializeField] Color enableCol;
    [SerializeField] Color disableCol;

    GameManager gm;

    void Start()
    {
        gm = GameManager.Instance;

        if(gm)
        {
            gm.OnVibrationChangeState += VibrationButtonChange;
        }
    }

    private void OnDisable()
    {
        if (gm)
        {
            gm.OnVibrationChangeState -= VibrationButtonChange;
        }
    }

    void VibrationButtonChange()
    {
        if (Vibrator.Instance == null)
            return;

        if(Vibrator.Instance.IsEnable)
        {
            vibrationImage.color = enableCol;
        }
        else
        {
            vibrationImage.color = disableCol;
        }
    }

}
