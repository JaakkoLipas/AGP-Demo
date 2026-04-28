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
        [Tooltip("Sets the magnitude of the X-axis parallax effect.\n0 = graphic moves 1:1 with camera\n1 = graphic does not move")]
        public float parallaxEffectMagnitudeX;
        [Tooltip("Sets the magnitude of the Y-axis parallax effect.\n0 = graphic moves 1:1 with camera\n1 = graphic does not move")]
        public float parallaxEffectMagnitudeY;
        [Tooltip("Z-axis offset, must be set manually to display the graphic correctly")]
        public float zOffset;

        private SpriteRenderer _graphicRenderer;
        private float _anchorPositionX;
        private float _graphicOffsetFromAnchorX;
        private float _spriteBoundsX;
        private float _anchorPositionY;
        private float _graphicOffsetFromAnchorY;
        private float _spriteBoundsY;

        private Transform _transform; // cache object transform to reduce native calls
        private Vector3 _offsetPoint;
        private Vector3 _anchorOffset;

        private void Awake()
        {
            _transform = this.transform;
            _graphicRenderer = GetComponent<SpriteRenderer>();
            _anchorPositionX = _anchorPoint.position.x;
            _anchorPositionY = _anchorPoint.position.y;
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
            CalculateOffsets();
            MoveGraphic();
            if (isInfiniteScrolling) WrapGraphic();
        }

        private void CalculateOffsets()
        {
            if (XAxisActive) _offsetPoint.x = _cameraPosition.position.x * parallaxEffectMagnitudeX;
            else _offsetPoint.x = _anchorPositionX;
            if (YAxisActive) _offsetPoint.y = _cameraPosition.position.y * parallaxEffectMagnitudeY;
            else _offsetPoint.y = _anchorPositionY;
            _offsetPoint.z = zOffset;
            _graphicOffsetFromAnchorX = _cameraPosition.position.x * parallaxEffectMagnitudeX;
            _graphicOffsetFromAnchorY = _cameraPosition.position.y * parallaxEffectMagnitudeY;
        }

        private void MoveGraphic()
        {
            _transform.position = _cameraPosition.position - _offsetPoint + _anchorOffset;
        }

        private void WrapGraphic()
        {
            if (XAxisActive)
            {
                if (_graphicOffsetFromAnchorX > _anchorPositionX + _spriteBoundsX) { _anchorPositionX += _spriteBoundsX; ShiftAnchor(); }
                else if (_graphicOffsetFromAnchorX < _anchorPositionX - _spriteBoundsX) { _anchorPositionX -= _spriteBoundsX; ShiftAnchor(); }
            }
            if (YAxisActive)
            {
                if (_graphicOffsetFromAnchorY > _anchorPositionY + _spriteBoundsY) { _anchorPositionY += _spriteBoundsY; ShiftAnchor(); }
                else if (_graphicOffsetFromAnchorY < _anchorPositionY - _spriteBoundsY) { _anchorPositionY -= _spriteBoundsY; ShiftAnchor(); }
            }
        }

        private void ShiftAnchor()
        {
            _anchorOffset.x = _anchorPositionX;
            _anchorOffset.y = _anchorPositionY;
        }
    }
}