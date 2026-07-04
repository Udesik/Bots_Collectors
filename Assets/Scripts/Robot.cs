using UnityEngine;
using UnityEngine.AI;

public class Robot : MonoBehaviour
{
    [SerializeField] private Transform _positionOre;
    [SerializeField] private int _countGetOre = 4;
    private Vein _target;
    private Vector3 _basePosition;
    private Base _base;
    private Camera _camera;
    private NavMeshAgent _agent;
    private GameObject _ore;

    private bool _isWaiting = true;
    private bool _hasOre = false;

    public bool IsWaiting => _isWaiting;

    private void Awake()
    {
        _camera = Camera.main;
        _agent = GetComponent<NavMeshAgent>();
    }

    public void TakeTarget(Vein target, Vector3 basePosition, Base baseObject)
    {
        _isWaiting = false;
        _hasOre = false;

        _target = target;
        _basePosition = basePosition;
        _base = baseObject;

        _agent.SetDestination(target.Position);
    }

    private void Update()
    {
        if (_isWaiting) return;

        if (!_agent.pathPending && _agent.remainingDistance <= _agent.stoppingDistance)
        {
            if (!_hasOre)
            {
                TryCollectOre();
            }
            else
            {
                DeliverOreToBase();
            }
        }
    }

    private void TryCollectOre()
    {
        if (_target != null && _target.HasOre)
        {
            _ore = _target.GetOre(_countGetOre);
            _ore.transform.SetParent(transform);
            _ore.transform.position = _positionOre.position;

            _hasOre = true;
            _agent.avoidancePriority = 10;
            _agent.SetDestination(_basePosition);
            _base.ReleaseVein(_target);
        }
        else
        {
            if (_target != null) _base.RemoveVein(_target);
            ResetToWaiting();
        }
    }

    private void DeliverOreToBase()
    {
        if (_ore != null)
        {
            _base.ReceiveOre(_ore.GetComponent<Ore>());
            _ore = null;
        }

        ResetToWaiting();
    }

    private void ResetToWaiting()
    {
        _isWaiting = true;
        _agent.avoidancePriority = 50;
        _hasOre = false;
        _agent.ResetPath();
    }
}
