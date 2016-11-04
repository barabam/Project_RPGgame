using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using JsonFx.Json;
using System;

public class LoginManager : MonoBehaviour
{

    public UIInput id;
    public UIInput pw;

    void Start()
    {
        ItemManager.Instance.LoadTable();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameData.Instance.charinfo.char_id = 1;//이 타이밍에 싱글턴이 생성된다.(최초로 호출될 때 싱글턴은 생성)
            RequestGetCharacterInfo();
            RequestGetInventoryInfo();
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            GameData.Instance.charinfo.char_id = 1;
            RequestGetInventoryInfo();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            ITEM item = ItemManager.Instance.GetInfo(11111001);
            if (item != null)
            {
                Debug.Log("itemManager로 확인한 아이템 정보 : " + item.name);
                Debug.Log(item.description);
            }
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            RequestGetItemInfo(11111001);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            RequestSetBuyItem(11111001);
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
        public int gold;
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
        GameData.Instance.charinfo.gold = data.gold;

        Debug.Log("Completed to save to game info !!" + data.gold + "Gold");
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

    public void RequestGetItemInfo(int id)
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "get_item_info");//contents : 디렉토리명, get_item_info : php파일명
        sendData.Add("item_id", id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplyGetItemInfo));
    }

    public void ReplyGetItemInfo(string json)
    {
        RecvGetItemInfoData data = JsonReader.Deserialize<RecvGetItemInfoData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString("응답 시간 yyyy-MM-dd-tt HH:mm:s"));

        Debug.Log("Item ID : " + data.item.id);
        Debug.Log("Item Attack : " + data.item.attack);
        Debug.Log("Item Defence : " + data.item.defence);
        Debug.Log("Item Name : " + data.item.name);
        Debug.Log("Item Description : " + data.item.description);
    }

    private class RecvGetItemInfoData
    {
        public string message;
        public int timestamp;
        public ITEM item;
    }

    public void RequestSetBuyItem(int item_id)
    {
        Dictionary<string, object> sendData = new Dictionary<string, object>();
        sendData.Add("contents", "set_buy_item");
        sendData.Add("char_id", GameData.Instance.charinfo.char_id);
        sendData.Add("item_id", item_id);

        StartCoroutine(NetworkManagerEX.Instance.ProcessNetwork(sendData, ReplySetBuyItem));
    }

    public void ReplySetBuyItem(string json)
    {
        RecvSetBuyItemData data = JsonReader.Deserialize<RecvSetBuyItemData>(json);

        DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0).AddSeconds(data.timestamp);
        Debug.Log(data.message + (origin.ToLocalTime()).ToString("응답 시간 yyyy-MM-dd-tt HH:mm:s"));

        GameData.Instance.charinfo.gold = data.gold;        
        GameData.Instance.invenInfo[data.slot].item_id = data.item_id;
        GameData.Instance.invenInfo[data.slot].item_count = 1;

        ITEM item = ItemManager.Instance.GetInfo(data.item_id);
        if (item != null)
        {
            Debug.Log(item.name + "이 " + item.price + "가격에 구매 되었습니다.");
            Debug.Log(item.description);
        }
    }

    private class RecvSetBuyItemData
    {
        public string message;
        public int timestamp;
        public int item_id;
        public int slot;
        public int gold;
    }
}
