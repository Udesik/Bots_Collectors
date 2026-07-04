using UnityEngine;
using System.Collections.Generic;
using System;
using Random = UnityEngine.Random;

public class VeinSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> _veins;

    private float _minRadius = 20f;
    private float _maxRadius = 38f;

    public event Action<Vein> VeinSpawned;

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        for (int i = 0; i < _veins.Count; i++)
        {
            Vector3 position = GetRandomRingPosition();
            GameObject vein = Instantiate(_veins[i], position, Quaternion.identity) as GameObject;

            VeinSpawned(vein.GetComponent<Vein>());
        }
    }

    private Vector3 GetRandomRingPosition()
    {
        float theta = Random.Range(0f, Mathf.PI * 2f);

        float u = Random.value;
        float r = Mathf.Sqrt(u * (_maxRadius * _maxRadius - _minRadius * _minRadius) + _minRadius * _minRadius);

        float x = r * Mathf.Cos(theta);
        float z = r * Mathf.Sin(theta);

        return transform.position + new Vector3(x, 0f, z);
    }
}
