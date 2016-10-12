using UnityEngine;
using System.Collections;

public class HeroAnimator : MonoBehaviour
{
    private Animator animator;
    private HeroAction heroaction;

    public GameObject hero;

    void Awake()
    {
        heroaction = hero.gameObject.GetComponent<HeroAction>();
        animator = GetComponent<Animator>();
    }

    //==================애니메이터에서 이벤트로 호출===========
    public void ReturnIdle()
    {
       heroaction.state = HeroAction.STATE.IDLE;
    }
    //=========================================================

    //=====스킬 사용 후 캐릭터의 Rotation을 바로 잡아주기======
    public void Orirotation()
    {
        animator.SetBool("SKILL01", false);
        transform.position = hero.transform.position;
        transform.rotation = hero.transform.rotation;
        Debug.Log("Orirotation");
    }
    //=========================================================
}
