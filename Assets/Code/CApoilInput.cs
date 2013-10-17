using UnityEngine;
using System.Collections;

public struct SPadInput
{
	bool MoveLeft;
	bool MoveRight;
	bool MoveUp;
	bool MoveDown;
	bool WalkFast;
	bool WalkSlow;
	bool PickUpObject;
	bool DropObject;
	bool ActivateMachine;
	float DirectionHorizontal;
	float DirectionVertical;
}

public class CApoilInput
{
	public static SPadInput PadInputPlayer1;

	public static Vector2 MousePosition;
	
	static CGame m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
	public static void Process(float fDeltatime) 
	{
		if(m_Game.IsPadXBoxMod())
		{	
			float fTolerance = 0.05f;
			MoveUp = (Input.GetAxis("moveVertical")) < -fTolerance;
			MoveDown = (Input.GetAxis("moveVertical")) > fTolerance;
			MoveLeft = (Input.GetAxis("moveHorizontal")) < -fTolerance;
			MoveRight = (Input.GetAxis("moveHorizontal")) > fTolerance;
			WalkFast = Input.GetKey(KeyCode.JoystickButton5); //RB
			WalkSlow = Input.GetKey(KeyCode.JoystickButton4); //LB
			
			PickUpObject = Input.GetKey(KeyCode.JoystickButton0); //A
			DropObject = Input.GetKey(KeyCode.JoystickButton2); //X
			ActivateMachine = Input.GetKey(KeyCode.JoystickButton0); //A
				
			MousePosition = new Vector2(0.0f, 0.0f);
			
			PadLightHorizontal = Input.GetAxis("lightHorizontal");
			PadLightVertical = Input.GetAxis("lightVertical");
		}
		else
		{
			MoveUp = Input.GetKey(KeyCode.Z);
			MoveDown = Input.GetKey(KeyCode.S);
			MoveLeft = Input.GetKey(KeyCode.Q);
			MoveRight = Input.GetKey(KeyCode.D);
			WalkFast = Input.GetKey(KeyCode.LeftShift);
			WalkSlow = Input.GetKey(KeyCode.LeftControl);
			
			PickUpObject = Input.GetMouseButton(0);
			DropObject = Input.GetMouseButton(2);
			ActivateMachine = Input.GetMouseButton(0);
			
			MousePosition = CalculateMousePosition();
			
			PadLightHorizontal = 0.0f;
			PadLightVertical = 0.0f;
		}
	}
	
	public static Vector2 CalculateMousePosition()
	{
		Vector3 posMouseTmp = Vector3.zero;
		RaycastHit vHit = new RaycastHit();
		Ray vRay = m_Game.m_CameraCone.ScreenPointToRay(Input.mousePosition);
		if(Physics.Raycast(vRay, out vHit, 100)) 
		{
			posMouseTmp = vHit.point;
		}
		return new Vector2(posMouseTmp.x, posMouseTmp.y);
	}
}
