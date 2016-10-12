using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        ACTION,
        DIE,
    }
    public Vector3 camera = new Vector3(0f, 10f, -10f);
    public float speed = 0.01f;
    public STATE state = STATE.IDLE;

    private GameObject hero;
    private Animator animator;

    void Start()
    {
        hero = GameObject.Find("Hero01");
        animator = this.GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        switch (state)
        {
            case STATE.IDLE://---------------Hero를 바라봄
                ProcessIDLE();
                break;
            case STATE.ACTION://-------------Hero가 스킬을 사용했울 때 애니메이션 시작
                ProcessACTION();
                break;
            case STATE.DIE://----------------Hero가 죽었을 경우
                ProcessDIE();
                break;
        }
    }

    void ProcessIDLE()
    {
        animator.enabled = false;

        this.transform.position = hero.transform.position + camera;
        this.transform.rotation = Quaternion.Euler(45f, 0f, 0f);

        if (hero.GetComponent<HeroAction>().state == HeroAction.STATE.SKILL)
        {
            state = STATE.ACTION;
        }
    }
    void ProcessACTION()
    {
        animator.enabled = true;
        this.transform.parent = hero.transform.parent;
        animator.SetBool("SKILL01",true);

        if (hero.GetComponent<HeroAction>().state != HeroAction.STATE.SKILL)
        {
            animator.enabled = false;
            state = STATE.IDLE;
        }
    }
    void ProcessDIE()
    {
       
    }


    void Hero_STATE_Change()
    {
        hero.GetComponent<HeroAction>().state = HeroAction.STATE.IDLE;
    }
}
