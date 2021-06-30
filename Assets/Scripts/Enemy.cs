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
    private AudioSource audioSource;
    private Animator animator;
    private Player _player;
    private int _scoreValue;
    private float _canFire = -1;

    
    
    private void Start() 
    {
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

    }

    private void Update() {

        MoveDown();
        if (Time.time > _canFire)
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

    private void MoveDown()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
        if (transform.position.y < -8f)
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
            DestroyEnemy();
        }

        if (other.CompareTag("Laser"))
        {
            Destroy(other.gameObject);
            _player.UpdateScore(_scoreValue);
            DestroyEnemy();
        }
    }

    private void DestroyEnemy()
    {
        Destroy(GetComponent<Collider2D>());
        animator.SetTrigger("OnEnemyDeath");
        audioSource.PlayOneShot(explosionSound);
        _speed = 0;
        Destroy(this.gameObject, 2.5f);
    }
}
