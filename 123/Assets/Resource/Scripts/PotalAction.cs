using UnityEngine;
using System.Collections;

public class PotalAction : MonoBehaviour
{
    public GameObject exitpoint;
    public GameObject hero;

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
