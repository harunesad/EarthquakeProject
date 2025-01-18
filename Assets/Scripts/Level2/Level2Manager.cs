using EZCameraShake;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level2Manager : MonoBehaviour
{
    [SerializeField] Level2UIManager level2UIManager;
    [SerializeField] LayerMask hideLayer, trapLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] List<Vector3> pos;
    public NavMeshAgent player;
    RaycastHit hit;
    float time = 10;
    int posId;
    bool move = false, dropTimer = false, nextLevel = false;
    void Start()
    {
        
    }
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0) && !move)
        {
            if (Physics.Raycast(ray, out hit, 100, hideLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
                //move = true;
            }
            else if (Physics.Raycast(ray, out hit, 100, trapLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
                //move = true;
                level2UIManager.info.InfoChange("Alice, burasý güvenli deðil! Sýranýn altýna geç ve baþýný koru!");
            }
        }
        if (dropTimer)
        {
            time -= Time.deltaTime;
            level2UIManager.timeText.text = ((int)time).ToString();
        }
        if (time < 0 && !nextLevel)
        {
            Camera.main.GetComponent<CameraShaker>().enabled = false;
            move = false;
            player.isStopped = true;
            dropTimer = false;
            level2UIManager.timeText.gameObject.SetActive(false);
            nextLevel = true;
            level2UIManager.NextLevel();
        }
        else if (time < 0 && !nextLevel)
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
