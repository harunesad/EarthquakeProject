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
                level1UIManager.GameoverOpen("Deprem sýrasýnda yanlýþ yerde durdun.");
            }
            if (level2UIManager)
            {
                level2UIManager.GameoverOpen("Deprem sýrasýnda yanlýþ yerde durdun.");
            }
        }
        else if (collision.gameObject.layer == 17)
        {
            level3UIManager.GameoverOpen();
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
        else if (other.gameObject.layer == 16)
        {
            Vector3 obstacleRot = other.transform.parent.localEulerAngles;
            other.transform.parent.DOLocalRotate(new Vector3(obstacleRot.x, obstacleRot.y, 90), .5f).SetEase(Ease.Linear);
        }
    }
}
