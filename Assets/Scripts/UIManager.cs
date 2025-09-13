using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panels")]
    public GameObject startPanel;
    public GameObject victoryPanel;
    public GameObject defeatPanel;

    [Header("Buttons")]
    public Button startButton;
    public Button victoryButton;
    public Button defeatButton;

    [Header("HUD")]
    public TextMeshProUGUI lifeText;
    public TextMeshProUGUI waveText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        ShowStart();
    }
    public void UpdateLife(int currentLife)
    {
        if (lifeText != null)
            lifeText.text = "Vida: " + currentLife;
    }
    public void UpdateWave(int wave)
    {
        if (waveText != null)
            waveText.text = "Oleada: " + wave;
    }

    public void ShowStart()
    {
        startPanel.SetActive(true);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        Time.timeScale = 0f;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(startButton.gameObject);
    }

    public void StartGame()
    {
        startPanel.SetActive(false);
        victoryPanel.SetActive(false);
        defeatPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void ShowVictory()
    {
        victoryPanel.SetActive(true);
        startPanel.SetActive(false);
        defeatPanel.SetActive(false);
        Time.timeScale = 0f;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(victoryButton.gameObject);
    }

    public void ShowDefeat()
    {
        defeatPanel.SetActive(true);
        startPanel.SetActive(false);
        victoryPanel.SetActive(false);
        Time.timeScale = 0f;
        
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(defeatButton.gameObject);
    }

    public void BackToStart()
    {
        ShowStart();
        FindObjectOfType<PlayerController>().ResetPlayer();
        FindObjectOfType<EnemySpawner>().ResetSpawner();
    }
}
