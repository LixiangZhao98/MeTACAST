using UnityEngine;
using UnityEngine.UI;

public class WidgetInteraction : MonoBehaviour {

public Selection s;   


public int type; //0 undo 1 redo 2 reset
	void OnTriggerEnter(Collider other){
		if(other.tag!="marker")
			return;	
		if(type==0)
        s.Undo();
        if(type==1)
        s.Redo();
        if(type==2)
        s.Reset();
        this.transform.parent.GetComponent<Image>().color = Color.white;
        this.transform.parent.GetComponentInChildren<Text>().color = Color.white;
	}
	void OnTriggerExit(Collider other){
		if(other.tag!="marker")
			return;	
		this.transform.parent.GetComponent<Image>().color = Color.gray;
		this.transform.parent.GetComponentInChildren<Text>().color = Color.gray;
	}
}
