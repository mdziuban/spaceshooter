using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    [SerializeField] private float speed = 3.5f;
    [SerializeField] private float _speedMuliplier = 2;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _tripleLaserPrefab;
    [SerializeField] private GameObject _shield;
    [SerializeField] private Vector3 _laserFireOffset = new Vector3(0,1.05f,0);
    [SerializeField] private float _fireRate = 0.15f;
    [SerializeField] private float _canFire = -1;
    [SerializeField] private int _lives = 3;
    [SerializeField] private int _shieldStrength = 3;
    [SerializeField] private bool _tripleFireActive = false;
    [SerializeField] private int _score;
    [SerializeField] private GameObject[] _shipDamages;
    [SerializeField] private AudioClip _laserShot;
    [SerializeField] private int _boostAmount = 2;
    [SerializeField] private int _ammoCount = 15;
    private AudioSource _audioSource;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField] private SpriteRenderer _shieldSprite;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null!");
        }
        transform.position = new Vector3(0,-2.7f,0);

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        _audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource for Player is NULL");
        }
    }


    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire && _ammoCount > 0)
        {
            ShootLaser();
        }
    }

    private void ShootLaser()
    {
        _canFire = Time.time + _fireRate;
        if (_tripleFireActive)
        {
            Instantiate(_tripleLaserPrefab, transform.position, Quaternion.identity);
            _ammoCount--;
            _uiManager.UpdateAmmo(_ammoCount);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserFireOffset, Quaternion.identity); 
            _ammoCount--; 
            _uiManager.UpdateAmmo(_ammoCount);  
        }

        _audioSource.PlayOneShot(_laserShot);
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _boostAmount = 2;
        }
        else 
        {
            _boostAmount = 1;
        }

        if (!_isSpeedBoostActive)
        {
            transform.Translate(direction * speed * _boostAmount * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (speed * _speedMuliplier) * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

        }
        
        //clamps the player to prevent them from going below 3.8 and above 0
        transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0));

        if (transform.position.x >= 11f || transform.position.x <= -11f)
        {
            transform.position = new Vector3(transform.position.x * -1, transform.position.y, 0);
        }
    }

    public void Damage()
    {
        if (_isShieldActive)
        {
            if (_shieldStrength > 0)
            {
                _shieldStrength--;
                _shieldSprite.color = new Color(1,1,1,_shieldStrength*.33f);
                return; 
            }

            _isShieldActive = false;
            _shield.SetActive(false);
            return;
            
        }

        _lives--;
        if(_lives > 0)
        {
            _shipDamages[2-_lives].SetActive(true);
            _uiManager.UpdateLives(_lives);
        }
        else if (_lives < 1)
        {
            _uiManager.UpdateLives(_lives);
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
            
        }
    }

    public void TripleFireSetActive()
    {
        _tripleFireActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(5);
        _tripleFireActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(5f);
        _isSpeedBoostActive = false;
    }

    public void ShieldActive()
    {
        _shieldStrength = 3;
        _isShieldActive = true;
        _shieldSprite.color = new Color(1,1,1,_shieldStrength*.33f);
        _shield.SetActive(true);
    }


    public void UpdateScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public int GetAmmoCount()
    {
        return _ammoCount;
    }

}
