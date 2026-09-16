using UnityEngine;

namespace Enemies
{
    public class EnemyMovement : MonoBehaviour
    {
        private Transform _target;
        private float _speed;
        private bool _isMoving;

        public void Init(Transform target, float speed)
        {
            _target = target;
            _speed = speed;
            _isMoving = true;
        }

        private void Update()
        {
            Vector3 direction = (_target.position - transform.position).normalized;
            transform.position += direction * (_speed * Time.deltaTime);
        }

        public void Stop()
        {
            _isMoving = false;
            _target = null;
        }
    }
}