using UnityEngine;
using UnityEngine.UI;

namespace LaserGame.UI
{
    public class SettingsUI : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private Text _fieldAngleGun;
        [SerializeField] private Text _fieldAnglePlatform;
        [SerializeField] private Text _fieldPower;
        [SerializeField] private Text _fieldLivePoints;
#pragma warning restore 0649

        public float AngleGun { set { _fieldAngleGun.text = value.ToString("0.00"); } }
        public float AnglePlatform { set { _fieldAnglePlatform.text = value.ToString("0.00"); } }
        public float Power { set { _fieldPower.text = value.ToString("0.00"); } }
        public float LivePoints { set { _fieldLivePoints.text = value.ToString("0.00"); } }
    }
}