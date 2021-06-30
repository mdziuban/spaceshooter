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
    private bool _stopSpawning = false;
    private bool _spawningStarted = false;


    public void StartSpawning()
    {
        if (!_spawningStarted)
        {
            _spawningStarted = true;
            StartCoroutine(SpawnEnemyRoutine());
            StartCoroutine(SpawnPowerUpRoutine());
            StartCoroutine(SpawnAmmoRoutine());
        }

    }

    IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(3f);
        while(!_stopSpawning)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-9,9),8,0);
            GameObject newEnemy = Instantiate(_enemyPrefab,positionToSpawn, Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(waitTime);
        }

    }

    IEnumerator SpawnPowerUpRoutine()
    {
        yield return new WaitForSeconds(3f);
        while(!_stopSpawning)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-9,9),8,0);
            int randomPowerUp = Random.Range(0,_PowerUpPrefabs.Length);
            GameObject newPowerUp = Instantiate(_PowerUpPrefabs[randomPowerUp],positionToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(minPowerupSpawnTime,maxPowerupSpawnTime));
        }
    }

    IEnumerator SpawnAmmoRoutine()
    {
        yield return new WaitForSeconds(3f);
        while(!_stopSpawning)
        {
            Vector3 positionToSpawn = new Vector3(Random.Range(-9,9),8,0);
            GameObject newPowerUp = Instantiate(_AmmoRestorePrefab,positionToSpawn, Quaternion.identity);
            newPowerUp.transform.parent = _powerUpContainer.transform;
            yield return new WaitForSeconds(Random.Range(minPowerupSpawnTime,maxPowerupSpawnTime));
        }
    }

    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }

}
