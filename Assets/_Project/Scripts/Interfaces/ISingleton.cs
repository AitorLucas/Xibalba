using UnityEngine;

public abstract class ISingleton<T> : MonoBehaviour where T: ISingleton<T> {
    public static T Instance { get; private set; }

    protected virtual void Awake() {
        if (Instance != null && Instance != this) {
            Destroy(this);
            return;
        }
        Instance = this as T;
        DontDestroyOnLoad(this);
    }
}
