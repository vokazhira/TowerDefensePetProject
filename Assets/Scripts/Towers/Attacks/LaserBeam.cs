using UnityEngine;

namespace Towers.Attacks
{
    public class LaserBeam : MonoBehaviour
    {
        [SerializeField] private float _minWidth = 0.05f;
        [SerializeField] private float _maxWidth = 0.25f;

        private LineRenderer _line;

        private void Awake()
        {
            _line = GetComponent<LineRenderer>();
            _line.positionCount = 2;
            _line.useWorldSpace = true;
        }
        
        public void Show(Vector3 from, Vector3 to, float power)
        {
            _line.enabled = true;
            _line.SetPosition(0, from);
            _line.SetPosition(1, to);
            _line.widthMultiplier = Mathf.Lerp(_minWidth, _maxWidth, power);
        }
        
        public void Hide()
        {
            _line.enabled = false;
        }
    }
}