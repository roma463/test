using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    enum cell
    {
        painted,
        unpainted,
        block
    }
    [SerializeField] private Transform _wall;
    [SerializeField] private RotationHome _moveHome;
    [SerializeField] private LayerMask _blockCollision;
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PaintOver _texture;

    private Vector3[,] _gridPoint;
    private cell[,] _busyPoint;
    private Vector2Int _currentPosition = Vector2Int.zero;
    private int _lengthX, _lengthY;

    private List<Vector3> _motionVector = new List<Vector3>();
    private List<Vector2> _positionPrintingOver = new List<Vector2>();

    private delegate bool ConditionSelection(int x);

    private void Start()
    {
        _lengthX = (int)_wall.localScale.x * 10;
        _lengthY = (int)_wall.localScale.y * 10;

        _gridPoint = new Vector3[_lengthX, _lengthY];
        _busyPoint = new cell[_lengthX, _lengthY];
        for (int x = 0; x < _lengthX; x++)
        {
            for (int y = 0; y < _lengthY; y++)
            {
                _gridPoint[x, y] = new Vector3(x +.5f, y + .5f) + _wall.transform.position - (_wall.transform.localScale / 2 * 10);
                _gridPoint[x, y].z = -1f;
            }
        }

        _playerMovement.StartPosition(StartPosition());
        _playerMovement.SetGrid(this);

        _texture.OnCreateTexture(new Vector2(_lengthX, _lengthY));

        SetBusyPoint();
        SetGridBusy();
    }
    private void SetBusyPoint()
    {
        for (int x = 0; x < _lengthX; x++)
        {
            for (int y = 0; y < _lengthY; y++)
            {
                var c = Physics.OverlapSphere(_gridPoint[x, y], 0.1f,_blockCollision).Length;
                if(c > 0)
                {
                    _busyPoint[x, y] = cell.block;
                }
            }
        }
    }
    public Vector3 BuildVectorMove(Vector2 direction)
    {
        _motionVector.Clear();
        _positionPrintingOver.Clear();

        bool accessCell = true;
        int offset = 1;
        float lenght = 0;
        int playerPosition = 0;
        Vector3 targetPosition = Vector3.zero;

        if (direction.x == 0)
        {
            playerPosition = _currentPosition.y;
            offset *= (int)direction.y;
            lenght = _lengthY;
        }
        if (direction.y == 0)
        {
            playerPosition = _currentPosition.x;
            offset *= (int)direction.x;
            lenght = _lengthX;
        }

        ConditionSelection conditionSeletcion = delegate(int i)
        {
            return offset > 0 ? i < lenght : i >= 0;
        };

        for (int i = playerPosition; conditionSeletcion(i); i += offset)
        {
            var temporaryPosition = CheckPointvector(i, direction, ref accessCell);
            if (accessCell)
            {
                targetPosition = temporaryPosition;
                _motionVector.Add(temporaryPosition);
                _positionPrintingOver.Add(_currentPosition);
            }
            else
            {
                return targetPosition;
            }
        }
        return targetPosition;
    }
    public void CheckPrinting()
    {
        for (int i = 0; i < _motionVector.Count; i++)
        {
            var lenght = Physics.OverlapSphere(_motionVector[i], .1f).Length;
            if(lenght > 0)
            {
                _busyPoint[(int)_positionPrintingOver[i].x, (int)_positionPrintingOver[i].y] = cell.painted;
                var position = new Vector2(_lengthX - 1-_positionPrintingOver[i].x, _lengthY - 1 - _positionPrintingOver[i].y);
                _texture.PrintingOver(position);
            }
        }
    }
    private void SetGridBusy()
    {
        for (int x = 0; x < _lengthX; x++)
        {
            for (int y = 0; y < _lengthY; y++)
            {
                if(_busyPoint[x,y] != cell.block)
                {
                    _busyPoint[x, y] = cell.unpainted;
                }
            }
        }
    }
    public bool CheckForWin()
    {
        for (int x = 0; x < _lengthX; x++)
        {
            for (int y = 0; y < _lengthY; y++)
            {
                if(_busyPoint[x,y] == cell.unpainted)
                {
                    return false;
                }
            }
        }
        _moveHome.SwitchGrid();
        return true;
    }
    private Vector3 CheckPointvector(int numberIteration, Vector2 direction, ref bool accessCell)
    {
        Vector2Int tenporarypositionPlayer = _currentPosition;
        if (direction.x == 0)
        {
            tenporarypositionPlayer.y = numberIteration;
        }
        else if (direction.y == 0)
        {
            tenporarypositionPlayer.x = numberIteration;
        }
        if (_busyPoint[(int)tenporarypositionPlayer.x,(int)tenporarypositionPlayer.y] == cell.block)
        {
            accessCell = false;
        }
        else
        {
            _currentPosition = tenporarypositionPlayer;
        }
        return _gridPoint[_currentPosition.x, _currentPosition.y];
    }
    public Vector3 StartPosition()
    {
        return _gridPoint[_currentPosition.x, _currentPosition.y];
    }
}
