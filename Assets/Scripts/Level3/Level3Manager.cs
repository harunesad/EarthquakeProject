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
        player.transform.position += (Time.deltaTime * player.transform.forward * 5);
        player.transform.position = new Vector3(player.transform.position.x, .25f, player.transform.position.z);
        //player.transform.Translate(Vector3.forward * Time.deltaTime * 5);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100, groundLayer))
        {
            if (hit.transform.name.EndsWith("X"))
            {
                player.DOMoveX(hit.point.x, Mathf.Abs(player.position.x - hit.point.x) / 3).SetEase(Ease.Linear);
            }
            else if (hit.transform.name.EndsWith("Z"))
            {
                player.DOMoveZ(hit.point.z, Mathf.Abs(player.position.z - hit.point.z) / 3).SetEase(Ease.Linear);
            }
        }
    }
}
