using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : BattleEntity
{
	[SerializeField] private TextMesh _turnsLabel;
    [SerializeField] private int _waitTurns;
    private int _turnsLeft;
    [SerializeField] private EnemyStrategy _enemyStrat;
	[SerializeField] private AudioSource _sfx;

	new private void Awake()
	{
		base.Awake();
		_turnsLeft = _waitTurns;
		UpdateTurnsLabel();
	}

	private void UpdateTurnsLabel()
	{
		if (_turnsLabel != null)
		{
			_turnsLabel.text = "(" + _turnsLeft + ")";
		}
	}

	// count down; if countdown over, take effect
	public void ExecuteTurn(BattleBoard board)
	{
		_turnsLeft--;
		if (_turnsLeft <= 0)
		{
			_turnsLeft = _waitTurns;
			if (_sfx != null)
			{
				_sfx.Play();
				_enemyStrat.Execute(board);
			}
		}
		UpdateTurnsLabel();
	}
}
