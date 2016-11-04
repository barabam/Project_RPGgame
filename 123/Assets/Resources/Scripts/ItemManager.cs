using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;                        //file사용
using System.Text.RegularExpressions;//Regex사용

public class ItemManager
{
    //싱글톤 인스턴스르 저장
    //volatile 동시에 실행 되는 여러 스레드에 의해 필드가 수정될 수 있음을 나타낸다.
    private static volatile ItemManager uniqueInstance;
    private static object _lock = new System.Object();
    //생성자
    private ItemManager() { }
    //싱글톤의 핵심 코드
    //외부에서 접근할 수 있도록 함 static으로 선언
    public static ItemManager Instance
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
                        uniqueInstance = new ItemManager();
                    }
                }
            }
            return uniqueInstance;
        }
    }
    private Dictionary<int, ITEM> table = new Dictionary<int, ITEM>();

    public string pathForDocumentsFile(string fileName)
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0, Application.dataPath.Length - 5);
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path, "Documents"), fileName);
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, fileName);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0, path.LastIndexOf('/'));
            return Path.Combine(path, fileName);
        }
    }

    public void LoadTable()
    {
        string path = pathForDocumentsFile("Assets/tables/ItemTable.csv");
        if (File.Exists(path) == false)
        {
            Debug.Log("파일이 존재하지 않습니다." + path);
            return;
        }
        string str;
        table.Clear();

        FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
        StreamReader st = new StreamReader(file);

        while ((str = st.ReadLine()) != null)
        {
            string[] datas = Regex.Split(str, "\r\n");

            foreach (string data in datas)
            {
                if (data == "" || data.Length == 0)
                    break;

                if (data[0] == '#')
                    continue;
                string[] temp = data.Split(',');            //콤마를 기준으로 데이터를 쪼개서 string배열로 만듦
                int key = int.Parse(temp[0]);
                table.Add(key, new ITEM
                {
                    id = int.Parse(temp[0]),
                    attack = int.Parse(temp[6]),
                    defence = int.Parse(temp[7]),
                    durabillity = int.Parse(temp[8]),
                    fire_regist = int.Parse(temp[9]),
                    ice_regist = int.Parse(temp[10]),
                    regeneration = int.Parse(temp[11]),
                    cooltime = float.Parse(temp[12]),
                    weight = int.Parse(temp[13]),
                    level = int.Parse(temp[14]),
                    price = int.Parse(temp[15]),
                    skill_01 = int.Parse(temp[16]),
                    skill_02 = int.Parse(temp[17]),
                    overlap = int.Parse(temp[18]),
                    name = temp[19],
                    description = temp[20],
                    icon = temp[21],
                    sound = temp[22],
                    effect = temp[23],
                });
                Debug.Log("table에 데이타 등록 : " + temp[0]);
            }
        }
        st.Close();
        file.Close();

        Debug.Log("파일 읽기 완료!!" + path);
    }

    public ITEM GetInfo(int key)
    {
        //해당하는 키의 아이템이 존재하면 아이템 정보를 리턴한다.
        if (table.ContainsKey(key) == true)
        {
            return table[key];
        }
        // 존재하지않으면 리턴
        return null;
    }
}

public enum ITEM_GENDER
{
    COMMON = 1,
    MALE = 2,
    FEMALE = 3,
}
public enum ITEM_TYPE
{
    EQUIP = 1,
    UNIVERSAL = 2,
    CONSUME = 3,
    COOPON = 4,
}
public enum ITEM_TYPE_EQIP
{
    WEAPON = 1,
    HELMET = 2,
    SHIELD = 3,
    ARMOR = 4,
    SHOES = 5,
    RING = 6,
    NECKLACE = 7,
}
public enum ITEM_TYPE_UNIVERSAL
{
    MATERIAL = 1,
    ENCHANT = 2,
    QUEST = 3,
    GOLD = 4,
}
public enum ITEM_TYPE_CONSUME
{
    SKILL = 1,
    HEALING = 2,
    MANA = 3,
    DEBUFF = 4,
    BUFF = 5,
}
public enum ITEM_TYPE_COOPON
{
    REUSE = 1,
    ONETIME = 2,
}
public enum ITEM_CLASS
{
    COMMON = 1,
    WARRIOR = 2,
    ARCHER = 3,
    MAGICIAN = 4,
}
public enum ITEM_GRADE
{
    BEGINNER = 1,
    JUNIOR = 2,
    MAJOR = 3,
    RARE = 4,
}

public class ITEM
{
    public int id;

    public int attack;
    public int defence;
    public int durabillity;             //내구력
    public int fire_regist;
    public int ice_regist;
    public int regeneration;
    public float cooltime;
    public int weight;
    public int level;
    public int price;
    public int skill_01;
    public int skill_02;
    public int overlap;                //중첩 가능 유무
    public int count;                   //아이템 개수
    public string name;
    public string description;
    public string icon;
    public string sound;
    public string effect;
}


