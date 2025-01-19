using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level2UIManager : MonoBehaviour
{
    [SerializeField] Button resumeMenuBtn, resumeBtn, resumeCloseBtn, resumeRestartBtn, gameoverRestartBtn, resumeExitBtn, gameoverExitBtn,
        timeFinishBtn;
    [SerializeField] GameObject resumeMenu, gameOverMenu;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect;
    [SerializeField] CanvasGroup bg;
    [SerializeField] Vector3 camRot;
    [SerializeField] GameObject environment1, environment2;
    public Level2Manager level2Manager;
    public Info info;
    public TextMeshProUGUI timeText;
    float time = 60;
    bool timer = false;
    void Start()
    {
        if (PlayerPrefs.HasKey("Music"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("Music");
        }
        if (PlayerPrefs.HasKey("Effect"))
        {
            effectSlider.value = PlayerPrefs.GetFloat("Effect");
        }
        music.volume = musicSlider.value;
        effect.volume = effectSlider.value;
        music.Play();

        resumeMenuBtn.onClick.AddListener(ResumeMenuOpen);
        resumeBtn.onClick.AddListener(ResumeMenuClose);
        resumeRestartBtn.onClick.AddListener(RestartGame);
        resumeExitBtn.onClick.AddListener(ExitGame);
        resumeCloseBtn.onClick.AddListener(ResumeMenuClose);
        gameoverRestartBtn.onClick.AddListener(RestartGame);
        gameoverExitBtn.onClick.AddListener(ExitGame);
        timeFinishBtn.onClick.AddListener(TimeFinish);

        musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });

        TimerStart();

    }
    void Update()
    {
        if (timer == true)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)time).ToString();
        }
        if (time < 0)
        {
            time = 0;
            timeText.text = ((int)time).ToString();
            timer = false;
            if (info.infoId == 1)
            {
                StartCoroutine(level2Manager.Dropping());
            }
            else if (info.infoId == 2)
            {
                GameoverOpen();
            }
        }
    }
    void ResumeMenuOpen()
    {
        if (!gameOverMenu.activeSelf)
        {
            effect.Play();
            resumeMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
    void ResumeMenuClose()
    {
        effect.Play();
        resumeMenu.SetActive(false);
        Time.timeScale = 1;
    }
    void SoundChanged(Slider slider, AudioSource source, string key)
    {
        PlayerPrefs.SetFloat(key, slider.value);
        source.volume = slider.value;
    }
    void RestartGame()
    {
        effect.Play();
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    void ExitGame()
    {
        effect.Play();
        Application.Quit();
    }
    void TimeFinish()
    {
        if (timer)
        {
            time = 0;
            timeText.text = ((int)time).ToString();
            timer = false;
            StartCoroutine(level2Manager.Dropping());
        }
    }
    public void TimerStart()
    {
        if (!timer)
        {
            time = 60;
            timeText.text = ((int)time).ToString();
            timeText.gameObject.SetActive(true);
            timer = true;
        }
    }
    public void NextLevel()
    {
        bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            timeFinishBtn.gameObject.SetActive(false);
            level2Manager.NextPosition();
            Vector3 camPos = Camera.main.transform.position;
            //Camera.main.transform.position = new Vector3(camPos.x, 7, camPos.z);
            Camera.main.transform.localEulerAngles = camRot;
            environment1.SetActive(false);
            environment2.SetActive(true);
            info.InfoShowing();
            time = 60;
            timeText.text = ((int)time).ToString();
            timeText.gameObject.SetActive(true);
            timer = true;
            bg.DOFade(0, 1).SetEase(Ease.Linear);
        });
    }
    public void CompetitionOpen()
    {
        bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            level2Manager.player.gameObject.SetActive(false);
            environment2.SetActive(false);
            info.QuestionShowing();
            bg.DOFade(0, 1).SetEase(Ease.Linear);
        });
    }
    public void GameoverOpen()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
