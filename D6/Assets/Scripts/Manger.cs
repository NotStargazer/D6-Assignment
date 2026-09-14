using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Manger : MonoBehaviour
{
    [SerializeField] private Dice _dicePrefab;
    [SerializeField] private Camera _camera;
    [SerializeField] private InputAction _mousePos;
    [SerializeField] private InputAction _mouseClick;
    [Range(-10, 10)] [SerializeField] private float _diceMinX;
    [Range(-10, 10)] [SerializeField] private float _diceMaxX;
    [Range(-10, 10)] [SerializeField] private float _diceMinY;
    [Range(-10, 10)] [SerializeField] private float _diceMaxY;
    [Range(-10, 10)] [SerializeField] private int _totalRolls;
    [SerializeField] private Vector2Int _diceRowColumn;
    [SerializeField] private Vector2Int _boardSize = new(5, 5);
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private Material _pink;
    [SerializeField] private Material _yellow;
    [SerializeField] private Transform _player;
    private Dice[] _dice;
    private int _rollsLeft;
    private Vector2 _currentMousePos;

    private void Awake()
    {
        _mousePos.performed += context =>
        {
            _currentMousePos = context.ReadValue<Vector2>();
        };
        _mouseClick.performed += _ =>
        {
            var ray = _camera.ScreenPointToRay(_currentMousePos);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.transform.TryGetComponent<Dice>(out var dice))
                {
                    dice.OnClick();
                }
            }
        };
        _mouseClick.Enable();
        _mousePos.Enable();
        
        _player.localPosition = _tiles[0].TilePosition + new Vector3(0,0.25f,0);
        _dice = new Dice[_diceRowColumn.x * _diceRowColumn.y];
        _rollsLeft = _totalRolls;
        
        for (var x = 0; x < _diceRowColumn.x; x++)
        {
            var xCord = 0.5f;
            if (_diceRowColumn.x > 1)
            {
                xCord = (float)x / (_diceRowColumn.x - 1); 
            }
            
            for (var y = 0; y < _diceRowColumn.y; y++)
            {
                var yCord = 0.5f;
                if (_diceRowColumn.y > 1)
                {
                    yCord = (float)y / (_diceRowColumn.y - 1);
                }

                var newDice = Instantiate(_dicePrefab,
                    new Vector3(Mathf.Lerp(_diceMinX, _diceMaxX, xCord), 4,
                    Mathf.Lerp(_diceMinY, _diceMaxY, yCord)), Quaternion.identity);
                _dice[x * _diceRowColumn.y + y] = newDice;
            }
        }
    }

    private void OnGUI()
    {
        GUILayout.Label($"Rolls left: {_rollsLeft}", new GUIStyle
        {
            fontSize = 50
        });
        
        GUI.enabled = true;
        foreach(var d in _dice)
        {
            if (!d.CanRoll)
            {
                GUI.enabled = false;
                break;
            }
        }
        
        if (GUILayout.Button("Roll Dice", GUILayout.Width(250), GUILayout.Height(100)) && _rollsLeft > 0)
        {
            foreach (var die in _dice)
            {
                die.Roll();
            }

            _rollsLeft--;
        }
        
        GUI.enabled = _rollsLeft > 0;
        foreach(var d in _dice)
        {
            if (!d.CanReset)
            {
                GUI.enabled = false;
                break;
            }
        }

        if (GUILayout.Button("Reroll", GUILayout.Width(250), GUILayout.Height(100)) && _rollsLeft > 0)
        {
            foreach (var die in _dice)
            {
                die.ResetDie();
            }
        }

        
    }
    private void OnValidate()
    {
        for (int z = 0; z < _boardSize.x; z++)
        {
            
            if (z % 2 == 0)
            {
                for (int x = 0; x < _boardSize.y; x++)
                {
                    _tiles[z * _boardSize.y + x].Material = (z * _boardSize.y + x) % 2 == 0 ? _pink : _yellow;
                    _tiles[z * _boardSize.y + x].TilePosition = new Vector3(x, 0, z) * 2;
                }
            }
            else
            {
                for (int x = _boardSize.y - 1; x >= 0; x--)
                {
                    _tiles[z * _boardSize.y + x].Material = (z * _boardSize.y + x) % 2 == 0 ? _pink : _yellow;
                    _tiles[z * _boardSize.y + x].TilePosition = new Vector3(_boardSize.x - x - 1, 0, z) * 2;
                }
            }
        }
    }
}
