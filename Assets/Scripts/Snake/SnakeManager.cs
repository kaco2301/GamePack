using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SnakeManager : MonoBehaviour
{
    [SerializeField]
    GameObject Snake;

    [SerializeField]
    GameObject panelDifficulty;
    [SerializeField]
    TextMeshProUGUI Mode;

    public float speedRate;



    void Awake()
    {
        Time.timeScale = 0;
        panelDifficulty.SetActive(true);

    }
    public void OnButtonClick(Button button)
    {
        switch (button.name)
        {

            case "Button_Hard":
                speedRate = 1.3f;
                Mode.text = "Hard Mode";
                Mode.color = Color.red;
                break;
            case "Button_Normal":
                speedRate = 1.2f;
                Mode.text = "Normal Mode";
                Mode.color = new Color(1f, 0.33f, 0f);
                break;
            case "Button_Easy":
                speedRate = 1.1f;
                Mode.text = "Easy Mode";
                Mode.color = new Color(1f, 0.67f, 0f);
                break;
        }
        Time.timeScale = 1;
        panelDifficulty.SetActive(false);
    }





    void Update()
    {

    }
}
