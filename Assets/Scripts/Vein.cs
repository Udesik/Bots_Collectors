using UnityEngine;
//using UnityEngine.Collider;

public class Vein : MonoBehaviour
{
    [SerializeField] private string _oreName;
    [SerializeField] private GameObject _orePrefab;
    [SerializeField] private int _oreCount = 100;
    private bool _isWaiting = true;

    public Vector3 Position => transform.position;
    public string Name => _oreName;
    public bool HasOre => _oreCount > 0;

    public GameObject GetOre(int count)
    {
        _oreCount -= count;
        GameObject ore = Instantiate(_orePrefab, transform.position, Quaternion.identity);
        Ore oreScript = ore.GetComponent<Ore>();
        oreScript.Init(count, _oreName);

        return ore;
    }
}
