using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level3UIManager : MonoBehaviour
{
    [SerializeField] Button resumeMenuBtn, resumeBtn, resumeCloseBtn, resumeRestartBtn, gameoverRestartBtn, resumeExitBtn, gameoverExitBtn
        , soundOn, soundOff;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] GameObject resumeMenu, gameOverMenu;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect;
    [SerializeField] Level3Manager level3Manager;
    [SerializeField] Vector3 camRot;
    [SerializeField] Image cursor;
    public CanvasGroup bg;
    public Button apply;
    public Info info;
    float time = 180;
    bool timer = false;
    void Start()
    {
        timeText.text = time.ToString();
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
        music.Play();

        resumeMenuBtn.onClick.AddListener(ResumeMenuOpen);
        resumeBtn.onClick.AddListener(ResumeMenuClose);
        resumeRestartBtn.onClick.AddListener(RestartGame);
        resumeExitBtn.onClick.AddListener(ExitGame);
        resumeCloseBtn.onClick.AddListener(ResumeMenuClose);
        gameoverRestartBtn.onClick.AddListener(RestartGame);
        gameoverExitBtn.onClick.AddListener(ExitGame);
        soundOn.onClick.AddListener(delegate { SoundOnOff(true); });
        soundOff.onClick.AddListener(delegate { SoundOnOff(false); });
        apply.onClick.AddListener(Apply);

        Cursor.visible = false;

        //musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        //effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });
    }
    void Update()
    {
        cursor.rectTransform.position = Input.mousePosition;
        if (timer)
        {
            time -= Time.deltaTime;
            timeText.text = ((int)time).ToString();
        }
        if (time <= 0)
        {
            timer = false;
            GameoverOpen();
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
            PlayerPrefs.SetFloat("OnOff", 0);
        }
    }
    void Apply()
    {
        effect.Play();
        //info.info.SetActive(false);
        info.alice.Stop();
        StopAllCoroutines();
        if (info.infoId == 1)
        {
            bg.GetComponent<Image>().DOColor(new Color(0, 0, 0, .5f), 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                info.InfoShowing();
                StartCoroutine(NextInfo());
            });
        }
        else if (info.infoId == 2)
        {
            info.info.SetActive(false);
            bg.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                bg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
                level3Manager.enabled = true;
                timer = true;
            });
        }
    }
    IEnumerator NextInfo()
    {
        yield return new WaitForSeconds(3);
        info.info.SetActive(false);
        bg.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            bg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            level3Manager.enabled = true;
            timer = true;
        });
    }
    //void SoundChanged(Slider slider, AudioSource source, string key)
    //{
    //    PlayerPrefs.SetFloat(key, slider.value);
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
    public void GameoverOpen()
    {
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
