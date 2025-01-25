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
    public NavMeshAgent player;
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
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                //move = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, bagLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
                //move = true;
                bagCollect = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, collectLayer) && collectCount != 0)
            {
                player.isStopped = false;
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
                //move = true;
                level1UIManager.info.ObjectInfoChange(hit.transform.name + "\n" + hit.transform.GetComponent<ObjectProp>().prop);
                collectObj = hit.transform;
                trueObj = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, notCollectLayer) && collectCount != 0)
            {
                player.isStopped = false;
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
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
            player.isStopped = true;
            dropTimer = false;
            level1UIManager.timeText.gameObject.SetActive(false);
            nextLevel = true;
            level1UIManager.NextLevel();
        }
        else if (time < 0 && !bagCollect && !nextLevel)
        {
            level1UIManager.GameoverOpen("Çantayý toplamalýydýn.");
        }
        if (collectCount == 0)
        {
            StartCoroutine(SceneLoad());
        }
    }
    IEnumerator SceneLoad()
    {
        yield return new WaitForSeconds(1);
        level1UIManager.info.InfoChange("Tebrikler");
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator Dropping() 
    {
        yield return new WaitForSeconds(1);
        //if (!player.hasPath)
        //{
        //    player.isStopped = true;
        //    for (int i = 0; i < dropObj.Count; i++)
        //    {
        //        dropObj[i].GetComponent<Rigidbody>().useGravity = true;
        //    }
        //    move = false;
        //    dropTimer = true;
        //}
        for (int i = 0; i < dropObj.Count; i++)
        {
            dropObj[i].GetComponent<Rigidbody>().useGravity = true;
            dropObj[i].GetComponent<Rigidbody>().isKinematic = false;
        }
        dropTimer = true;

        Camera.main.GetComponent<CameraShaker>().enabled = true;
        CameraShaker.Instance.StartShake(2, 4, .1f);
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
