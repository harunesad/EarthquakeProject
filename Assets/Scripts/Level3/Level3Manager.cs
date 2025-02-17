using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Level3Manager : MonoBehaviour
{
    [SerializeField] Transform player;
    [SerializeField] LayerMask groundLayer;
    Level3UIManager level3UIManager;
    Vector3 target;
    bool move = true;
    RaycastHit hit;
    void Start()
    {
        level3UIManager = FindAnyObjectByType<Level3UIManager>();
    }
    void FixedUpdate()
    {
        player.transform.position += (Time.deltaTime * player.transform.forward * 12);
        player.transform.position = new Vector3(player.transform.position.x, 1, player.transform.position.z);
        //player.transform.Translate(Vector3.forward * Time.deltaTime * 5);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && Physics.Raycast(ray, out hit, 100, groundLayer) && move)
        {
            if (hit.transform.name.EndsWith("X"))
            {
                //level3UIManager.cursor.color = Color.red;
                //move = false;
                //player.DOMoveX(hit.point.x, Mathf.Abs(player.position.x - hit.point.x) / 3).SetEase(Ease.Linear).OnComplete(() =>
                //{
                //    level3UIManager.cursor.color = Color.green;
                //    move = true;
                //});
            }
            else if (hit.transform.name.EndsWith("Z"))
            {
                player.DOMoveZ(hit.point.z, Mathf.Abs(player.position.z - hit.point.z) / 3).SetEase(Ease.Linear);
            }
        }
        target = new Vector3(hit.point.x, player.transform.position.y, player.transform.position.z);
        player.transform.position = Vector3.Lerp(player.transform.position, target, Time.deltaTime);
    }
}
