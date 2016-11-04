using UnityEngine;
using System.Collections;

public class HeroAnimator : MonoBehaviour
{
    public GameObject hero;
    public GameObject hero_collider;
    public GameObject camera_;

    private Animator animator;
    private HeroAction heroaction;

    void Awake()
    {
        heroaction = hero.gameObject.GetComponent<HeroAction>();
        animator = GetComponent<Animator>();
    }

    //=======Normal_Attack 후 애니메이터에서 이벤트로 호출======
    public void ReturnIdle()
    {
       heroaction.state = HeroAction.STATE.IDLE;
    }
    //==========================================================

    //=====SKILL 사용 후 캐릭터의 Rotation을 바로 잡아주기======
    public void Orirotation()
    {
        hero.GetComponent<HeroAction>().state = HeroAction.STATE.IDLE;
        animator.SetBool("SKILL01", false);
        camera_.transform.parent = hero.transform.parent;
        transform.position = hero.transform.position;
        transform.rotation = hero.transform.rotation;
    }
    //==========================================================
}
