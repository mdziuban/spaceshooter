using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] GameObject[] _PowerUpPrefabs;
    [SerializeField] float waitTime = 5;
    [SerializeField] GameObject _enemyContainer;
    [SerializeField] GameObject _powerUpContainer;
    [SerializeField] float minPowerupSpawnTime = 3f;
    [SerializeField] float maxPowerupSpawnTime = 7f;
    [SerializeField] GameObject _AmmoRestorePrefab;
    [SerializeField] int _numberOfWaves = 10;
    [SerializeField] float _timeBetweenWaves = 10f;
    [SerializeField] private UIManager _uiManager;
    private bool _stopSpawning = false;
    private bool _spawningStarted = false;



    public void StartSpawning()
    {
        if (!_spawningStarted)
        {
            _spawningStarted = true;
            StartCoroutine(SpawnPowerUpRoutine());
            StartCoroutine(WaveSpawnerRoutine());
        }

    }

    IEnumerator WaveSpawnerRoutine()
    {
        for (int i = 1; i < _numberOfWaves+1; i++)
        {
            _uiManager.NewWaveIncomingText(true);
            yield return new WaitForSeconds(2);
            _uiManager.NewWaveIncomingText(false);
            for (int j = 0; j < i; j++)
            {
                Vector3 positionToSpawn = new Vector3(Random.Range(-9,9),8,0);
                GameObject newEnemy = Instantiate(_enemyPrefab,positionToSpawn, Quaternion.identity);
                newEnemy.transform.parent = _enemyContainer.transform;
            }
            yield return new WaitForSeconds(_timeBetweenWaves);
            

        }
    }


    IEnumerator SpawnPowerUpRoutine()
    {
    //0 = health
    //1 = speed 
    //2 = shield
    //3 = triple shot 
    //4 = ammo
    //5 = slowDown - negative effect
        yield return new WaitForSeconds(3f);
        while(!_stopSpawning)
        {
            int randomPowerUp;
            Vector3 positionToSpawn = new Vector3(Random.Range(-9,9),8,0);
            int roll = Random.Range(0,100);
            if (roll < 10) //this is the health powerup
            {
                randomPowerUp = 0;
            }
            else if (roll > 9 && roll < 30)
            {
                randomPowerUp = Random.Range(1,_PowerUpPrefabs.Length-2); //speed, shield, triple shot
            }
            else if (roll > 29 && roll < 90)
            {
                randomPowerUp = 4; //ammo 
            }
            else 
            {
                randomPowerUp = 5; //slow down
            }
            GameObject newPowerUp = Instantiate(_PowerUpPrefabs[randomPowerUp],positionToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(minPowerupSpawnTime,maxPowerupSpawnTime));
        }
    }


    public void OnPlayerDeath()
    {
        _stopSpawning = true;
        StopAllCoroutines();

    }

}
