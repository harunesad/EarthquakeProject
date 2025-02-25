using DG.Tweening;
using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] Level2UIManager level2UIManager;
    [SerializeField] LayerMask hideLayer, bagLayer, trapLayer, groundLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] List<Vector3> pos;
    [SerializeField] GameObject ground;
    [SerializeField] AudioSource selectSource;
    [SerializeField] AudioClip trueSelect, falseSelect, safe, notSafe;
    [SerializeField] string earthquakeInfo, lifeInfo;
    public AudioSource alice;
    public Transform player;
    public List<GameObject> selections;
    public bool move = false, stop = false;
    RaycastHit hit;
    float time = 10;
    int posId;
    bool dropTimer = false, bagCollect = false, nextLevel = false;
    void Start()
    {
        
    }
    void Update()
    {
        if (stop)
        {
            StopAllCoroutines();
            stop = false;
        }
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && move && !dropTimer)
        {
            if (Physics.Raycast(ray, out hit, 100, hideLayer))
            {
                //level2UIManager.moveInfo.GetComponentInChildren<TextMeshProUGUI>().text = lifeInfo;
                //level2UIManager.moveInfo.SetActive(true);
                hit.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                if (selectSource.isPlaying)
                {
                    selectSource.Stop();
                    selectSource.clip = trueSelect;
                    selectSource.Play();
                }
                else
                {
                    selectSource.clip = trueSelect;
                    selectSource.Play();
                }
                //player.position = hit.transform.GetComponent<HideProp>().pos;
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                move = false;
                //if (alice.isPlaying && safe)
                //{
                //    alice.Stop();
                //    alice.clip = safe;
                //    alice.Play();
                //}
                //else if (!alice.isPlaying && safe)
                //{
                //    alice.clip = safe;
                //    alice.Play();
                //}
                level2UIManager.NextLevel();
                //level2UIManager.info.InfoChange("Alice, yaþam üçgenini doðru uyguladýn!");
            }
            else if (Physics.Raycast(ray, out hit, 100, trapLayer))
            {
                for (int i = 0; i < selections.Count; i++)
                {
                    selections[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                hit.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                if (selectSource.isPlaying)
                {
                    selectSource.Stop();
                    selectSource.clip = falseSelect;
                    selectSource.Play();
                }
                else
                {
                    selectSource.clip = falseSelect;
                    selectSource.Play();
                }
                //player.position = hit.transform.GetComponent<HideProp>().pos;
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                //move = false;
                //if (alice.isPlaying && notSafe)
                //{
                //    alice.Stop();
                //    alice.clip = notSafe;
                //    alice.Play();
                //}
                //else if (!alice.isPlaying && notSafe)
                //{
                //    alice.clip = notSafe;
                //    alice.Play();
                //}
                ////level2UIManager.GameoverOpen();
                //level2UIManager.info.InfoChange("Alis, burasý güvenli deðil! Sýranýn yanýna geç ve baþýný koru!");
            }
            else if (Physics.Raycast(ray, out hit, 100, bagLayer))
            {
                if (selectSource.isPlaying)
                {
                    selectSource.Stop();
                    selectSource.clip = trueSelect;
                    selectSource.Play();
                }
                else
                {
                    selectSource.clip = trueSelect;
                    selectSource.Play();
                }
                player.position = hit.transform.GetComponent<HideProp>().pos;
                //player.gameObject.SetActive(true);
                hit.transform.parent.gameObject.SetActive(false);
                //player.SetDestination(hit.point);
                //move = true;
                bagCollect = true;
            }
            //else if (Physics.Raycast(ray, out hit, 100, groundLayer))
            //{
            //    player.isStopped = false;
            //    player.SetDestination(hit.point);
            //    //move = true;
            //}
        }
        if (dropTimer)
        {
            time -= Time.deltaTime;
            level2UIManager.timeText.text = ((int)time).ToString();
        }
        if (time < 0 && bagCollect && !nextLevel)
        {
            Camera.main.GetComponent<CameraShaker>().enabled = false;
            ground.layer = 11;
            move = false;
            //player.isStopped = true;
            dropTimer = false;
            level2UIManager.timeText.gameObject.SetActive(false);
            nextLevel = true;
            level2UIManager.NextLevel();
            for (int i = 0; i < dropObj.Count; i++)
            {
                dropObj[i].layer = 0;
            }
        }
        else if (time < 0 && !bagCollect && !nextLevel)
        {
            level2UIManager.GameoverOpen();
        }
    }
    public IEnumerator Dropping()
    {
        yield return new WaitForSeconds(1);
        //if (!player.gameObject.activeSelf)
        //{
        //    level2UIManager.GameoverOpen("Saklanmak için bir yer seçmelisin");
        //    yield break;
        //}
        //for (int i = 0; i < dropObj.Count; i++)
        //{
        //    dropObj[i].GetComponent<Rigidbody>().useGravity = true;
        //}
        //dropTimer = true;

        Camera.main.GetComponent<CameraShaker>().enabled = true;
        CameraShaker.Instance.StartShake(.5f, 4, .1f);
        //level2UIManager.apply.gameObject.SetActive(false);
        yield return new WaitForSeconds(2);
        Camera.main.GetComponent<CameraShaker>().enabled = false;
        yield return new WaitForSeconds(1);
        level2UIManager.bg.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
        level2UIManager.bg.DOFade(1, 1).OnComplete(() =>
        {
            //level2UIManager.moveInfo.GetComponentInChildren<TextMeshProUGUI>().text = earthquakeInfo;
            level2UIManager.moveInfo.SetActive(true);
            level2UIManager.info.InfoShowing();
        });
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(alice.clip.length);
        level2UIManager.apply.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        level2UIManager.info.info.SetActive(false);
        level2UIManager.moveInfo.SetActive(false);
        level2UIManager.bg.DOFade(0, 1).OnComplete(() =>
        {
            move = true;
            for (int i = 0; i < selections.Count; i++)
            {
                selections[i].SetActive(true);
            }
            level2UIManager.bg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        });
    }
    public void NextPosition()
    {
        player.transform.position = pos[posId++];
    }
    //public void RunFinished()
    //{
    //    Debug.Log("Hide is playing");
    //    move = false
    //}
    //public void HideFinished()
    //{
    //    //Earthquake started
    //    level1UIManager.TimerStart();
    //    for (int i = 0; i < dropObj.Count; i++)
    //    {
    //        dropObj[i].GetComponent<Rigidbody>().useGravity = true;
    //    }
    //}
}
