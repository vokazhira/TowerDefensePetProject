using System;
using UnityEngine;

namespace Towers.Projectiles
{
    public abstract class HomingProjectile : Projectile
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _arrivalDistance;

        private void Update()
        {
            if (!IsAlive(_target))
            {
                OnTargetLost();
                return;
            }
            
            Vector3 targetPosition = _target.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) <= _arrivalDistance)
            {
                HitTarget();
            }
        }

        protected virtual void OnTargetLost()
        {
            Despawn();
        }
    }
}