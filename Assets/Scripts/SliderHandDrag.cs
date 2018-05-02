using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag: HandDraggable {

		public float maxHeight = 3.0f;
		public float minHeight = 1.0f;
		public float scaleFactor = 5f;
		public float percentageThreshold = 0.9f;
		public float timeThreshold = 3.0f;
		private GameObject barsHolder;
		private List<GameObject> investBlocks;
		private float distanceCam;
		private float timeCounter;
		private TransitionManager transitionManager;
		private bool pedastalCheck = false;
		private bool graphCompleted= false;


		protected override void Start() {
			base.Start();
			transitionManager = FindObjectOfType<TransitionManager>();
			HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
			SpawnObjectsController.instance.FallingBlocksInstance.GraphCompleted += OnGraphCompleted;

			if (percentageThreshold > 1.0f)
				percentageThreshold = 1.0f;
			if (percentageThreshold <= 0.0f)
				percentageThreshold = 0.01f;

			if (timeThreshold <= 0.0f)
				timeThreshold = 0.1f;
		}

		protected override void Update() {
			base.Update();
			if (!pedastalCheck) {
				if (HostTransform.position.y >= (percentageThreshold * maxHeight))
					timeCounter += Time.deltaTime;

				if (timeCounter >= timeThreshold) {
					if (transitionManager != null)
						transitionManager.RaisePedastal();
					else
						GameObject.FindObjectOfType<TransitionManagerScaling>().RaisePedastal();
					pedastalCheck = true;
				}
			}
		}

		protected override void StartDragging(Vector3 initialDraggingPosition) {
			base.StartDragging(initialDraggingPosition);
			ConstraintCheck();
			if (graphCompleted)
				ChangeScale();
		}


		protected override void UpdateDragging() {
			//HostTransform.Rotate(Vector3.up, speed * Time.deltaTime);
			base.UpdateDragging();
			ConstraintCheck();
			if (graphCompleted)
				ChangeScale();
		}


		protected override void StopDragging() {
			base.StopDragging();
			ConstraintCheck();
			if (graphCompleted)
				ChangeScale();
		}

		private void ConstraintCheck() {
			//distanceCam = Vector3.Distance(HostTransform.position, VR_camera.transform.position);
			if (HostTransform.position.y > maxHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, maxHeight, HostTransform.position.z);

			if (HostTransform.position.y < minHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
		}

		public void ChangeScale() {
			float scaleNum = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);

			foreach (GameObject IB in SpawnObjectsController.instance.InvestBlocks) {
				IB.transform.localScale = new Vector3(IB.transform.localScale.x, scaleFactor * scaleNum , IB.transform.localScale.z);
			}
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
			rg.GetComponent<BoxCollider>().enabled = false;
			GetComponent<SliderHandDrag>().enabled = false;
		}
	}
}

