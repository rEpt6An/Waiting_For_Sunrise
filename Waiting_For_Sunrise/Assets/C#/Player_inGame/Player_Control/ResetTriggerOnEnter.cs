using UnityEngine;

public class ResetTriggerOnEnter : StateMachineBehaviour
{
    [Tooltip("Ҫ�ڴ�״̬����ʱ���õĴ���������")]
    public string triggerName;

    // ��һ��ת����ʼ����״̬����ʼ�������״̬ʱ������
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!string.IsNullOrEmpty(triggerName))
        {
            animator.ResetTrigger(triggerName);
        }
    }
}