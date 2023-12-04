using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bird : MonoBehaviour
{
    Animator anim;

    int dir;
    float speed;

    void Start()
    {
        anim = GetComponent<Animator>();
        InitBird();
    }

    
    void Update()
    {
        float amount = speed * dir * Time.deltaTime;
        transform.Translate(Vector3.right * amount);

        CheckAlive();
    }

    void InitBird()
    {
        dir = (Random.Range(0, 2) == 0) ? -1 : 1;
        transform.localEulerAngles = new Vector3(dir, 1, 1);

        speed = Random.Range(5, 8f);
        anim.speed = 1 + (speed - 5) / 3;

        Vector3 worldPos = transform.position;
        Vector3 pos = Camera.main.WorldToScreenPoint(worldPos);

        if(dir == -1)
        {
            pos.x = Screen.width + 30;
        }
        else
        {
            pos.x = -30;
        }

        pos.z = 10;
        transform.position = Camera.main.ScreenToWorldPoint(pos);
    }

    void CheckAlive()
    {
        Vector3 worldPos = transform.position;
        worldPos.z = 10;

        Vector2 pos = Camera.main.WorldToScreenPoint(worldPos);
        if ((pos.y <-30) || (dir==-1&&pos.x < -30) || (dir==1&& pos.x>Screen.width+30))
        {
            Destroy(gameObject);
        }
    }

    public void DropBird()
    {
        FindObjectOfType<OwlManager>().SendMessage("BirdStrike");
        GetComponent<AudioSource>().Play();
        transform.localEulerAngles = new Vector3(0, 0, 180);
        anim.enabled = false;

        Destroy(GetComponent<Collider2D>());
        GetComponent<Rigidbody2D>().gravityScale = 1;
        speed = 0;

        GameObject score = Instantiate(Resources.Load("Score")) as GameObject;
        score.transform.position = transform.position;

        score.SendMessage("SetScore", -100);
    }

      
}
