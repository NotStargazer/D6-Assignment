using System;
using UnityEngine;

public class Manger : MonoBehaviour
{
    [SerializeField] private Dice _dicePrefab;
    [Range(-10, 10)][SerializeField] private float _diceMinX;
    [Range(-10, 10)][SerializeField] private float _diceMaxX;
    [Range(-10, 10)][SerializeField] private float _diceMinY;
    [Range(-10, 10)][SerializeField] private float _diceMaxY;
    [Range(-10, 10)][SerializeField] private int _totalRolls;
    [SerializeField] private Vector2Int _diceRowColumn;
    [SerializeField] private Vector2Int _boardSize = new(5, 5);
    [SerializeField] private Tile[] _tiles;
    [SerializeField] private Material _pink;
    [SerializeField] private Material _yellow;
    [SerializeField] private Transform _player;
    private Dice[] _dice;
    private int _rollsLeft;

    private void Awake()
    {
        _player.localPosition = _tiles[0].TilePosition + new Vector3(0,0.25f,0);

        Physics.queriesHitTriggers = true;
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
        GUILayout.Label($"Rolls left: {_rollsLeft}");
        if (GUILayout.Button("Roll Dice", GUILayout.Width(250), GUILayout.Height(100)) && _rollsLeft > 0)
        {
            foreach (var die in _dice)
            {
                die.Roll();
            }

            _rollsLeft--;
        }

        foreach(var d in _dice)
        {
            if (!d.HasScore)
            {
                return;
            }
        }

        if (GUILayout.Button("Reroll", GUILayout.Width(250), GUILayout.Height(100)) && _rollsLeft > 0)
        {
            foreach (var die in _dice)
            {
                die.DiceReset();
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
