using UnityEngine;

public class PaintOver : MonoBehaviour
{
    [SerializeField] private Texture2D _texture;
    [SerializeField] private TextureWrapMode _wrapMode;
    [SerializeField] private FilterMode _filterMode;
    
    public void OnCreateTexture(Vector2 size)
    {
        _texture = new Texture2D((int)size.x, (int)size.y);
        GetComponent<MeshRenderer>().material.mainTexture = _texture;

        _texture.wrapMode = _wrapMode;
        _texture.filterMode = _filterMode;
    }
    public void PrintingOver(Vector2 position)
    {
        _texture.SetPixel((int)position.x, (int)position.y, Color.green);
        _texture.Apply();
    }
    
}
