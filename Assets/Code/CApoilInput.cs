using UnityEngine;
using System.Collections;

public struct SPlayerInput
{
	public bool MoveLeft;
	public bool MoveRight;
	public bool MoveUp;
	public bool MoveDown;
	public bool WalkFast;
	public bool WalkSlow;
	public bool PickUpObject;
	public bool DropObject;
	public bool ActivateMachine;
	public float DirectionHorizontal;
	public float DirectionVertical;
}

public class CApoilInput
{
	public static SPlayerInput InputPlayer1;

	public static Vector2 MousePosition;
	
	static CGame m_Game = GameObject.Find("_Game").GetComponent<CGame>();
		
	public static void Process(float fDeltatime) 
	{
		if(m_Game.IsPadXBoxMod())
		{	
			float fTolerance = 0.05f;
			InputPlayer1.MoveUp = (Input.GetAxis("moveVertical")) < -fTolerance;
			InputPlayer1.MoveDown = (Input.GetAxis("moveVertical")) > fTolerance;
			InputPlayer1.MoveLeft = (Input.GetAxis("moveHorizontal")) < -fTolerance;
			InputPlayer1.MoveRight = (Input.GetAxis("moveHorizontal")) > fTolerance;
			InputPlayer1.WalkFast = Input.GetKey(KeyCode.JoystickButton5); //RB
			InputPlayer1.WalkSlow = Input.GetKey(KeyCode.JoystickButton4); //LB
			
			InputPlayer1.PickUpObject = Input.GetKey(KeyCode.JoystickButton0); //A
			InputPlayer1.DropObject = Input.GetKey(KeyCode.JoystickButton2); //X
			InputPlayer1.ActivateMachine = Input.GetKey(KeyCode.JoystickButton0); //A
				
			MousePosition = new Vector2(0.0f, 0.0f);
			
			InputPlayer1.DirectionHorizontal = Input.GetAxis("lightHorizontal");
			InputPlayer1.DirectionVertical = Input.GetAxis("lightVertical");
		}
		else
		{
			InputPlayer1.MoveUp = Input.GetKey(KeyCode.Z);
			InputPlayer1.MoveDown = Input.GetKey(KeyCode.S);
			InputPlayer1.MoveLeft = Input.GetKey(KeyCode.Q);
			InputPlayer1.MoveRight = Input.GetKey(KeyCode.D);
			InputPlayer1.WalkFast = Input.GetKey(KeyCode.LeftShift);
			InputPlayer1.WalkSlow = Input.GetKey(KeyCode.LeftControl);
			
			InputPlayer1.PickUpObject = Input.GetMouseButton(0);
			InputPlayer1.DropObject = Input.GetMouseButton(2);
			InputPlayer1.ActivateMachine = Input.GetMouseButton(0);
			
			MousePosition = CalculateMousePosition();
			
			InputPlayer1.DirectionHorizontal = 0.0f;
			InputPlayer1.DirectionVertical = 0.0f;
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
