using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PanelAnimations : MonoBehaviour
{
    [System.Serializable]
    public enum AnimationType
    {
        ScaleApear,
        FromLeft,
        FromRight
    }
    #region EXPOSED VALUES
    [SerializeField] AnimationType typeAnim;
    [SerializeField] GameObject parentHolder;
    #endregion

    #region PRIVATE VALUES
    CanvasGroup canvasGrp;
    Animator animator;
    #endregion

    void Start()
    {
        canvasGrp = parentHolder.GetComponent<CanvasGroup>();
        canvasGrp.alpha = 0;
        canvasGrp.interactable = false;

        IEnumerator WaitUntilAnimatorRefFinded()
        {
            bool okAnimator = false;
            while (!okAnimator)
            {
                okAnimator = TryGetComponent<Animator>(out animator);
                yield return new WaitForEndOfFrame();
            }
            ExcuteCloseAnimation(0);
            yield break;
        }
        StartCoroutine(WaitUntilAnimatorRefFinded());
    }

    public void ExcuteOpenAnimation()
    {
        if (!parentHolder.activeSelf)
        {
            parentHolder.SetActive(true);            
        }

        animator.SetBool("InteedClose", false);
        animator.SetBool("InteedOpen", true);

        switch (typeAnim)
        {
            case AnimationType.ScaleApear:
                animator.SetFloat("Opens", 1f);
                break;
            case AnimationType.FromLeft:
                animator.SetFloat("Opens", 2f);
                break;
            case AnimationType.FromRight:
                animator.SetFloat("Opens", 3f);
                break;
        }

        IEnumerator WaitSecondsToAlpha()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            canvasGrp.alpha = 1;
            canvasGrp.interactable = true;
            yield break;
        }
        StartCoroutine(WaitSecondsToAlpha());
    }

    public void ExcuteCloseAnimation(float secondsUntilReset)
    {
        animator.SetBool("InteedOpen", false);
        animator.SetBool("InteedClose", true);

        switch (typeAnim)
        {
            case AnimationType.ScaleApear:
                animator.SetFloat("Closes", 1f);
                break;
            case AnimationType.FromLeft:
                animator.SetFloat("Closes", 2f);
                break;
            case AnimationType.FromRight:
                animator.SetFloat("Closes", 3f);
                break;
        }

        StartCoroutine(WaitUnitilAnimationEnds(secondsUntilReset));
    }

    IEnumerator WaitUnitilAnimationEnds(float secondsToWait)
    {
        yield return new WaitForSeconds(secondsToWait);

        if (animator.GetBool("InteedClose") && !animator.GetBool("InteedOpen"))
        {
            if (parentHolder.activeSelf)
                parentHolder.SetActive(false);
        }

        yield break;
    }
}
