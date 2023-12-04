using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Random = UnityEngine.Random;
using System.IO;
using Newtonsoft.Json;
[RequireComponent(typeof(AudioSource))]
public class OwlManager : MonoBehaviour
{
    Transform owl;

    Image pnButton;
    Image pnOver;

    [SerializeField]
    TextMeshProUGUI txtHeight;
    [SerializeField]
    TextMeshProUGUI txtGift;
    [SerializeField]
    TextMeshProUGUI txtBird;
    [SerializeField]
    TextMeshProUGUI txtScore;

    float owlHeight = 0;
    int giftScore = 0;
    int giftCnt = 0;
    int birdCnt = 0;
    int score = 0;



    public bool isMobile;
    public float btnAxis;

    int dir;
    bool isOver;

    AudioSource music;
    Transform spPoint;
    Vector3 wrdSize;

    [System.Serializable]
    public class GameData
    {
        public float owlHeight;
        public int giftScore;
        public int giftCnt;
        public int birdCnt;
        public int score;
        
        
    }

    void Awake()
    {
        InitGame();
        InitWidget();
    }

    void InitWidget()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;

        if (isMobile == false)
            Cursor.visible = true;
        else
            Cursor.visible = false;
        pnButton = GameObject.Find("PanelButton").GetComponent<Image>();
        pnButton.gameObject.SetActive(isMobile);

        pnOver = GameObject.Find("PanelOver").GetComponent<Image>();
        pnOver.gameObject.SetActive(false);


        owl = GameObject.Find("Owl").transform;
    }

    void InitGame()
    {
        music = GetComponent<AudioSource>();
        music.loop = true;

        if (music.clip != null)
        {
            music.Play();
        }

        spPoint = GameObject.Find("SpawnPoint").transform;

        Vector3 scrSize = new Vector3(Screen.width, Screen.height);
        scrSize.z = 10;
        wrdSize = Camera.main.ScreenToWorldPoint(scrSize);
    }
    // Update is called once per frame
    void Update()
    {
        MakeBranch();
        MakeBird();
        MakeGift();

        if (!isOver) SetScore();
    }

    void SetScore()
    {
        if (owl.position.y > owlHeight)
        {
            owlHeight = owl.position.y;
        }

        score = Mathf.FloorToInt(owlHeight) * 100 + giftScore - birdCnt * 100;

        txtHeight.text = owlHeight.ToString("#,##0.0");
        txtGift.text = giftCnt.ToString();
        txtBird.text = birdCnt.ToString();
        txtScore.text = score.ToString("#,##0.0");
    }

    void GetGift(int kind)
    {
        giftCnt++;
        giftScore += (kind * 100) + 100;
    }

    void BirdStrike()
    {
        birdCnt++;
    }

    void MakeBranch()
    {
        int cnt = GameObject.FindGameObjectsWithTag("Branch").Length;
        if (cnt > 3) return;

        Vector3 pos = spPoint.position;
        pos.x = Random.Range(-wrdSize.x * 0.5f, wrdSize.x * 0.5f);

        GameObject branch = Instantiate(Resources.Load("Branch")) as GameObject;
        branch.transform.position = pos;

        spPoint.position += new Vector3(0, 3, 0);
    }

    void MakeBird()
    {
        int cnt = GameObject.FindGameObjectsWithTag("Bird").Length;
        if (cnt > 7 || Random.Range(0, 1000) < 980) return;

        Vector3 pos = spPoint.position;
        pos.y -= Random.Range(0, 5f);

        GameObject bird = Instantiate(Resources.Load("Bird")) as GameObject;
        bird.transform.position = pos;
    }

    void MakeGift()
    {
        int cnt = GameObject.FindGameObjectsWithTag("Gift").Length;
        if (cnt > 5 || Random.Range(0, 1000) < 980) return;

        Vector3 pos = spPoint.position;
        pos.x = Random.Range(-wrdSize.x * 0.5f, wrdSize.x * 0.5f);
        pos.y += Random.Range(0.5f, 1.5f);

        GameObject gift = Instantiate(Resources.Load("Gift0")) as GameObject;
        gift.name = "Gift" + Random.Range(0, 3);
        gift.transform.position = pos;
    }

    
    void SetGameOver()
    {
        GameData gameData = new GameData();
        gameData.owlHeight = owlHeight;
        gameData.giftScore = giftScore;
        gameData.giftCnt = giftCnt;
        gameData.birdCnt = birdCnt;
        gameData.score = score;
        string json = JsonConvert.SerializeObject(gameData);
        string path = Path.Combine(Application.persistentDataPath, "gameData.json");
        File.WriteAllText(path, json);

        isOver = true;
        pnOver.gameObject.SetActive(true);
        Cursor.visible = true;

        music.clip = Resources.Load("gameover", typeof(AudioClip)) as AudioClip;
        music.loop = false;
        music.Play();
    }

    public void OnButtonClick(GameObject button)
    {
        switch (button.name)
        {
            case "BtnAgain":
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                break;
            case "BtnQuit":
                Application.Quit();
                break;
        }
    }

    public void OnButtonPress(GameObject button)
    {
        switch (button.name)
        {
            case "BtnLeft":
                dir = -1;
                StartCoroutine(GetButtonAxis());
                break;
            case "BtnRight":
                StartCoroutine(GetButtonAxis());
                dir = 1;
                break;
        }
    }

    public void OnButtonUp()
    {
        dir = 0;
        StartCoroutine(GetButtonAxis());
    }

    IEnumerator GetButtonAxis()
    {
        while (true)
        {
            if (dir == 0 && Mathf.Abs(btnAxis) < 0.01)
            {
                btnAxis = 0; ;
                yield break;
            }


            if (Mathf.Abs(dir) - Mathf.Abs(btnAxis) < 0.01)
            {
                btnAxis = dir;
                yield break;
            }


            btnAxis = Mathf.MoveTowards(btnAxis, dir, 3 * Time.deltaTime);
            yield return new WaitForFixedUpdate();
        }
    }
}

