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
	public static SPlayerInput InputPlayer2;
	public static SPlayerInput InputPlayer3;
	public static SPlayerInput InputPlayer4;

	public static Vector2 MousePosition;
	
	static CGame m_Game = GameObject.Find("_Game").GetComponent<CGame>();

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public static void Process(float fDeltatime) 
	{
		if(m_Game.IsPadXBoxMod())
		{	
			float fTolerance = 0.05f;
			
			ProcessPlayer("player1", ref CApoilInput.InputPlayer1, fTolerance);
			ProcessPlayer("player2", ref CApoilInput.InputPlayer2, fTolerance);
			//ProcessPlayer("player3", ref CApoilInput.InputPlayer3, fTolerance);
			//ProcessPlayer("player4", ref CApoilInput.InputPlayer4, fTolerance);
			
			MousePosition = new Vector2(0.0f, 0.0f);
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
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	static void ProcessPlayer(string playerName, ref SPlayerInput InputPlayer,float fTolerance)
	{
		InputPlayer.MoveUp = (Input.GetAxis(playerName+"_MoveVertical")) < -fTolerance;
		InputPlayer.MoveDown = (Input.GetAxis(playerName+"_MoveVertical")) > fTolerance;
		InputPlayer.MoveLeft = (Input.GetAxis(playerName+"_MoveHorizontal")) < -fTolerance;
		InputPlayer.MoveRight = (Input.GetAxis(playerName+"_MoveHorizontal")) > fTolerance;
		
		InputPlayer.DirectionHorizontal = Input.GetAxis(playerName+"_DirectionHorizontal");
		InputPlayer.DirectionVertical = Input.GetAxis(playerName+"_DirectionVertical");
		
		switch(playerName)
		{
			case "player1":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick1Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick1Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick1Button0); //A
				InputPlayer.DropObject = Input.GetKey(KeyCode.Joystick1Button2); //X
				InputPlayer.ActivateMachine = Input.GetKey(KeyCode.Joystick1Button0); //A
				break;
			}
			case "player2":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick2Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick2Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick2Button0); //A
				InputPlayer.DropObject = Input.GetKey(KeyCode.Joystick2Button2); //X
				InputPlayer.ActivateMachine = Input.GetKey(KeyCode.Joystick2Button0); //A
				break;
			}
			case "player3":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick3Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick3Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick3Button0); //A
				InputPlayer.DropObject = Input.GetKey(KeyCode.Joystick3Button2); //X
				InputPlayer.ActivateMachine = Input.GetKey(KeyCode.Joystick3Button0); //A
				break;
			}
			case "player4":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick4Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick4Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick4Button0); //A
				InputPlayer.DropObject = Input.GetKey(KeyCode.Joystick4Button2); //X
				InputPlayer.ActivateMachine = Input.GetKey(KeyCode.Joystick4Button0); //A
				break;
			}
		}
		
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------	
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
