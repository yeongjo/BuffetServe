#define __UseCharacterController
#define __UseCapsuleCast

using UnityEngine;
using System.Collections;

public class Player : Staff
{
	public float moveSpeed = 1;
	public float forwardCheckDistance = 1f;

	protected Rigidbody rigid;
	protected CharacterController cc;

	void Awake(){
		GM.localPlayer = this;
	}

	protected new void Start(){
		// Only use trans
		base.Start();
		rigid = GetComponent<Rigidbody> ();
		cc = GetComponent<CharacterController> ();

		//Test ();
	}

	void Test(){
		// y = (x/8)*(3-1) + 0.5f
		for (int i = 0; i < 8; i++) {
			print (4f * (i / 8.0f));
		}
	}

	protected void Update(){
		var camTrans = Camera.main.transform;
		var forwardVec = camTrans.forward;
		forwardVec.y = 0;
		forwardVec = forwardVec.normalized;
		var controlVec = Input.GetAxis ("Horizontal")*camTrans.right+Input.GetAxis ("Vertical") * forwardVec;
		if(GetWASDIsDown() && controlVec.magnitude > 0.01f)
			trans.rotation = Quaternion.LookRotation (controlVec.normalized);

		#if __UseCharacterController
		cc.Move(controlVec * moveSpeed * Time.deltaTime);
		#else


		rigid.velocity = (controlVec * moveSpeed);
		#endif

		// Left click
		if (Input.GetMouseButtonDown (0)) {
			if (CheckForward ()) {
				float minDis = 10;
				IUse shortestUse = null;
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider == null)
						continue;
					var t = hits [i].collider.GetComponent<IUse> ();
					if (hits [i].distance < minDis && t != null) {
						minDis = hits [i].distance;
						shortestUse = t;
					}
				}
				if (shortestUse != null) {
					shortestUse.Use (this);
				}
			}

		}

		// Right click
		if (Input.GetMouseButtonDown (1)) {
			if (CheckForward ()) {
				float minDis = 10;
				IRightUse shortestUse = null;
				for (int i = 0; i < hits.Length; i++) {
					if (hits [i].collider == null)
						continue;
					var t = hits [i].collider.GetComponent<IRightUse> ();
					if (hits [i].distance < minDis && t != null) {
						minDis = hits [i].distance;
						shortestUse = t;
					}
				}
				if (shortestUse != null) {
					shortestUse.RightUse (this);
				}
			}

		}
	}

	protected new void FixedUpdate(){
		RaycastHit hit;
		if (Physics.Raycast (trans.position + Vector3.up * .5f, Vector3.down, out hit, 10, -1, QueryTriggerInteraction.Ignore)) {
			cc.Move (new Vector3 (0, hit.point.y - trans.position.y, 0));
		}
	}


	#if __UseCapsuleCast
	float upDownAmount = .5f;
	float radius = .4f;

	// Should call Use form hits object
	// that distance shortest obj
	RaycastHit[] hits = new RaycastHit[5];
	bool CheckForward(){
		var playerMidPos = transform.position + Vector3.up * upOffset;
		var index = Physics.CapsuleCastNonAlloc (playerMidPos+trans.forward + Vector3.up * upDownAmount, playerMidPos+trans.forward + Vector3.down * upDownAmount, radius, Vector3.up, hits);

		return index > 0 ? true : false;
		#else
	bool CheckForward(out RaycastHit hit){
		return Physics.Raycast (trans.position+Vector3.up*.5f, trans.forward, out hit, forwardCheckDistance, -1-(1 << 9), QueryTriggerInteraction.Collide);
		#endif
	}
	float upOffset = 0.82f;
	void OnDrawGizmos(){
		Gizmos.color = new Color (0, 0, 0, .4f);
		Gizmos.DrawSphere (transform.position + Vector3.up*upOffset+transform.forward + Vector3.up * upDownAmount, radius);
		Gizmos.DrawSphere (transform.position + Vector3.up*upOffset+transform.forward + Vector3.down * upDownAmount, radius);
	}

	bool GetWASDIsDown(){
		return Input.GetKey (KeyCode.W) || Input.GetKey (KeyCode.A) || Input.GetKey (KeyCode.S) || Input.GetKey (KeyCode.D);
	}
}

