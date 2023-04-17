using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMainController : MonoBehaviour
{
    [SerializeField]
    public TextMeshProUGUI timeText;
    [SerializeField]
    public TextMeshProUGUI scoreText;
    [SerializeField]
    private GrapplingGun grapplingGun;
    [SerializeField]
    private Image oxygenBar;

    [SerializeField]
    private GameObject deathPanel;
    [SerializeField]
    public TextMeshProUGUI deathScoreText;
    [SerializeField]
    public TextMeshProUGUI deathHighScoreText;
    [SerializeField]
    private GameObject lorePanel;

    private float timeLeft = 10.0f;
    private float timeElapsed = 0.0f;
    private int score = 0;

    void Start()
    {
        deathPanel.SetActive(false);
        InvokeRepeating("DecreaseTimeRemaining", 1.0f, 1.0f);

        if (PlayerPrefs.GetInt("LoreRead", 0) == 1)
        {
            lorePanel.SetActive(false);
        }
        else
        {
            lorePanel.SetActive(true);
            Time.timeScale = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!grapplingGun.IsPlayerDead)
        {
            timeText.text = timeLeft.ToString();
            oxygenBar.fillAmount = timeLeft / 10f;
            timeElapsed += Time.deltaTime;
            score = Mathf.RoundToInt(timeElapsed * 10);
        }
        scoreText.text = score.ToString();
    }
    void DecreaseTimeRemaining()
    {
        if (timeLeft >= 0)
        {
            timeLeft--;
        }
        if (timeLeft < 0)
        {
            grapplingGun.IsPlayerDead = true;
            Invoke("PlayerDied", 0.75f);
        }
    }
    void PlayerDied()
    {
        deathPanel.SetActive(true);
        deathScoreText.text = score.ToString();
        float hs = PlayerPrefs.GetFloat("HighScore", 0f);
        if (score > hs)
        {
            deathHighScoreText.text = score.ToString();
            PlayerPrefs.SetFloat("HighScore", score);
        }
        else
        {
            deathHighScoreText.text = hs.ToString();
        }

    }
    public void RestockOxygen()
    {
        timeLeft = 10f;
        CancelInvoke("DecreaseTimeRemaining");
        InvokeRepeating("DecreaseTimeRemaining", 1.0f, 1.0f);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RemoveStartLore()
    {
        lorePanel.SetActive(false);
        PlayerPrefs.SetInt("LoreRead", 1);
        Time.timeScale = 1;

    }
}
