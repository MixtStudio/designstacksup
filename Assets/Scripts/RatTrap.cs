using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class RatTrap : HandDraggable {

		private TransitionManager transitionManager;
		private bool fallingCheck = false;

		protected override void Start() {
			base.Start();
			transitionManager = FindObjectOfType<TransitionManager>();
		}

		protected override void StartDragging(Vector3 initialDraggingPosition) {
			base.StartDragging(initialDraggingPosition);

			if (!fallingCheck) {
				transitionManager.BeginFalling();
				fallingCheck = true;
			}

		}

	}
}
