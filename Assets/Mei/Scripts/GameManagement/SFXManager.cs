using System.Collections.Generic;
using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    [System.Serializable]
    public class Sound
    {
        public SoundType type;
        public AudioClip clip;
    }

    public enum SoundType
    {
        Click,
        CardDrag,
        CardPlace,
        Select,
        CoinGain,
        BuyPack,
        BulletFire,
        BulletSpit,
        BulletLaser,
        BulletGun,
        Win,
        Lose
    }

    [SerializeField] 
    private List<Sound> soundFiles;
    private Dictionary<SoundType, Sound> soundDictionary;

    private AudioSource audioSource;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = GetComponent<AudioSource>();

        soundDictionary = new Dictionary<SoundType, Sound>();
        foreach (var s in soundFiles)
        {
            if (!soundDictionary.ContainsKey(s.type))
                soundDictionary.Add(s.type, s);
        }
    }

    public void PlaySound(SoundType type)
    {
        Sound sound = soundDictionary[type];
        audioSource.PlayOneShot(sound.clip);
    }
}
