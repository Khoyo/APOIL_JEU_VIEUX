using UnityEngine;
using System.Collections;

public interface IMachineAction
{
	void Activate(CPlayer player);
	void ActivateContinuous(CPlayer player);
}
