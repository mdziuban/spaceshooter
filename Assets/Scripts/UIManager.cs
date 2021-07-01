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
    [SerializeField] private Text _outOfAmmoText;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private Text _ammoCount;

     private Player player;

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

        player = GameObject.Find("Player").GetComponent<Player>();
        if (player == null)
        {
            Debug.LogError("Player on UI Manager is NULL");
        }

    }

    void Update()
    {
        if (player.GetAmmoCount() == 0)
        {
            _outOfAmmoText.gameObject.SetActive(true);
        }
        else if (player.GetAmmoCount() > 0)
        {
            _outOfAmmoText.gameObject.SetActive(false);
        }
    }

    public void UpdateAmmo(int ammo)
    {
        //_ammoFillBar.SetFillBar(ammo);
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
