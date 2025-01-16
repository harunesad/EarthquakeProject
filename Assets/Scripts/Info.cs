using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] GameObject info;
    [SerializeField] List<string> infos;
    [SerializeField] Sprite injuredBird;
    public int infoId;
    TextMeshProUGUI infoText;
    void Start()
    {
        infoText = info.GetComponentInChildren<TextMeshProUGUI>();
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
}
