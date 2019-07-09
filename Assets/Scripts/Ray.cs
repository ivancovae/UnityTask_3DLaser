using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LaserGame
{
    public class Ray : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private LineRenderer _lineRenderer;
#pragma warning restore 0649
        public float LivePoints { get; set; }
        public float MaxDistance { get; set; }

        [SerializeField] private LayerMask _interactiveMask;

        private List<Vector3> _points = new List<Vector3>();
        private List<Vector3> _dirs = new List<Vector3>();
        private Coroutine _routine;

        private bool _isFull = false;

        private float _distance;

        [SerializeField] private System.Action OnAddPoint = delegate { };

        public void Awake()
        {
            _interactiveMask = 1 << LayerMask.NameToLayer("Interactive");
            OnAddPoint += UpdateLine;
        }

        public void InitRay()
        {
            _lineRenderer.positionCount = 0;
            _points.Add(transform.position);
            _dirs.Add(transform.up);
            if (_routine != null)
                StopCoroutine(_routine);
            _routine = StartCoroutine(nameof(FindNextPoint));
        }

        public void UpdateLine()
        {
            UpdateDistance();
            if (MaxDistance - _distance <= 0.0f || LivePoints <= 0.0f)
            {
                _isFull = true;
            }
            var drawPoints = new List<Vector3>();
            drawPoints.AddRange(_points);
            _lineRenderer.positionCount = drawPoints.Count;
            _lineRenderer.SetPositions(drawPoints.ToArray());
        }

        private void UpdateDistance()
        {
            _distance = 0.0f;
            var temp = _points.First();
            for (var i = 0; i < _points.Count; i++)
            {
                _distance += Vector3.Distance(temp, _points[i]);
                temp = _points[i];
            }
        }

        private IEnumerator FindNextPoint()
        {
            while (!_isFull)
            {
                var lastPoint = _points.Last();
                var nextDir = _dirs.Last();
                var distance = MaxDistance - _distance;
                Debug.DrawRay(lastPoint, nextDir * distance, Color.yellow);
                if (Physics.Raycast(lastPoint, nextDir, out RaycastHit hit, distance, _interactiveMask))
                {
                    var hitObject = hit.collider.GetComponent<InteractiveSurface>();
                    LivePoints -= hitObject.AbsorptionValue;
                    _points.Add(hit.point);
                    _dirs.Add(Vector3.Reflect(hit.point, hit.normal).normalized);
                    OnAddPoint.Invoke();
                }
                else
                {
                    _points.Add(lastPoint + nextDir * distance);
                    OnAddPoint.Invoke();
                }
                yield return new WaitForEndOfFrame();
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (Application.isPlaying)
            {
                Gizmos.color = Color.blue;
                var prevPoint = _points.First();
                Gizmos.DrawWireSphere(prevPoint, .1f);
                for (var i = 1; i < _points.Count; i++)
                {
                    Gizmos.DrawLine(prevPoint, _points[i]);
                    prevPoint = _points[i];
                    Gizmos.DrawWireSphere(prevPoint, .1f);
                }
            }
        }
    }
}