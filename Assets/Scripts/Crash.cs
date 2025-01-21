using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crash : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] Level2UIManager level2UIManager;
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
            Debug.Log("s");
            level2UIManager.CompetitionOpen();
        }
        else if (other.gameObject.layer == 13)
        {
            transform.DOLocalRotate(new Vector3(transform.rotation.x, -90, transform.rotation.z), .5f).SetEase(Ease.Linear);
            Vector3 camRot = Camera.main.transform.localEulerAngles;
            Camera.main.transform.DOLocalRotate(new Vector3(camRot.x, -90, camRot.z), .5f).SetEase(Ease.Linear);
        }
    }
}
