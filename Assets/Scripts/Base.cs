using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RobotSpawner))]
public class Base : MonoBehaviour
{
    private RobotSpawner _spawner;
    private List<Robot> _robots;

    private void Awake()
    {
        _robots = new List<Robot>();
        _spawner = GetComponent<RobotSpawner>();
    }

    private void OnEnable()
    {
        _spawner.RobotSpawned += AddRobot;
    }

    private void OnDisable()
    {
        _spawner.RobotSpawned -= AddRobot;
    }

    private void AddRobot(Robot robot)
    {
        _robots.Add(robot);
    }
}
