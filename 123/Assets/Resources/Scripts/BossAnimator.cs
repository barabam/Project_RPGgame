using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour
{
    private Animator animator;
    private BossAction bossaction;
    private GameObject monster;
    void Awake()
    {
        monster = GameObject.Find("Monster_King(Clone)");
        bossaction = monster.gameObject.GetComponent<BossAction>();
        animator = GetComponent<Animator>();
    }

    //==================애니메이터에서 이벤트로 호출===========
    public void ReturnCHASE()
    {
        animator.SetBool("attack", false);
        animator.SetBool("skill01", false);
        animator.SetBool("skill02", false);
        animator.SetBool("skill03", false);
        bossaction.col.enabled = true;
        bossaction.state = BossAction.STATE.CHASE;
    }
    //=========================================================
    //===================스킬01 이펙트 생성====================
    public void Skill01_BoomEffect()
    {
        Instantiate(bossaction.effect_skill01_2, this.transform.position, Quaternion.identity);
    }
    //=========================================================
    //===================스킬02 이펙트 생성====================
    public void Skill02_SmogEffect()
    {
        Instantiate(bossaction.effect_skill02, this.transform.position, Quaternion.identity);
    }
    //=========================================================
    //===================스킬03 이펙트 생성====================
    public void Skill03_HealEffect()
    {
        bossaction.StartCoroutine("hp_heal");
    }
    //=========================================================
}
