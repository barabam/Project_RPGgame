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
}
