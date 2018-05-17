using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Input;

public class LaserPointer : MonoBehaviour {

	public GameObject target;
	
	private Transform controller;
	private LineRenderer LR;
	private bool draw;
	private InteractionSourceState[] interactionSourceStates;
	
	
	void Start () {		
		LR = GetComponent<LineRenderer>();
		
	}
	
	void Update () {
		if(controller == null){
			LR.enabled = false;
			Initialize();
		}
		
		if(!draw && controller != null) {
			interactionSourceStates = InteractionManager.GetCurrentReading();
			foreach(InteractionSourceState interactionSourceState in interactionSourceStates){
				if(interactionSourceState.anyPressed){
					draw = true;
					break;
				}
			}
		}
		
		if(draw && controller != null)
			DrawLaser();
	}
	
	private void Initialize(){
		if(transform.childCount > 0)
			controller = transform.GetChild(0);
		if(controller != null)
			LR.enabled = true;
	}
	
	private void DrawLaser(){
		Vector3[] positions = new Vector3[2];
		positions[0] = controller.transform.position;
		positions[1] = target.transform.position;
		LR.positionCount = positions.Length;
		LR.SetPositions(positions);
	}
}
