using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace Minigame27
{
    public class Cell : MonoBehaviour
    {
        #region variables

        /// <summary>
        /// Позиция клетки на поле по координате X
        /// </summary>
        public int positionX { get { return _positionX; } }
        /// <summary>
        /// Позиция клетки на поле по координате Y
        /// </summary>
        public int positionY { get { return _positionY; } }

        public Cell cellUp, cellDown, cellRight, cellLeft;
        /// <summary>
        /// Тип объекта в клетке
        /// </summary>
        public ObjectType type
        {
            get { return _type; }
            set { _type = value; UpdateSprite(); }
        }

        /// <summary>
        /// Тип объекта в клетке
        /// </summary>
        private ObjectType _type = ObjectType.o0;
        /// <summary>
        /// true - клетка корректна
        /// </summary>
        private bool _correctCell = false;
        /// <summary>
        /// Спрайт клетки
        /// </summary>
        private UISprite _sprite;
        /// <summary>
        /// Позиция клетки на поле по координате X
        /// </summary>
        private int _positionX;
        /// <summary>
        /// Позиция клетки на поле по координате Y
        /// </summary>
        private int _positionY;

        #endregion

        
        void Awake()
        {
            if (transform.childCount > 0)
                _sprite = transform.GetChild(0).GetComponent<UISprite>();
            Init();
        }

        void Start()
        {
            Reset();
        }

        /// <summary>
        /// Сбрасывает объект в клетке на пустой
        /// </summary>
        public void Reset()
        {
            _type = ObjectType.o0;
            UpdateSprite();
        }

        /// <summary>
        /// Установка случайного объекта
        /// </summary>
        public void SetRandomObject()
        {
            _type = (ObjectType)UnityEngine.Random.Range(1, Enum.GetValues(typeof(ObjectType)).Length);
            UpdateSprite();
        }

        /// <summary>
        /// Устанавливает в клетку объект определенного типа
        /// </summary>
        /// <param name="obj">Тип объекта</param>
        public void SetObject(ObjectType obj)
        {
            type = obj;
        }

        /// <summary>
        /// Проверка на соседство с клеткой
        /// </summary>
        /// <param name="cell">Указатель на клетку</param>
        /// <returns>true - соседи</returns>
        public bool IsNeighbour(Cell cell)
        {
            if (cell == cellDown || cell == cellUp || cell == cellRight || cell == cellLeft)
                return true;
            return false;
        }

        /// <summary>
        /// Проверяет клетку и соседние клетки на условие "три в ряд"
        /// </summary>
        /// <param name="list">Список, в который будут помещены клетки, образующие "три в ряд"</param>
        /// <returns>true - условие выполнено</returns>
        public bool ThreeInARow(ref List<Cell> list)
        {
            list = new List<Cell>();

            if (!CompareType(this, cellUp, cellDown, ref list))
            {
                if (cellUp != null)
                    CompareType(this, cellUp, cellUp.cellUp, ref list);
                if (cellDown != null)
                    CompareType(this, cellDown, cellDown.cellDown, ref list);
            }
            if (!CompareType(this, cellRight, cellLeft, ref list))
            {
                if (cellRight != null)
                    CompareType(this, cellRight, cellRight.cellRight, ref list);
                if (cellLeft != null)
                    CompareType(this, cellLeft, cellLeft.cellLeft, ref list);
            }

            return (list.Count > 0) ? true : false;
        }

        /// <summary>
        /// Проверяет клетку и соседние клетки на условие "три в ряд"
        /// При этом меняет текущую клетку и переданную в качестве параметра местами.
        /// Необходимо для проверки, можно ли поменять клетки местами, и при этом выполнить условие "три в ряд"
        /// </summary>
        /// <param name="cell">Клетка, которая будет менять местами с текущей</param>
        /// <returns></returns>
        public bool ThreeInARowAfterMove(Cell cell)
        {
            if (cell == null)
                return false;

            bool b = false;

            ObjectType t = this.type;
            this.type = cell.type;
            cell.type = t;

            if (CompareType(this, cellUp, cellDown) || CompareType(this, cellRight, cellLeft))
                b = true;

            if (cellUp != null)
                if (CompareType(this, cellUp, cellUp.cellUp))
                    b = true;
            if (cellDown != null)
                if (CompareType(this, cellDown, cellDown.cellDown))
                    b = true;
            if (cellRight != null)
                if (CompareType(this, cellRight, cellRight.cellRight))
                    b = true;
            if (cellLeft != null)
                if (CompareType(this, cellLeft, cellLeft.cellLeft))
                    b = true;

            t = this.type;
            this.type = cell.type;
            cell.type = t;

            return b;
        }

        /// <summary>
        /// Инициализация
        /// </summary>
        private void Init()
        {
            // Парсит имя клетки, получает из него координаты. Затем ищет и запоминает соседние клетки
            try
            {
                int index1 = name.IndexOf('(');
                int index2 = name.IndexOf(')');
                int index3 = name.LastIndexOf('(');
                int index4 = name.LastIndexOf(')');

                int length1 = index2 - 1 - index1;
                int length2 = index4 - 1 - index3;
                if (length1 <= 0 || length2 <= 0)
                    return;

                _positionX = Convert.ToInt32(name.Substring(index1 + 1, length1));
                _positionY = Convert.ToInt32(name.Substring(index3 + 1, length2));

                _correctCell = true;
            }
            catch
            {
                _correctCell = false;
                Debug.LogWarning("Incorrect cell");
                return;
            }

            cellUp = GetCell(_positionX, _positionY + 1);
            cellDown = GetCell(_positionX, _positionY - 1);
            cellRight = GetCell(_positionX + 1, _positionY);
            cellLeft = GetCell(_positionX - 1, _positionY);

        }

        /// <summary>
        /// Обновление спрайта для отображения корректного объекта в клетке
        /// </summary>
        private void UpdateSprite()
        {
            if (_sprite != null)
            {
                string s = "";
                switch (type)
                {
                    case ObjectType.o1:
                        s = "27_lopata";
                        break;

                    case ObjectType.o2:
                        s = "27_lopata2";
                        break;

                    case ObjectType.o3:
                        s = "27_parovoz";
                        break;

                    case ObjectType.o4:
                        s = "27_pesochn1";
                        break;

                    case ObjectType.o5:
                        s = "27_pesochn2";
                        break;

                    case ObjectType.o6:
                        s = "27_pyramid";
                        break;

                    case ObjectType.o7:
                        s = "27_vedro1";
                        break;

                    case ObjectType.o8:
                        s = "27_vedro2";
                        break;
                }

                _sprite.spriteName = s;
            }
        }

        /// <summary>
        /// Поиск клетки по ее координатам
        /// </summary>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        /// <returns>Клетка</returns>
        private Cell GetCell(int x, int y)
        {
            if (transform.parent == null)
                return null;
            Transform t = transform.parent.FindChild("Cell (" + x.ToString() + ") (" + y.ToString() + ")");
            if (t != null)
                return t.GetComponent<Cell>();

            return null;
        }

        /// <summary>
        /// Сравнивает объекты в трех клетках
        /// </summary>
        /// <param name="cell1">Клетка 1</param>
        /// <param name="cell2">Клетка 2</param>
        /// <param name="cell3">Клетка 3</param>
        /// <param name="list">Список, в который будут добавлены эти три клетке при равенстве объектов в них</param>
        /// <returns>true - клетки содержат объекты одного типа</returns>
        private bool CompareType(Cell cell1, Cell cell2, Cell cell3, ref List<Cell> list)
        {
            if (cell1 == null || cell2 == null || cell3 == null)
                return false;
            if (cell1.type == cell2.type && cell1.type == cell3.type)
            {
                if (list != null)
                {
                    list.Add(cell1);
                    list.Add(cell2);
                    list.Add(cell3);
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Сравнивает объекты в трех клетках
        /// </summary>
        /// <param name="cell1">Клетка 1</param>
        /// <param name="cell2">Клетка 2</param>
        /// <param name="cell3">Клетка 3</param>
        /// <returns>true - клетки содержат объекты одного типа</returns>
        private bool CompareType(Cell cell1, Cell cell2, Cell cell3)
        {
            if (cell1 == null || cell2 == null || cell3 == null)
                return false;
            if (cell1.type == cell2.type && cell1.type == cell3.type)
                return true;
            return false;
        }

        void Update()
        {
            if (!MiniGame27_Manager.instance.isPlay || MiniGame27_Manager.instance.isShadowBlock || type == ObjectType.o0)
                return;

            if (UICamera.hoveredObject == gameObject)
            {
                if (Input.GetMouseButtonDown(0))
                        MiniGame27_Manager.instance.SetCurrentCell(this);
                else if (Input.GetMouseButtonUp(0))
                {
                    Cell c = MiniGame27_Manager.instance.currentCell;
                    if (c != null && c.type != ObjectType.o0 && c != this && c.type != type)
                    {
                        Debug.Log(ThreeInARowAfterMove(c) + "   " + c.ThreeInARowAfterMove(this));
                        if (IsNeighbour(c) && (ThreeInARowAfterMove(c) || c.ThreeInARowAfterMove(this)))
                            MiniGame27_Manager.instance.SendShadowBetweenCell(this, c);
                    }
                }
            }
        }
    }


    public enum ObjectType { o0 = 0, o1, o2, o3, o4, o5, o6, o7, o8}
}