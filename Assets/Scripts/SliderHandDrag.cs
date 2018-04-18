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
		private float distanceCam;
		public float speed = 20f;

		private GameObject barsHolder;

		protected override void Start() {
			base.Start();
			HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
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
			Debug.Log("Distance of " + VR_camera.name + "is "+ distanceCam);
			if (HostTransform.position.y > maxHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, maxHeight, HostTransform.position.z);

			if (HostTransform.position.y < minHeight)
				HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
		}

		public void SetBarsHolder(GameObject obj) { barsHolder = obj; }

		public void ChangeScale() {
			if (barsHolder == null) return;

			float scaleNum = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);
			barsHolder.transform.localScale = new Vector3(barsHolder.transform.localScale.x, (scaleFactor*scaleNum)+1, barsHolder.transform.localScale.z);
		}

	}
}

