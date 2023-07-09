using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    SHUFFLING,
    HEROTURN,
    ENEMYTURN,
    WAITFORENEMIES
}

public class BattleBoard : MonoBehaviour
{
    private List<List<Enemy>> _rows = new List<List<Enemy>>();
    [SerializeField] private Hero _hero;
    [SerializeField] private GameState _state;
    [SerializeField] private float _actionWaitTime = 1f; // how long to wait between actions on turns (should be global but whatevs)

    // DEFINED THIS WAY FOR INITIALIZATION SO IT'LL APPEAR IN THE INSPECTOR CUZ I CAN'T BE BOTHERED OTHERWISE
    [SerializeField] private List<GameObject> _row1;
    [SerializeField] private List<GameObject> _row2;
    [SerializeField] private List<GameObject> _row3;
    [SerializeField] private List<GameObject> _row4;
    [SerializeField] private List<GameObject> _row5;

    private float _shuffleTimeMax = 20.0f; // how long the player can spend shuffling
    private float _shuffleTime;

    // -- PROPERTIES --
    public Hero Hero
	{
        get { return _hero; }
	}
    public List<List<Enemy>> Rows
	{
        get { return _rows; }
	}
    public GameState State
	{
        get { return _state; }
        set { _state = value; }
	}
    public float ActionWaitTime
	{
        get { return _actionWaitTime; }
	}


    // -- METHODS --

    // take all given prefabs and chuck their Enemy component references in the rows list
    void Start()
    {
        _shuffleTime = _shuffleTimeMax;

        var objRows = new List<List<GameObject>>();
        objRows.Add(_row1);
        objRows.Add(_row2);
        objRows.Add(_row3);
        objRows.Add(_row4);
        objRows.Add(_row5);

        var rowNum = 0;
        foreach (List<GameObject> objRow in objRows)
		{
            foreach (GameObject o in objRow)
			{
                _rows[rowNum].Add(o.GetComponent<Enemy>()); // add the enemy component to the list
			}
            rowNum++;
		}
    }

    // Update is called once per frame
    void Update()
    {
        // figure out what to do based on state
        switch (_state) {
            case GameState.SHUFFLING: // the player shuffling their cards
                _shuffleTime -= Time.deltaTime;
                if (_shuffleTime <= 0)
				{
                    _shuffleTime = _shuffleTimeMax;
                    _state = GameState.HEROTURN;
                    var heroCoroutine = _hero.ExecuteTurn(this);
                    _hero.StartCoroutine(heroCoroutine);
				}
                // TODO: somewhere in here, draw the timer on-screen
                break;
            case GameState.HEROTURN:
                // nothing needs to be done, since the hero's turn coroutine will be running
                break;
            case GameState.ENEMYTURN:
                // this is a pretty shoddy way of doing it, but ~fuck it~
                var enemyCoroutine = RunEnemies();
                StartCoroutine(enemyCoroutine);
                _state = GameState.WAITFORENEMIES;
				break;
            case GameState.WAITFORENEMIES:
                // again, nothing needs to be done, since a coroutine got run
                break;
		}
	}

    private IEnumerator RunEnemies()
	{
        foreach (var row in _rows)
		{
            foreach (var enemy in row)
			{
                enemy.ExecuteTurn(this);
                yield return new WaitForSeconds(_actionWaitTime);
            }
		}

        // once done, set back to shuffling
        _state = GameState.SHUFFLING;
	}
}
