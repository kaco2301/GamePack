using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void owl()
    {
        SceneManager.LoadScene("OwlScene");
    }
    public void rsp()
    {
        SceneManager.LoadScene("RSPScene");
    }
    public void snake()
    {
        SceneManager.LoadScene("SnakeScene");
    }
    public void GetOut()
    {
        Application.Quit();
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("MainScene");
    }

}
