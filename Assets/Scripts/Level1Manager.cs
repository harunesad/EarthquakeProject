using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] LayerMask hideLayer, trapLayer;
    RaycastHit hit;
    void Start()
    {
        
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit, 100, hideLayer))
            {
                Debug.Log("Animation is palyþng");
                hit.transform.gameObject.layer = 0;
                level1UIManager.NextLevel();
            }
            else if (Physics.Raycast(ray, out hit, 100, trapLayer))
            {
                level1UIManager.GameoverOpen();
            }
        }
    }
}
