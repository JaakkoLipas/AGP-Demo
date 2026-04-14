using UnityEngine;

namespace AoV.Gameplay
{
    public class Projectile : MonoBehaviour
    {
        // class is agnostic to projectile source, determined by prefab tag

        public DamageType ProjectileDamageType;
        [SerializeField] private float _damage;
        [SerializeField] private bool _invokesIFrames;
        [Tooltip("Projectile lifetime in seconds")]
        [SerializeField] private float _lifetime;
        [Tooltip("If false, colliding does not destroy the projectile and destruction is solely determined by lifetime")]
        [SerializeField] private bool _destroyedByCollision = true;

        private void Awake()
        {
            Destroy(this.gameObject, _lifetime);
        }

        private void OnCollisionEnter2D(Collision2D coll)
        {
            if (coll.collider.TryGetComponent<IDamageable>(out IDamageable damageable))
            {
                if (damageable.EffectiveDamageTypes.Contains(ProjectileDamageType))
                {
                    // TODO: effective VFX/SFX calls
                    damageable.TakeDamage(_damage, _invokesIFrames);
                }
                else
                {
                    // TODO: ineffective VFX/SFX calls
                }
            }

            if (_destroyedByCollision) { Destroy(this.gameObject); }
        }

        //private void OnDestroy()
        //{
        // TODO: generic projectile destruction VFX/SFX
        //}
    } 
}