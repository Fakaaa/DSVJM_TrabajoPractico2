using UnityEngine;

public class Die : StateMachineBehaviour
{
    Player pj;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        pj = animator.GetComponent<Player>();
    }
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(pj != null)
            pj.playerDefeat?.Invoke();

        Destroy(animator.gameObject, 0.1f);
    }
}
