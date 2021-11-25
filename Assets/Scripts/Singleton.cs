using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
	private static T _instance;

	public static T Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			else
			{
				var objs = FindObjectsOfType<T>();

				_instance = objs[0];

				for (int i = 1; i < objs.Length; i++)
				{
					Destroy(objs[i]);
				}
				return _instance;
			}
		}
	}
}
