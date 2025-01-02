using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider speedSlider;
    [SerializeField] private Button btnStart;
    [SerializeField] private Button btnClear;
    [SerializeField] private Button btnRandom;
    [SerializeField] private TMP_Text lbStartButton;
    [SerializeField] private GridGenerator gridGenerator;

    private void Start()
    {
        speedSlider.onValueChanged.AddListener(onSpeedSliderValueChange);
        btnStart.onClick.AddListener(onStartButtonClick);
        btnClear.onClick.AddListener(onClearButtonClick);
        btnRandom.onClick.AddListener(onRandomButtonClick);

        speedSlider.value = GameManager.instance.speed;
    }

    private void Update()
    {
        lbStartButton.text = GameManager.instance.isPlaying ? "Pause" : "Start";
    }

    private void onSpeedSliderValueChange(float sliderValue)
    {
        GameManager.instance.speed = sliderValue;
        gridGenerator.updateDrawTime(sliderValue);
    }

    private void onStartButtonClick()
    {
        GameManager.instance.isPlaying = !GameManager.instance.isPlaying;
    }

    private void onClearButtonClick()
    {
        gridGenerator.initEmptyNewGridPi();
        GameManager.instance.isPlaying = false;
    }

    private void onRandomButtonClick()
    {
        gridGenerator.initRandomNewGridPi();
    }
}
