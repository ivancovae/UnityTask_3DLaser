using UnityEngine;

namespace LaserGame
{
    public class InteractiveSurface : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private int _absorptionValue;
#pragma warning restore 0649
        public int AbsorptionValue => _absorptionValue;
    }
}