using UnityEngine;

public class UI : MonoBehaviour
{
    [SerializeField] private GameObject _winText;
    public void Win() => _winText.SetActive(true);
}
