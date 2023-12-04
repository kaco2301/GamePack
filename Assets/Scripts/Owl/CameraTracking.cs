using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTracking : MonoBehaviour
{
    [SerializeField]
    Transform target;

    [SerializeField]
    float target_y;

    [SerializeField]
    float camera_y;

    float height;

    // Start is called before the first frame update
    void Start()
    {
            height = target.transform.position.y;
        
        height = target.transform.position.y;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        target_y = target.transform.position.y;

        //목적값까지 보간
        camera_y = transform.position.y;
        camera_y = Mathf.Lerp(camera_y, target_y, 5 * Time.deltaTime);

        //카메라 높이 조정
        Vector3 pos = transform.position;
        pos.y = camera_y;
        transform.position = pos;

        height = target_y;
    }
}
