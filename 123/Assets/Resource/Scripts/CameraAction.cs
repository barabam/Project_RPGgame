using UnityEngine;
using System.Collections;

public class CameraAction : MonoBehaviour
{
    private GameObject hero;

    public Vector3 camera = new Vector3(0f, 10f, -10f);
    public float speed = 0.01f;

    void Start()
    {
        hero = GameObject.Find("Hero01");
    }

    void FixedUpdate()
    {
        //this.transform.position = Vector3.Slerp(this.transform.position, hero.transform.position + camera, speed);
        this.transform.position = hero.transform.position + camera ;
        this.transform.rotation = Quaternion.Euler(45f, 0f, 0f);
    }
}
