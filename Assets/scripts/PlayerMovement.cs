using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Grid _grid;
    [SerializeField] private float _speed;
    private Vector3 _targetPosition;
    private bool _isMove = false;
    private bool _moveNow;
    public void StartPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void SetGrid(Grid newGrid)
    {
        _grid = newGrid;
    }
   
    public void DirectionMove(Vector2 direction)
    {
        _targetPosition = _grid.BuildVectorMove(direction);
        _isMove = true;
        StartCoroutine(MoveingToPoint(_targetPosition));
    }

    private IEnumerator MoveingToPoint(Vector3 target)
    {
        _isMove = false;
        _moveNow = true;
        while(Vector2.Distance((Vector2)transform.position, target) > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, _speed * Time.deltaTime);
            _grid.PrintindOver();
            yield return null;
        }
        _moveNow = false;
        var state = _grid.CheckForWin();
    }

    
}
