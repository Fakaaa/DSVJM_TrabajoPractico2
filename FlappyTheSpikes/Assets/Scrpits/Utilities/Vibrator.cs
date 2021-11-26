using UnityEngine;
using MonoBehaviourSingletonScript;

public class Vibrator : MonoBehaviourSingleton<Vibrator>
{
    private bool vibratorEnable;
    public bool IsEnable
    {
        get
        {
            return vibratorEnable;
        }
        set
        {
            vibratorEnable = value;
        }
    }

    public void SwitchVibratorState()
    {
        IsEnable = !IsEnable;
    }

    public void MakeVibe()
    {
        if (IsEnable)
            Handheld.Vibrate();
        else
            Debug.Log("Vibrator disable! If you want feel the power activate the vibrator!");
    }
}
