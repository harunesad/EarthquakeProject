using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Crash : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] Level2UIManager level2UIManager;
    [SerializeField] Level3UIManager level3UIManager;
    [SerializeField] GameObject environment2;
    [SerializeField] Info info;
    [SerializeField] Button restart;
    [SerializeField] AudioSource alice;
    [SerializeField] AudioClip gameFinish;
    void Start()
    {
        if (restart)
        {
            restart.onClick.AddListener(Restart);
        }
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
        else if (collision.gameObject.layer == 17)
        {
            level3UIManager.GameoverOpen();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        //if (other.gameObject.layer == 12)
        //{
        //    level2UIManager.CompetitionOpen();
        //}
        if (other.gameObject.layer == 13)
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
            if (gameFinish)
            {
                alice.clip = gameFinish;
            }
            alice.Play();
            FindAnyObjectByType<Level3Manager>().enabled = false;
            FindAnyObjectByType<Level3UIManager>().enabled = false;
            level3UIManager.bg.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            level3UIManager.bg.alpha = 1;
            level3UIManager.bg.GetComponent<Image>().DOColor(new Color(0, 0, 0, .5f), 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                //Time.timeScale = 0;
                restart.gameObject.SetActive(true);
                level3UIManager.apply.gameObject.SetActive(false);
                info.InfoShowing();
            });
            //info.InfoChange("Harika Alis! Güvenli bölgedesin! Deprem sýrasýnda sakin kalýp, doðru kararlar alarak güvenli bir yere ulaþtýk.");
        }
        else if (other.gameObject.layer == 16)
        {
            Vector3 obstacleRot = other.transform.parent.localEulerAngles;
            other.transform.parent.DOLocalRotate(new Vector3(obstacleRot.x, obstacleRot.y, 90), .5f).SetEase(Ease.Linear);
        }
    }
    void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
