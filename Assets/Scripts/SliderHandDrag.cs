using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag: HandDraggable {

		public float maxHeight = 3.0f;
		public float minHeight = 1.0f;
		public float scaleFactor = 3.0f;

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

