using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    Material mat;
    float speed = 0.05f;

    private void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    private void Update()
    {
        Vector2 ofs = mat.mainTextureOffset;
        ofs.x += speed * Time.deltaTime;

        mat.mainTextureOffset = ofs;
    }
}
