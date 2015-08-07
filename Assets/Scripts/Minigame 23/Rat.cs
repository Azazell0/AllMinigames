using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame23
{
    public class Rat : MonoBehaviour
    {
        #region variables

        //Список стартовых точек
        public List<Point> listStartPoints;
        /// <summary>
        /// Точка, к которой стремится крыса
        /// </summary>
        public Point pointFinish;
        /// <summary>
        /// Задержка в секундах
        /// </summary>
        public float delay = 2f;

        private bool _isActive = false;

        private List<Point> _currentPath;

        private Point _currentPoint;
        private Point _targetPoint;
        private float _rateSpeed = 100;
        private float _lastSearchPathTime = -100f;

        #endregion

        
        void Start()
        {
            Reset();
        }

        void Update()
        {
            if (!MiniGame23_Manager.instance.isPlay || !_isActive)
                return;

            if (delay > 0)
                delay -= Time.deltaTime;

            if (delay <= 0)
            {
                if (_targetPoint != null)
                {
                    // Расчет поворота
                    transform.right += Vector3.Lerp(transform.right, _targetPoint.transform.localPosition - transform.localPosition, 0.7f * Time.deltaTime);

                    // Расчет перемещения
                    float speedThisFrame = _rateSpeed * Time.deltaTime;
                    transform.localPosition += (_targetPoint.transform.localPosition - transform.localPosition).normalized * speedThisFrame;
                    if (Vector3.Distance(transform.localPosition, _targetPoint.transform.localPosition) < speedThisFrame * 1.5f)
                        NewPosition(_targetPoint);
                }
                else NewPosition(_currentPoint);
            }
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            gameObject.SetActive(true);
            transform.right = Vector3.right;
            _lastSearchPathTime = -100f;
            delay = 2f;
            _currentPoint = null;
            _currentPath = new List<Point>();
            if (listStartPoints.Count > 0)
            {
                _currentPoint = listStartPoints[Random.Range(0, listStartPoints.Count)];
                transform.localPosition = _currentPoint.transform.localPosition;
            }

            _isActive = true;
        }

        private void NewPosition(Point p)
        {
            Point prewPoint = _currentPoint;

            _currentPoint = p;
            _targetPoint = null;

            // Проверка на победу или проигрыш
            if (_currentPoint.isMousetrap)
            {
                MiniGame23_Manager.instance.KilledRat(this);
                return;
            }
            else if (_currentPoint.isCable)
            {
                MiniGame23_Manager.instance.EatCable(_currentPoint);
                return;
            }
            else if (_currentPoint.isPugalka)
            {
                if (_currentPath != null)
                    _currentPath.Clear();
                _targetPoint = prewPoint;
                return;
            }

            // Если нет пути, пытаемся найти
            if ((_currentPath == null || _currentPath.Count == 0) && Time.timeSinceLevelLoad - _lastSearchPathTime > 3f)
            {
                _lastSearchPathTime = Time.timeSinceLevelLoad;
                if (_currentPath == null)
                    _currentPath = new List<Point>();
                else _currentPath.Clear();
                List<Point> l = new List<Point>();
                if (!MiniGame23_Manager.instance.FindShorterPath(ref _currentPath, ref l, null, _currentPoint, pointFinish))
                    _currentPath.Clear();
                //else
                //{
                //    string s = "";
                //    foreach (Point pp in _currentPath)
                //        s += pp.ToString() + "  ";
                //    Debug.Log(s);
                //}
            }

            // Если нашли, выбираем новую точку, к которой будем двигаться
            if (_currentPath != null && _currentPath.Count > 0)
            {
                if (_currentPath.Contains(_currentPoint))
                {
                    int index = _currentPath.IndexOf(_currentPoint);
                    if (_currentPath.Count > index + 1)
                    {
                        _targetPoint = _currentPath[index + 1];
                        //Debug.Log("true");
                    }
                }
                else _currentPath.Clear();
            }
            
            if (_targetPoint == null || _targetPoint.isPugalka)
            {
                if (_currentPath != null)
                    _currentPath.Clear();
                // Выбираем точки из текущей позиции, к которым можем двигаться и двигаемся к случайной
                List<Point> list = new List<Point>();
                foreach (Point point in _currentPoint.listPoints)
                    if (!point.isPugalka && point != _currentPoint)
                        list.Add(point);

                //Debug.Log(list.Count);
                if (list.Count > 1)
                    if (list.Contains(prewPoint))
                        list.Remove(prewPoint);

                if (list.Count > 0)
                    _targetPoint = list[Random.Range(0, list.Count)];
                else _targetPoint = null;
            }
        }
    }
}