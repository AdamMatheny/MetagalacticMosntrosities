using UnityEngine;
using System.Collections;

public class AspectRatioPositionScaleAdjuster : MonoBehaviour 
{
	//Adjust the position and scale of a Canvas UI element based on what aspect ratio is employed -Adam
	[SerializeField] private Vector3 mSixteenByNinePos;
	[SerializeField] private Vector3 mSixteenByNineScale;

	[SerializeField] private Vector3 mSixteenByTenPos;
	[SerializeField] private Vector3 mSixteenByTenScale;

	[SerializeField] private Vector3 mFourByThreePos;
	[SerializeField] private Vector3 mFourByThreeScale;

	[SerializeField] private Vector3 mFiveByFourPos;
	[SerializeField] private Vector3 mFiveByFourScale;

	//Aspect ratios for mobile portait view
	[SerializeField] private Vector3 mNineBySixteenPos;
	[SerializeField] private Vector3 mNineBySixteenScale;

	// Use this for initialization
	void Start () 
	{

		if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(16f/9f,2))
		{
			GetComponent<RectTransform>().localPosition = mSixteenByNinePos;
			GetComponent<RectTransform>().localScale = mSixteenByNineScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(16f/10f,2))
		{
			GetComponent<RectTransform>().localPosition = mSixteenByTenPos;
			GetComponent<RectTransform>().localScale = mSixteenByTenScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) ==System.Math.Round(4f/3f,2))
		{
			GetComponent<RectTransform>().localPosition = mFourByThreePos;
			GetComponent<RectTransform>().localScale = mFourByThreeScale;
		}
		else if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(5f/4f,2))
		{
			GetComponent<RectTransform>().localPosition = mFiveByFourPos;
			GetComponent<RectTransform>().localScale = mFiveByFourScale;
		}

		else if(System.Math.Round(Camera.main.aspect,2) == System.Math.Round(9f/16f,2))
		{
			GetComponent<RectTransform>().localPosition = mNineBySixteenPos;
			GetComponent<RectTransform>().localScale = mNineBySixteenScale;
		}
		else
		{
			GetComponent<RectTransform>().localPosition = mFourByThreePos;
			GetComponent<RectTransform>().localScale = mFourByThreeScale;
		}

	}

	void Update()
	{

	}

}
