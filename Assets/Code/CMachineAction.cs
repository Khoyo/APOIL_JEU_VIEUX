using UnityEngine;
using System.Collections;

public interface IMachineAction
{
	void Init();
	void Update();
	void Activate(CPlayer player);
}
