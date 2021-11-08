using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class OpenAnimation : MonoBehaviour
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
    Animator animator;
    #endregion

    void Start()
    {
        IEnumerator WaitUntilAnimatorRefFinded()
        {
            bool okAnimator = false;
            while (!okAnimator)
            {
                okAnimator = TryGetComponent<Animator>(out animator);
                yield return new WaitForEndOfFrame();
            }
            ExcuteCloseAnimation();
            yield break;
        }
        StartCoroutine(WaitUntilAnimatorRefFinded());
    }

    void FixedUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !animator.IsInTransition(0))
        {
            if(animator.GetBool("InteedClose") && !animator.GetBool("InteedOpen"))
            {
                if(parentHolder.activeSelf)
                    parentHolder.SetActive(false);
            }
            else if(!animator.GetBool("InteedClose") && animator.GetBool("InteedOpen"))
            {
                if(!parentHolder.activeSelf)
                    parentHolder.SetActive(true);
            }
        }
    }

    public void ExcuteOpenAnimation()
    {
        if (!parentHolder.activeSelf)
            parentHolder.SetActive(true);

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
    }

    public void ExcuteCloseAnimation()
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
    }
}
