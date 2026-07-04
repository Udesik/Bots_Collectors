using UnityEngine;

public class Ore : MonoBehaviour
{
    [SerializeField] private GameObject _orePrefab;
    [SerializeField] private Color _colorGold;
    [SerializeField] private Color _colorAmethyst;
    [SerializeField] private Color _colorLazurit;
    [SerializeField] private Color _colorRuby;

    private int _oreCount;
    private string _oreName;

    public void Init(int oreCount, string oreName)
    {
        _oreCount = oreCount;
        _oreName = oreName;

        if (oreName == "Gold")
        {
            GetComponent<MeshRenderer>().material.color = _colorGold;
        }
        else if (oreName == "Amethyst")
        {
            GetComponent<MeshRenderer>().material.color = _colorAmethyst;
        }
        else if (oreName == "Lazurit")
        {
            GetComponent<MeshRenderer>().material.color = _colorLazurit;
        }
        else
        {
            GetComponent<MeshRenderer>().material.color = _colorRuby;
        }
    }

    public int GetOre()
    {
        return _oreCount;
    }

    public string GetOreName()
    {
        return _oreName;
    }

    public void SetParentToOre(Transform parent, Vector3 position)
    {
        transform.SetParent(parent, false);
        transform.position = position;
    }
}
