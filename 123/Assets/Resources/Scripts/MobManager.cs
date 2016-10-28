using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MobManager : MonoBehaviour
{
    public List<GameObject> spawn_position = new List<GameObject>();
    public GameObject boss_spawn_position = null;
    public List<GameObject> createMobs = new List<GameObject>();
    public GameObject camera_;
    public GameObject map03;
    public int wave;

    private int mobCount;

    void Start()
    {
        map03 = GameObject.Find("Map_03");
        camera_ = GameObject.Find("Main Camera");
    }
    //======================   던전 포탈을 들어가는 순간 몬스터를 생성해주는 함수   ==================
    IEnumerator CreateMob()
    {
        wave++;

        Vector3 ran_pos = new Vector3(Random.Range(-2f, 3), 0f, Random.Range(-2f, 3)); // 몬스터들의 젠 위치를 랜덤으로 조정
        int count = 7;                                                                 // 랜덤으로 생성할 몹수를 정한다.
        mobCount = count;                                                              // 이번 Wave에 생성될 몹수
        while (count > 0)
        {
            GameObject resMob = (GameObject)Resources.Load("Prefabs/Monster/Monster1/Monster01");
            if (resMob == null)
            {
                Debug.Log("몹 생성 실패!!!");
                yield return null;
            }
            GameObject mob = (GameObject)Instantiate(resMob);

            mob.transform.parent = map03.transform;
            mob.GetComponent<MonsterAction>().startpos = spawn_position[count].transform.position + ran_pos;
            mob.transform.position = spawn_position[count].transform.position + ran_pos;
            mob.transform.rotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            mob.GetComponent<MonsterAction>().navigation.enabled = true;

            createMobs.Add(mob);    // 몹 리스트에 몹을 추가 한다.

            yield return new WaitForSeconds(0.1f);
            count--;
        }
    }
    //================= 보스 생성, 위치 지정  ==========================================================
    IEnumerator CreateBoss()
    {
        yield return new WaitForSeconds(0.5f);

        GameObject bossMob = (GameObject)Resources.Load("Prefabs/Monster/Monster_King/Monster_King");

        GameObject mob = (GameObject)Instantiate(bossMob);
        mob.transform.parent = map03.transform;
        mob.GetComponent<BossAction>().startpos = boss_spawn_position.transform.position;
        mob.transform.position = boss_spawn_position.transform.position;

        mob.GetComponent<BossAction>().navigation.enabled = true;

        camera_.GetComponent<CameraAction>().StartCoroutine("CreatBossEvent");
    }
    //==================================================================================================
    //===============  몬스터가 죽었을 때, 리스트에서 지우고 모두 죽었을 경우 보스 생성  ===============
    public void UpdateMobCount(GameObject mob)
    {
        // 죽은 몹을 찾아서 리스트에서 제거 한다.
        for (int i = createMobs.Count - 1; i >= 0; i--)
        {
            if (createMobs[i] == mob)
            {
                createMobs.RemoveAt(i);
                break;
            }
        }
        mobCount--;
        Debug.Log(mobCount);
        if (mobCount <= 0)
        {
            StartCoroutine(CreateBoss());
        }
    }
    //===================================================================================================
}
