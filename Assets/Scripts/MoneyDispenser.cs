using System;
using UnityEngine;

public class MoneyDispenser : MonoBehaviour
{
	[SerializeField, Min(0)] private float _cashDelay = 1f;
	[SerializeField, Min(0)] private int _cashAmount = 1;

	private float _timer;
	
	public event Action<int> MoneyDispensed;
	
	private void Update()
	{
		_timer += Time.deltaTime;

		if (_timer >= _cashDelay)
		{
			MoneyDispensed?.Invoke(_cashAmount);

			_timer -= _cashDelay;
		}
	}
}
