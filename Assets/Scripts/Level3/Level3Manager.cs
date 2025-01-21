using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level3Manager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer;
    RaycastHit hit;
    void Start()
    {
        
    }
    void Update()
    {
        player.transform.Translate(Vector3.forward * Time.deltaTime * 5);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            player.DOMoveX(hit.point.x, Mathf.Abs(player.transform.position.x - hit.point.x) / 3).SetEase(Ease.Linear);
            //Vector3 playerPos = player.transform.position;
            //player.transform.position = Vector3.Lerp(playerPos, new Vector3(hit.point.x, playerPos.y, playerPos.z), Time.deltaTime * 10);
        }
    }
}
