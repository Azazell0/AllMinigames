using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame17
{
    public class Plate : MonoBehaviour
    {
        #region variables

        public PlateType type;
        public List<PlateCell> listCells;
        public float xLocalMin, yLocalMin, xLocalMax, yLocalMax;

        public PlateCell currentCell { get { return _currentCell; } }
        public PlateCell correctCell { get { return _correctCell; } }

        private Vector3 _startPosition;
        private UISprite _sprite;
        private BoxCollider _collider;
        private bool _mouseDown = false;
        private PlateCell _currentCell;
        private PlateCell _correctCell;
        private bool _isInit = false;

        #endregion


        void Awake()
        {
            Init();
        }

        private void Init()
        {
            if (_isInit)
                return;

            if (listCells == null)
                listCells = new List<PlateCell>();
            _collider = GetComponent<BoxCollider>();
            _sprite = GetComponent<UISprite>();
            _startPosition = transform.position;
            _currentCell = null;

            _isInit = true;
        }

        void Update()
        {
            if (!MiniGame17_Manager.instance.isPlay || _collider == null || !_collider.enabled)
                return;

            if (UICamera.hoveredObject == this.gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _mouseDown = true;
                    if (_sprite != null)
                        _sprite.depth++;
                    if (_currentCell != null)
                        _currentCell.plate = null;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    _mouseDown = false;
                    if (_sprite != null)
                        _sprite.depth--;

                    if (_currentCell != null)
                    {
                        if (_currentCell.plate == null)
                            _currentCell.plate = this;
                        else
                            _currentCell = null;
                    }
                }
            }

            if (_mouseDown)
            {
                if (UICamera.hoveredObject != null)
                {
                    transform.position = new Vector3(UICamera.lastHit.point.x, UICamera.lastHit.point.y, transform.position.z);
                    transform.localPosition = new Vector3(Mathf.Clamp(transform.localPosition.x, xLocalMin, xLocalMax), Mathf.Clamp(transform.localPosition.y, yLocalMin, yLocalMax), transform.localPosition.z);
                }
            }
            else if (_currentCell != null && _currentCell.plate == this)
                transform.position = _currentCell.transform.position;
            else if (transform.position != _startPosition)
            {
                // Расчет перемещения
                transform.position = Vector3.Lerp(transform.position, _startPosition, 0.1f);
                if (Vector3.Distance(_startPosition, transform.position) < 0.01f)
                    transform.position = _startPosition;
            }
        }

        public void Reset()
        {
            Init();

            StartCoroutine(ResetCoroutine());
        }

        private IEnumerator ResetCoroutine()
        {
            if (_collider != null)
                _collider.enabled = false;

            if (_currentCell != null)
                _currentCell.plate = null;

            _currentCell = null;
            _correctCell = (listCells.Count > 0) ? listCells[Random.Range(0, listCells.Count)] : null;
            if (_correctCell != null && _sprite != null)
            {
                _sprite.alpha = 0f;
                transform.position = _correctCell.transform.position;
                while (_sprite.alpha < 1)
                {
                    _sprite.alpha += 0.05f;
                    yield return new WaitForSeconds(0.03f);
                }
                yield return new WaitForSeconds(4f);
                while (_sprite.alpha > 0)
                {
                    _sprite.alpha -= 0.05f;
                    yield return new WaitForSeconds(0.03f);
                }
                transform.position = _startPosition;
                while (_sprite.alpha < 1)
                {
                    _sprite.alpha += 0.05f;
                    yield return new WaitForSeconds(0.03f);
                }
            }

            if (_collider != null)
                _collider.enabled = true;
        }

        void OnTriggerEnter(Collider other)
        {
            if (!_mouseDown)
                return;

            PlateCell cell = other.GetComponent<PlateCell>();
            if (cell != null && type == cell.type)
            {
                _currentCell = cell;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (!_mouseDown)
                return;

            PlateCell cell = other.GetComponent<PlateCell>();
            if (cell != null && cell == _currentCell)
            {
                _currentCell = null;
            }
        }
    }

    public enum PlateType { Small, Large}
}