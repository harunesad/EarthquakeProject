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
        timeFinishBtn, soundOn, soundOff, apply, a, b;
    [SerializeField] GameObject resumeMenu, gameOverMenu;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect, select;
    [SerializeField] Vector3 camRot;
    [SerializeField] GameObject environment1, environment2;
    [SerializeField] Image cursor;
    public CanvasGroup bg;
    public Level2Manager level2Manager;
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
        a.onClick.AddListener(delegate { Answer(true); });
        b.onClick.AddListener(delegate { Answer(false); });

        Cursor.visible = false;

        //musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        //effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });

        //TimerStart();

    }
    void Update()
    {
        cursor.rectTransform.position = Input.mousePosition;
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
        effect.Play();
        environment1.SetActive(true);
        info.info.SetActive(false);
        info.alice.Stop();
        level2Manager.stop = true;
        StopAllCoroutines();
        bg.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            //apply.gameObject.SetActive(false);
            if (info.infoId == 1)
            {
                StartCoroutine(level2Manager.Dropping());
            }
            else if (info.infoId == 2)
            {
                level2Manager.move = true;
                for (int i = 0; i < level2Manager.selections.Count; i++)
                {
                    level2Manager.selections[i].SetActive(true);
                }
                bg.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            }
            else if (info.infoId == 3)
            {
                for (int i = 0; i < level2Manager.selections.Count; i++)
                {
                    level2Manager.selections[i].SetActive(false);
                }
                info.info.SetActive(false);
                //level2Manager.bagInside.gameObject.SetActive(true);
                bg.gameObject.SetActive(false);
            }
            //TimerStart();
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
        //if (!timer)
        //{
        //    time = 60;
        //    timeText.text = ((int)time).ToString();
        //    timeText.gameObject.SetActive(true);
        //    timer = true;
        //}
    }
    void Answer(bool result)
    {
        environment1.SetActive(true);
        a.gameObject.SetActive(false);
        b.gameObject.SetActive(false);
        if (result)
        {
            bg.GetComponent<Image>().color = new Color(0, 0, 0, .5f);
            CompetitionOpen();
            //bg.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
            //{
            //    CompetitionOpen();
            //});
        }
        else
        {
            bg.alpha = 0;
            info.info.SetActive(false);
            GameoverOpen();
        }
    }
    public void NextLevel()
    {
        bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            //timeFinishBtn.gameObject.SetActive(false);
            apply.gameObject.SetActive(false);
            info.InfoShowing();
            a.gameObject.SetActive(true);
            b.gameObject.SetActive(true);
            //StartCoroutine(info.InfoClose());
            //time = 60;
            //timeText.text = ((int)time).ToString();
            //timeText.gameObject.SetActive(true);
            //timer = true;
        });
    }
    public void CompetitionOpen()
    {
        bg.DOFade(1, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            //level2Manager.player.gameObject.SetActive(false);
            info.QuestionShowing();
            //bg.DOFade(0, 1).SetEase(Ease.Linear);
        });
    }
    public void GameoverOpen()
    {
        //gameOverMenu.GetComponentInChildren<TextMeshProUGUI>().text = description;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
