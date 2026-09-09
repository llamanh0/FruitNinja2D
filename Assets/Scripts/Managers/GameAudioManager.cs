using UnityEngine;

public enum Sound
{
    Slice,
    Boom
}

public class GameAudioManager : MonoBehaviour
{
    public static GameAudioManager Instance { get; private set; }

    [System.Serializable]
    public class SoundAudioClip
    {
        public Sound SoundType;
        public AudioClip AudioClip;
    }

    [SerializeField] private SoundAudioClip[] _soundAudioClipArray;

    private AudioSource _audioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            _audioSource = gameObject.AddComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        
    }

    private void OnEnable()
    {
        BladeController.OnSlicableSliced += OnFruitSliced;
        BladeController.OnBombSliced += OnBombSliced;
    }

    private void OnDisable()
    {
        BladeController.OnSlicableSliced -= OnFruitSliced;
        BladeController.OnBombSliced -= OnBombSliced;
    }

    public static void PlaySound(Sound sound)
    {
        AudioClip clip = GetAudioClip(sound);
        if (clip != null)
        {
            Instance._audioSource.PlayOneShot(clip);
        }
    }

    private static AudioClip GetAudioClip(Sound sound)
    {
        foreach (SoundAudioClip soundAudioClip in Instance._soundAudioClipArray)
        {
            if(soundAudioClip.SoundType == sound)
            {
                return soundAudioClip.AudioClip;
            }
        }
        return null;
    }

    private void OnFruitSliced() => PlaySound(Sound.Slice);

    private void OnBombSliced() => PlaySound(Sound.Boom);

}
