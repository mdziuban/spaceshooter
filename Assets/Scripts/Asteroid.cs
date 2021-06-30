using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed = 10f;
    [SerializeField] private GameObject explosionPrefab;
    private SpawnManager _spawnManager;


    private void Start() {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();

    }

    void Update()
    {
        transform.Rotate(0,0,_rotationSpeed * Time.deltaTime);
    }


    private void OnTriggerEnter2D(Collider2D other) 
    {
        if (other.CompareTag("Laser"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            Destroy(other.gameObject);
            _spawnManager.StartSpawning();
            Destroy(this.gameObject, .25f);
            
        }    
    }

}
