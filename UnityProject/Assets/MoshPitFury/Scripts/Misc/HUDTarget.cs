using UnityEngine;
using System.Collections.Generic;

/**
  * @class FollowAtOrtho
  * @brief Description.
  * @author Sylvain Lafon
  * @see MonoBehaviour
  */
public class HUDTarget : MonoBehaviour
{

	private Camera orthoCamera;
	public Transform target;

	private Vector3 InitialPosition;
	private Vector3 InitialScale;

	public void Awake()
	{
		InitialPosition = this.transform.position;
		InitialScale = this.transform.localScale;
	}

	public void OnEnable()
	{
		this.transform.position = InitialPosition;
		this.transform.localScale = InitialScale;

		if (orthoCamera == null)
		{
			orthoCamera = GameObject.FindGameObjectWithTag("InterfaceCamera").camera;
		}

		if (target)
		{
			Vector3 pos = WorldToNormalizedViewportPoint(
				Camera.main,
				target.transform.position
			);

			pos = NormalizedViewportToWorldPoint(orthoCamera, pos);
			pos.z = InitialPosition.z;

			iTween.MoveTo(gameObject, iTween.Hash(
				"position", pos, 
				"easeType", iTween.EaseType.easeInSine, 
				"time", 0.5f, 
				"looptype", iTween.LoopType.none
			));

			iTween.ScaleTo(gameObject, iTween.Hash(
				"scale", InitialScale * 0.3f,
				"easeType", iTween.EaseType.easeInSine,
				"time", 0.5f,
				"looptype", iTween.LoopType.none
			));			
		}
	}
	
	public static Vector3 WorldToNormalizedViewportPoint(Camera camera, Vector3 point)
	{
		point = camera.WorldToViewportPoint(point);

		if (camera.isOrthoGraphic)
		{
			point.z = (2 * (point.z - camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) - 1f;
		}
		else
		{
			point.z = ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane))
			+ (1 / point.z) * (-2 * camera.farClipPlane * camera.nearClipPlane / (camera.farClipPlane - camera.nearClipPlane));
		}

		return point;
	}

	public static Vector3 NormalizedViewportToWorldPoint(Camera camera, Vector3 point)
	{
		if (camera.isOrthoGraphic)
		{
			point.z = (point.z + 1f) * (camera.farClipPlane - camera.nearClipPlane) * 0.5f + camera.nearClipPlane;
		}
		else
		{
			point.z = ((-2 * camera.farClipPlane * camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)) /
			(point.z - ((camera.farClipPlane + camera.nearClipPlane) / (camera.farClipPlane - camera.nearClipPlane)));
		}

		return camera.ViewportToWorldPoint(point);
	}
}
