using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeFinder : Mixt.Singleton<GazeFinder> {

	public GameObject gazeCursor;
	
	private RaycastHit hit;
	private GraphInteraction graphInteraction;
	private int layerMask;
	
	public RaycastHit GetRayCastHit(){return hit;}
	
	protected override void Init() {}

	void Start () {
		layerMask = 1 << LayerMask.NameToLayer("Gaze");
		gazeCursor.SetActive(false);
	}
	
	void Update()
    {		
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, layerMask))
        {			
			if(graphInteraction == null){
				graphInteraction = hit.transform.parent.GetComponent<GraphInteraction>();
				if(!gazeCursor.activeSelf)
					gazeCursor.SetActive(true);
				return;
			}
			
			GraphInteraction newGraphInteraction = hit.transform.parent.GetComponent<GraphInteraction>();

			if (newGraphInteraction != null) {
				if (newGraphInteraction != graphInteraction) {
					graphInteraction.OnGazeExit();
					newGraphInteraction.OnGazeEnter();
					graphInteraction = newGraphInteraction;
				}
			}			
        } 
		else {
			if(graphInteraction != null){
				graphInteraction.OnGazeExit();
			}
		}
    }
}
