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
    [SerializeField] private int _ammoPowerUpAmount = 15;
    [SerializeField] private FillBar _boostFillBar;
    [SerializeField] private FillBar _ammoFillBar;
    [SerializeField] private int _maxBoostTime = 5;
    [SerializeField] private float _slowDownPowerUpActive = .5f;
    [SerializeField] private float _powerUpCooldown = 5f;
    private float _slowDownPowerUpNormal = 1f;
    private float _slowDownPowerUp;
    private float _currentBoostTime;
    [SerializeField] private AudioSource _audioSource;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;
    private SpawnManager _spawnManager;
    private UIManager _uiManager;
    [SerializeField] private SpriteRenderer _shieldSprite;
    private CameraShake _cameraShake;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        _uiManager = GameObject.Find("UIManager").GetComponent<UIManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("Spawn manager is null!");
        }
        transform.position = new Vector3(0,-2.7f,0);

        if (_uiManager == null)
        {
            Debug.LogError("UI Manager is NULL");
        }

        //_audioSource = GetComponent<AudioSource>();
        if (_audioSource == null)
        {
            Debug.LogError("AudioSource for Player is NULL");
        }

        _cameraShake = GameObject.Find("Main Camera").GetComponent<CameraShake>();
        if (_cameraShake == null)
        {
            Debug.LogError("CameraShake is NULL");
        }
        _currentBoostTime = _maxBoostTime;
        _boostFillBar.SetMaxFill(_maxBoostTime);
        _ammoFillBar.SetMaxFill(_ammoCount);
        _slowDownPowerUp = _slowDownPowerUpNormal;
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
            _ammoFillBar.SetFillBar(_ammoCount);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserFireOffset, Quaternion.identity); 
            _ammoCount--; 
            _ammoFillBar.SetFillBar(_ammoCount);  
        }

        _audioSource.PlayOneShot(_laserShot);
        
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        if (Input.GetKey(KeyCode.LeftShift) && _currentBoostTime > 0)
        {
            _boostAmount = 2;
            _currentBoostTime -= Time.deltaTime;
            _boostFillBar.SetFillBar(_currentBoostTime);
            
        }
        else 
        {
            _boostAmount = 1;
            _currentBoostTime += (Time.deltaTime * .5f);
            _boostFillBar.SetFillBar(_currentBoostTime);
        }

        if (!_isSpeedBoostActive)
        {
            transform.Translate(direction * speed * _boostAmount * _slowDownPowerUp * Time.deltaTime);
        }
        else
        {
            transform.Translate(direction * (speed * _speedMuliplier) * _slowDownPowerUp * Time.deltaTime);
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
        _cameraShake.ShakeTheCamera(1f);
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
        yield return new WaitForSeconds(_powerUpCooldown);
        _tripleFireActive = false;
    }

    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostPowerDownRoutine());
    }

    IEnumerator SpeedBoostPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpCooldown);
        _isSpeedBoostActive = false;
    }

    public void SlowDownActive()
    {
        _slowDownPowerUp = _slowDownPowerUpActive;
    }

    IEnumerator SlowDownPowerDownRoutine()
    {
        yield return new WaitForSeconds(_powerUpCooldown);
        _slowDownPowerUp = _slowDownPowerUpNormal;

    }

    public void ShieldActive()
    {
        _shieldStrength = 3;
        _isShieldActive = true;
        _shieldSprite.color = new Color(1,1,1,_shieldStrength*.33f);
        _shield.SetActive(true);
    }

    public void HealthAdd()
    {
        if (_lives < 3)
        {
            _lives++;
            _shipDamages[3-_lives].SetActive(false);
            _uiManager.UpdateLives(_lives);
        }
    }

    public void AmmoAdd()
    {
        _ammoCount = _ammoPowerUpAmount;
        _ammoFillBar.SetFillBar(_ammoCount);
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
