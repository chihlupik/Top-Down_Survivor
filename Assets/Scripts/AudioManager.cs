using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [System.Serializable]
    public struct SFX
    {
        public string name;
        public AudioClip clip;
    }

    [SerializeField] private SFX[] sfxList;
    [SerializeField] private int poolSize = 5;

    private Dictionary<string, AudioClip> clipDict;
    private AudioSource[] pool;
    private int index;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        clipDict = new Dictionary<string, AudioClip>();
        foreach (var s in sfxList)
            if (!string.IsNullOrEmpty(s.name) && s.clip != null)
                clipDict[s.name] = s.clip;

        pool = new AudioSource[poolSize];
        for (int i = 0; i < poolSize; i++)
        {
            var go = new GameObject($"SFX_{i}");
            go.transform.SetParent(transform);
            pool[i] = go.AddComponent<AudioSource>();
            pool[i].playOnAwake = false;
        }
    }

    public void PlaySFX(string name)
    {
        if (!clipDict.TryGetValue(name, out AudioClip clip)) return;

        AudioSource src = pool[index];
        index = (index + 1) % poolSize;
        src.clip = clip;
        src.Play();
    }
}