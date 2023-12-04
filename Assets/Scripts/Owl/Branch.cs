using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
public class Branch : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InitBranch();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y < -30)
        {
            Destroy(gameObject);
        }
    }

    void InitBranch()
    {
        float sx = Random.Range(0.5f, 1);

        int x = (Random.Range(0, 2) == 0) ? -1 : 1;
        int y = (Random.Range(0, 2) == 0) ? -1 : 1;

        transform.localScale = new Vector3(sx * x, y, 1);
    }
}
