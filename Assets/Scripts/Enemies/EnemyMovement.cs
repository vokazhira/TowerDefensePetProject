using UnityEngine;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _target;
        private float _speed;
        private bool _isMoving;
        private bool _isPaused;

        public void Init(Transform target, float speed)
        {
            _target = target;
            _speed = speed;
            _isMoving = true;
            _isPaused = false;
        }

        private void Update()
        {
            if (!_isMoving || _isPaused || _target == null) return;
            
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * (_speed * Time.deltaTime);
        }

        public void SetPaused(bool isPaused)
        {
            _isPaused = isPaused;
        }

        public void Stop()
        {
            _isMoving = false;
            _target = null;
        }
    }
}