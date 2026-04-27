using UnityEngine;
using EditorAttributes;

namespace AoV.Gameplay
{
    [RequireComponent (typeof(Collider2D))]
    public class Door : MonoBehaviour, IPhysicsInteractable
    {
        [SerializeField] private GameObject _interactorObject;
        private IPhysicsInteractor _interactor;
        [SerializeField] private Transform _target;
        private Transform _transform;
        [SerializeField] private float _moveSpeed = 1f;
        private float _moveStep;
        private bool _moving = false;
        private Vector2 _originalPosition;

        private void Awake()
        {
            _transform = transform;
            _originalPosition = _transform.position;
            if (_interactorObject.TryGetComponent<IPhysicsInteractor>(out IPhysicsInteractor interactor))
            {
                _interactor = interactor;
            }
            else Debug.LogWarning("GameObject " + _interactorObject + " attached to " + this.gameObject + " does not have an IPhysicsInteractor component!");
        }

        private void Update()
        {
            if (_moving)
            {
                _moveStep = _moveSpeed * Time.deltaTime;
                if (Vector2.Distance (_transform.position, _target.position) > 0.001f)
                {
                    _transform.position = Vector2.MoveTowards(_transform.position, _target.position, _moveStep);
                }
                else
                {
                    _moving = false;
                    _target.position = _originalPosition;
                    if (_interactor != null && !_interactor.IsOneShot) _interactor.ToggleEnabled();
                }
            }
        }

        public void Interact()
        {
            _originalPosition = _transform.position;
            _moving = true;
        }
    } 
}