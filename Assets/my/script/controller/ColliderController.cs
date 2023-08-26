using UnityEngine;
using LixaingZhao.MeTACAST;
public class ColliderController : MonoBehaviour {

private Selection s;

void Start()
{
    s=GameObject.Find("script").GetComponentInChildren<Selection>();
}
public int type; //0 undo 1 redo 2 reset
	void OnTriggerEnter(Collider other){
		if(type==0)
        s.Undo();
        if(type==1)
        s.Redo();
        if(type==2)
        s.Reset();
	}

}
