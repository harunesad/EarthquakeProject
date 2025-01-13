using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] LayerMask hideLayer, bagLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] NavMeshAgent player;
    [SerializeField] List<Vector3> pos;
    RaycastHit hit;
    float time = 10;
    int posId = 0;
    bool move = false, dropTimer = false, bagCollect = false, nextLevel = false;
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
            else if (Physics.Raycast(ray, out hit, 100, bagLayer))
            {
                Debug.Log("Animation is palyþng");
                player.SetDestination(hit.transform.position);
                //move = true;
                bagCollect = true;
            }
        }
        if (dropTimer)
        {
            time -= Time.deltaTime;
            level1UIManager.timeText.text = ((int)time).ToString();
        }
        if (time < 0 && bagCollect && nextLevel)
        {
            nextLevel = true;
            level1UIManager.NextLevel();
        }
        else if (time < 0 && !bagCollect && nextLevel)
        {
            level1UIManager.GameoverOpen();
        }
        //if (move)
        //{
        //    StartCoroutine(Dropping());
        //}
    }
    public IEnumerator Dropping() 
    {
        yield return new WaitForSeconds(1);
        if (!player.hasPath)
        {
            player.isStopped = true;
            //level1UIManager.TimerStart();
            for (int i = 0; i < dropObj.Count; i++)
            {
                dropObj[i].GetComponent<Rigidbody>().useGravity = true;
            }
            move = false;
            dropTimer = true;
        }
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
