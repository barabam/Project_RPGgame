using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour
{
    private Animator animator;
    private BossAction bossaction;
    private GameObject monster;

    void Awake()
    {
        monster = GameObject.Find("Monster_King");
        bossaction = monster.gameObject.GetComponent<BossAction>();
        animator = GetComponent<Animator>();
    }

    //==================애니메이터에서 이벤트로 호출===========
    public void ReturnCHASE()
    {
        animator.SetBool("attack", false);
        bossaction.state = BossAction.STATE.CHASE;
    }
    //=========================================================
}
