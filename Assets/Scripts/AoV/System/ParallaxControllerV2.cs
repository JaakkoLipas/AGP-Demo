using System.Collections.Generic;
using UnityEngine;

namespace AoV.System
{
    /// <summary>
    /// Universal parallax controller for any 2D sprite graphics that may parallax, based on an anchor point and the camera's position.
    /// Only requires one SpriteRenderer and will dynamically tile the sprite based on which parallax axes are active.
    /// </summary>
    public class ParallaxControllerV2 : MonoBehaviour
    {
        [SerializeField] private Transform _anchorPoint;
        [SerializeField] private Transform _cameraPosition;
        [SerializeField] private bool _xAxisActive;
        public bool XAxisActive { get => _xAxisActive; set { _xAxisActive = value; CheckWrapping(); } }
        [SerializeField] private bool _yAxisActive;
        public bool YAxisActive { get => _yAxisActive; set { _yAxisActive = value; CheckWrapping(); } }
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

        private List<GameObject> _graphicClones;
        private Transform _transform; // cache object transform to reduce native calls
        private Vector3 _offsetPoint;
        private Vector3 _spriteBoundsXOffset;
        private Vector3 _spriteBoundsYOffset;
        private Vector3 _anchorOffset;

        private void Awake()
        {
            _transform = this.transform;
            _graphicRenderer = GetComponent<SpriteRenderer>();
            _startPositionX = _anchorPoint.position.x;
            _startPositionY = _anchorPoint.position.y;
            _spriteBoundsX = _graphicRenderer.bounds.size.x;
            _spriteBoundsXOffset = new Vector3(_spriteBoundsX, 0, 0);
            _spriteBoundsY = _graphicRenderer.bounds.size.y;
            _spriteBoundsYOffset = new Vector3(0, _spriteBoundsY, 0);
            if (_anchorPoint == null)
            {
                _anchorPoint = _transform;
            }
            if (_cameraPosition == null)
            {
                _cameraPosition = GameObject.FindGameObjectWithTag("MainCamera").transform;
            }
            CheckWrapping();
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

        private void CheckWrapping()
        {
            // TODO: make this work with object pooling instead of expensive instantiation/destruction
            if (_graphicClones != null)
            {
                foreach (GameObject go in _graphicClones)
                {
                    _graphicClones.Remove(go);
                    Destroy(go);
                } 
            }
            if (XAxisActive && !YAxisActive)
            {
                _graphicClones.Add(InstantiateBackgroundTile(_transform.position + _spriteBoundsXOffset));
                _graphicClones.Add(InstantiateBackgroundTile(_transform.position - _spriteBoundsXOffset));
            }
            if (!XAxisActive && YAxisActive)
            {
                _graphicClones.Add(InstantiateBackgroundTile(_transform.position + _spriteBoundsYOffset));
                _graphicClones.Add(InstantiateBackgroundTile(_transform.position - _spriteBoundsYOffset));
            }
            if (XAxisActive && YAxisActive)
            {
                for (int i = -1; i < 2; i++)
                {
                    _graphicClones.Add(InstantiateBackgroundTile(_transform.position + _spriteBoundsXOffset + (_spriteBoundsYOffset * i)));
                    if (i != 0) _graphicClones.Add(InstantiateBackgroundTile(_transform.position + (_spriteBoundsYOffset * i)));
                    _graphicClones.Add(InstantiateBackgroundTile(_transform.position - _spriteBoundsXOffset + (_spriteBoundsYOffset * i)));
                }
            }
        }

        private GameObject InstantiateBackgroundTile(Vector3 position)
        {
            GameObject go = Instantiate(_graphicRenderer.gameObject);
            go.transform.SetParent(_transform);
            go.transform.position = position;
            return go;
        }
    }
}