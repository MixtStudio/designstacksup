using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler: Mixt.Singleton<ObjectPooler> {

	[System.Serializable]
	public class Pool {
		public string tag;
		public GameObject prefab;
		public int size;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;

	protected override void Init() {
		poolDictionary = new Dictionary<string, Queue<GameObject>>();

		foreach (Pool pool in pools) {
			Queue<GameObject> objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++) {
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, objectPool);
		}
	}


	public GameObject SpawnFromPool(string tag, bool interactive, Vector3 position,Quaternion rotation) {
		if (!poolDictionary.ContainsKey(tag)) {
			Debug.LogWarning("Pool with tag " + tag + "doesn't exist");
			return null;
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		IPooledObjects pooledObject= objectToSpawn.GetComponent<IPooledObjects>();

		if (pooledObject!= null) {
			pooledObject.OnObjectSpawn(interactive);
		}

			poolDictionary[tag].Enqueue(objectToSpawn);
		return objectToSpawn;
	}
}
