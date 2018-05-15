using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag: HandDraggable {

		public float maxHeight = 3.0f;
		public float minHeight = 1.0f;
		public float scaleFactor = 10f;
		public float percentageThreshold = 0.9f;
		public float timeThreshold = 3.0f;
		private GameObject barsHolder;
		private List<GameObject> investBlocks;
		private float distanceCam;
		private float timeCounter;
		private TransitionManager transitionManager;
		private bool pedastalCheck = false;
		private bool graphCompleted= false;
		private AudioManager audioManager;
		private float[] thresholds;
		private List<float> thresholdCheck;


		protected override void Start() {
			base.Start();
			audioManager = FindObjectOfType<AudioManager>();
			transitionManager = FindObjectOfType<TransitionManager>();
			HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
			SpawnObjectsController.instance.FallingBlocksInstance.GraphCompleted += OnGraphCompleted;

			if (percentageThreshold > 1.0f)
				percentageThreshold = 1.0f;
			if (percentageThreshold <= 0.0f)
				percentageThreshold = 0.01f;

			if (timeThreshold <= 0.0f)
				timeThreshold = 0.1f;

			thresholds = new float[5];
			float increment = percentageThreshold / (thresholds.Length+1);
			for (int i = 0; i < thresholds.Length; i++) {
				thresholds[i] = increment * (i + 1);
			}
			thresholdCheck = new List<float>(); 
		}

		protected override void Update() {
			base.Update();
			if (!pedastalCheck) {
				if (HostTransform.position.y >= (percentageThreshold * maxHeight))
					timeCounter += Time.deltaTime;
				AudioCheck();
				if (timeCounter >= timeThreshold) {
					//audioManager.NowPlay(AudioManager.Audio.StatStepFinal);
					if (transitionManager != null)
						transitionManager.RaisePedastal();
					else
						GameObject.FindObjectOfType<TransitionManagerScaling>().RaisePedastal();
					pedastalCheck = true;
				}
			}
		}

		private void AudioCheck() {
			//Debug.Log("AudioCheck");
			float heightRatio = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);
			//Debug.Log("heightRatio: "+ heightRatio);
			if (heightRatio >= thresholds[0] && !thresholdCheck.Contains(thresholds[0])) {
				Debug.Log("Now Playing 1");
				audioManager.NowPlay(AudioManager.Audio.StatStep1);
				thresholdCheck.Add(thresholds[0]);
			}
			else if (heightRatio >= thresholds[1] && !thresholdCheck.Contains(thresholds[1])) {
				Debug.Log("Now Playing 2");
				audioManager.NowPlay(AudioManager.Audio.StatStep2);
				thresholdCheck.Add(thresholds[1]);
			}
			else if (heightRatio >= thresholds[2] && !thresholdCheck.Contains(thresholds[2])) {
				Debug.Log("Now Playing 3");
				audioManager.NowPlay(AudioManager.Audio.StatStep3);
				thresholdCheck.Add(thresholds[2]);
			}
			else if (heightRatio >= thresholds[3] && !thresholdCheck.Contains(thresholds[3])) {
				Debug.Log("Now Playing 4");
				audioManager.NowPlay(AudioManager.Audio.StatStep4);
				thresholdCheck.Add(thresholds[3]);
			}
			else if (heightRatio >= thresholds[4] && !thresholdCheck.Contains(thresholds[4])) {
				Debug.Log("Now Playing 5");
				audioManager.NowPlay(AudioManager.Audio.StatStep5);
				thresholdCheck.Add(thresholds[4]);
			} else if (heightRatio >= percentageThreshold && !thresholdCheck.Contains(percentageThreshold)) {
				Debug.Log("Now Playing Final");
				audioManager.NowPlay(AudioManager.Audio.StatStepFinal);
				thresholdCheck.Add(percentageThreshold);
			}

		}

		protected override void StartDragging(Vector3 initialDraggingPosition) {
			base.StartDragging(initialDraggingPosition);
			ConstraintCheck();
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}


		protected override void UpdateDragging() {
			//HostTransform.Rotate(Vector3.up, speed * Time.deltaTime);
			base.UpdateDragging();
			ConstraintCheck();
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}


		protected override void StopDragging() {
			base.StopDragging();
			ConstraintCheck();
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}

		private void ConstraintCheck() {
			//distanceCam = Vector3.Distance(HostTransform.position, VR_camera.transform.position);
			if (HostTransform.position.y > maxHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, maxHeight, HostTransform.position.z);

			if (HostTransform.position.y < minHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
		}

		private IEnumerator ChangeScale() {
			var wait = new WaitForSeconds(.01f);
			float scaleNum = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);
			//Scales the numbers
			foreach(Category c in SpawnObjectsController.CategoryList) {
				
				c.Sum = c.Sum*(scaleNum + 1);
				//Debug.Log("Scale number is: "+ scaleNum +" "+ c.Name + " " +c.Sum);
			}


			foreach (GameObject IB in SpawnObjectsController.instance.InvestBlocks) {
				IB.transform.localScale = new Vector3(IB.transform.localScale.x, scaleFactor * scaleNum, IB.transform.localScale.z);
				yield return wait;
			}

			yield break;
		}

		public void OnGraphCompleted(object source, EventArgs e) {
			MakeInvestBlocksReady();

			graphCompleted = true;
		}

		public void MakeInvestBlocksReady() {

			foreach(GameObject IB in SpawnObjectsController.instance.InvestBlocks) {
				Renderer rend =IB.GetComponentInChildren<Renderer>();
				rend.enabled=true;
				IB.transform.localScale = new Vector3(IB.transform.localScale.x, 0, IB.transform.localScale.z);
			}
		}

		public void BeginFalling() {
			SetDragging(false);
			Rigidbody rg = GetComponent<Rigidbody>();
			rg.isKinematic = false;
			rg.useGravity = true;
			rg.GetComponent<SphereCollider>().enabled = false;
			GetComponent<SliderHandDrag>().enabled = false;
		}
	}
}

