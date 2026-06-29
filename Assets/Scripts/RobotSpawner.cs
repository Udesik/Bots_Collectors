using UnityEngine;
using System;
using Random = UnityEngine.Random;

public class RobotSpawner : MonoBehaviour
{
    [SerializeField] private GameObject _robotPrefab;
    [SerializeField] private int _robotCount;
    [SerializeField] private int _robotSpawnRadiusMin;
    [SerializeField] private int _robotSpawnRadiusMax;
    
    public event Action<Robot> RobotSpawned;

    private void Start()
    {
        for (int i = 0; i < _robotCount; i++)
        {
            Vector3 position = GetRandomRingPosition();
            GameObject robot = Instantiate(_robotPrefab, position, Quaternion.identity) as GameObject;

            RobotSpawned(robot.GetComponent<Robot>());
        }
    }

    private Vector3 GetRandomRingPosition()
    {
        float theta = Random.Range(0f, Mathf.PI * 2f);

        float u = Random.value;
        float r = Mathf.Sqrt(u * (_robotSpawnRadiusMax * _robotSpawnRadiusMax - _robotSpawnRadiusMin * _robotSpawnRadiusMin) + _robotSpawnRadiusMin * _robotSpawnRadiusMin);

        float x = r * Mathf.Cos(theta);
        float z = r * Mathf.Sin(theta);

        return transform.position + new Vector3(x, 0f, z);
    }
}
