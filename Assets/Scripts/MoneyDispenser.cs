using System;
using UnityEngine;

public class MoneyDispenser : MonoBehaviour
{
	[SerializeField, Min(0)] private float _cashDelay = 1f;
	[SerializeField, Min(0)] private int _cashAmount = 1;

	private float _timer;
	private static MoneyDispenser _instance;
	
	public event Action<int> MoneyDispensed;
	public static MoneyDispenser Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			else
			{
				var objs = FindObjectsOfType<MoneyDispenser>();

				_instance = objs[0];

				for (int i = 1; i < objs.Length; i++)
				{
					Destroy(objs[i]);
				}
				return _instance;
			}
		}
	}

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
