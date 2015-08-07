using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Minigame24
{
    public class Resource : MonoBehaviour
    {
        #region variables

        public ResourceType type = ResourceType.None;
        public Cell currentCell;

        #endregion

        
        void Start()
        {

        }

        
        void Update()
        {

        }

        public void Click()
        {
            MiniGame24_Manager.instance.SetCurrentResourceType(type);
        }

        public void Rotate()
        {
            if (type == ResourceType.TubeLine || type == ResourceType.TubeAngle)
            {
                Vector3 r = transform.rotation.eulerAngles + new Vector3(0f, 0f, 90f);
                transform.rotation = Quaternion.Euler(r);
            }
        }

        /// <summary>
        /// Возвращает направления, которые соединяет ресурс
        /// </summary>
        /// <returns></returns>
        public List<Direction> GetDirections()
        {
            List<Direction> list = new List<Direction>();

            switch (type)
            {
                case ResourceType.TubeLine:
                    float z = transform.localRotation.eulerAngles.z;
                    if (Mathf.Abs(z) < 1f || (Mathf.Abs(180 - z) < 1f))
                    {
                        list.Add(Direction.Left);
                        list.Add(Direction.Right);
                    }
                    else if (Mathf.Abs(90 - z) < 1f || (Mathf.Abs(270 - z) < 1f))
                    {
                        list.Add(Direction.Up);
                        list.Add(Direction.Down);
                    }
                    break;

                case ResourceType.TubeAngle:
                    float zz = transform.localRotation.eulerAngles.z;
                    if (Mathf.Abs(zz) < 1f)
                    {
                        list.Add(Direction.Up); list.Add(Direction.Right);
                    }
                    else if (Mathf.Abs(90 - zz) < 1f)
                    {
                        list.Add(Direction.Up); list.Add(Direction.Left);
                    }
                    else if (Mathf.Abs(180 - zz) < 1f)
                    {
                        list.Add(Direction.Down); list.Add(Direction.Left);
                    }
                    else if (Mathf.Abs(270 - zz) < 1f)
                    {
                        list.Add(Direction.Down); list.Add(Direction.Right);
                    }
                    break;

                case ResourceType.Controller:
                case ResourceType.Counter:
                case ResourceType.Pump:
                case ResourceType.Valve:
                    list.Add(Direction.Left);
                    list.Add(Direction.Right);
                    break;
            }

            return list;
        }
    }

    public enum ResourceType { None = 0, TubeLine, TubeAngle, Valve, Pump, Controller, Counter}

    public enum Direction { Up = 0, Right, Down, Left}
}