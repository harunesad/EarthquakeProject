using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] Level2UIManager level2UIManager;
    [SerializeField] Level3UIManager level3UIManager;
    [SerializeField] GameObject environment2;
    [SerializeField] Info info;

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
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            level2UIManager.CompetitionOpen();
        }
        else if (other.gameObject.layer == 13)
        {
            Time.timeScale = 0;
            info.TurnSelectOn(other.GetComponent<TurnProp>());
        }
        else if (other.gameObject.layer == 14)
        {
            level3UIManager.GameoverOpen();
        }
        else if (other.gameObject.layer == 15)
        {
            Time.timeScale = 0;
        }
    }
}
