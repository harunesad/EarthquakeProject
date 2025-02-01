using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class MenuUIManager : MonoBehaviour
{
    [SerializeField] CanvasGroup main, settings, credits, writers;
    [SerializeField] Button backBtn, playBtn, settingsBtn, creditsBtn, writersBtn, soundOn, soundOff;
    [SerializeField] Slider musicSlider, effectSlider;
    [SerializeField] AudioSource music, effect;
    CanvasGroup currentGroup;
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
        music.Play();

        currentGroup = main;
        backBtn.onClick.AddListener(Back);
        playBtn.onClick.AddListener(PlayGame);
        settingsBtn.onClick.AddListener(delegate { GroupChange(settings); });
        creditsBtn.onClick.AddListener(delegate { GroupChange(credits); });
        writersBtn.onClick.AddListener(delegate { GroupChange(writers); });
        soundOn.onClick.AddListener(delegate { SoundOnOff(true); });
        soundOff.onClick.AddListener(delegate { SoundOnOff(false); });

        //musicSlider.onValueChanged.AddListener(delegate { SoundChanged(musicSlider, music, "Music"); });
        //effectSlider.onValueChanged.AddListener(delegate { SoundChanged(effectSlider, effect, "Effect"); });
    }
    void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    void GroupChange(CanvasGroup group)
    {
        effect.Play();
        currentGroup.blocksRaycasts = false;
        currentGroup.interactable = false;
        currentGroup.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
        {
            group.blocksRaycasts = true;
            group.interactable = true;
            currentGroup = group;
            group.DOFade(1, 1).SetEase(Ease.Linear);
        });
        if (group != main)
        {
            backBtn.transform.parent = group.transform;
            backBtn.gameObject.SetActive(true);
        }
        else
        {
            backBtn.gameObject.SetActive(false);
        }
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
    //void SoundChanged(Slider slider, AudioSource source, string key)
    //{
    //    //PlayerPrefs.SetFloat(key, slider.value);
    //    source.volume = slider.value;
    //}
    void Back()
    {
        if (currentGroup != main)
        {
            effect.Play();
            currentGroup.blocksRaycasts = false;
            currentGroup.interactable = false;
            currentGroup.DOFade(0, 1).SetEase(Ease.Linear).OnComplete(() =>
            {
                main.blocksRaycasts = true;
                main.interactable = true;
                currentGroup = main;
                main.DOFade(1, 1).SetEase(Ease.Linear);
            });
        }
    }
}
