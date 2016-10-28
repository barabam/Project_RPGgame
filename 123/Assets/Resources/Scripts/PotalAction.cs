using UnityEngine;
using System.Collections;

public class PotalAction : MonoBehaviour
{
    public enum STATE
    {
        NONE,
        IDLE,
        START,
        END,
    }
    public GameObject exitpoint;
    public GameObject hero;
    public MobManager mobmanager;

    private STATE state;
    private float timer;

    void Awake()
    {
        state = STATE.IDLE;
        mobmanager = GameObject.Find("MobManager").GetComponent<MobManager>();
    }

    void Update()
    {
        timer -= Time.deltaTime;
    }

    //-----------   hero가 충돌박스에 들어가서 Space버튼을 누르면 exitpoint로 이동-----------
    void OnTriggerStay(Collider col)
    {
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject.name == "Cube_StageStart")
        {
            if (timer <= 0)
            {
                timer = 5f;
                hero.transform.position = exitpoint.transform.position;
                mobmanager.StartCoroutine("CreateMob");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject.name == "Cube")
        {
            hero.transform.position = exitpoint.transform.position;
        }
        //---------------------------------------------------------------------------------------
    }
}
