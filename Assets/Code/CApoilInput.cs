using UnityEngine;
using System.Collections;

public struct SPlayerInput
{
	public bool MoveLeft;
	public bool MoveRight;
	public bool MoveUp;
	public bool MoveDown;
}

public class CApoilInput
{
	public static SPlayerInput InputPlayer;
	
	public static bool Quit;

	//Debug
	public static bool DebugF9;
	public static bool DebugF10;
	public static bool DebugF11;
	public static bool DebugF12;
	
	
	public static void Init()
	{
		/*if(!Application.isEditor)
			Screen.lockCursor = true;*/
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public static void Process(float fDeltatime) 
	{	
		InputPlayer.MoveUp = Input.GetKey(KeyCode.Z) | Input.GetKey(KeyCode.W);
		InputPlayer.MoveDown = Input.GetKey (KeyCode.S);
		InputPlayer.MoveLeft = Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A);
		InputPlayer.MoveRight = Input.GetKey(KeyCode.D);		
		
		Quit = Input.GetKeyDown(KeyCode.Escape);

		DebugF9 = Input.GetKeyDown(KeyCode.F9);
		DebugF10 = Input.GetKeyDown(KeyCode.F10);
		DebugF11 = Input.GetKeyDown(KeyCode.F11);
		DebugF12 = Input.GetKeyDown(KeyCode.F12);
	}	

}
