using UnityEngine;

namespace Assets.Scripts
{
    internal class Siren : MonoBehaviour
    {
        public static readonly Siren Instance = (new GameObject("SirenContainer")).AddComponent<Siren>();

        private bool _isInitialized = false;

        private AudioSource _siren1;
        private AudioSource _siren2;

        // Explicit static constructor to tell C# compiler
        // not to mark type as beforefieldinit
        static Siren()
        {

        }
        //private constructor is called before static consructor
        private Siren()
        {

        }

        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public void Initialize()
        {
            if (_isInitialized) return;
            AddHandlers();
            _isInitialized = true;
        }

        private void AddHandlers()
        {
            EventAggregator.RadarShieldDestroyed += EventAggregator_RadarShieldDestroyed;
            EventAggregator.BaseDestroyed += EventAggregator_BaseDestroyed;
            EventAggregator.LevelCompleted += EventAggregator_LevelCompleted;
        }

        public void OnDestroy()
        {
            _siren1 = null;
            _siren2 = null;

            EventAggregator.RadarShieldDestroyed -= EventAggregator_RadarShieldDestroyed;
            EventAggregator.BaseDestroyed -= EventAggregator_BaseDestroyed;
            EventAggregator.LevelCompleted -= EventAggregator_LevelCompleted;
            _isInitialized = false;
        }

        private static AudioSource LoadSoundEffect(string resourceName)
        {
            var resourceRequest = Resources.LoadAsync<AudioSource>(resourceName);
            var prefabAsset = resourceRequest.asset as AudioSource;
            return prefabAsset;
        }

        private void EventAggregator_RadarShieldDestroyed(object sender, System.EventArgs e)
        {
            var sirenAudioSource1 = LoadSoundEffect("audio/SoundEffect - Siren 1");
            var sirenAudioSource2 = LoadSoundEffect("audio/SoundEffect - Siren 2");
            _siren1 = Instantiate(sirenAudioSource1);
            _siren2 = Instantiate(sirenAudioSource2);

            _siren1.volume = 0.3f;
            _siren2.volume = 0.3f;

            _siren1.Play();
            _siren2.Play();
        }

        private void EventAggregator_LevelCompleted(object sender, System.EventArgs e)
        {
            StopSiren();
        }

        private void EventAggregator_BaseDestroyed(object sender, System.EventArgs e)
        {
            StopSiren();
        }

        private void StopSiren()
        {
            if (_siren1 != null && _siren1.isPlaying) _siren1.Stop();
            if (_siren2 != null && _siren2.isPlaying) _siren2.Stop();
        }
    }
}