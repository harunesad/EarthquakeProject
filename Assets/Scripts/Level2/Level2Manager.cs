using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] Level2UIManager level2UIManager;
    [SerializeField] LayerMask hideLayer, bagLayer, trapLayer, groundLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] List<Vector3> pos;
    [SerializeField] GameObject ground;
    public NavMeshAgent player;
    RaycastHit hit;
    float time = 10;
    int posId;
    bool move = false, dropTimer = false, bagCollect = false, nextLevel = false;
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
                level2UIManager.info.InfoChange("Alice, burasý güvenli!");
            }
            else if (Physics.Raycast(ray, out hit, 100, trapLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                //move = true;
                level2UIManager.info.InfoChange("Alice, burasý güvenli deðil! Sýranýn altýna geç ve baþýný koru!");
            }
            else if (Physics.Raycast(ray, out hit, 100, bagLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.point);
                //move = true;
                bagCollect = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, groundLayer))
            {
                player.isStopped = false;
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.point);
                //move = true;
            }
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
            player.isStopped = true;
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
        }
        dropTimer = true;

        Camera.main.GetComponent<CameraShaker>().enabled = true;
        CameraShaker.Instance.StartShake(2, 4, .1f);
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
