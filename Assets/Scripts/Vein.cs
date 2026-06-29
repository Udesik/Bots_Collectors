using UnityEngine;

public class Vein : MonoBehaviour
{
    [SerializeField] private Transform _position;
    private GameObject _orePrefab;
    private string _oreName;
    private int _countOre;

    public void Init(GameObject orePrefab, string oreName, int countOre)
    {
        _orePrefab = orePrefab;
        _oreName = oreName;
        _countOre = countOre;
    }

    
}
