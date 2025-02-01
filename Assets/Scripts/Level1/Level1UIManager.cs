using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1UIManager : MonoBehaviour
{
    [SerializeField] Button resumeMenuBtn, resumeBtn, resumeCloseBtn, resumeRestartBtn, gameoverRestartBtn, resumeExitBtn, gameoverExitBtn,
        timeFinishBtn, soundOn, soundOff, apply;
    [SerializeField] GameObject resumeMenu, gameOverMenu;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect, select;
    [SerializeField] CanvasGroup bg;
    [SerializeField] Level1Manager level1Manager;
    [SerializeField] Vector3 camRot;
    [SerializeField] GameObject environment1, environment2;
    public Info info;
    public TextMeshProUGUI timeText;
    float time = 60;
    bool timer = false;
    void Start()
    {
        //if (PlayerPrefs.HasKey("Music"))
        //{
        //    musicSlider.value = PlayerPrefs.GetFloat("Music");
        //}
        //if (PlayerPrefs.HasKey("Effect"))
        //{
        //    effectSlider.value = PlayerPrefs.GetFloat("Effect");
        //}
        if (PlayerPrefs.HasKey("OnOff"))
        {
            musicSlider.value = PlayerPrefs.GetFloat("OnOff");
            effectSlider.value = PlayerPrefs.GetFloat("OnOff");
        }
        music.volume = musicSlider.value;
        effect.volume = effectSlider.value;
        select.volume = musicSlider.value;
        music.Play();

        resumeMenuBtn.onClick.AddListener(ResumeMenuOpen);
        resumeBtn.onClick.AddListener(ResumeMenuClose);
        resumeRestartBtn.onClick.AddListener(RestartGame);
        resumeExitBtn.onClick.AddListener(ExitGame);
        resumeCloseBtn.onClick.AddListener(ResumeMenuClose);
        gameoverRestartBtn.onClick.AddListener(RestartGame);
        gameoverExitBtn.onClick.AddListener(ExitGame);
        timeFinishBtn.onClick.AddListener(TimeFinish);
        soundOn.onClick.AddListener(delegate { SoundOnOff(true); });
        soundOff.onClick.AddListener(delegate { SoundOnOff(false); });
        apply.onClick.AddListener(Apply);

        //musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        //effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });

        //TimerStart();
    }
    private void Update()
    {
        if (timer == true)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)time).ToString();
        }
        if (time < 0)
        {
            time = 0;
            //timeText.text = Mathf.FloorToInt(time % 60).ToString();
            timeText.text = ((int)time).ToString();
            timer = false;
            //timeText.gameObject.SetActive(false);
            if (info.infoId == 1)
            {
                StartCoroutine(level1Manager.Dropping());
            }
            else if (info.infoId == 2) 
            {
                GameoverOpen("Gerekli ilk yardým malzemelerini süre bitmeden toplamalýydýn.");
            }
            //NextLevel();
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
    void SoundOnOff(bool on)
    {
        if (on)
        {
            music.volume = 1;
            effect.volume = 1;
            select.volume = 1;
            if (!music.isPlaying)
            {
                music.Play();
            }
            PlayerPrefs.SetFloat("OnOff", 1);
        }
        else
        {
            music.Stop();
            music.volume = 0;
            effect.volume = 0;
            select.volume = 0;
            PlayerPrefs.SetFloat("OnOff", 0);
        }
    }
    void Apply()
    {
        bg.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            info.info.SetActive(false);
            apply.gameObject.SetActive(false);
            TimerStart();
        });
    }
    //void SoundChanged(Slider slider, AudioSource source, string key)
    //{
    //    //PlayerPrefs.SetFloat(key, slider.value);
    //    source.volume = slider.value;
    //}
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
            //timeText.gameObject.SetActive(false);
            StartCoroutine(level1Manager.Dropping());
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
            level1Manager.NextPosition();
            Vector3 camPos = Camera.main.transform.position;
            //Camera.main.transform.position = new Vector3(camPos.x, 7, camPos.z);
            Camera.main.transform.localEulerAngles = camRot;
            environment1.SetActive(false);
            environment2.SetActive(true);
            info.InfoShowing();
            StartCoroutine(info.InfoClose());
            bg.DOFade(0, 1).SetEase(Ease.Linear);
            time = 60;
            timeText.text = ((int)time).ToString();
            timeText.gameObject.SetActive(true);
            timer = true;
        });
    }
    public void GameoverOpen(string description)
    {
        gameOverMenu.GetComponentInChildren<TextMeshProUGUI>().text = description;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
