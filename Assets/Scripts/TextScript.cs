using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextScript : MonoBehaviour
{
    public TextMesh Text;
    public Color TextColor;

    public float Speed = 2f;
    public float Timer = 1f;

    private float CurrentLerpTime = 0f;
    
    private GameObject Camera;

    // Start is called before the first frame update
    void Start()
    {
        Camera = GameObject.Find("Main Camera");
        Text.color = TextColor;
        Destroy(gameObject, Timer);

        switch(Random.Range(0,2))
        {
            case 0:
                Text.text = "Ouch!!";
                break;
            case 1:
                Text.text = "UGH!";
                break;
            case 2:
                Text.text = ":(";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float t = CurrentLerpTime/ Timer;
        t = Mathf.Cos(t * Mathf.PI * .5f) * 2f;

        TextColor.a = t;
        Text.color = TextColor;
        transform.position += new Vector3(0f, Time.deltaTime * Speed, 0f);

        CurrentLerpTime += Time.deltaTime;

        gameObject.transform.rotation = Quaternion.LookRotation( gameObject.transform.position - Camera.transform.position );
    }
}
