using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Newtonsoft.Json;
using System.IO;

public class GameManager : MonoBehaviour
{
    public enum GameMode { RockPaperScissors, MukJjiPpa }

    public enum Attacker { None, Player, Computer }


    private GameMode currentGameMode = GameMode.RockPaperScissors;


    [Header("Img")]
    [SerializeField]    RawImage imgYou;
    [SerializeField]    RawImage imgCom;

    [Header("Text")]
    [SerializeField]    TextMeshProUGUI txtDraw;
    [SerializeField]    TextMeshProUGUI txtYou;
    [SerializeField]    TextMeshProUGUI txtCom;
    [SerializeField]    TextMeshProUGUI txtResult;
    [SerializeField]    TextMeshProUGUI txtWinPoint;

    [Header("Particle")]
    [SerializeField] ParticleSystem winParticle;
    [SerializeField] ParticleSystem loseParticle;

    [Header("Sound")]
    [SerializeField] AudioClip[] btnSound;
    AudioSource AS;

    int cntDraw = 0;    //���º�
    int cntYou = 0;     //��
    int cntCom = 0;     //��
    int winpoint = 0;   //����

    private Attacker currentAttacker = Attacker.None;// �÷��̾ ���������� ���� 

    public class GameData_RSP
    {
        public int winpoint;
    }

    private void Awake()
    {
        AS = this.GetComponent<AudioSource>();
    }
    public void SetGameMode(GameMode gameMode)
    {
        currentGameMode = gameMode;
        InitGame();
    }

    void Start()
    {
        InitGame();
    }

    void InitGame()
    {
        txtResult.text = "Click the Button";
        currentAttacker = Attacker.None;
    }

    void CheckResult(int you)
    {
        int com = UnityEngine.Random.Range(1, 4);
        int k = you - com;

        switch(currentGameMode)
        {
            case GameMode.RockPaperScissors:

                if (k == 0)
                {
                    winpoint += 1;
                    cntDraw++;

                    txtResult.text = "Draw.";
                }
                else if (k == 1 || k == -2)
                {
                    winpoint += 3;
                    cntYou++;
                    txtResult.text = "You Win.";
                    winParticle.Play();
                }
                else
                {
                    cntCom++;
                    txtResult.text = "Computer Win.";
                    loseParticle.Play();
                }
                break;

            case GameMode.MukJjiPpa:
                if (k == 0)
                {
                    if (currentAttacker == Attacker.Player)
                    {
                        cntYou++;
                        txtResult.text = "You Win.";
                        winParticle.Play();
                        currentAttacker = Attacker.None;
                        Debug.Log("�ʱ�ȭ�Ǿ����ϴ�.");
                    }
                    else if (currentAttacker == Attacker.Computer)
                    {
                        cntCom++;
                        txtResult.text = "Computer Win.";
                        loseParticle.Play();
                        currentAttacker = Attacker.None;
                        Debug.Log("�ʱ�ȭ�Ǿ����ϴ�.");
                    }
                }

                else if (k == 1 || k == -2)
                {
                    currentAttacker = Attacker.Player;
                    Debug.Log("���� �����ڴ� �÷��̾�");
                }
                else
                {   
                    currentAttacker = Attacker.Computer;
                    Debug.Log("���� �����ڴ� ���");
                }

                break;
        }

        
        SetResult(you, com);
    }

    void SetResult(int you, int com)
    {
        imgYou.texture = Resources.Load("img_"+ you, typeof(Texture)) as Texture;
        imgCom.texture = Resources.Load("img_"+ com, typeof(Texture)) as Texture;

        imgCom.transform.localScale = new Vector3(-1, 1, 1);

        txtDraw.text = cntDraw.ToString();
        txtYou.text = cntYou.ToString();
        txtCom.text = cntCom.ToString();
        txtWinPoint.text = winpoint.ToString();

        GameData_RSP gamedata = new GameData_RSP();
        gamedata.winpoint = winpoint;

        string json = JsonConvert.SerializeObject(gamedata);

        string path = Path.Combine(Application.persistentDataPath, "gameData_RSP.json");
        File.WriteAllText(path, json);
        txtResult.GetComponent<Animator>().Play("TextScale", -1, 0);

    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    public void OnButtonClick(GameObject button)
    {
        AS.clip = btnSound[Random.Range(0, btnSound.Length)];
        AS.Play();
        int you = int.Parse(button.name.Substring(7, 1));
        CheckResult(you);
    }

    public void OnMouseExit(GameObject button)
    {
        Animator anim = button.GetComponent<Animator>();
        anim.Play("Normal");

    }

    public void SetRockPaperScissorsMode()
    {
        SetGameMode(GameMode.RockPaperScissors);
        Debug.Log("modechange to rsp");
    }

    public void SetMukJjiPpaMode()
    {
        SetGameMode(GameMode.MukJjiPpa);
        Debug.Log("modechange to mjp");
    }
}
