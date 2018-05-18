using UnityEngine;

namespace Mixt {

	public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour {

		public static T Instance { get; private set; }

		private void Awake() {
			if (Instance != null && Instance != (this as T)) {
				Debug.LogWarning("There cannot be more than one instance of " + typeof(T));
				if (Application.isPlaying) {
					Destroy(this);
				} else {
					DestroyImmediate(this);
				}
				return;
			}

			Instance = this as T;
			Init();
		}

		protected abstract void Init();

	}

}