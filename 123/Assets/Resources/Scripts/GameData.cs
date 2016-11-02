using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public sealed class GameData
{
    //싱글톤 인스턴스르 ㄹ저장
    //volatile 동시에 실행 되는 여러 스레드에 의해 필드가 수정될 수 있음을 나타낸다.
    private static volatile GameData uniqueInstance;
    private static object _lock = new System.Object();
    //생성자
    private GameData() { }
    //싱글톤의 핵심 코드
    //외부에서 접근할 수 있도록 함 static으로 선언
    public static GameData Instance
    {
        get
        {
            if (uniqueInstance == null)
            {
                //lock으로 지정된 블록안의 코드를 하나의 스레드만 접근하도록 한다.
                lock (_lock)
                {
                    if (uniqueInstance == null)
                    {
                        uniqueInstance = new GameData();
                    }
                }
            }
            return uniqueInstance;
        }
    }
    public HeroInfo charinfo = new HeroInfo();
    public List<InvenSlotInfo> invenInfo = new List<InvenSlotInfo>();
}

public class HeroInfo
{
    public int char_id;
    public string nickname;
    public int clas;
    public int level;
    public int exp;
    public int stat_hp;
    public int stat_attack;
    public int stat_defence;
    public List<int> skill = new List<int>();
    public List<int> equip = new List<int>();
}

public class InvenSlotInfo //인벤토리 슬롯 정보 정의
{
    public int item_id;
    public int item_count;
}

