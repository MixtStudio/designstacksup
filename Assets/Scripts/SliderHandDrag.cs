using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag : HandDraggable {

		/// <summary>
		/// Axis to limit movement
		/// </summary>
		public enum Axis { x, y, z }
		[SerializeField]
		private Axis _axis;

		/// <summary>
		/// Update only one axis
		/// </summary>
//		override protected void UpdateDragging() {
//			Transform cameraTransform = CameraCache.Main.transform;

//			Vector3 inputPosition = Vector3.zero;
//#if UNITY_2017_2_OR_NEWER
//			InteractionSourceInfo sourceKind;
//			currentInputSource.TryGetSourceKind(currentInputSourceId, out sourceKind);
//			switch (sourceKind) {
//				case InteractionSourceInfo.Hand:
//					currentInputSource.TryGetGripPosition(currentInputSourceId, out inputPosition);
//					break;
//				case InteractionSourceInfo.Controller:
//					currentInputSource.TryGetPointerPosition(currentInputSourceId, out inputPosition);
//					break;
//			}
//#else
//            currentInputSource.TryGetPointerPosition(currentInputSourceId, out inputPosition);
//#endif

//			Vector3 pivotPosition = GetHandPivotPosition(cameraTransform);

//			Vector3 newHandDirection = Vector3.Normalize(inputPosition - pivotPosition);

//			newHandDirection = cameraTransform.InverseTransformDirection(newHandDirection); // in camera space
//			Vector3 targetDirection = Vector3.Normalize(gazeAngularOffset * newHandDirection);
//			targetDirection = cameraTransform.TransformDirection(targetDirection); // back to world space

//			float currentHandDistance = Vector3.Magnitude(inputPosition - pivotPosition);

//			float distanceRatio = currentHandDistance / handRefDistance;
//			float distanceOffset = distanceRatio > 0 ? (distanceRatio - 1f) * DistanceScale : 0;
//			float targetDistance = objRefDistance + distanceOffset;

//			draggingPosition = pivotPosition + (targetDirection * targetDistance);

//			if (RotationMode == RotationModeEnum.OrientTowardUser || RotationMode == RotationModeEnum.OrientTowardUserAndKeepUpright) {
//				draggingRotation = Quaternion.LookRotation(HostTransform.position - pivotPosition);
//			} else if (RotationMode == RotationModeEnum.LockObjectRotation) {
//				draggingRotation = HostTransform.rotation;
//			} else // RotationModeEnum.Default
//			  {
//				Vector3 objForward = cameraTransform.TransformDirection(objRefForward); // in world space
//				Vector3 objUp = cameraTransform.TransformDirection(objRefUp);           // in world space
//				draggingRotation = Quaternion.LookRotation(objForward, objUp);
//			}

//			Vector3 newPosition = Vector3.Lerp(HostTransform.position, draggingPosition + cameraTransform.TransformDirection(objRefGrabPoint), PositionLerpSpeed);
//			// Apply Final Position
//			if (hostRigidbody == null) {
//				HostTransform.position = newPosition;
//			} else {
//				hostRigidbody.MovePosition(newPosition);
//			}

//			// Apply Final Rotation
//			Quaternion newRotation = Quaternion.Lerp(HostTransform.rotation, draggingRotation, RotationLerpSpeed);
//			if (hostRigidbody == null) {
//				HostTransform.rotation = newRotation;
//			} else {
//				hostRigidbody.MoveRotation(newRotation);
//			}

//			if (RotationMode == RotationModeEnum.OrientTowardUserAndKeepUpright) {
//				Quaternion upRotation = Quaternion.FromToRotation(HostTransform.up, Vector3.up);
//				HostTransform.rotation = upRotation * HostTransform.rotation;
//			}
//		}
	}
}

