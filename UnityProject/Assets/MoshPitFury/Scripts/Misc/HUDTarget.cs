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
	private static HUDTarget instance;
	private Camera orthoCamera;
	private static Transform Target;

	private Vector3 InitialPosition;
    private Quaternion InitialRotation;
	private Vector3 InitialScale;
	
	public Transform test;
	public float multScale;
	public float duration;
	public float delay;
    public float rotation;
	public iTween.EaseType easeType;

	public static void Targets(Transform target)
	{
		Target = target;
		instance.enabled = true;
	}

	public void Awake()
	{
		Target = test;
		InitialPosition = this.transform.position;
		InitialScale = this.transform.localScale;
        InitialRotation = this.transform.rotation;
		instance = this;
	}

	public void OnEnable()
	{
		this.GetComponent<Renderer>().enabled = true;
		this.transform.position = InitialPosition;
        this.transform.localScale = InitialScale;
        this.transform.rotation = InitialRotation;

		if (orthoCamera == null)
		{
			orthoCamera = GameObject.FindGameObjectWithTag("InterfaceCamera").GetComponent<Camera>();
		}

		if (Target)
		{
			Vector3 pos = WorldToNormalizedViewportPoint(
				Camera.main,
				Target.transform.position
			);

			pos = NormalizedViewportToWorldPoint(orthoCamera, pos);
			pos.z = InitialPosition.z;

			iTween.MoveTo(gameObject, iTween.Hash(
				"position", pos, 
				"easeType", easeType, 
				"time", duration, 
				"looptype", iTween.LoopType.none
			));

			iTween.ScaleTo(gameObject, iTween.Hash(
				"scale", InitialScale * multScale,
				"easeType", easeType,
				"time", duration,
				"looptype", iTween.LoopType.none
			));

            iTween.RotateTo(gameObject, iTween.Hash(
                "rotation", (InitialRotation * Quaternion.Euler(0, 0, rotation)).eulerAngles,
                "easeType", easeType,
                "time", duration,
                "looptype", iTween.LoopType.none
            ));

			Invoke("reset", duration + delay);
		}
	}

	void reset()
	{
		this.GetComponent<Renderer>().enabled = false;
		this.enabled = false;
	}
	
	public static Vector3 WorldToNormalizedViewportPoint(Camera camera, Vector3 point)
	{
		point = camera.WorldToViewportPoint(point);

		if (camera.orthographic)
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
		if (camera.orthographic)
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
