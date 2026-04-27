using Unity.VisualScripting;
using UnityEngine;

namespace AoV.System
{
    /// <summary>
    /// Universal parallax controller for any 2D sprite graphics that may parallax, based on the camera's position.
    /// Requires an object/prefab to be set up with the correct tiling.
    /// </summary>
    public class ParallaxController : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        [SerializeField] private Transform _cameraPosition;
        [SerializeField] private bool _xAxisActive;
        public bool XAxisActive { get => _xAxisActive; set { _xAxisActive = value; } }
        [SerializeField] private bool _yAxisActive;
        public bool YAxisActive { get => _yAxisActive; set { _yAxisActive = value; } }
        [Tooltip("If true, this graphic is meant to scroll infinitely and will wrap around when reaching its sprite bounds")]
        public bool isInfiniteScrolling;
        [Tooltip("Sets the magnitude of the parallax effect.\n0 = graphic moves 1:1 with camera\n1 = graphic does not move")]
        public float parallaxEffectMagnitude;

        private SpriteRenderer _graphicRenderer;
        private float _startPositionX;
        private float _movementX;
        private float _spriteBoundsX;
        private float _startPositionY;
        private float _movementY;
        private float _spriteBoundsY;

        private Transform _transform; // cache object transform to reduce native calls
        private Vector3 _offsetPoint;
        private Vector3 _anchorOffset;

        private void Awake()
        {
            _transform = this.transform;
            _graphicRenderer = GetComponent<SpriteRenderer>();
            _startPositionX = _anchorPoint.position.x;
            _startPositionY = _anchorPoint.position.y;
            _spriteBoundsX = _graphicRenderer.bounds.size.x;
            _spriteBoundsY = _graphicRenderer.bounds.size.y;
            if (_anchorPoint == null)
            {
                _anchorPoint = _transform;
            }
            if (_cameraPosition == null)
            {
                _cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
            }
            ShiftAnchor();
        }

        private void Update()
        {
            CalculateDistanceAndMovement();
            MovePosition();
            if (isInfiniteScrolling) WrapGraphic();
        }

        private void CalculateDistanceAndMovement()
        {
            _offsetPoint = _cameraPosition.position * parallaxEffectMagnitude;
            if (!XAxisActive) _offsetPoint.x = _startPositionX;
            if (!YAxisActive) _offsetPoint.y = _startPositionY;
            _offsetPoint.z = -10f;
            _movementX = _cameraPosition.position.x * (1 - parallaxEffectMagnitude);
            _movementY = _cameraPosition.position.y * (1 - parallaxEffectMagnitude);
        }

        private void MovePosition()
        {
            _transform.position = _cameraPosition.position - _offsetPoint + _anchorOffset;
        }

        private void WrapGraphic()
        {
            if (XAxisActive)
            {
                if (_movementX > _startPositionX + _spriteBoundsX) { _startPositionX += _spriteBoundsX; ShiftAnchor(); }
                else if (_movementX < _startPositionX - _spriteBoundsX) { _startPositionX -= _spriteBoundsX; ShiftAnchor(); }
            }
            if (YAxisActive)
            {
                if (_movementY > _startPositionY + _spriteBoundsY) { _startPositionY += _spriteBoundsY; ShiftAnchor(); }
                else if (_movementY < _startPositionY - _spriteBoundsY) { _startPositionY -= _spriteBoundsY; ShiftAnchor(); }
            }
        }

        private void ShiftAnchor()
        {
            _anchorOffset.x = _startPositionX;
            _anchorOffset.y = _startPositionY;
        }
    }
}