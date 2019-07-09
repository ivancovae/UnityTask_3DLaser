using UnityEngine;
using LaserGame.UI;

namespace LaserGame
{
    public class LaserGun : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private SettingsUI _settingsUI;
        [SerializeField] private Transform _platform;
        [SerializeField] private Transform _gun;
        [SerializeField] private Transform _exitPoint;
        [SerializeField] private Ray _rayPrefab;
#pragma warning restore 0649
        [SerializeField] private float _maxAngleGun = 0.0f;
        [SerializeField] private float _maxAnglePlatform = 0.0f;
        [SerializeField] private float _stepAngle = 0.0f;


        [SerializeField] private Vector3 _angleGun = Vector3.zero;
        public float AngleGun => _angleGun.z;
        public System.Action<float> OnAngleGun_Changed = delegate { };

        [SerializeField] private Vector3 _anglePlatform = Vector3.zero;
        public float AnglePlatform => _anglePlatform.y;
        public System.Action<float> OnAnglePlatform_Changed = delegate { };
        
        [SerializeField] private Ray _ray;

        [SerializeField] private float _maxDistance = 200.0f;
        public float Power => _maxDistance;
        public System.Action<float> OnPower_Changed = delegate { };
        [SerializeField] private float _livePoints = 4;
        public float LivePoints => _livePoints;
        public System.Action<float> OnLivePoints_Changed = delegate { };

        public void RotateGun(bool isClockwise)
        {
            var value = Mathf.Clamp(_angleGun.z + (isClockwise ? _stepAngle : -_stepAngle), -_maxAngleGun, _maxAngleGun);
            _angleGun.z = value;
            OnAngleGun_Changed.Invoke(value);
            _settingsUI.AngleGun = value;
        }

        public void RotatePlatform(bool isClockwise)
        {
            var value = Mathf.Clamp(_anglePlatform.y + (isClockwise ? _stepAngle : - _stepAngle), -_maxAnglePlatform, _maxAnglePlatform);
            _anglePlatform.y = value;
            OnAnglePlatform_Changed.Invoke(value);
            _settingsUI.AnglePlatform = value;
        }

        public void AddLives()
        {
            _livePoints++;
            OnLivePoints_Changed.Invoke(_livePoints);
            _settingsUI.LivePoints = _livePoints;
        }
        public void MinusLives()
        {
            _livePoints--;
            OnLivePoints_Changed.Invoke(_livePoints);
            _settingsUI.LivePoints = _livePoints;
        }

        public void AddPower()
        {
            _maxDistance++;
            OnPower_Changed.Invoke(_maxDistance);
            _settingsUI.Power = _maxDistance;
        }
        public void MinusPower()
        {
            _maxDistance--;
            OnPower_Changed.Invoke(_maxDistance);
            _settingsUI.Power = _maxDistance;
        }

        private void Awake()
        {
            _settingsUI.AngleGun = _angleGun.z;
            _settingsUI.AnglePlatform = _anglePlatform.y;
            _settingsUI.LivePoints = _livePoints;
            _settingsUI.Power = _maxDistance;

            OnAngleGun_Changed += ApplyRotateGun;
            OnAnglePlatform_Changed += ApplyRotatePlatform;
            OnLivePoints_Changed += ApplyChange;
            OnLivePoints_Changed.Invoke(_livePoints);
            OnPower_Changed += ApplyChange;
            OnLivePoints_Changed.Invoke(_maxDistance);
        }

        private void ApplyChange(float value)
        {
            if (_platform == null)
                return;
            if (_ray != null)
            {
                Destroy(_ray.gameObject);
            }
            Fire();
        }

        private void ApplyRotatePlatform(float angle)
        {
            if (_platform == null)
                return;
            if (_ray != null)
            {
                Destroy(_ray.gameObject);
            }
            _platform.eulerAngles = _anglePlatform;

            Fire();
        }

        private void ApplyRotateGun(float angle)
        {
            if (_gun == null)
                return;
            if (_ray != null)
            {
                Destroy(_ray.gameObject);
            }
            _gun.eulerAngles = _angleGun;

            Fire();
        }

        public void Fire()
        {
            if (_exitPoint == null)
                return;
            if (_rayPrefab == null)
                return;
            if (_ray != null)
            {
                Destroy(_ray.gameObject);
            }

            _ray = Instantiate(_rayPrefab, _exitPoint.position, _gun.rotation);
            _ray.LivePoints = _livePoints;
            _ray.MaxDistance = _maxDistance;
            _ray.InitRay();
        }
    }
}