using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

[RequireComponent(typeof(RobotSpawner), typeof(VeinSpawner))]
public class Base : MonoBehaviour
{
    private RobotSpawner _robotSpawner;
    private VeinSpawner _veinSpawner;
    private List<Robot> _robots;
    private List<Vein> _veins;
    private int _radiuseReturn = 4;

    private int[] _oreCounts = {0, 0, 0, 0};

    private int _maxOreCount = 30;
    private HashSet<Vein> _targetedVeins = new HashSet<Vein>();

    public event Action<int[], int> ResourceReceived;

    private void Awake()
    {
        _robots = new List<Robot>();
        _veins = new List<Vein>();

        _robotSpawner = GetComponent<RobotSpawner>();
        _veinSpawner = GetComponent<VeinSpawner>();
    }

    private void OnEnable()
    {
        _robotSpawner.RobotSpawned += AddRobot;
        _veinSpawner.VeinSpawned += AddVein;
    }

    private void OnDisable()
    {
        _robotSpawner.RobotSpawned -= AddRobot;
        _veinSpawner.VeinSpawned -= AddVein;
    }

    private void Update()
    {
        ScanLocations();
    }

    private void ScanLocations()
    {
        if (_veins.Count == 0 || _robots.Count == 0) return;

        List<Robot> waitingRobots = GetWaitingRobots();
        if (waitingRobots.Count == 0) return;

        foreach (Robot robot in waitingRobots)
        {
            int missingOreIndex = GetMostNeededOreIndex();

            if (missingOreIndex == -1) break;

            string targetedOreName = GetOreNameByIndex(missingOreIndex);

            Vein targetVein = FindFreeVeinByName(targetedOreName);

            if (targetVein == null)
            {
                targetVein = FindAnyAvailableVein();
            }

            if (targetVein != null)
            {
                _targetedVeins.Add(targetVein);
                robot.TakeTarget(targetVein, GetRandomCirclePosition(_radiuseReturn), this);
            }
        }
    }

    private List<Robot> GetWaitingRobots()
    {
        List<Robot> waiting = new List<Robot>();

        foreach (Robot robot in _robots)
        {
            if (robot != null && robot.IsWaiting)
            {
                waiting.Add(robot);
            }
        }
        return waiting;
    }

    private int GetMostNeededOreIndex()
    {
        int bestIndex = -1;
        int lowestCount = int.MaxValue;

        for (int i = 0; i < _oreCounts.Length; i++)
        {
            if (_oreCounts[i] < _maxOreCount)
            {
                if (_oreCounts[i] < lowestCount)
                {
                    lowestCount = _oreCounts[i];
                    bestIndex = i;
                }
            }
        }
        return bestIndex;
    }

    private Vein FindFreeVeinByName(string oreName)
    {
        foreach (Vein vein in _veins)
        {
            if (vein != null && vein.Name == oreName && vein.HasOre && !_targetedVeins.Contains(vein))
            {
                return vein;
            }
        }
        return null;
    }

    private Vein FindAnyAvailableVein()
    {
        foreach (Vein vein in _veins)
        {
            if (vein == null || !vein.HasOre || _targetedVeins.Contains(vein)) continue;

            int index = GetOreIndexByName(vein.Name);
            if (index != -1 && _oreCounts[index] < _maxOreCount)
            {
                return vein;
            }
        }
        return null;
    }

    private string GetOreNameByIndex(int index)
    {
        switch (index)
        {
            case 0: return "Gold";
            case 1: return "Amethyst";
            case 2: return "Lazurit";
            case 3: return "Ruby";
            default: return "";
        }
    }

    private int GetOreIndexByName(string oreName)
    {
        switch (oreName)
        {
            case "Gold": return 0;
            case "Amethyst": return 1;
            case "Lazurit": return 2;
            case "Ruby": return 3;
            default: return -1;
        }
    }

    private void AddRobot(Robot robot)
    {
        _robots.Add(robot);
    }

    private void AddVein(Vein vein)
    {
        _veins.Add(vein);
    }

    public void RemoveVein(Vein vein)
    {
        if (_veins.Contains(vein)) _veins.Remove(vein);
        if (_targetedVeins.Contains(vein)) _targetedVeins.Remove(vein);
    }

    public void ReleaseVein(Vein vein)
    {
        if (_targetedVeins.Contains(vein)) _targetedVeins.Remove(vein);
    }

    public void ReceiveOre(Ore ore)
    {
        int index = GetOreIndexByName(ore.GetOreName());
        
        if (index != -1)
        {
            _oreCounts[index] = Mathf.Min(_oreCounts[index] + ore.GetOre(), _maxOreCount);
            Debug.Log($"[База] Доставлен ресурс {ore.GetOreName()}. На складе: {_oreCounts[index]}/{_maxOreCount}");
            ResourceReceived?.Invoke(_oreCounts, _maxOreCount);
        }

        Destroy(ore.gameObject);
    }

    private Vector3 GetRandomCirclePosition(float radius)
    {
        float theta = UnityEngine.Random.Range(0f, Mathf.PI * 2f);

        float x = radius * Mathf.Cos(theta);
        float z = radius * Mathf.Sin(theta);

        return transform.position + new Vector3(x, 0f, z);
    }

}
