using UnityEngine;
using System.Collections;

public class HeroAction : MonoBehaviour
{
    public int lv = 1;
    public float hp = 100f;
    public float attack = 10f;
    public float move_speed = 10f;                               //움직임 속도
    public GameObject xweapontail;
    public GameObject camera_;

    public enum STATE
    {
        NONE = -1,
        IDLE = 0,
        RUN = 1,
        ATTACK,
        SKILL,
        DIE,
    }
    public float rotate_speed = 0f;                             //돌때 돌아가는 속도
    public STATE state = STATE.NONE;

    private float timer;
    private float debuff_timer;
    private Animator animator;                                  //자식의 애니메이션이 들어갈 자리.
    private Vector3 move_vector;
    private float monster_attack;
    private float boss_attack;
    private float state_timer;                                  //스테이트 오류가 발생할 시 주기적으로 IDLE로 STATE를 변경시켜줌

    void Start()
    {
        //---------시작하자마자 상태를 IDEL로 변경------------------------
        state = STATE.IDLE;
        //==============  자식의 애니메이션을 사용  ======================
        animator = GetComponentInChildren<Animator>();
        //================================================================
    }

    void Update()
    {
        switch (state)
        {
            case STATE.IDLE://---------------아무 움직임 없음
                CheckKeyDown();
                break;
            case STATE.RUN://----------------움직임
                move_rotation_control();
                break;
            case STATE.ATTACK://-------------일반공격
                ComboAttack();
                break;
            case STATE.SKILL://--------------스킬
                SkillAttack();
                break;
            case STATE.DIE://----------------죽음
                break;
        }
        //----------------WeaponTail On/Off 기능--------------------------
        onoff_WeaponTail();
        //----------------------------------------------------------------
        timer += Time.deltaTime;
        debuff_timer += Time.deltaTime;
    }

    private void move_rotation_control()
    {
        //=============   움직임 / 회전   =================================
        Vector3 position = this.transform.position;
        move_vector = Vector3.zero;

        MoveVector();     // 움직이는 방향을 체크하는 함수

        move_vector.Normalize();
        move_vector *= move_speed * Time.deltaTime;
        position += move_vector;
        this.transform.position = position;
        //=============      회전     =====================================
        if (move_vector.magnitude > 0.01f)
        {
            Quaternion q = Quaternion.LookRotation(move_vector, Vector3.up);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, q, rotate_speed);
        }
        //=============   애니메이션   ====================================
        if (move_vector != new Vector3(0f, 0f, 0f))
        {
            animator.SetBool("HeroRun", true);
        }
        else if (move_vector == new Vector3(0f, 0f, 0f))
        {
            animator.SetBool("HeroRun", false);
        }
        if (move_vector == new Vector3(0f, 0f, 0f))
            state = STATE.IDLE;
        //=================================================================
    }

    private void CheckKeyDown()
    {
        //============= 눌린 버튼 검사 함수 ================================
        if (move_vector != new Vector3(0f, 0f, 0f))
            state = STATE.RUN;
        else
            state = STATE.IDLE;
        //-------------       움직임       ---------------------------------
        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.LeftArrow)
         || Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.DownArrow))
        {
            state = STATE.RUN;
        }
        //-------------        공격        ---------------------------------
        if (Input.GetKeyDown(KeyCode.A))
        {
            //----------자신이 맞은 collider의 Tag으로 피격 이펙트 결정------------------
            GetComponentInChildren<HeroAnimator>().hero_collider.tag = "Hero_Normal";
            //----------------------------------------------------------------------------  
            animator.SetTrigger("HeroAttack");
            attack = 100f;
            state = STATE.ATTACK;
            onoff_WeaponTail();
        }
        //-----------------        스킬01       ---------------------------
        if (Input.GetKeyDown(KeyCode.S))
        {
            //----------자신의 공격 collider의 Tag으로 피격 이펙트 결정------------------
            GetComponentInChildren<HeroAnimator>().hero_collider.tag = "Hero_SKILL01";
            //-------------- 스킬의 공격력과 애니메이션 값을 넣어줌----------------------
            skill_control("SKILL01", 30f);
            //---------------------------------------------------------------------------
            camera_.GetComponent<CameraAction>().StartCoroutine("HeroSKILL01");
        }
        //==================================================================
    }
    //----연속 공격을 위해 STATE가 ATTACK일 때 A버튼이 눌리는걸 다시 확인---
    void ComboAttack()
    {
        state_timer += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("HeroAttack");
        }
        //---------------------STATE 변경 오류시 IDLE로 변경-----
        if (state_timer >= 2f)
        {
            state = STATE.IDLE;
            state_timer = 0f;
        }
        //-------------------------------------------------------
    }
    //-----------------------------------------------------------------------
    //=================       스킬01 사용 중     ============================
    void SkillAttack()
    {
        Camera.main.transform.parent = transform;
    }
    //=======================================================================
    //================   MoveVector값을 넘겨줄 함수  ========================
    public void MoveVector()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            move_vector += Vector3.right;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            move_vector += Vector3.left;
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            move_vector += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            move_vector += Vector3.back;
        }
    }
    //=======================================================================
    //--------------------     맞았을 경우      -----------------------------------
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Monster_Attack")
        {
            if (timer > 0.6f)
            {
                monster_attack = col.GetComponentInParent<MonsterAction>().attack;

                hp = hp - monster_attack;
                Debug.Log("Hero : " + hp);

                timer = 0f;
            }
        }
        if (col.tag == "Boss_Attack")
        {
            if (timer > 0.5f)
            {
                boss_attack = col.GetComponentInParent<BossAction>().attack;

                hp = hp - boss_attack;
                Debug.Log("Hero : " + hp);

                timer = 0f;
            }
        }
        if (col.tag == "debuff_smog")
        {
            if (debuff_timer > 10f)
            {
                Debug.Log("smog");
                StartCoroutine("debuff_off");
            }
        }
    }
    //--------------------------------------------------------------------------------

    //===========   WeaponTail On/Off 함수 ============================================
    void onoff_WeaponTail()
    {
        if (state == STATE.ATTACK || state == STATE.SKILL)
        {
            if (!xweapontail.activeSelf)
            {
                xweapontail.GetComponent<Xft.XWeaponTrail>().Activate();
            }
            
        }
        else if (state != STATE.ATTACK && state != STATE.SKILL)
        {
            if (xweapontail.activeSelf)
            {
                xweapontail.GetComponent<Xft.XWeaponTrail>().Deactivate();
            }
        }
    }
    //=================================================================================
    //============    스킬 정리   =====================================================
    void skill_control(string ani_name, float skill_att)
    {
        animator.SetBool(ani_name,true);
        attack = skill_att;
        state = STATE.SKILL;
    }
    //=================================================================================
    //-------------------------디버프 : 이동 속도 감소---------------------------------
    IEnumerator debuff_off()
    {
        move_speed = move_speed / 2;

        debuff_timer = 0f;

        yield return new WaitForSeconds(10f);

        move_speed = move_speed * 2;
    }
    //=================================================================================
}
