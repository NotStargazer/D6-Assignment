using UnityEngine;

public class Tile : MonoBehaviour
{
    [SerializeField] private MeshRenderer _renderer;

    public Vector3 TilePosition { get => transform.localPosition; set => transform.localPosition = value; }
    public Material Material { set => _renderer.material = value; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
