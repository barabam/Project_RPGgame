using UnityEngine;
using System.Collections;

public class PotalAction : MonoBehaviour
{
    public GameObject exitpoint;
    public GameObject hero;
    public MobManager mobmanager;
    public PotalManager potalmanager;

    private float timer;

    void Awake()
    {
        mobmanager = GameObject.Find("MobManager").GetComponent<MobManager>();
        potalmanager = GameObject.Find("PotalManager").GetComponent<PotalManager>();
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
                //-------          포탈 on / off        ------
                potalmanager.GetComponent<PotalManager>().on_off = 1;
                //--------------------------------------------
                timer = 5f;
                hero.transform.position = exitpoint.transform.position;
                mobmanager.StartCoroutine("CreateMob");
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && this.gameObject.name == "Cube")
        {
            if (potalmanager.GetComponent<PotalManager>().on_off == 0)
                hero.transform.position = exitpoint.transform.position;
            else
                Debug.Log("몬스터가 남아있습니다.");
        }
    }
    //---------------------------------------------------------------------------------------
}
