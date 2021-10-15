using UnityEngine;

public class EffectEnded : StateMachineBehaviour
{
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Jump", false);
        animator.gameObject.SetActive(false);
    }
}
