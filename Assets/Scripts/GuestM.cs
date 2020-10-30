using UnityEngine;
using System.Collections;

public class GuestM : MyBehaviour
{
	public GameObject[] guestGroupTemplate;
	public Transform guestSpawnPoint;

	public int createCount = 3;
	public float createDelay = 2f;

	void Start ()
	{
		guestGroupTemplate = Resources.LoadAll<GameObject> ("GuestGroup");
		StartCoroutine (Delay ());
	}

	IEnumerator Delay(){
		for (int i = 0; i < createCount; i++) {
			CreateGuest (i%2);
			yield return new WaitForSeconds (createDelay);
		}
	}

	void CreateGuest(int i){
		RaycastHit hit;
		Physics.Raycast (guestSpawnPoint.position+Vector3.up * 3, Vector3.down, out hit);
		//Physics.SphereCast (,);
		Vector3 randomCircle = Random.insideUnitSphere;
		randomCircle.y = 0;
		Instantiate(guestGroupTemplate[i], (guestSpawnPoint.position + randomCircle), Quaternion.identity);
	}
}

