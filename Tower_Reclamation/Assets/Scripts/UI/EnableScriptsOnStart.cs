using UnityEngine;
using UnityEngine.UI;

public class EnableScriptsOnStart : MonoBehaviour {


    [SerializeField] Canvas canvasToEnable;

	void Awake () {
        canvasToEnable.gameObject.SetActive(true);
	}
	
}
