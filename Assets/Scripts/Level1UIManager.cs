using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level1UIManager : MonoBehaviour
{
    [SerializeField] Button resumeMenuBtn, resumeBtn, resumeCloseBtn, resumeRestartBtn, gameoverRestartBtn, resumeExitBtn, gameoverExitBtn;
    [SerializeField] GameObject resumeMenu, gameOverMenu;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect;
    [SerializeField] Info info;
    [SerializeField] CanvasGroup bg;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] Level1Manager level1Manager;
    float time = 0;
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

        musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });
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
            timeText.gameObject.SetActive(false);
            NextLevel();
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
    public void TimerStart()
    {
        if (!timer)
        {
            time = 5;
            Debug.Log("a");
            timeText.text = ((int)time).ToString();
            timeText.gameObject.SetActive(true);
            timer = true;
        }
    }
    public void NextLevel()
    {
        bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            level1Manager.NextPosition();
            info.InfoShowing();
            bg.DOFade(0, 1).SetEase(Ease.Linear);
        });
    }
    public void GameoverOpen()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
