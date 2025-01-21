using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] Vector3 offset;
    void Start()
    {
        
    }
    void LateUpdate()
    {
        Vector3 newOffset = new Vector3(offset.x * player.transform.forward.x, offset.y, offset.z * player.transform.forward.z);
        transform.position = Vector3.Lerp(transform.position, player.position + newOffset, Time.deltaTime * 100);
    }
}
