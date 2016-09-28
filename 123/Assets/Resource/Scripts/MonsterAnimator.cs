using UnityEngine;
using System.Collections;

public class MonsterAnimator : MonoBehaviour
{
    private Animator animator;
    private MonsterAction monsteraction;

    public GameObject monster;

    void Awake()
    {
        monster = GameObject.Find("Monster01");
        monsteraction = monster.gameObject.GetComponent<MonsterAction>();
        animator = GetComponent<Animator>();
    }

    //==================애니메이터에서 이벤트로 호출===========
    public void ReturnIdle()
    {
        monsteraction.state = MonsterAction.STATE.IDLE;
    }
    //=========================================================
}
