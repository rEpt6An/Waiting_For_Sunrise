using UnityEngine;

public class ResetTriggerOnEnter : StateMachineBehaviour
{
    [Tooltip("要在此状态进入时重置的触发器名称")]
    public string triggerName;

    // 当一个转换开始并且状态机开始评估这个状态时被调用
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
        }
    }
}