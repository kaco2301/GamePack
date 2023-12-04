using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class Snake : MonoBehaviour
{
    public GameObject AchieveImage;
    public TextMeshProUGUI achieveTxt;
    [SerializeField]
    TextMeshProUGUI txtSpeed;
    [SerializeField]
    Transform coin;
    [SerializeField]
    GameObject panelOver;
    [SerializeField]
    GameObject panelStick;
    [SerializeField]
    TextMeshProUGUI txtCoin;
    [SerializeField]
    TextMeshProUGUI txtTime;
    [SerializeField]
    Transform item;
    [SerializeField]
    float speedMove = 2.5f;
    [SerializeField]
    float speedRot = 120f;


    public SnakeManager gameManager;



    Joystick stick;
    bool isMobile;

    int coinCnt = 0;
    float startTime;
    List<GameObject> walls = new List<GameObject>();
    List<Transform> tails = new List<Transform>();
    

    bool isDead = false;


    public class GameData_snake
    {
        public int coinCnt;
    }
    void InitGame()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        panelStick.SetActive(isMobile);
        stick = panelStick.transform.GetChild(0).GetComponent<Joystick>();
    }

    private void Awake()
    {
        gameManager = GameObject.FindObjectOfType<SnakeManager>();
        AchieveImage.SetActive(false);
        txtSpeed.color = new Color(255, 255, 0, 255);
        InitGame();
        startTime = Time.time;
        panelOver.SetActive(false);
    }
    void Update()
    {
        if(!isDead)
        {
            MoveHead();
            MoveTail();
            SetTime();
        }
    }

    void SetTime()
    {
        txtCoin.text = coinCnt.ToString("Coin : 0");

        float span = Time.time - startTime;
        int h = Mathf.FloorToInt(span / 3600);
        int m = Mathf.FloorToInt(span / 60 % 60);
        float s = span % 60;

        txtTime.text = string.Format("Time : {0:0}:{1:0}:{2:00.0}", h, m, s);
    }
    void MoveTail()
    {
        Transform target = transform;

        foreach(Transform tail in tails)
        {
            Vector3 pos = target.position;
            Quaternion rot = target.rotation;

            tail.position = Vector3.Lerp(tail.position, pos, 4 * Time.deltaTime);
            tail.rotation = Quaternion.Lerp(tail.rotation, rot, 4 * Time.deltaTime);

            target = tail;
        }
    }

    public void AddTail()
    {
        GameObject tail = Instantiate(Resources.Load("Tail")) as GameObject;
        
        Vector3 pos = transform.position;

        int cnt = tails.Count;

        if (cnt == 0)
        {
            tail.tag = "Untagged";
            Collider collider = tail.GetComponent<Collider>();
            if (collider != null)
                collider.isTrigger = true;
        }
        else
        {
            pos = tails[cnt - 1].position;
        }

        
        tail.transform.position = pos;

        Color[] colors = { new Color(0, 0.5f, 0, 1), new Color(0, 0.5f, 1, 1) };
        int n = cnt / 3 % 2;
        tail.GetComponent<Renderer>().material.color = colors[n];

        tails.Add(tail.transform);
        Debug.Log(cnt);
        if(coinCnt % 5 ==0 )
        { 
            StartCoroutine(AchievementEffect(coinCnt));
            StartCoroutine(CreateWall());
        }
    }

    IEnumerator AchievementEffect(int count)
    {
        float currentSpeedRate = gameManager.speedRate;
        AchieveImage.SetActive(true);
        achieveTxt.text = count.ToString() + "tails, SpeedUp!!";
        yield return new WaitForSeconds(3);
        AchieveImage.SetActive(false);
        Debug.Log("achieve");

        speedMove = speedMove * currentSpeedRate;
    }
    void MoveHead()
    {
        float amount = speedMove * Time.deltaTime;
        transform.Translate(Vector3.forward * amount);

        if (!isMobile)
        {
            amount = Input.GetAxis("Horizontal") * speedRot;
        }   
        else
        {
            amount = stick.Horizontal() * speedRot;

        }
        transform.Rotate(Vector3.up * amount * Time.deltaTime);

        txtSpeed.text = "Speed : " + speedMove.ToString();

        if (speedMove > 3)
            txtSpeed.color = new Color(1f, 0.67f, 0f); // Orange

        if (speedMove > 3.5)
            txtSpeed.color = new Color(1f, 0.33f, 0f); // Dark Orange

        if (speedMove > 4)
            txtSpeed.color = new Color(1f, 0f, 0f); // Red



    }

    IEnumerator CreateWall()
    {
        GameObject wall = Instantiate(Resources.Load("AddWall")) as GameObject;
        float x2 = Random.Range(-9f, 9f);
        float z2 = Random.Range(-4f, 4f);

        // 충돌하지 않는 상태로 위치 설정
        Vector3 position = new Vector3(x2, 0.5f, z2);
        wall.transform.position = position;

        // 회전값 랜덤 설정
        float randomYRotation = Random.Range(0f, 360f);
        wall.transform.rotation = Quaternion.Euler(0, randomYRotation, 0);

        // 충돌하지 않는 상태일 때의 색상 설정 (Yellow)
        Renderer renderer = wall.GetComponent<Renderer>();

        if (renderer != null)
            renderer.material.color = new Color(1f,1f,0f, 1f);

        // 대기 시간 동안은 Collider를 비활성화합니다.
        Collider collider = wall.GetComponent<Collider>();
        if (collider != null)
            collider.enabled = false;

        // 대기 시간 후에 Collider를 활성화하고 색상을 변경합니다.
        yield return new WaitForSeconds(2f);

        if (collider != null)
            collider.enabled = true;

        if (renderer != null)
            renderer.material.color = Color.white; // 색상 변경 (White)

        walls.Add(wall);

    }

    void MoveCoin()
    {
        coinCnt++;
        float x = Random.Range(-9f, 9f);
        float z = Random.Range(-4f, 4f);

        Vector3 newPosition = new Vector3(x, 0, z);

        coin.position = newPosition;

        // Coin 위치 변경 후 기존 Wall과의 충돌 체크 및 파괴
        foreach (GameObject wall in walls)
        {
            if (Vector3.Distance(wall.transform.position, newPosition) < 1)
            {
                Destroy(wall);
                walls.Remove(wall);
                break;
            }
        }

    }

    


    public void OnButtonClick(Button button)
    {
        switch(button.name)
        {
            case "BtnYes":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "BtnNo":
                Application.Quit();
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        switch(collision.transform.tag)
        {
            case "Coin":
                Debug.Log("충돌");
                MoveCoin(); 
                AddTail();
                
                break;

            
            case "Wall":
            case "Tail":
                isDead = true;
                GameData_snake gameData = new GameData_snake();
                gameData.coinCnt = coinCnt;

                // 이 인스턴스를 JSON 문자열로 변환합니다.
                string json = JsonConvert.SerializeObject(gameData);

                // JSON 문자열을 파일로 저장합니다.
                string path = Path.Combine(Application.persistentDataPath, "gameData_snake.json");
                File.WriteAllText(path, json);
                panelOver.SetActive(isDead);
                break;
        }
    }

}
