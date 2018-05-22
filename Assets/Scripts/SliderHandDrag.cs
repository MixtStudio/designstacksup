using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;


namespace HoloToolkit.Unity.InputModule {

	public class SliderHandDrag: HandDraggable {

		public float maxHeight = 3.0f;
		public float minHeight = 1.0f;
		public float scaleFactor = 10f;
		public float percentageThreshold = 0.8f;
		public float timeThreshold = 2.0f;
		public float backupTimeThreshold = 30.0f;

		private GameObject barsHolder;
		private List<GameObject> investBlocks;

		private float timeCounter;
		private bool pedastalCheck = false;
		private bool graphCompleted = false;
		private float[] thresholds;
		private List<float> thresholdCheck;
		private float timer = 0.0f;

		private DynamicTextController DIAL_UP_DESIGN;

		//To change dial from GRAB ME to DIAL UP DESIGN 
		private bool DIAL_UP_FLAG = false;

		private float offset_DIAL = .4f;

		protected override void Start() {
			base.Start();
			Visiblity(false);
			HostTransform.position = new Vector3(HostTransform.position.x, minHeight, HostTransform.position.z);
			SpawnObjectsController.Instance.FallingBlocksInstance.GraphCompleted += OnGraphCompleted;
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
			Update_DIAL_UP_DESIGN();

			if (!pedastalCheck) {
				if (HostTransform.position.y >= (percentageThreshold * maxHeight))
					timeCounter += Time.deltaTime;
				AudioCheck();
				if ( (timeCounter >= timeThreshold) || ((Time.time - timer) >= backupTimeThreshold) ) {
					TransitionManager.Instance.RaisePedastal();
					pedastalCheck = true;
				}					
			}
		}

	
		private void AudioCheck() {
			float heightRatio = Mathf.InverseLerp(minHeight, maxHeight, HostTransform.position.y);

			if (heightRatio >= thresholds[0] && !thresholdCheck.Contains(thresholds[0])) {
				Debug.Log("Now Playing 1");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStep1);
				thresholdCheck.Add(thresholds[0]);
			}
			else if (heightRatio >= thresholds[1] && !thresholdCheck.Contains(thresholds[1])) {
				Debug.Log("Now Playing 2");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStep2);
				thresholdCheck.Add(thresholds[1]);
			}
			else if (heightRatio >= thresholds[2] && !thresholdCheck.Contains(thresholds[2])) {
				Debug.Log("Now Playing 3");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStep3);
				thresholdCheck.Add(thresholds[2]);
			}
			else if (heightRatio >= thresholds[3] && !thresholdCheck.Contains(thresholds[3])) {
				Debug.Log("Now Playing 4");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStep4);
				thresholdCheck.Add(thresholds[3]);
			}
			else if (heightRatio >= thresholds[4] && !thresholdCheck.Contains(thresholds[4])) {
				Debug.Log("Now Playing 5");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStep5);
				thresholdCheck.Add(thresholds[4]);
			} else if (heightRatio >= percentageThreshold && !thresholdCheck.Contains(percentageThreshold)) {
				Debug.Log("Now Playing Final");
				AudioManager.Instance.NowPlay(AudioManager.Audio.StatStepFinal);
				thresholdCheck.Add(percentageThreshold);
			}

		}

		protected override void StartDragging(Vector3 initialDraggingPosition) {
			base.StartDragging(initialDraggingPosition);

			if (!DIAL_UP_FLAG) {
				Prompts.DestroyPrompt(DIAL_UP_DESIGN);
				Quaternion rotation = TransformUtils.GetLookAtRotation(transform);
				DIAL_UP_DESIGN = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + offset_DIAL, transform.position.z), rotation, Prompts.PromptName.SCN1_DIAL_UP_DESIGN, .4f);
				DIAL_UP_FLAG = true;
			}

			ConstraintCheck();
			AudioManager.Instance.NowPlay(AudioManager.Audio.UserControl,true, true);
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}


		protected override void UpdateDragging() {
			base.UpdateDragging();
			ConstraintCheck();
			//Update_DIAL_UP_DESIGN();
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}


		protected override void StopDragging() {
			base.StopDragging();
			AudioManager.Instance.NowStop(AudioManager.Audio.UserControl);
			ConstraintCheck();
			if (graphCompleted)
				StartCoroutine(ChangeScale());
		}

		private void ConstraintCheck() {
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
				c.CurrentSum = (int)Math.Round(c.Sum + (scaleNum*100));
			}


			foreach (GameObject IB in SpawnObjectsController.Instance.InvestBlocks) {
				IB.transform.localScale = new Vector3(IB.transform.localScale.x, scaleFactor * scaleNum, IB.transform.localScale.z);
				yield return wait;
			}

			yield break;
		}

		public void OnGraphCompleted(object source, EventArgs e) {
			Visiblity(true);
			MakeInvestBlocksReady();
			Quaternion rotation = TransformUtils.GetLookAtRotation(transform);
			DIAL_UP_DESIGN = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + .2f, transform.position.z), rotation, Prompts.PromptName.GRAB_ME, .4f);
			graphCompleted = true;
			AudioManager.Instance.NowPlay(AudioManager.Audio.DesignBallSpawn);
		}

		private void Visiblity(bool visible) {
			timer = Time.time;
			GetComponent<SphereCollider>().enabled = visible;
			for(int i = 0; i < transform.childCount; i++)
				transform.GetChild(i).gameObject.SetActive(visible);			
		}

		public void MakeInvestBlocksReady() {

			foreach(GameObject IB in SpawnObjectsController.Instance.InvestBlocks) {
				Renderer rend =IB.GetComponentInChildren<Renderer>();
				rend.enabled=true;
				IB.transform.localScale = new Vector3(IB.transform.localScale.x, 0.01f, IB.transform.localScale.z);
			}
		}

		//private void Create_DIAL_UP_DESIGN(Prompts.PromptName prompt,float offset) {
		//	Quaternion rotation = TransformUtils.GetLookAtRotation(transform);
		//	DIAL_UP_DESIGN = Prompts.GetPrompt(new Vector3(transform.position.x, transform.position.y + offset, transform.position.z),rotation,prompt,.4f);
		//	//Vector3 direction = transform.position - Camera.main.transform.position;
		//	//DIAL_UP_DESIGN.transform.rotation = Quaternion.LookRotation(direction.normalized);
		//}

		private void Update_DIAL_UP_DESIGN() {
			if (DIAL_UP_DESIGN != null) {
				//DIAL_UP_DESIGN.transform.position = new Vector3(transform.position.x,transform.position.y + offset_DIAL, transform.position.z);
				//DIAL_UP_DESIGN.transform.rotation = TransformUtils.GetLookAtRotation(DIAL_UP_DESIGN.transform);
			}
		}


		public void BeginFalling() { 
			SetDragging(false);
			Rigidbody rg = GetComponent<Rigidbody>();
			rg.isKinematic = false;
			rg.useGravity = true;
			rg.GetComponent<SphereCollider>().enabled = false;
			Debug.Log("SliderHandleDrag::BeginFalling | About to destroy self");
			Destroy(this.gameObject);
		}

		public void DestroyPrompt() {
			Debug.Log("SliderHandleDrag::OnDestroy | Destroying");
			Prompts.DestroyPrompt(DIAL_UP_DESIGN);
		}
	
	}
}

