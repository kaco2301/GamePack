using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gift : MonoBehaviour
{
    int kind;
    // Start is called before the first frame update
    void Start()
    {
        InitGift();
    }

    private void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if(pos.y < -30)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void InitGift()
    {
        kind = int.Parse(transform.name.Substring(4, 1));

        Sprite[] sprites = Resources.LoadAll<Sprite>("Gift");
        GetComponent<SpriteRenderer>().sprite = sprites[kind];
    }

    public void GetGift()
    {
        GetComponent<AudioSource>().Play();

        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        GameObject.Find("GameManager").SendMessage("GetGift", kind);
        score.SendMessage("SetScore", 100 + kind * 100);
        score.transform.position = transform.position;

        Destroy(GetComponent<Collider>());
        GetComponent<SpriteRenderer>().sprite = null;
        Destroy(gameObject, 0.5f);
    }
    
}
