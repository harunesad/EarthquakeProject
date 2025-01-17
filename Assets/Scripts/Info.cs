using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] GameObject info, objectInfo;
    [SerializeField] List<string> infos;
    [SerializeField] Sprite injuredBird;
    [SerializeField] Level1Manager level1Manager;
    public int infoId;
    TextMeshProUGUI infoText, objectInfoText;
    Button collectBtn;
    void Start()
    {
        infoText = info.GetComponentInChildren<TextMeshProUGUI>();
        if (objectInfo)
        {
            objectInfoText = objectInfo.GetComponentInChildren<TextMeshProUGUI>();
            collectBtn = objectInfo.GetComponentInChildren<Button>();
            collectBtn.onClick.AddListener(Collect);
        }
        InfoShowing();
    }
    public void InfoShowing()
    {
        //Time.timeScale = 0;
        if (infoId == 1)
        {
            info.GetComponent<Image>().sprite = injuredBird;
        }
        infoText.text = infos[infoId];
        info.SetActive(true);
        infoId++;
    }
    public void InfoChange(string message)
    {
        infoText.text = message;
    }
    public void ObjectInfoChange(string message)
    {
        objectInfo.SetActive(true);
        objectInfoText.text = message;
    }
    void Collect()
    {
        level1Manager.CollectableObj();
        objectInfo.SetActive(false);
    }
}
