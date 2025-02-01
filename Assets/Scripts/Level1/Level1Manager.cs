using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using EZCameraShake;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] LayerMask hideLayer, bagLayer, collectLayer, notCollectLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] List<Vector3> pos;
    [SerializeField] int collectCount;
    [SerializeField] AudioSource selectSource, alice;
    [SerializeField] AudioClip trueSelect, falseSelect, finish;
    public Transform player;
    RaycastHit hit;
    Transform collectObj;
    float time = 10;
    int posId;
    bool move = false, dropTimer = false, bagCollect = false, nextLevel = false, trueObj = false;
    void Start()
    {
        
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !move && !dropTimer)
        {
            if (Physics.Raycast(ray, out hit, 100, hideLayer))
            {
                player.position = hit.transform.GetComponent<HideProp>().pos;
                //player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                //move = true;
                if (hit.transform.name == "table")
                {
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
                }
                else
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
                }
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
                hit.transform.gameObject.SetActive(false);
                //player.SetDestination(hit.transform.position);
                //move = true;
                bagCollect = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, collectLayer) && collectCount != 0)
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
                //player.isStopped = false;
                player.position = hit.transform.position;
                //player.SetDestination(hit.transform.position);
                //move = true;
                level1UIManager.info.ObjectInfoChange(hit.transform.name + "\n" + hit.transform.GetComponent<ObjectProp>().prop);
                collectObj = hit.transform;
                trueObj = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, notCollectLayer) && collectCount != 0)
            {
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
                //player.isStopped = false;
                player.position = hit.transform.position;
                //player.SetDestination(hit.transform.position);
                //move = true;
                level1UIManager.info.ObjectInfoChange(hit.transform.name + "\n" + hit.transform.GetComponent<ObjectProp>().prop);
                collectObj = hit.transform;
                trueObj = false;
            }
        }
        if (dropTimer)
        {
            time -= Time.deltaTime;
            level1UIManager.timeText.text = ((int)time).ToString();
        }
        if (time < 0 && bagCollect && !nextLevel)
        {
            Camera.main.GetComponent<CameraShaker>().enabled = false;
            move = false;
            //player.isStopped = true;
            dropTimer = false;
            level1UIManager.timeText.gameObject.SetActive(false);
            nextLevel = true;
            level1UIManager.NextLevel();
        }
        else if (time < 0 && !bagCollect && !nextLevel)
        {
            level1UIManager.GameoverOpen("Çantayý almalýydýn.");
        }
        if (collectCount == 0)
        {
            StartCoroutine(SceneLoad());
        }
    }
    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(1);
        if (finish)
        {
            alice.clip = finish;
            alice.Play();
        }
        level1UIManager.info.InfoChange("Tebrikler");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator Dropping() 
    {
        yield return new WaitForSeconds(1);
        for (int i = 0; i < dropObj.Count; i++)
        {
            dropObj[i].GetComponent<Rigidbody>().useGravity = true;
            dropObj[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        dropTimer = true;

        Camera.main.GetComponent<CameraShaker>().enabled = true;
        CameraShaker.Instance.StartShake(.5f, 4, .1f);
        level1UIManager.info.InfoShowing();
        StartCoroutine(level1UIManager.info.InfoClose());
    }
    public void NextPosition()
    {
        player.transform.position = pos[posId++];
    }
    public void CollectableObj()
    {
        if (trueObj)
        {
            collectObj.gameObject.SetActive(false);
            level1UIManager.info.InfoChange(collectObj.name + " doðru");
            collectCount--;
        }
        else
        {
            level1UIManager.info.InfoChange(collectObj.name + " yanlýþ");
        }
    }
}
