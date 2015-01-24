using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Permet de suivre un objet avec un certain offset.
/// </summary>
public class FollowOffset : MonoBehaviour
{
	/// <summary>
	/// Objet à suivre
	/// </summary>
	public Transform target;

	/// <summary>
	/// Offset à garder
	/// </summary>
	public Vector3 offset;

	/// <summary>
	/// Vitesse du smooth pour le déplacement
	/// </summary>
	public float smoothSpeed;

	/// <summary>
	/// Vitesse du smooth de la rotation
	/// </summary>
	public float rotSmoothSpeed;

	/// <summary>
	/// Si vrai : on regardera la cible
	/// </summary>
	public bool lookAt;

	/// <summary>
	/// Si lookAt est faux : la rotation désirée
	/// </summary>
	public Vector3 wantedRotation;

	public bool removeSmooth;

	public bool removeSmoothRotation;

	public bool useTargetPosition;

	public Vector3 targetPosition;

	/// <summary>
	/// Chaque frame
	/// </summary>
	private void LateUpdate()
	{
		// Si on a une cible
		if (target != null || useTargetPosition)
		{
			this.UpdatePosition(); // On update la position
			this.UpdateRotation(); // Puis la rotation
		}
	}

	/// <summary>
	/// Suit la cible avec un offset
	/// </summary>
	private void UpdatePosition()
	{
		Vector3 targetPos;
		if (target != null)
			targetPos = target.transform.position;
		else
			targetPos = targetPosition;

		if (removeSmooth)
		{
			this.transform.position = targetPos + offset;
		}
		else
		{
			this.transform.position = Vector3.Lerp(
				this.transform.position,
				targetPos + offset,
				Time.deltaTime * smoothSpeed
			);
		}
	}

	/// <summary>
	/// Regarde la cible ou s'approche de la rotation voulue
	/// </summary>
	private void UpdateRotation()
	{
		Quaternion prev = this.transform.rotation;

		if (lookAt)
		{
			if (target)
				this.transform.LookAt(target);
			else
				this.transform.LookAt(targetPosition, Vector3.up);
		}
		else
		{
			this.transform.localEulerAngles = wantedRotation;
		}

		if(!removeSmoothRotation)
			this.transform.rotation = Quaternion.Slerp(prev, this.transform.rotation, Time.deltaTime * rotSmoothSpeed);
	}
}