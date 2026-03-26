using UnityEngine;

/// <summary>
/// Base Singleton class for MonoBehaviours. Ensures that only one instance of the class exists.
/// Does not persist across scenes, nor instantiate itself if it doesn't exist. 
/// </summary>
public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this as T;
    }

    protected virtual void OnDestroy()
    {
        if (Instance == this)
        {
            Instance = null;
        }
    }
}
