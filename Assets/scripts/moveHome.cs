using System.Collections;
using UnityEngine;

public class moveHome : MonoBehaviour
{
    [SerializeField] private Grid _gridOne, _gridTwo;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private UI _ui;
    [SerializeField] private float _speed;
    public void SwitchGrid()
    {
        if (_gridOne.enabled == true)
        {
           
            StartCoroutine(RotateHome());
        }
        else
        {
            _ui.Win();
        }
    }
    private IEnumerator RotateHome()
    {
        _playerMovement.gameObject.SetActive(false);
        for (float i = 0; i < 90; i += Time.deltaTime * _speed)
        {
            transform.rotation = Quaternion.Euler(Vector3.up * i);
            yield return null;
        }
        _gridOne.enabled = false;
        _gridTwo.enabled = true;
        _playerMovement.gameObject.SetActive(true);
    }
}
