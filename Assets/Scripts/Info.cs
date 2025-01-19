using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Info : MonoBehaviour
{
    [SerializeField] List<Button> answers;
    [SerializeField] GameObject info, objectInfo;
    [SerializeField] List<string> infos;
    [SerializeField] Sprite injuredBird;
    [SerializeField] Level1Manager level1Manager;
    [SerializeField] List<Competition> competition;
    public int infoId;
    int questionId;
    bool answer;
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

        for (int i = 0; i < answers.Count; i++)
        {
            int j = i;
            answers[i].onClick.AddListener(delegate { ToAnswer(j); });
        }
    }
    public void InfoShowing()
    {
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
}
[System.Serializable]
public class Competition
{
    public string question;
    public List<string> answers;
    public int correctAnswerId;
}
