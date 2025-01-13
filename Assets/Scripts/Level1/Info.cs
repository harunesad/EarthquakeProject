using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Info : MonoBehaviour
{
    [SerializeField] GameObject info;
    [SerializeField] List<string> infos;
    int infoId;
    TextMeshProUGUI infoText;
    void Start()
    {
        infoText = info.GetComponentInChildren<TextMeshProUGUI>();
        InfoShowing();
    }
    public void InfoShowing()
    {
        //Time.timeScale = 0;
        infoText.text = infos[infoId];
        info.SetActive(true);
        infoId++;
    }
}
