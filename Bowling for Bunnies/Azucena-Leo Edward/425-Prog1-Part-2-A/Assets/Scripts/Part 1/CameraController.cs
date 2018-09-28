using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{

    // Follows the same camera controller as the one in the Roll a Boll 

    public GameObject player;

    private Vector3 offset;

    void Start()
    {
        offset = transform.position - player.transform.position;
        
    }

    void LateUpdate()
    {
        if(transform.position.z < 10.0f)
        {
            transform.position = player.transform.position + offset;
        }
        
    }
}