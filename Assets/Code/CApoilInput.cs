using UnityEngine;
using System.Collections;

public struct SPlayerInput
{
	public float MoveHorizontal;
	public float MoveVertical;
	public float DirectionHorizontal;
	public float DirectionVertical;
	public bool Run;

	public bool ActivateMachine;
	public bool ActivateMachineContinuous;
}

public class CApoilInput
{
	public static SPlayerInput[] InputPlayer;
	
	public static bool Quit;

	//Debug
	public static bool DebugF9;
	public static bool DebugF10;
	public static bool DebugF11;
	public static bool DebugF12;
	
	//-------------------------------------------------------------------------------
	/// 
	//-------------------------------------------------------------------------------
	public static void Init()
	{
		InputPlayer = new SPlayerInput[4];
		/*if(!Application.isEditor)
			Screen.lockCursor = true;*/
	}
	
	//-------------------------------------------------------------------------------
	///
	//-------------------------------------------------------------------------------
	public static void Process(float fDeltatime) 
	{	
		ProcessPlayer (0, "Joystick1");
		ProcessPlayer (1, "Joystick2");
		ProcessPlayer (2, "Joystick3");
		ProcessPlayer (3, "Joystick4");

		for(int i = 0; i < 4; i++) //T'a vu Tim, c'est une boucle... C'est pratique avec les tableaux et tout
		{
			InputPlayer[i].Run = Input.GetKey(KeyCode.Joystick1Button5);

			InputPlayer[i].ActivateMachine = Input.GetKeyDown(KeyCode.Joystick1Button0+20*i); //Twenty button by joysticks, UGLY hack
			InputPlayer[i].ActivateMachineContinuous = Input.GetKey(KeyCode.Joystick1Button0+20*i); //A
		}

		Quit = Input.GetKeyDown(KeyCode.Escape);

		DebugF9 = Input.GetKeyDown(KeyCode.F9);
		DebugF10 = Input.GetKeyDown(KeyCode.F10);
		DebugF11 = Input.GetKeyDown(KeyCode.F11);
		DebugF12 = Input.GetKeyDown(KeyCode.F12);
	}	
	 
	public static void ProcessPlayer(int nId, string name)
	{
		InputPlayer [nId].MoveHorizontal = Input.GetAxis (name+"_LeftXAxis");
		InputPlayer [nId].MoveVertical = Input.GetAxis (name+"_LeftYAxis");
		InputPlayer [nId].DirectionHorizontal = Input.GetAxis (name+"_RightXAxis");
		InputPlayer [nId].DirectionVertical = Input.GetAxis (name+"_RightYAxis");

	}

	public static SPlayerInput GetInput(CPlayer.EIdPlayer id)
	{
		return InputPlayer[(int)id];
	}


}
