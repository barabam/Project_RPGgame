using UnityEngine;
using System.Collections;

public class BossAction : MonoBehaviour
{
    //---------- Hero 관련----------
    [HideInInspector]
    public float speed;
    public float attack = 5f;
    public float hp = 50f;
    //-----------  STATE  ---------
    public enum STATE
    {
        IDLE,
        CHASE,
        ATTACK,
        DIE,
    }
    public STATE state = STATE.IDLE;
    //-------------------------------

    private float die_timer;
    private float timer;
    private float hero_attack;
    private GameObject hero;
    private Vector3 heading = new Vector3();
    private Vector3 other = new Vector3();
    private float dp;
    private Animator animator;
    private Vector3 startpos;                                           //hero를 쫓아가다가 범위에서 벗어났을 때 돌아갈 위치
    private NavMeshAgent navigation;                                         //네비게이션을 위해

    void Awake()
    {
        hero = GameObject.Find("Hero01");
        animator = this.GetComponentInChildren<Animator>();
        navigation = GetComponent<NavMeshAgent>();
        startpos = this.transform.position;
        navigation.speed = 5f;
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
                ProcessDIE();
                break;
        }
        //-------------중복된 피격을 방지하기 위한 타이머------------------------
        timer += Time.deltaTime;
    }

    //----------------------근처에 hero가 없을 때-----------------------------------
    void ProcessIDLE()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);

        navigation.SetDestination(startpos);
        if (dp > Mathf.Cos(60f * Mathf.Deg2Rad) && length < 5)
        {
            state = STATE.CHASE;
            animator.SetBool("move", true);
        }
        //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
        else if (hp <= 0)
            state = STATE.DIE;
        //------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------
    //-----------------------hero와의 시야각과 거리가 가까워 졌을 경우---------------
    void ProcessCHASE()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);

        if (length > 1 && length < 10)                                          //-------시야,감지범위에 들어가고, 공격범위까지
        {
            animator.SetBool("move", true);
            navigation.SetDestination(hero.transform.position);
        }
        else if (length >= 10)                                                  //----------일정 사거리 이상 벗어났을 경우
        {
            animator.SetBool("move", true);
            navigation.SetDestination(startpos);
        }
        else if (length <= 1.5f)                                                //----------공격 범위에 들어갔을 경우
        {
            animator.SetBool("move", false);
            navigation.SetDestination(this.transform.position);
            state = STATE.ATTACK;
        }
        //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
        else if (hp <= 0)
        {
            animator.SetBool("move", false);
            state = STATE.DIE;
        }
        //------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------
    //-----------------hero가 공격 사거리에 들어왔을 경우----------------------------
    void ProcessATTACK()
    {
        float length = Vector3.Distance(hero.transform.position, transform.position);

        if (length < 1.5f)
        {
            animator.SetBool("attack", true);

            //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
            if (hp <= 0)
            {
                animator.SetBool("attack", false);
                state = STATE.DIE;
            }
            //------------------------------------------------------------
        }
        else
        {
            //navigation.SetDestination(hero.transform.position);
            animator.SetBool("attack", false);
            state = STATE.CHASE;
        }
    }
    //--------------------------------------------------------------------------------
    //--------------------DIE상태일 때 -----------------------------------------------
    void ProcessDIE()
    {
        animator.SetBool("die", true);
        die_timer += Time.deltaTime;

        if (die_timer >= 5)
            Destroy(this.gameObject);
    }
    //--------------------------------------------------------------------------------
    //--------------------hero의 무기에 맞았을 경우-----------------------------------
    void OnTriggerEnter(Collider col)
    {
        if (timer >= 0.5f)
        {
            if (col.tag == "Hero_Attack")
            {
                hero_attack = hero.GetComponent<HeroAction>().attack;

                hp = hp - hero_attack;
                Debug.Log("Monster : " + hp);

                timer = 0;
            }
        }
    }
    //--------------------------------------------------------------------------------
}
