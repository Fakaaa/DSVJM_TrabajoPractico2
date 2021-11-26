using UnityEngine;
using UnityEngine.EventSystems;
using AudioManagerScript;

public class ButtonSoundTriggerer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [SerializeField] public string soundHover;
    [SerializeField] public string soundClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        AudioManager.Instance.Play(soundClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AudioManager.Instance.Play(soundHover);
    }

}