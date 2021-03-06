using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed = 4f;
    [SerializeField] private int _minScore = 5;
    [SerializeField] private int _maxScore = 10;
    [SerializeField] private float _fireRate = 3.0f;
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private GameObject _laserPrefab;
    [SerializeField] private GameObject _shield;
    private int _shieldSelectionRandom;
    private AudioSource audioSource;
    private Animator animator;
    private Player _player;
    private int _scoreValue;
    private float _canFire = -1;
    private int _directionToMove;
    private bool _notDead = true;

    
    
    private void Start() 
    {
        _shieldSelectionRandom = Random.Range(0,3);
        ShieldActive(_shieldSelectionRandom);
        _player = GameObject.Find("Player").GetComponent<Player>();
        if (_player == null)
        {
            Debug.LogError("Player is NULL");
        }
        animator = gameObject.GetComponent<Animator>();
        if(animator == null)
        {
            Debug.LogError("Animator is NULL");
        }
        _scoreValue = Random.Range(_minScore, _maxScore);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource on enemy is NULL");
        }
        _directionToMove = Random.Range(-1,2);

    }


    private void Update() {
        MoveDown(_directionToMove);
        if (Time.time > _canFire && _notDead)
        {
            _fireRate = Random.Range(3f, 7f);
            _canFire = Time.time + _fireRate;
            
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);
            Laser[] lasers = enemyLaser.GetComponentsInChildren<Laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    private void MoveDown(int moveXDirection)
    {
        transform.Translate(new Vector3(moveXDirection,-1,0) * _speed * Time.deltaTime);
        if (transform.position.y < -8f || transform.position.x > 12f || transform.position.x < -12f)
        {
            ReturnToTop();
        }
    }

    private void ReturnToTop()
    {
        float xValue = Random.Range(-9f, 9f);
        transform.position = new Vector3(xValue,8f,0);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.CompareTag("Player"))
        {
            Player player = other.transform.GetComponent<Player>();
            if (player != null)
            {
                player.Damage();
            }
            if (_shield.activeSelf == true)
            {
            _shield.SetActive(false);
            return;
            }
            DestroyEnemy();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _player.UpdateScore(_scoreValue);
            if (_shield.activeSelf == true)
            {
                _shield.SetActive(false);
                return;
            }
            DestroyEnemy();
        }
    }

    private void CheckShieldActive()
    {
        if (_shield.activeSelf == true)
        {
            _shield.SetActive(false);
            return;
        }
    }

    private void DestroyEnemy()
    {
        _notDead = false;
        Destroy(GetComponent<Collider2D>());
        animator.SetTrigger("OnEnemyDeath");
        audioSource.PlayOneShot(explosionSound);
        _speed = 0;
        Destroy(this.gameObject, 2.5f);
    }

    private void ShieldActive(int shieldRandom)
    {
        if (shieldRandom == 0)
        {
            _shield.SetActive(true);
            Debug.Log("SHIELDS ACTIVE!!!");
        }
        
    }
}
