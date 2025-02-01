using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] List<Button> answers;
    [SerializeField] GameObject objectInfo, turn, player;
    [SerializeField] List<string> infos;
    [SerializeField] Sprite injuredBird;
    [SerializeField] Level1Manager level1Manager;
    [SerializeField] List<Competition> competition;
    [SerializeField] AudioSource alice;
    [SerializeField] List<AudioClip> aliceClips;
    public GameObject info;
    public int infoId;
    int questionId, turnId;
    bool answer;
    TextMeshProUGUI infoText, objectInfoText;
    Button collectBtn;
    TurnProp turnProp;
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

        for (int i = 0; i < answers.Count; i++)
        {
            int j = i;
            answers[i].onClick.AddListener(delegate { ToAnswer(j); });
        }

        if (turn)
        {
            Button right = turn.transform.GetChild(0).GetComponent<Button>();
            Button left = turn.transform.GetChild(1).GetComponent<Button>();
            Button center = turn.transform.GetChild(2).GetComponent<Button>();
            right.onClick.AddListener(TurnRight);
            left.onClick.AddListener(TurnLeft);
            center.onClick.AddListener(ContinueCenter);
        }

        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            StartCoroutine(InfoClose());
        }
    }
    public void InfoShowing()
    {
        if (alice.isPlaying && aliceClips.Count > 0)
        {
            alice.Stop();
            alice.clip = aliceClips[infoId];
            alice.Play();
        }
        else if (!alice.isPlaying && aliceClips.Count > 0)
        {
            alice.clip = aliceClips[infoId];
            alice.Play();
        }
        if (infoId == 2)
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
        info.SetActive(true);
        StartCoroutine(InfoClose());
    }
    public IEnumerator InfoClose()
    {
        yield return new WaitForSeconds(2);
        info.SetActive(false);
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
    public void QuestionShowing()
    {
        if (!info.transform.GetChild(1).gameObject.activeSelf)
        {
            info.transform.GetChild(1).gameObject.SetActive(true);
        }
        infoText.text = competition[questionId].question;
        answers[0].GetComponentInChildren<TextMeshProUGUI>().text = competition[questionId].answers[0];
        answers[1].GetComponentInChildren<TextMeshProUGUI>().text = competition[questionId].answers[1];
        answers[2].GetComponentInChildren<TextMeshProUGUI>().text = competition[questionId].answers[2];
        answers[3].GetComponentInChildren<TextMeshProUGUI>().text = competition[questionId].answers[3];
    }
    void ToAnswer(int answerId)
    {
        if (!answer && questionId < competition.Count)
        {
            if (competition[questionId].correctAnswerId == answerId)
            {
                var answerColor = answers[answerId].colors;
                answerColor.selectedColor = Color.green;
                answerColor.normalColor = Color.green;
                answers[answerId].colors = answerColor;
            }
            else if (competition[questionId].correctAnswerId != answerId)
            {
                var answerColor = answers[answerId].colors;
                answerColor.selectedColor = Color.red;
                answerColor.normalColor = Color.red;
                answers[answerId].colors = answerColor;

                var correctAnswerColor = answers[competition[questionId].correctAnswerId].colors;
                correctAnswerColor.normalColor = Color.green;
                correctAnswerColor.selectedColor = Color.green;
                answers[competition[questionId].correctAnswerId].colors = correctAnswerColor;
            }
            answer = true;
            StartCoroutine(NextQuestion(answerId));
        }
    }
    IEnumerator NextQuestion(int answerId)
    {
        yield return new WaitForSeconds(2);

        if (questionId >= competition.Count - 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            yield break;
        }

        var answerColor = answers[answerId].colors;
        answerColor.selectedColor = Color.white;
        answerColor.normalColor = Color.white;
        answers[answerId].colors = answerColor;

        var correctAnswerColor = answers[competition[questionId].correctAnswerId].colors;
        correctAnswerColor.normalColor = Color.white;
        correctAnswerColor.selectedColor = Color.white;
        answers[competition[questionId].correctAnswerId].colors = correctAnswerColor;
        questionId++;

        answer = false;
        QuestionShowing();
    }
    public void TurnSelectOn(TurnProp turnProp)
    {
        this.turnProp = turnProp;
        //if (turnOptions[turnId].turn[0] == false && turnOptions[turnId].turn[1] == false && turnOptions[turnId].turn[2] == false)
        //{
        //    return;
        //}
        for (int i = 0; i < turn.transform.childCount; i++)
        {
            turn.transform.GetChild(i).gameObject.SetActive(false);
        }
        if (turnProp.right == true)
        {
            turn.transform.GetChild(0).gameObject.SetActive(true);
        }
        if (turnProp.left == true)
        {
            turn.transform.GetChild(1).gameObject.SetActive(true);
        }
        if (turnProp.center == true)
        {
            turn.transform.GetChild(2).gameObject.SetActive(true);
        }
        turn.SetActive(true);
    }
    void TurnRight()
    {
        turn.SetActive(false);
        Time.timeScale = 1;
        Vector3 playerRot = Camera.main.transform.localEulerAngles;
        player.transform.DOLocalRotate(new Vector3(playerRot.x, playerRot.y + 90, playerRot.z), .5f).SetEase(Ease.Linear);
        Vector3 camRot = Camera.main.transform.localEulerAngles;
        Camera.main.transform.DOLocalRotate(new Vector3(camRot.x, camRot.y + 90, camRot.z), .5f).SetEase(Ease.Linear);

        if (turnProp.correctPath[0] == false)
        {
            InfoChange("Burada güvenli deðilsin! Hemen baþka bir yola git!");
        }
        else
        {
            InfoChange("Burada güvenlisin! Bu yoldan devam et!");
        }
    }
    void TurnLeft()
    {
        turn.SetActive(false);
        Time.timeScale = 1;
        Vector3 playerRot = Camera.main.transform.localEulerAngles;
        player.transform.DOLocalRotate(new Vector3(playerRot.x, playerRot.y - 90, playerRot.z), .5f).SetEase(Ease.Linear);
        Vector3 camRot = Camera.main.transform.localEulerAngles;
        Camera.main.transform.DOLocalRotate(new Vector3(camRot.x, camRot.y - 90, camRot.z), .5f).SetEase(Ease.Linear);

        if (turnProp.correctPath[1] == false)
        {
            InfoChange("Burada güvenli deðilsin! Hemen baþka bir yola git!");
        }
        else
        {
            InfoChange("Burada güvenlisin! Bu yoldan devam et!");
        }
    }
    void ContinueCenter()
    {
        turn.SetActive(false);
        Time.timeScale = 1;

        if (turnProp.correctPath[2] == false)
        {
            InfoChange("Burada güvenli deðilsin! Hemen baþka bir yola git!");
        }
        else
        {
            InfoChange("Burada güvenlisin! Bu yoldan devam et!");
        }
    }
}
[System.Serializable]
public class Competition
{
    public string question;
    public List<string> answers;
    public int correctAnswerId;
}
