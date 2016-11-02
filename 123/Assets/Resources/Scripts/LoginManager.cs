using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System;

public class LoginManager : MonoBehaviour
{

    public UIInput id;
    public UIInput pw;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameData.Instance.charinfo.char_id = 1;//이 타이밍에 싱글턴이 생성된다.(최초로 호출될 때 싱글턴은 생성)
            RequestGetCharacterInfo();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameData.Instance.charinfo.char_id = 1;
            RequestGetInventoryInfo();
        }
    }

    public void RequestLogin()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "login");
        sendData.Add("id", id.value);
        sendData.Add("pw", pw.value);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyLogin));
    }

    private class RecvLoginData
    {
        public int account;
        public string acc_name;
        public int timestamp;
        public string message;
    }
    public void ReplyLogin(string json)
    {
        RecvLoginData data = JsonReader.Deserialize<RecvLoginData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);

        Debug.Log(data.message);
        Debug.Log((origin.ToLocalTime()).ToString("yyyy년 MM월 dd일의 tt HH시 mm분 s초에 로그인 하였습니다."));
    }

    public void RequestGetCharacterInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_character_info");//contents : 디렉토리명, get_character_info : php파일명
        sendData.Add("char_id", GameData.Instance.charinfo.char_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetCharacterInfo));
    }

    private class RecvGetCharInfoData
    {
        public string message;
        public int timestamp;
        public int char_id;
        public string nickname;
        public int clas;
        public int level;
        public int exp;
        public int stat_hp;
        public int stat_attack;
        public int stat_defence;
        public List<int> skill = new List<int>();
        public List<int> equipitem = new List<int>();
    }

    public void ReplyGetCharacterInfo(string json)
    {
        RecvGetCharInfoData data = JsonReader.Deserialize<RecvGetCharInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message);
        Debug.Log((origin.ToLocalTime()).ToString("응답 시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.charinfo.nickname = data.nickname;
        GameData.Instance.charinfo.clas = data.clas;
        GameData.Instance.charinfo.level = data.level;
        GameData.Instance.charinfo.exp = data.exp;
        GameData.Instance.charinfo.stat_hp = data.stat_hp;
        GameData.Instance.charinfo.stat_attack = data.stat_attack;
        GameData.Instance.charinfo.stat_defence = data.stat_defence;
        GameData.Instance.charinfo.skill = data.skill;
        GameData.Instance.charinfo.equip = data.equipitem;

        Debug.Log("Completed to save to game info !!");
    }

    public void RequestGetInventoryInfo()
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_inventory");//contents : 디렉토리명, get_inventory : php파일명
        sendData.Add("char_id", GameData.Instance.charinfo.char_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetInventoryInfo));
    }

    public void ReplyGetInventoryInfo(string json)
    {
        RecvGetInvenInfoData data = JsonReader.Deserialize<RecvGetInvenInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message);
        Debug.Log((origin.ToLocalTime()).ToString("응답 시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.invenInfo = data.inventory;

        Debug.Log("Completed to save to Inventory info !!");
    }

    private class RecvGetInvenInfoData
    {
        public string message;
        public int timestamp;
        public List<InvenSlotInfo> inventory = new List<InvenSlotInfo>();
    }
}
