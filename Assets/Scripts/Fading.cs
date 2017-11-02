using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fading : MonoBehaviour {

	public Texture2D fadeOutTexture; 				//Texture which will overlay the screen
	public float fadeSpeed = 0.8f;				 	//Fading speed

	private int drawDepth = -1000;					//texture order in hierarchy
	private float alpha = 1.0f;						//texture alpha value between 1 and 0
	private int fadeDirection = -1;					//direction to fade: in = -1 out = 1

	void OnGUI () {
		alpha += fadeDirection * fadeSpeed * Time.deltaTime; //fade in / out alpha using direction
		alpha = Mathf.Clamp01(alpha); //force the number between 0 and 1 for GUI.Color

		GUI.color = new Color (GUI.color.r, GUI.color.g, GUI.color.b, alpha); //set alpha value
		GUI.depth = drawDepth; //make the black texture render on top
		GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), fadeOutTexture); //Draw to the screen size
	}

	//Sets fadeDirection to the direction parameter
	public float BeginFade (int direction) {
		fadeDirection = direction;
		return(fadeSpeed); //return the fade speed
	}

	void OnLevelWasLoaded () {
		alpha = 1; 		//use if not set to 1 by default;
		BeginFade (-1);		//call fade function
	}

}
