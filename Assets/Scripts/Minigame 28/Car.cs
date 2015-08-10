using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame28
{
    public class Car : MonoBehaviour
    {
        #region variables

        public Transform containerPath;

        private float _speed = 180;
        private UISprite _sprite;
        private bool _isMove = false;
        private List<Transform> listPath;
        private Transform _targetPoint;

        #endregion

        
        void Awake()
        {
            Reset();
        }

        void Start()
        {
            MiniGameHelper.FindChildObjects<Transform>(containerPath, ref listPath);
        }

        void Update()
        {
            if (!MiniGame28_Manager.instance.isPlay || !_isMove)
                return;

            if (_targetPoint && _targetPoint.position != transform.position)
            {
                // Расчет поворота
                transform.right -= Vector3.Lerp(transform.right, _targetPoint.transform.localPosition - transform.localPosition, 0.3f * Time.deltaTime);

                // Расчет перемещения
                float speedThisFrame = _speed * Time.deltaTime;
                transform.localPosition += (_targetPoint.transform.localPosition - transform.localPosition).normalized * speedThisFrame;
                if (Vector3.Distance(transform.localPosition, _targetPoint.transform.localPosition) < speedThisFrame * 1.1f)
                    GetNewTarget();
            }
            else if (!GetNewTarget())
                _isMove = false;
        }

        public void Move()
        {
            if (listPath.Count > 0)
                transform.position = listPath[0].position;
            if (listPath.Count > 1)
                _targetPoint = listPath[1];
            if (_sprite != null)
                _sprite.alpha = 1f;
            _isMove = true;
        }

        public void Reset()
        {
            if (_sprite == null)
                _sprite = GetComponent<UISprite>();
            if (_sprite != null)
                _sprite.alpha = 0f;
            _isMove = false;
        }

        private bool GetNewTarget()
        {
            if (_targetPoint != null)
            {
                int i = listPath.IndexOf(_targetPoint);
                if (i == listPath.Count - 1)
                {
                    _targetPoint = null;
                    MiniGame28_Manager.instance.Finish();
                    return false;
                }
                else if (i >= 0 && i < listPath.Count - 1)
                {
                    _targetPoint = listPath[i + 1];
                    return true;
                }
                else
                {
                    _targetPoint = null;
                    return false;
                }
            }
            return false;
        }
    }
}