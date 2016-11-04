using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour
{
    public enum STATE
    {
        IDLE,
        EVENT,
    }
    public Vector3 camera = new Vector3(0f, 10f, -10f);

    private GameObject hero;
    private GameObject boss01;
    private Animator animator;
    private STATE state;

    void Start()
    {
        state = STATE.IDLE;
        hero = GameObject.Find("Hero01");
        animator = this.GetComponent<Animator>();
    }

    void Update()
    {
        switch (state)
        {
            case STATE.IDLE://---------------아무 움직임 없음
                ProcessIDLE();
                break;
            case STATE.EVENT://--------------Hero 발견 후 추적
                break;
        }
    }

    void ProcessIDLE()
    {
        animator.enabled = false;
        this.transform.position = hero.transform.position + camera;
        this.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }

    public IEnumerator CreatBossEvent()
    {
        state = STATE.EVENT;
        boss01 = GameObject.Find("Monster_King(Clone)");
        this.transform.parent = boss01.transform;
        animator.enabled = true;
        animator.SetBool("boss_creat_event", true);

        yield return null;
    }

    public IEnumerator HeroSKILL01()
    {
        state = STATE.EVENT;
        this.transform.parent = hero.transform;
        animator.enabled = true;
        animator.SetBool("hero_skill01", true);

        yield return null;
    }

    public void StateRetrunIDLE()
    {
        Debug.Log("!!");
        animator.SetBool("boss_creat_event", false);
        animator.SetBool("hero_skill01", true);
        state = STATE.IDLE;
        this.transform.parent = hero.transform.parent;
    }
}
