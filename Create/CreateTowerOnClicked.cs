using UnityEngine;
using System.Collections;

public class CreateTowerOnClicked : MonoBehaviour
{
	public GameObject tower;
	
	void Clicked(Vector3 position)
	{
        Vector3 vec = new Vector3(Mathf.Round(position.x), Mathf.Round(position.y), Mathf.Round(position.z));
		Instantiate(tower,vec + Vector3.up*0.5f,tower.transform.rotation);
        Debug.Log(position);
	}
}
