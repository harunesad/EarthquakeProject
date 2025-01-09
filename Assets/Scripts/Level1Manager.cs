using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] LayerMask hideLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] NavMeshAgent player;
    [SerializeField] List<Vector3> pos;
    RaycastHit hit;
    float time = 0;
    int posId = 0;
    bool move = false, timer = false;
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
                move = true;
            }
        }
        if (move)
        {
            StartCoroutine(Dropping());
        }
    }
    IEnumerator Dropping() 
    {
        yield return new WaitForSeconds(1);
        if (!player.hasPath)
        {
            player.isStopped = true;
            level1UIManager.TimerStart();
            for (int i = 0; i < dropObj.Count; i++)
            {
                dropObj[i].GetComponent<Rigidbody>().useGravity = true;
            }
            move = false;
        }
    }
    public void NextPosition()
    {
        player.transform.position = pos[posId++];
    }
    //public void RunFinished()
    //{
    //    Debug.Log("Hide is playing");
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
