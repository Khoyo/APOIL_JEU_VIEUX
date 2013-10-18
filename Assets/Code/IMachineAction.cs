using UnityEngine;
using System.Collections;

public interface IMachineAction
{
	void Init();
	void Process();
	void Activate(CPlayer player);
}
