using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler: Mixt.Singleton<ObjectPooler> {

	[System.Serializable]
	public class Pool {
		public string tag;
		public GameObject prefab;
		public int size;
		public Queue<GameObject> objectPool;
	}

	public List<Pool> pools;
	public Dictionary<string, Queue<GameObject>> poolDictionary;
	
	////Working
	//protected override void Init() {

	//	poolDictionary = new Dictionary<string, Queue<GameObject>>();
	//	foreach (Pool pool in pools) {
	//		Queue<GameObject> objectPool = new Queue<GameObject>();

	//		for (int i = 0; i < pool.size; i++) {
	//			GameObject obj = Instantiate(pool.prefab);
	//			obj.SetActive(false);
	//			objectPool.Enqueue(obj);
	//		}

	//		poolDictionary.Add(pool.tag, objectPool);
	//	}
	//}


	protected override void Init() {

		poolDictionary = new Dictionary<string, Queue<GameObject>>();
		foreach (Pool pool in pools) {
			pool.objectPool = new Queue<GameObject>();

			for (int i = 0; i < pool.size; i++) {
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				pool.objectPool.Enqueue(obj);
			}

			poolDictionary.Add(pool.tag, pool.objectPool);
		}
	}


	public GameObject SpawnFromPool(string tag, bool enqueue, bool interactive, Vector3 position,Quaternion rotation) {

		if (!poolDictionary.ContainsKey(tag)) {
			Debug.LogWarning("Pool with tag " + tag + "doesn't exist");
			return null;
		}

		Debug.Log("Numer "+ poolDictionary[tag].Count);
		if (poolDictionary[tag].Count == 0) {
			//GrowPool(tag);
		}

		GameObject objectToSpawn = poolDictionary[tag].Dequeue();

		objectToSpawn.SetActive(true);
		objectToSpawn.transform.position = position;
		objectToSpawn.transform.rotation = rotation;
		IPooledObjects pooledObject= objectToSpawn.GetComponent<IPooledObjects>();

		if (pooledObject!= null) {
			pooledObject.OnObjectSpawn(interactive);
		}

		if (enqueue) {
			poolDictionary[tag].Enqueue(objectToSpawn);
		}
			
		return objectToSpawn;
	}

	public void AddToPool(string tag, GameObject obj) {
		obj.SetActive(false);
		poolDictionary[tag].Enqueue(obj);
	}

	private void GrowPool(string tag) {
		try {
			Pool pool = pools.Find(x => x.tag.Contains(tag));
			Debug.Log("YES I ENTERED");
			for (int i = 0; i < 10; i++) {
				GameObject obj = Instantiate(pool.prefab);
				obj.SetActive(false);
				pool.objectPool.Enqueue(obj);
			}
		} 
		
		catch{
			Debug.Log("No pool found");
		}	
	}
}
