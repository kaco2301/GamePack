using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreText : MonoBehaviour
{
    public TextMeshProUGUI txtScore;
    float speed = 1f;

    private void Start()
    {
        StartCoroutine(FadeOut());
    }

    private void Update()
    {
        float amount = speed * Time.deltaTime;
        transform.Translate(Vector3.up * amount);
    }

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(1f);
        Color color = txtScore.color;

        for (float alpha = 1; alpha > 0; alpha -= 0.02f)
        {
            color.a = alpha;
            txtScore.color = color;

            yield return null;
        }

        Destroy(gameObject);
    }

    void SetScore(int score)
    {
        txtScore.text = score.ToString("+0; -0");

        if (score < 0)
        {
            txtScore.color = Color.red;
        }
    }
}