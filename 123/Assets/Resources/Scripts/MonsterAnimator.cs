using UnityEngine;
using System.Collections;

public class MonsterAnimator : MonoBehaviour
{
    private Animator animator;
    private MonsterAction monsteraction;
    private GameObject monster;

    void Awake()
    {
        if (this.gameObject.name == "mon01")
        {
            monster = GameObject.Find("Monster01(Clone)");
        }
        else if (this.gameObject.name == "mon02")
        {
            monster = GameObject.Find("Monster02(Clone)");
        }

        monsteraction = monster.gameObject.GetComponent<MonsterAction>();
        animator = GetComponent<Animator>();
    }

    //==================애니메이터에서 이벤트로 호출===========
    public void ReturnCHASE()
    {
        monsteraction.state = MonsterAction.STATE.CHASE;
    }
    //=========================================================
}
