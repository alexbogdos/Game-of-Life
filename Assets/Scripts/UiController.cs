using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    [SerializeField] private GameObject titlePanel;
    [SerializeField] private GameObject buttonsPanel;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private GameObject simulateButton;
    [SerializeField] private GameObject generationCounter;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private GameObject backButton;



    [SerializeField] private Slider lengthSlider;
    [SerializeField] private Slider durationSlider;




    [SerializeField] private float titleDelay;
    [SerializeField] private float buttonDelay;

    [SerializeField] BoardController boardController;


    void Awake()
    {
        simulateButton.SetActive(false);
        generationCounter.SetActive(false);
        settingsPanel.SetActive(false);
        titlePanel.SetActive(false);
        buttonsPanel.SetActive(false);
        backButton.SetActive(false);
        exitButton.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SetActiveDelay(titleDelay, titlePanel));
        StartCoroutine(SetActiveDelay(buttonDelay, buttonsPanel));
    }

    IEnumerator SetActiveDelay(float delay, GameObject gameObject)
    {
        // Delay excecution for @delay seconds
        yield return new WaitForSeconds(delay);

        gameObject.SetActive(true);
    }

    public void StartSetUp()
    {
        boardController.StartSetUp();

        titlePanel.SetActive(false);
        buttonsPanel.SetActive(false);
        simulateButton.SetActive(true);

        exitButton.SetActive(false);
        backButton.SetActive(true);

    }

    public void OpenSettings()
    {
        buttonsPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void setLength()
    {
        float newLength = lengthSlider.value;
        boardController.setLength((int)(newLength));
    }

    public void setDuration()
    {
        float newDuration = durationSlider.value;
        boardController.setRoundDuration(newDuration/10);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        buttonsPanel.SetActive(true);
    }

    public void StartSimulation()
    {
        exitButton.SetActive(false);
        backButton.SetActive(true);
        StartCoroutine(waitToLoad(boardController.getRoundDuration() * 1.25f));
    }

    IEnumerator waitToLoad(float delay)
    {
        simulateButton.SetActive(false);
        generationCounter.SetActive(true);

        // Delay excecution for @delay seconds
        yield return new WaitForSeconds(delay);

        boardController.FinishSetUp();
        boardController.StartGame();
    }

    public void exitApp()
    {
        Application.Quit();
    }

    public void mainMenu()
    {
        simulateButton.SetActive(false);
        generationCounter.SetActive(false);
        settingsPanel.SetActive(false);
        titlePanel.SetActive(true);
        buttonsPanel.SetActive(true);
        backButton.SetActive(false);
        exitButton.SetActive(true);

        boardController.Reset();
    }
}
