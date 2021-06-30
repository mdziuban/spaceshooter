using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    [SerializeField] private Text _scoreText;
    [SerializeField] private Text _gameOverText;
    [SerializeField] private Sprite[] liveSprites;
    [SerializeField] private Image _livesImage;
    [SerializeField] private Text _restartLevelText;
    [SerializeField] private GameManager gameManager;

    void Start()
    {
        _scoreText.text = "Score: " + 00;
        _gameOverText.gameObject.SetActive(false);
        _restartLevelText.gameObject.SetActive(false);
        gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();

        if (gameManager == null)
        {
            Debug.LogError("GameManager is NULL");
        }
    }

    void Update()
    {
        
    }

    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        _livesImage.sprite = liveSprites[currentLives];

        if (currentLives == 0)
        {
            GameOverSequence();
        }
    }

    private void GameOverSequence()
    {
        StartCoroutine(GameOverFlicker());
        _restartLevelText.gameObject.SetActive(true);
        gameManager.GameOver();
    }

    IEnumerator GameOverFlicker()
    {
        while (true)
        {
            _gameOverText.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            _gameOverText.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
