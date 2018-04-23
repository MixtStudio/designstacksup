using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag: HandDraggable {

		public float maxHeight = 3.0f;
		public float minHeight = 1.0f;
		public float scaleFactor = 3.0f;
		public Camera VR_camera;
		public float percentageThreshold = 0.9f;
		public float timeThreshold = 3.0f;

		private GameObject barsHolder;
		private float distanceCam;
		private float timeCounter;
		private TransitionManager transitionManager;
		private bool pedastalCheck = false;


		protected override void Start() {
			base.Start();
			transitionManager = FindObjectOfType<TransitionManager>();
			HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);

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
			ChangeScale();
		}


		protected override void UpdateDragging() {
			//HostTransform.Rotate(Vector3.up, speed * Time.deltaTime);
			base.UpdateDragging();
			ConstraintCheck();
			ChangeScale();
		}


		protected override void StopDragging() {
			base.StopDragging();
			ConstraintCheck();
			ChangeScale();
		}

		private void ConstraintCheck() {
			distanceCam = Vector3.Distance(HostTransform.position, VR_camera.transform.position);
			//Debug.Log("Distance of " + VR_camera.name + "is "+ distanceCam);
			if (HostTransform.position.y > maxHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, maxHeight, HostTransform.position.z);

			if (HostTransform.position.y < minHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
		}

		public void ChangeScale() {
			if (barsHolder == null)
				barsHolder = transitionManager.GetBarsHolder();

			if (barsHolder == null)
				barsHolder = GameObject.FindObjectOfType<TransitionManagerScaling>().GetBarsHolder();

			float scaleNum = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);
			barsHolder.transform.localScale = new Vector3(barsHolder.transform.localScale.x, (scaleFactor*scaleNum)+1, barsHolder.transform.localScale.z);
		}

		public void BeginFalling() {
			Rigidbody rg = GetComponent<Rigidbody>();
			rg.isKinematic = false;
			rg.useGravity = true;
			rg.GetComponent<BoxCollider>().enabled = false;
			GetComponent<SliderHandDrag>().enabled = false;
		}

	}
}

