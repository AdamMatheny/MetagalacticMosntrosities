using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Collections.Generic;

public class ParticleScaler : EditorWindow
{
	//Put the window in the tools menu
	[MenuItem("Tools/Particle Scaler")]
	
	//Gets called to open the window
	public static void ShowWindow()
	{
		EditorWindow.GetWindow(typeof(ParticleScaler));
	}
	//GameObject for our currently selected editor object
	public GameObject sel;
	//Does the object have a particle system?
	private bool hasParticleSystem;
	//Live scale
	private float scale;
	//This holds the current object, makes sure everything is reset if we change
	private GameObject ready;
	
	//Variables for the particle system
	private List<float> startSpeed = new List<float>();
	private List<float> startLife = new List<float>();
	private List<float> startSize = new List<float>();
	private List<float> startRate = new List<float>();
	private List<float> startGrav = new List<float>();
	private List<float> startMax = new List<float>();
	
	//Object's scale
	private Vector3 startScale;
	
	
	//Bools to check if things are getting scaled
	private bool scaleSpeed = true;
	private bool scaleLife = true;
	private bool scaleSize = true;
	private bool scaleRate = true;
	private bool scaleGrav = true;
	private bool scaleMax = true;
	private bool scaleEmission = true;
	
	void OnGUI()
	{
		Vector3 test = new Vector3(3,2,1);
		test *= 2;
		//Show the game object to the user
		GUILayout.BeginHorizontal();
		sel = (GameObject)EditorGUILayout.ObjectField(Selection.activeGameObject,typeof(GameObject),true);
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		//Temp string to show text
		string temp = "";
		
		//If we have no object selected
		if(sel == null)
		{
			//empty the last object
			ready = null;
			//tell the user they have nothing selected
			temp = "NO OBJECT SELECTED";
		}
		else //We have an object selected
		{
			//See if the object has a particle system
			if(sel.GetComponent<ParticleSystem>())
			{
				//Particle system found
				hasParticleSystem = true;
				//Show the use the object is valid
				temp = "VALID GAME OBJECT";
				//If this is a new object, empty the last one
				if(sel != ready)
					ready = null;
			}
			else //Object has no particle system
			{
				hasParticleSystem = false;
				//Inform the user
				temp = "GAME OBJECT DOES NOT HAVE PARTICLE SYSTEM";
				//Empty previous
				ready = null;
			}
		}
		
		GUILayout.Label(temp,GUILayout.Width(300));
		GUILayout.EndHorizontal();
		GUILayout.BeginHorizontal();
		//No previous object, and current has particles
		if(ready == null && hasParticleSystem)
		{
			//Show a button to allow scaling
			if(GUILayout.Button("ALLOW SCALING"))
			{
				//Reset arrays
				startSpeed = new List<float>();
				startLife = new List<float>();
				startSize = new List<float>();
				startRate = new List<float>();
				startGrav = new List<float>();
				startMax = new List<float>();
				//If the button is pressed we'll get reference to the objects particle system
				ParticleSystem p = sel.GetComponent<ParticleSystem>();
				//Set ready to our current object (stops this being called again)
				ready = sel;
				//Default scale to 1
				scale = 1f;
				//Get values from the particle system
				startSpeed.Add(p.startSpeed);
				startLife.Add(p.startLifetime);
				startSize.Add(p.startSize);
				startRate.Add(p.emissionRate);
				startGrav.Add(p.gravityModifier);
				startMax.Add(p.maxParticles);
				
				startScale = sel.transform.localScale;
				//Get default values for children	
				if(sel.transform.childCount > 0)
				{
					for(int x = 0; x < sel.transform.childCount; ++x)
					{
						if(sel.transform.GetChild(x).GetComponent<ParticleSystem>())
						{
							ParticleSystem cp = sel.transform.GetChild(x).GetComponent<ParticleSystem>();
							startSpeed.Add(cp.startSpeed);
							startLife.Add(cp.startLifetime);
							startSize.Add(cp.startSize);
							startRate.Add(cp.emissionRate);
							startGrav.Add(cp.gravityModifier);
							startMax.Add(cp.maxParticles);
						}
					}
				}
			}
		}
		//We have particle system, and have filled our temp object
		else if(ready != null && hasParticleSystem)
		{
			//Show a bunch of tick boxes to give user some control
			scaleSpeed = EditorGUILayout.Toggle("Scale Speed?",scaleSpeed);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleLife = EditorGUILayout.Toggle("Scale Lifetime?",scaleLife);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleSize = EditorGUILayout.Toggle("Scale Size?",scaleSize);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleRate = EditorGUILayout.Toggle("Scale Emission Rate?",scaleRate);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleGrav = EditorGUILayout.Toggle("Scale Gravity?",scaleGrav);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleMax = EditorGUILayout.Toggle("Scale Max Particles?",scaleMax);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			scaleEmission = EditorGUILayout.Toggle("Scale Emission Shape?",scaleEmission);
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			//Get reference to the particle system
			ParticleSystem p = sel.GetComponent<ParticleSystem>();
			//Draw a slider bar for the scale (0 - 10)
			scale = EditorGUILayout.Slider("Scale",scale,0f,10f);
			//Update particle system
			UpdateScale(p,0);
			if(scaleEmission)
				sel.transform.localScale = startScale * scale;
			//See how many children we have
			int chCount = sel.transform.childCount;
			
			//If we have children
			if(chCount > 0)
			{
				int count = 1;
				//Count through them
				for(int x = 0; x < chCount; ++x)
				{
					//If they have particle systems
					if(sel.transform.GetChild(x).GetComponent<ParticleSystem>())
					{
						//Scale those systems, too!
						UpdateScale(sel.transform.GetChild(x).GetComponent<ParticleSystem>(), count);
						++count;
					}
				}
			}
		}
	}
	
	//Update particle system
	void UpdateScale(ParticleSystem p, int index)
	{
		if(scaleSpeed)
			p.startSpeed = startSpeed[index] * scale;
		if(scaleLife)
			p.startLifetime = startLife[index] * scale;
		if(scaleSize)
			p.startSize = startSize[index] * scale;
		if(scaleRate)
			p.emissionRate = startRate[index] * scale;
		if(scaleGrav)
			p.gravityModifier = startGrav[index] * scale;
		if(scaleMax)
			p.maxParticles = (int)(startMax[index] * scale);
	}
}
