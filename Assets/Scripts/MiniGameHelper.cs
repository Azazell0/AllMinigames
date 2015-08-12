using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MiniGameHelper
{
    /// <summary>
    /// Инстанциирует объект нужного типа
    /// </summary>
    /// <typeparam name="T">Тип объекта</typeparam>
    /// <param name="Path">Путь к префабу</param>
    /// <param name="Parent">Указатель на родителя</param>
    /// <returns>Возвращает указатель на экземпляр класса указанного типа</returns>
    public static T InstanceObject<T>(string Path, Transform Parent = null) where T : MonoBehaviour
    {
        GameObject go = MonoBehaviour.Instantiate(Resources.Load(Path)) as GameObject;
        if (go == null)
        {
            Debug.LogError("Prefab wasn't find!. Path = " + Path);
            return null;
        }

        T classIn = go.GetComponent<T>();
        if (classIn == null)
        {
            Debug.LogError("Prefab hasn't script " + typeof(T).ToString());
            MonoBehaviour.Destroy(go);
            return null;
        }

        if (Parent != null)
        {
            Vector3 localScale = classIn.transform.localScale;
            Vector3 localPosition = classIn.transform.localPosition;
            classIn.transform.parent = Parent;
            classIn.transform.localScale = localScale;
            classIn.transform.localPosition = localPosition;
        }

        return classIn;
    }

    /// <summary>
    /// Активирование рандомных трансформов среди детей Transform'а
    /// </summary>
    /// <param name="root">Указатель на Transform</param>
    /// <param name="count">Количество активируемых детей</param>
    public static void ActiveRandomChilds(Transform root, int count, bool deactivateRest = false)
    {
        if (root == null || root.childCount == 0 || count < 1)
            return;

        if (count > root.childCount)
            count = root.childCount;

        List<Transform> listChilds = new List<Transform>();
        foreach (Transform t in root)
            listChilds.Add(t);

        for (int i = 0; i < count; i++)
        {
            int k = Random.Range(0, listChilds.Count);
            listChilds[k].gameObject.SetActive(true);
            listChilds.RemoveAt(k);
        }

        if (deactivateRest)
            foreach (Transform t in listChilds)
                t.gameObject.SetActive(false);
    }

    /// <summary>
    /// Пытается вернуть случайный объект из списка, который не содержится в исключениях
    /// </summary>
    /// <typeparam name="T">Тип объектов</typeparam>
    /// <param name="list">Список объектов</param>
    /// <param name="obj">Параметр в который будет помещен результат</param>
    /// <param name="exceptionObject">Исключения</param>
    /// <returns>true - возвращен рандомный элемент списка, который не содержится в исключениях.
    /// false - возвращенный элемент может содержаться в списке исключений или быть значением default.</returns>
    public static bool GetRandomObjectIfMay<T>(List<T> list, ref T obj, params T[] exceptionObject)
    {
        List<T> listEx = new List<T>();
        if (exceptionObject != null)
            for (int i = 0; i < exceptionObject.Length; i++)
                listEx.Add(exceptionObject[i]);
        return GetRandomObjectIfMay(list, ref obj, listEx);
    }

    private static bool GetRandomObjectIfMay<T>(List<T> list, ref T obj, List<T> exceptionObjectList)
    {
        obj = default(T);
        if (list == null || list.Count == 0)
            return false;

        if (list.Count == 1)
        {
            obj = list[0];
            if (exceptionObjectList != null)
                return (exceptionObjectList.Contains(obj)) ? false : true;
            else return true;
        }
        else
        {
            List<T> listNew = new List<T>(list);
            if (exceptionObjectList != null)
                foreach (T v in exceptionObjectList)
                    if (listNew.Contains(v))
                        listNew.Remove(v);

            if (listNew.Count > 0)
            {
                obj = listNew[Random.Range(0, listNew.Count)];
                return true;
            }
            else
            {
                obj = list[Random.Range(0, list.Count)];
                return false;
            }
        }
    }

    /// <summary>
    /// Просматривает всех детей трансформа root и добавляет в список тех детей, кто содержит указанный скрипт
    /// </summary>
    /// <typeparam name="T">Тип скрипта</typeparam>
    /// <param name="root">Трансформ-отец</param>
    /// <param name="list">Список</param>
    public static void FindChildObjects<T>(Transform root, ref List<T> list) where T : UnityEngine.Component
    {
        if (list == null)
            list = new List<T>();
        if (root == null)
        {
            Debug.LogWarning("Root transform is null");
            return;
        }
        foreach (Transform t in root)
        {
            T p = t.GetComponent<T>();
            if (p != null)
                list.Add(p);
        }
    }

    /// <summary>
    /// Случайная сортировка списка (перемешивание)
    /// </summary>
    /// <param name="list">Указатель на список</param>
    /// <param name="iterationsCount">Количество итераций</param>
    public static void ListRandomSort<T>(ref List<T> list, int iterationsCount)
    {
        if (list == null || list.Count < 2)
            return;
        for (int i = 0; i < iterationsCount; i++)
        {
            int j = Random.Range(0, list.Count);
            T mb = list[j];
            list.RemoveAt(j);
            list.Insert(Random.Range(0, list.Count), mb);
        }
    }

    /// <summary>
    /// Случайная сортировка позиций трансформов в списке (перемешивание)
    /// </summary>
    /// <param name="list">Указатель на список</param>
    /// <param name="iterationsCount">Количество итераций</param>
    public static void ListRandomSortTransformPositions<T>(ref List<T> list, int iterationsCount) where T : MonoBehaviour
    {
        if (list == null || list.Count < 2)
            return;
        for (int i = 0; i < iterationsCount; i++)
        {
            int j1 = Random.Range(0, list.Count);
            int j2 = j1;
            while (j1 == j2)
                j2 = Random.Range(0, list.Count);

            T transform1 = list[j1];
            T transform2 = list[j2];

            Vector3 v = transform1.transform.position;
            transform1.transform.position = transform2.transform.position;
            transform2.transform.position = v;
        }
    }

    /// <summary>
    /// Рекурсивно устанавливает слой для объекта и всех его дочерних объектов
    /// </summary>
    /// <param name="tr">Указатель на объект</param>
    /// <param name="layer">Слой</param>
    public static void SetLayerRecursieve(Transform tr, int layer)
    {
        if (tr == null)
            return;
        tr.gameObject.layer = layer;
        foreach (Transform t in tr)
            SetLayerRecursieve(t, layer);
    }
        

    public static bool ArrayContains<T>(T[] array, T value)
    {
        if (array == null)
            return false;

        for (int i = 0; i < array.Length; i++)
            if (array[i].Equals(value))
                return true;

        return false;
    }
}
