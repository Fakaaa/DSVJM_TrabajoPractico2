using UnityEngine;
using TMPro;

public class PostProcessSttng : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textOffOn;

    GameManager refInstance;

    void Start()
    {
        refInstance = GameManager.Instance;

        if(refInstance!= null)
        {
            refInstance.OnPostProccesOff += TextOff;
            refInstance.OnPostProccesOn += TextOn;
        }

        if(!refInstance.firstTimePostProscess)
            refInstance.ChangeStatePostProcces();
    }

    private void OnDestroy()
    {
        if (refInstance != null)
        {
            refInstance.OnPostProccesOff -= TextOff;
            refInstance.OnPostProccesOn -= TextOn;
        }
    }

    void TextOff()
    {
        textOffOn.text = "Off";
    }

    void TextOn()
    {
        textOffOn.text = "On";
    }
}
