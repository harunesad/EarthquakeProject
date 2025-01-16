using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] Level2UIManager level2UIManager;

    void Start()
    {
        
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 6)
        {
            if (level1UIManager)
            {
                level1UIManager.GameoverOpen();
            }
            if (level2UIManager)
            {
                level2UIManager.GameoverOpen();
            }
        }
    }
}
