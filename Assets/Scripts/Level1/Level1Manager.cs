using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using EZCameraShake;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Level1Manager : MonoBehaviour
{
    [SerializeField] Level1UIManager level1UIManager;
    [SerializeField] LayerMask hideLayer, bagLayer, collectLayer, notCollectLayer;
    [SerializeField] List<GameObject> dropObj;
    [SerializeField] List<Vector3> pos;
    [SerializeField] AudioSource selectSource;
    [SerializeField] AudioClip trueSelect, falseSelect, finish;
    [SerializeField] string earthquakeInfo, bagInfo;
    public AudioSource alice;
    public GameObject bagInside;
    public List<GameObject> selections;
    public Transform player;
    public bool bagCollect = false, stop = false;
    public int collectCount;
    RaycastHit hit;
    Transform collectObj;
    float time = 10;
    int posId;
    bool move = false, dropTimer = false, nextLevel = false, trueObj = false;
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
        if (Input.GetMouseButtonDown(0) && !dropTimer)
        {
            if (Physics.Raycast(ray, out hit, 100, hideLayer) && bagCollect && !move)
            {
                for (int i = 0; i < selections.Count; i++)
                {
                    selections[i].transform.GetChild(0).gameObject.SetActive(false);
                }
                level1UIManager.moveýnfo.SetActive(false);
                level1UIManager.earthquakeInfo.SetActive(false);
                hit.transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                //player.position = hit.transform.GetComponent<HideProp>().pos;
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.GetComponent<HideProp>().pos);
                if (hit.transform.name == "table" || hit.transform.name == "carpet" || hit.transform.name == "door")
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
                    ////level1UIManager.GameoverOpen();
                }
                else
                {
                    move = true;
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
                    //level1UIManager.apply.gameObject.SetActive(false);
                    level1UIManager.bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
                    {
                        level1UIManager.earthquakeInfo.GetComponentInChildren<TextMeshProUGUI>().text = bagInfo;
                        level1UIManager.earthquakeInfo.SetActive(true);
                        Camera.main.transform.localEulerAngles = new Vector3(0, 0, 0);
                        level1UIManager.info.InfoShowing();
                        StartCoroutine(BagInside());
                    });
                }
            }
            else if (Physics.Raycast(ray, out hit, 100, bagLayer) && !level1UIManager.info.info.activeSelf && !move)
            {
                //level1UIManager.apply.gameObject.SetActive(false);
                hit.transform.gameObject.layer = 0;
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
                player.position = hit.transform.GetComponent<HideProp>().pos;
                StartCoroutine(BagMissing(hit));
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.position);
                //move = true;
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
                //player.position = hit.transform.position;
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.position);
                //move = true;
                level1UIManager.info.ObjectInfoChange(hit.transform.name + "\n" + hit.transform.GetComponent<ObjectProp>().prop);
                collectObj = hit.transform;
                collectObj.gameObject.SetActive(false);
                trueObj = true;
                collectCount--;
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
                //player.position = hit.transform.position;
                //player.gameObject.SetActive(true);
                //player.SetDestination(hit.transform.position);
                //move = true;
                level1UIManager.info.ObjectInfoChange(hit.transform.name + "\n" + hit.transform.GetComponent<ObjectProp>().prop);
                collectObj = hit.transform;
                collectObj.gameObject.SetActive(false);
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
            level1UIManager.GameoverOpen();
        }
        if (collectCount == 0)
        {
            //level1UIManager.apply.gameObject.SetActive(false);
            level1UIManager.bg.alpha = 0;
            level1UIManager.bg.gameObject.SetActive(true);
            level1UIManager.bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                bagInside.SetActive(false);
                collectCount = -1;
                level1UIManager.info.InfoShowing();
                StartCoroutine(SceneLoad());
            });
        }
    }
    public IEnumerator BagMissing(RaycastHit hit)
    {
        yield return new WaitForSeconds(1);
        hit.transform.parent.gameObject.SetActive(false);
        level1UIManager.bg.DOFade(1, 1).OnComplete(() =>
        {
            level1UIManager.earthquakeInfo.GetComponentInChildren<TextMeshProUGUI>().text = earthquakeInfo;
            level1UIManager.earthquakeInfo.SetActive(true);
            level1UIManager.info.InfoShowing();
        });
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(alice.clip.length - 1);
        level1UIManager.apply.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        level1UIManager.info.info.SetActive(false);
        level1UIManager.bg.DOFade(0, 1).OnComplete(() =>
        {
            StartCoroutine(Dropping());
        });
        yield return new WaitForSeconds(2);
        level1UIManager.earthquakeInfo.SetActive(false);
        //level1UIManager.apply.gameObject.SetActive(false);
        level1UIManager.bg.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
        level1UIManager.bg.DOFade(1, 1).OnComplete(() =>
        {
            level1UIManager.moveýnfo.SetActive(true);
            level1UIManager.info.InfoShowing();
        });
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(alice.clip.length);
        level1UIManager.apply.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        level1UIManager.info.info.SetActive(false);
        level1UIManager.bg.DOFade(0, 1).OnComplete(() =>
        {
            for (int i = 0; i < selections.Count; i++)
            {
                selections[i].SetActive(true);
            }
            level1UIManager.bg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            bagCollect = true;
        });
    }
    IEnumerator BagInside()
    {
        Camera.main.GetComponent<CameraShaker>().enabled = false;
        yield return new WaitForSeconds(alice.clip.length);
        level1UIManager.apply.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        for (int i = 0; i < selections.Count; i++)
        {
            selections[i].SetActive(false);
        }
        level1UIManager.info.info.SetActive(false);
        bagInside.gameObject.SetActive(true);
        level1UIManager.bg.gameObject.SetActive(false);
    }
    IEnumerator SceneLoad()
    {
        //yield return new WaitForSeconds(1);
        //if (finish)
        //{
        //    alice.clip = finish;
        //    alice.Play();
        //}
        //level1UIManager.info.InfoChange("Tebrikler");
        yield return new WaitForSeconds(alice.clip.length);
        level1UIManager.apply.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public IEnumerator Dropping() 
    {
        yield return new WaitForSeconds(0);
        //if (!player.gameObject.activeSelf)
        //{
        //    level1UIManager.GameoverOpen("Saklanmak için bir yer seçmelisin");
        //    yield break;
        //}
        //for (int i = 0; i < dropObj.Count; i++)
        //{
        //    dropObj[i].GetComponent<Rigidbody>().useGravity = true;
        //    dropObj[i].GetComponent<Rigidbody>().isKinematic = false;
        //}
        //dropTimer = true;

        Camera.main.GetComponent<CameraShaker>().enabled = true;
        CameraShaker.Instance.StartShake(.5f, 4, .1f);
        //StartCoroutine(level1UIManager.info.InfoClose());
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
