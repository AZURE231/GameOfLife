using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnClear;
    [SerializeField] private Button btnRandom;
    [SerializeField] private TMP_Text lbStartButton;
    [SerializeField] private GridGenerator gridGenerator;

    [SerializeField] private Toggle togBomb;
    [SerializeField] private Toggle togLaser;
    [SerializeField] private Toggle togPandemic;
    [SerializeField] private GameObject losePopup;


    private void Start()
    {
        gridGenerator.OnLose += GridGenerator_OnLose;

        speedSlider.onValueChanged.AddListener(onSpeedSliderValueChange);
        btnStart.onClick.AddListener(onStartButtonClick);
        btnClear.onClick.AddListener(onClearButtonClick);
        btnRandom.onClick.AddListener(onRandomButtonClick);
        togBomb.onValueChanged.AddListener(delegate
        {
            onToggleBomb();
        });
        togLaser.onValueChanged.AddListener(delegate
        {
            onToggleLaser();
        });
        togPandemic.onValueChanged.AddListener(delegate
        {
            onTogglePandemic();
        });

        speedSlider.value = GameManager.instance.speed;

        setHazard();

    }

    private void GridGenerator_OnLose(object sender, EventArgs e)
    {
        losePopup.SetActive(true);
        StartCoroutine(closeWinAfterTwoSecond());
    }

    IEnumerator closeWinAfterTwoSecond()
    {
        yield return new WaitForSeconds(2f);
        losePopup.SetActive(false);
        GameManager.instance.isPlaying = false;
        lbStartButton.text = "Start";
        gridGenerator.initEmptyNewGridPi();
    }

    private void onSpeedSliderValueChange(float sliderValue)
    {
        GameManager.instance.speed = sliderValue;
        gridGenerator.updateDrawTime(sliderValue);
    }

    private void onStartButtonClick()
    {
        GameManager.instance.isPlaying = !GameManager.instance.isPlaying;
        lbStartButton.text = GameManager.instance.isPlaying ? "Pause" : "Start";
    }

    private void onClearButtonClick()
    {
        gridGenerator.initEmptyNewGridPi();
        GameManager.instance.isPlaying = false;
        lbStartButton.text = "Pause";
    }

    private void onRandomButtonClick()
    {
        gridGenerator.initRandomNewGridPi();
    }

    private void onToggleBomb()
    {
        GameManager.instance.isBombing = togBomb.isOn;
        Debug.Log("Is bomb " + GameManager.instance.isBombing);
    }

    private void onTogglePandemic()
    {
        GameManager.instance.isPandemic = togPandemic.isOn;
        Debug.Log("Is Pan " + GameManager.instance.isPandemic);
    }

    private void onToggleLaser()
    {
        GameManager.instance.isLasering = togLaser.isOn;
        Debug.Log("Is Laser " + GameManager.instance.isLasering);

    }

    private void setHazard()
    {
        GameManager.instance.isBombing = togBomb.isOn;
        GameManager.instance.isLasering = togLaser.isOn;
        GameManager.instance.isPandemic = togPandemic.isOn;
    }
}
