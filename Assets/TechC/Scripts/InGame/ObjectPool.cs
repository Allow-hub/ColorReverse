using System.Collections.Generic;
using UnityEngine;

namespace TechC
{
    [System.Serializable]
    public class ObjectPoolItem
    {
        public string name;
        public GameObject prefab;      // プールするプレハブ
        public GameObject parent;      // プールの親オブジェクト
        public int initialSize;        // 初期サイズ
    }

    public class ObjectPool : MonoBehaviour
    {
        [Header("Object Pool Settings")]
        [SerializeField] private ObjectPoolItem[] poolItems; // 各プレハブの設定をひとまとめにした配列

        // デリゲートの定義
        public delegate void ObjectCreatedHandler(GameObject createdObject, int poolIndex);
        public event ObjectCreatedHandler OnObjectCreated;
        // 各プレハブごとのオブジェクトプール
        private Dictionary<GameObject, Queue<GameObject>> objectPools = new Dictionary<GameObject, Queue<GameObject>>();

        // 初期化メソッド：コンストラクタの代わりにこのメソッドを使用
        public void Initialize(ObjectPoolItem[] items)
        {
            poolItems = items;
        }

        private void Awake()
        {
            // 初期化: 各プレハブに対してオブジェクトプールを作成し、初期サイズ分だけオブジェクトを生成
            if (poolItems == null)
            {
                Debug.LogError("Object Pool is not initialized properly. Please call Initialize() before using the pool.");
                return;
            }

            foreach (var poolItem in poolItems)
            {
                GameObject prefab = poolItem.prefab;
                GameObject parent = poolItem.parent;
                int initialSize = poolItem.initialSize;

                if (!objectPools.ContainsKey(prefab))
                {
                    objectPools[prefab] = new Queue<GameObject>();
                }

                // 初期サイズ分オブジェクトを生成してプールに格納
                for (int i = 0; i < initialSize; i++)
                {
                    GameObject newObject = Instantiate(prefab);
                    newObject.SetActive(false); // 初期状態は非アクティブ
                    newObject.transform.SetParent(parent.transform); // Object Pool の子オブジェクトとして配置
                    objectPools[prefab].Enqueue(newObject);   // プールに格納
                }
            }
        }

        // プレハブからオブジェクトを取得する
        public GameObject GetObject(GameObject prefab)
        {
           
            int poolIndex = GetPoolIndex(prefab);

            if (objectPools.ContainsKey(prefab) && objectPools[prefab].Count > 0)
            {
                GameObject pooledObject = objectPools[prefab].Dequeue();
                pooledObject.SetActive(true);
                return pooledObject;
            }
            else
            {
               
                GameObject newObject = Instantiate(prefab);
                OnObjectCreated?.Invoke(newObject, poolIndex);
                return newObject;
            }
        }

        // オブジェクトをプールに返却する
        public void ReturnObject(GameObject obj)
        {
            // オブジェクトを非アクティブにしてからプールに戻す
            obj.SetActive(false);
            GameObject prefab = GetPrefabFromObject(obj);

            if (prefab != null && objectPools.ContainsKey(prefab))
            {
                obj.transform.SetParent(GetParentFromPrefab(prefab).transform); // プールの親オブジェクトに設定
                objectPools[prefab].Enqueue(obj);    // プールに戻す
                Debug.Log("Object returned to pool: " + obj.name);
            }
            else
            {
                Debug.Log("オブジェクトが見つかりませんでした、破棄します: " + obj.name);
                Destroy(obj); // プレハブが見つからなければオブジェクトを破棄
            }
        }


        // オブジェクトからそのプレハブを取得する
        private GameObject GetPrefabFromObject(GameObject obj)
        {
            foreach (var kvp in objectPools)
            {
                if (kvp.Value.Contains(obj))
                {
                    return kvp.Key;
                }
            }
            return null;
        }

        // プレハブから親オブジェクトを取得する
        private GameObject GetParentFromPrefab(GameObject prefab)
        {
            foreach (var poolItem in poolItems)
            {
                if (poolItem.prefab == prefab)
                {
                    return poolItem.parent;
                }
            }
            return null;
        }

        private int GetPoolIndex(GameObject prefab)
        {
            for (int i = 0; i < poolItems.Length; i++)
            {
                if (poolItems[i].prefab == prefab)
                {
                    return i;
                }
            }
            return -1; 
        }
        public ObjectPoolItem GetPoolItem(int index)
        {
            if (poolItems != null && poolItems.Length > 0)
            {
                return poolItems[index];
            }
            else
            {
                Debug.LogWarning("poolItems 配列が空、または初期化されていません。");
                return null;
            }
        }
    }
}
