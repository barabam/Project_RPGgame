using UnityEngine;
using System.Collections;

public class MonsterAction : MonoBehaviour
{
    public GameObject hero;
    public float speed;
    public enum STATE
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE,
    }
    public STATE state = STATE.IDLE;

    private Vector3 heading = new Vector3();
    private Vector3 other = new Vector3();
    private float dp;
    private Animator animator;
    private Vector3 startpos;                                           //hero를 쫓아가다가 범위에서 벗어났을 때 돌아갈 위치
    private NavMeshAgent navigation;                                    //네비게이션을 위해

    void Awake()
    {
        animator = this.GetComponentInChildren<Animator>();
        navigation = GetComponent<NavMeshAgent>();
        startpos = this.transform.position;
    }

    void Update()
    {
        heading = transform.TransformDirection(Vector3.forward);
        other = hero.transform.position - transform.position;
        heading.y = 0.0f;
        other.y = 0.0f;
        heading.Normalize();
        other.Normalize();
        dp = Vector3.Dot(heading, other);

        switch (state)
        {
            case STATE.IDLE://---------------아무 움직임 없음
                ProcessIDLE();
                break;
            case STATE.CHASE://--------------Hero 발견 후 추적
                ProcessCHASE();
                break;
            case STATE.ATTACK://-------------Hero 를 공격
                ProcessATTACK();
                break;
            case STATE.DIE://----------------죽음
                break;
        }
    }

    //----------------------근처에 hero가 없을 때-----------------------------------
    void ProcessIDLE()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);
        navigation.speed = 1f;

        navigation.SetDestination(startpos);
        if (dp > Mathf.Cos(60f * Mathf.Deg2Rad) && length < 5)
        {
            state = STATE.CHASE;
            animator.SetBool("move", true);
        }
    }
    //-------------------------------------------------------------------------------
    //-----------------------hero와의 시야각과 거리가 가까워 졌을 경우---------------
    void ProcessCHASE()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);
        navigation.speed = 3f;

        if (length > 1 && length < 10)   //-------시야,감지범위에 들어가고, 공격범위까지
        {
            //if (dp < Mathf.Cos(60f * Mathf.Deg2Rad))
            //{
            //    navigation.SetDestination(hero.transform.position);
            //}
            animator.SetBool("move", true);
            navigation.SetDestination(hero.transform.position);
        }
        else if (length >= 10)                                                  //----------일정 사거리 이상 벗어났을 경우
        {
            animator.SetBool("move", false);
            navigation.SetDestination(startpos);
        }
        else if (length <= 1)                                                   //----------공격 범위에 들어갔을 경우
        {
            animator.SetBool("move", false);
            navigation.SetDestination(this.transform.position);
            state = STATE.ATTACK;
        }
    }
    //-------------------------------------------------------------------------------
    //-----------------hero가 공격 사거리에 들어왔을 경우----------------------------
    void ProcessATTACK()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);

        if (length < 1)
        {
            animator.SetBool("attack", true);
        }
        else
        {
            navigation.SetDestination(hero.transform.position);
            animator.SetBool("attack", false);
            state = STATE.CHASE;
        }
    }
    //--------------------------------------------------------------------------------
}
