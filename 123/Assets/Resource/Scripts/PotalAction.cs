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

    private STATE state;

    void Awake()
    {
        state = STATE.IDLE;
    }

    //-----------   hero가 충돌박스에 들어가서 Space버튼을 누르면 exitpoint로 이동-----------
    void OnTriggerStay(Collider col)
    {
        if (gameObject.tag == "Hero" || Input.GetKeyDown(KeyCode.Space))
        {
            hero.transform.position = exitpoint.transform.position;
        }
    }
    //---------------------------------------------------------------------------------------
}
