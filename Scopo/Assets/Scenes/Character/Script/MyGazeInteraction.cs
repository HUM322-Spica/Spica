using UnityEngine;
using UnityEngine.Events;
using Unity.Cinemachine; // Added for Cinemachine support
using NarrationsJouables;

    public class MyGazeInteraction : MonoBehaviour, IObservable
    {
        [Header("Gaze Settings")]
        [SerializeField] private UnityEvent OnGazeEnter;
        [SerializeField] private UnityEvent OnGazeExit;

        [SerializeField] private GameObject highlight;
        [SerializeField] private bool useMinMax = true;
        [SerializeField] private float minDistance = 0f;
        [SerializeField] private float maxDistance = 3f;

        [SerializeField] private float shakeIntensity = 2f;
        [SerializeField] private float shakeDuration = 1f;

        private bool isObserved;

        [Header("Camera Shake Settings")]
        private CinemachineCamera virtualCamera;
        private CinemachineBasicMultiChannelPerlin noise;
        private float shakeTimer;

        private bool hasBeenInvoked =false; // To prevent multiple invocations of the shake

    private void Start()
        {
            if (highlight != null) highlight.SetActive(false);

            // Automatically find the virtual camera in the scene
            virtualCamera = FindFirstObjectByType<CinemachineCamera>();

            if (virtualCamera != null)
            {
                noise = virtualCamera.GetComponent<CinemachineBasicMultiChannelPerlin>();
            }
            else
            {
                Debug.LogWarning("GazeInteraction: No CinemachineCamera found in the scene!");
            }
        }

        private void Update()
        {
            // Handle the shake countdown
            if (shakeTimer > 0)
            {
                shakeTimer -= Time.deltaTime;

                if (shakeTimer <= 0 && noise != null)
                {
                    noise.AmplitudeGain = 0f;
                }
            }
        }

        public bool ObservationStateChanged(bool _observed, float _distance = -1)
        {

         // Prevent interaction from happening more than once
         if (hasBeenInvoked)
            return false;
        var valid = !useMinMax || _distance > minDistance && _distance < maxDistance;
            var state = _observed && valid;

            // if state changed
            if (isObserved != state)
            {
                isObserved = state;

                if (isObserved)
                {
                    hasBeenInvoked = true; // Set the flag to true to prevent future invocations
                    OnGazeEnter?.Invoke();

                    // Trigger the shake directly right here
                    LocalShake(shakeIntensity, shakeDuration);
                }
                else
                {
                    OnGazeExit?.Invoke();
                }

                if (highlight != null) highlight.SetActive(isObserved);
            }

            return isObserved;
        }

        // Your original Shake method, renamed slightly to avoid conflicts
        private void LocalShake(float intensity, float duration)
        {
            Debug.Log("LocalShake called with intensity: " + intensity + " and duration: " + duration);
        if (noise != null)
            {
                noise.AmplitudeGain = intensity;
                shakeTimer = duration;
            }
        }
    }
