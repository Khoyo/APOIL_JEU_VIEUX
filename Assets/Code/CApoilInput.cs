using UnityEngine;
using System.Collections;

public struct SPlayerInput
{
	public bool MoveLeft;
	public bool MoveRight;
	public bool MoveUp;
	public bool MoveDown;
	public bool LookLeft;
	public bool LookRight;
	public bool LookUp;
	public bool LookDown;
	public bool WalkFast;
	public bool WalkSlow;
	public bool PickUpObject;
	public bool DropObject;
	public bool ClickButton;
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
	public static bool Quit;
	
	public static bool MenuValidate;
	public static bool MenuUp;
	public static bool MenuDown;
	public static bool MenuPause;
	public static bool MenuMenu;
	
	//Debug
	public static bool DebugF9;
	public static bool DebugF10;
	public static bool DebugF11;
	public static bool DebugF12;
	
	static CGame m_Game = GameObject.Find("_Game").GetComponent<CGame>();

	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public static void Process(float fDeltatime) 
	{
		float fTolerance = 0.05f;
		if(m_Game.IsPadXBoxMod())
		{		
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
		
		Quit = Input.GetKey(KeyCode.Escape);
		
		MenuValidate = Input.GetKeyDown(KeyCode.JoystickButton0); //A
		
		MenuUp = (Input.GetAxis("player1_MoveVertical")) < -fTolerance;
		MenuDown = (Input.GetAxis("player1_MoveVertical")) > fTolerance;
		
		MenuPause = Input.GetKeyUp(KeyCode.JoystickButton7); //start
		MenuMenu = 	Input.GetKeyDown(KeyCode.JoystickButton6); //back
		
		DebugF9 = Input.GetKeyDown(KeyCode.F9);
		DebugF10 = Input.GetKeyDown(KeyCode.F10);
		DebugF11 = Input.GetKeyDown(KeyCode.F11);
		DebugF12 = Input.GetKeyDown(KeyCode.F12);
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
		
		InputPlayer.LookUp = (Input.GetAxis(playerName+"_DirectionHorizontal")) < -fTolerance;
		InputPlayer.LookDown = (Input.GetAxis(playerName+"_DirectionHorizontal")) > fTolerance;
		InputPlayer.LookLeft = (Input.GetAxis(playerName+"_DirectionVertical")) < -fTolerance;
		InputPlayer.LookRight = (Input.GetAxis(playerName+"_DirectionVertical")) > fTolerance;
		
		InputPlayer.DirectionHorizontal = Input.GetAxis(playerName+"_DirectionHorizontal");
		InputPlayer.DirectionVertical = Input.GetAxis(playerName+"_DirectionVertical");
		
		switch(playerName)
		{
			case "player1":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick1Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick1Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick1Button0); //A
				InputPlayer.DropObject = Input.GetKeyDown(KeyCode.Joystick1Button2); //X
				InputPlayer.ActivateMachine = Input.GetKeyDown(KeyCode.Joystick1Button0); //A
				InputPlayer.ClickButton = Input.GetKeyDown(KeyCode.Joystick1Button0); //A
				break;
			}
			case "player2":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick2Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick2Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick2Button0); //A
				InputPlayer.DropObject = Input.GetKeyDown(KeyCode.Joystick2Button2); //X
				InputPlayer.ActivateMachine = Input.GetKeyDown(KeyCode.Joystick2Button0); //A
				InputPlayer.ClickButton = Input.GetKeyDown(KeyCode.Joystick2Button0); //A
				break;
			}
			case "player3":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick3Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick3Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick3Button0); //A
				InputPlayer.DropObject = Input.GetKeyDown(KeyCode.Joystick3Button2); //X
				InputPlayer.ActivateMachine = Input.GetKeyDown(KeyCode.Joystick3Button0); //A
				InputPlayer.ClickButton = Input.GetKeyDown(KeyCode.Joystick3Button0); //A
				break;
			}
			case "player4":
			{
				InputPlayer.WalkFast = Input.GetKey(KeyCode.Joystick4Button5); //RB
				InputPlayer.WalkSlow = Input.GetKey(KeyCode.Joystick4Button4); //LB			
				InputPlayer.PickUpObject = Input.GetKey(KeyCode.Joystick4Button0); //A
				InputPlayer.DropObject = Input.GetKeyDown(KeyCode.Joystick4Button2); //X
				InputPlayer.ActivateMachine = Input.GetKeyDown(KeyCode.Joystick4Button0); //A
				InputPlayer.ClickButton = Input.GetKeyDown(KeyCode.Joystick4Button0); //A
				break;
			}
		}
		
	}
	
	public static SPlayerInput GetInput(int player_number){
		switch(player_number){
			case 0:
				return InputPlayer1;
			case 1:
				return InputPlayer2;
			case 2: 
				return InputPlayer3;
			case 3: 
				return InputPlayer4;
		}
		Debug.LogError("Player "+player_number+" does not exist");
		return new SPlayerInput();
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
