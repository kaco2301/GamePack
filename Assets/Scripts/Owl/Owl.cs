using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    OwlManager manager;
    Animator anim;
    Transform chkPoint;

    float moveSpeed = 8f;
    float jumpSpeed = 12f;
    float gravity = 19f;

    Vector3 moveDir;
    bool isDead = false;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        chkPoint = transform.Find("CheckPoint");
        manager = FindObjectOfType<OwlManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            
            anim.SetFloat("velocity", -1);
            if(anim.GetFloat("velocity")==-1)
            {
                Debug.Log("asd");
            }
        }
        if(Input.GetButtonDown("Fire2"))
        {
            anim.SetFloat("velocity", 1);
        }
        if (isDead) return;

        CheckBranch();
        MoveOwl();
    }

    void MoveOwl()
    {
        Vector2 pos = Camera.main.WorldToScreenPoint(transform.position);
        if(pos.y < -100)
        {
            isDead = true;
            manager.SendMessage("SetGameOver");
            return;
        }

        float keyValue = Input.GetAxis("Horizontal");
        if(manager.isMobile)
        {
            keyValue = manager.btnAxis;
        }

        if((keyValue < 0 && pos.x < 40) || (keyValue > 0 && pos.x > Screen.width - 40))
        {
            keyValue = 0;
        }

        moveDir.x = keyValue * moveSpeed;

        moveDir.y -= gravity * Time.deltaTime;
        

        transform.Translate(moveDir * Time.deltaTime);
        anim.SetFloat("velocity", moveDir.y);
    }

    void CheckBranch()
    {
        RaycastHit2D hit = Physics2D.Raycast(chkPoint.position, Vector2.down, 0.2f);
        Debug.DrawRay(chkPoint.position, Vector2.down * 1f, Color.blue);

        if(hit.collider != null && hit.collider.tag == "Branch")
        {
            moveDir.y = jumpSpeed;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Transform other = collision.transform;

        switch (other.tag)
        {
            case "Bird":
                Bird bird = other.GetComponent<Bird>();
                if (bird != null)
                {
                    bird.DropBird();
                }
                break;

            case "Gift":
                Gift gift = other.GetComponent<Gift>();
                if (gift != null)
                {
                    gift.GetGift();
                }
                break;
        }
    }
}
