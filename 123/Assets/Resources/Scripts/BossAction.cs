using UnityEngine;
using System.Collections;

public class BossAction : MonoBehaviour
{
    //---------- Hero 관련----------
    [HideInInspector]
    public float speed;
    public float attack = 5f;
    private float max_hp = 50f;
    public float hp = 50f;
    //-----------  STATE  ---------
    public enum STATE
    {
        IDLE,
        CHASE,
        ATTACK,
        SKILL01,
        SKILL02,
        SKILL03,
        DIE,
    }
    public STATE state = STATE.IDLE;
    //-------------------------------
    public GameObject effect_normal = null;
    public GameObject effect_skill01 = null;

    public GameObject effect_skill01_1 = null;
    public GameObject effect_skill01_2 = null;

    public GameObject effect_skill02 = null;

    public GameObject effect_skill03 = null;

    public Collider col;
    public Vector3 startpos;                                           //hero를 쫓아가다가 범위에서 벗어났을 때 돌아갈 위치
    public NavMeshAgent navigation;                                         //네비게이션을 위해

    private float die_timer;
    private float timer;
    private float hero_attack;
    private GameObject hero;
    private Vector3 heading = new Vector3();
    private Vector3 other = new Vector3();
    private float dp;
    private Animator animator;
    private PotalManager potalmanager;

    void Awake()
    {
        col = this.GetComponent<CapsuleCollider>();
        hero = GameObject.Find("Hero01");
        animator = this.GetComponentInChildren<Animator>();
        potalmanager = GameObject.Find("PotalManager").GetComponent<PotalManager>();
        navigation = GetComponent<NavMeshAgent>();
        startpos = this.transform.position;
        navigation.speed = 5f;
        hp = max_hp;
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
            case STATE.SKILL01://-------------Hero 를 skill 공격
                ProcessSKILL01();
                break;
            case STATE.SKILL02:
                ProcessSKILL02();
                break;
            case STATE.SKILL03:
                ProcessSKILL03();
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
        if (dp > Mathf.Cos(60f * Mathf.Deg2Rad) && length < 15)
        {
            state = STATE.CHASE;
            animator.SetBool("move", true);
        }
        //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
        else if (hp <= 0)
        {
            animator.SetTrigger("die");
            state = STATE.DIE;
        }
        //------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------
    //-----------------------hero를 추적하는 상태 -----------------------------------
    void ProcessCHASE()
    {
        float random = Random.Range(1f, 121f);                                  //-------패턴이 나올 확률

        float length = Vector3.Distance(hero.transform.position, transform.position);

        if (length > 3f)                                          //-------시야,감지범위에 들어가고, 공격범위까지
        {
            animator.SetBool("move", true);
            navigation.SetDestination(hero.transform.position);
        }
        //else if (length >= 10f)                                                  //----------일정 사거리 이상 벗어났을 경우
        //{
        //    animator.SetBool("move", true);
        //    navigation.SetDestination(startpos);
        //}
        else if (length <= 3f)                                                //----------공격 범위에 들어갔을 경우
        {
            animator.SetBool("move", false);
            navigation.SetDestination(this.transform.position);

            //---------------------------보스 패턴---------------------------------------
            if ((int)random < 60f)
            {
                attack = 10f;
                state = STATE.ATTACK;
            }
            if ((int)random >= 60f && (int)random < 80f)
            {
                attack = 50f;
                state = STATE.SKILL01;
                Instantiate(effect_skill01_1, this.transform.position, Quaternion.identity);
            }
            if ((int)random >= 80f && (int)random < 100f)
            {
                attack = 50f;
                state = STATE.SKILL02;
            }
            if ((int)random >= 100f && (int)random < 120f)
            {
                if (hp <= max_hp / 2)
                {
                    state = STATE.SKILL03;
                }
            }
            //----------------------------------------------------------------------------
        }
        //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
        else if (hp <= 0f)
        {
            animator.SetTrigger("die");
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

        if (length < 3f)
        {
            animator.SetBool("attack", true);

            //----------------HP가 0이 될 시 STATE를 DIE로 변경-----------
            if (hp <= 0)
            {
                animator.SetTrigger("die");
                animator.SetBool("attack", false);
                state = STATE.DIE;
            }
            //------------------------------------------------------------
        }
    }
    //--------------------------------------------------------------------------------

    //--------------------SKILL01 STATE일 때--------------------------------------------
    void ProcessSKILL01()
    {
        col.enabled = false;
        animator.SetBool("skill01", true);
    }
    //--------------------------------------------------------------------------------
    //--------------------SKILL02 STATE일 때--------------------------------------------
    void ProcessSKILL02()
    {
        col.enabled = false;
        animator.SetBool("skill02", true);
    }
    //--------------------------------------------------------------------------------
    //--------------------SKILL03 STATE일 때--------------------------------------------
    void ProcessSKILL03()
    {
        col.enabled = false;
        animator.SetBool("skill03", true);
    }
    //--------------------------------------------------------------------------------

    //--------------------DIE상태일 때 -----------------------------------------------
    void ProcessDIE()
    {
        col.enabled = false;

        die_timer += Time.deltaTime;

        if (die_timer >= 5f)
        {
            Destroy(this.gameObject);
            potalmanager.GetComponent<PotalManager>().on_off = 0;
        }
    }
    //--------------------------------------------------------------------------------
    //--------------------hero의 무기에 맞았을 경우-----------------------------------
    void OnTriggerEnter(Collider col)
    {
        if (timer >= 0.5f)
        {
            if (col.name == "collider")
            {
                //----------------공격받았을 때 이펙트 종류 검사----------------------
                if (col.tag == "Hero_Normal")
                {
                    Instantiate(effect_normal, this.transform.position, Quaternion.identity);
                }

                else if (col.tag == "Hero_SKILL01")
                {
                    Instantiate(effect_skill01, this.transform.position, Quaternion.identity);
                }
                //-------애니메이션---------------------------------------------------
                animator.SetTrigger("hit");
                //-------HeroAction에서 공격력을 받아옴-----------
                hero_attack = hero.GetComponent<HeroAction>().attack;

                hp = hp - hero_attack;
                Debug.Log("Monster : " + hp);

                timer = 0;
            }
        }
    }
    //--------------------------자신의 체력이 50퍼센트 이하일 때 최대 체력의 4분에 1을 회복-----------------------------

    public IEnumerator hp_heal()
    {
        for (int i = 0; i < 5f; i++)
        {
            hp += max_hp / 20f;
            Instantiate(effect_skill03, this.transform.position + new Vector3(0f, 5f, 0f), Quaternion.identity);
            yield return new WaitForSeconds(0.7f);
        }
    }
    //-------------------------------------------------------------------------------------------------------------------
}
