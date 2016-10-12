using UnityEngine;
using System.Collections;

public class HeroAction : MonoBehaviour
{
    public int lv = 1;
    public float hp = 100f;
    public float attack = 10f;

    public enum STATE
    {
        NONE = -1,
        IDLE = 0,
        RUN = 1,
        ATTACK,
        SKILL,
        DIE,
    }
    public float move_speed = 3f;                               //움직임 속도
    public float rotate_speed = 0f;                             //돌때 돌아가는 속도
    public STATE state = STATE.NONE;

    private float timer;
    private Animator animator;                                  //자식의 애니메이션이 들어갈 자리.
    private Vector3 move_vector;
    private float monster_attack;

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

        timer += Time.deltaTime;
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
            animator.SetTrigger("HeroAttack");
            attack = 10f;
            state = STATE.ATTACK;
        }
        //-----------------    스킬01       ---------------------------------
        if (Input.GetKeyDown(KeyCode.S))
        {
            animator.SetTrigger("SKILL01");
            attack = 30f;
            state = STATE.SKILL;
        }
        //==================================================================
    }
    //----연속 공격을 위해 STATE가 ATTACK일 때 A버튼이 눌리는걸 다시 확인---
    void ComboAttack()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            animator.SetTrigger("HeroAttack");
        }
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
    //--------------------Monster에게 맞았을 경우-----------------------------------
    void OnTriggerEnter(Collider col)
    {
        if (timer >= 0.5f)
        {
            if (col.tag == "Monster_Attack")
            {
                monster_attack = col.GetComponentInParent<MonsterAction>().attack;

                hp = hp - monster_attack;
                Debug.Log("Hero : " + hp);

                timer = 0f;
            }
        }
    }
    //--------------------------------------------------------------------------------
}
