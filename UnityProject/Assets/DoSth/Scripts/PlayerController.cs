﻿using UnityEngine;
using System.Collections;
using XInputDotNetPure;

public class PlayerController : MonoBehaviour 
{
	private Player player;
		
	void Awake()
	{
		player = this.GetComponent<Player>();
	}

	private GamePadState PadState;

	public float	MaxSpeed;
	public float	MaxSpeedDash;

	public float	Acceleration;
	public float	AccelerationDash;

	public float	DashDuration;
	public float	DashCooldown;
	private float	DashTimer;

	private bool	isDashing = false;
	
	private Vector3 Speed;
	public float BumpPower = 2;

	private void UpdateState()
	{
		this.PadState = GamePad.GetState((PlayerIndex)(this.player.Id - 1));
		Debug.Log(
			((PlayerIndex)(this.player.Id - 1)).ToString() + " is " + 
			(this.PadState.IsConnected ? "connected" : "disconnnected"));
	}

	private bool AButton
	{
		get
		{
			if (this.PadState.IsConnected)
				return this.PadState.Buttons.A == ButtonState.Pressed;

			return false;
		}
	}

	private Vector3 PadDirection
	{
		get
		{
			const float sqrDeadZone = 0.09f;

			Vector3 padController = Vector3.zero;

			if (this.PadState.IsConnected)
			{
				padController.x = this.PadState.ThumbSticks.Left.X;
				padController.z = this.PadState.ThumbSticks.Left.Y;
			}

		
			if (Mathf.Abs(padController.x) < 0.1f)
				padController.x = 0;

			if (Mathf.Abs(padController.z) < 0.1f)
				padController.z = 0;

			if (padController.sqrMagnitude < sqrDeadZone)
				padController = Vector2.zero;
			
			return padController;
		}
	}

	void UpdateDash()
	{
		if(DashTimer > 0)
			DashTimer -= Time.deltaTime;
		
		// On dashe.
		if(this.isDashing)
		{
			// Fin du dash
			if(this.DashTimer <= 0)
			{
				this.isDashing = false;
				this.DashTimer = this.DashCooldown;

				if (this.Speed.sqrMagnitude > MaxSpeed * MaxSpeed)
				{
					this.Speed = this.Speed.normalized * MaxSpeed;
				}
			}
		}
		else
		{
			// Fin du cooldown
			if (DashTimer <= 0)
			{
				this.isDashing = this.AButton;

				// Nouveau dash.
				if(this.isDashing)
					this.DashTimer = this.DashDuration;
			}
		}
	}

	Vector3 CameraRelatedDirection(Vector3 padDir)
	{
		Transform t = Camera.main.transform;

		Vector3 dir = Quaternion.Euler(0, t.eulerAngles.y, 0) * padDir;
		
		return dir.normalized;
	}

	void OnPlayerPlaced()
	{
		this.Speed = Vector3.zero;
		if (!this.rigidbody.isKinematic)
		{
			this.rigidbody.velocity = Vector3.zero;
			this.rigidbody.angularVelocity = Vector3.zero;
		}
	}

	void UpdateSpeed()
	{
		Vector3 perfectSpeed;
		if (player.CanMove)
		{
			if (isDashing)
			{
				perfectSpeed = this.Speed.normalized * MaxSpeedDash;
			}
			else
			{
				perfectSpeed = CameraRelatedDirection(this.PadDirection) * MaxSpeed;
			}
		}
		else
		{
			perfectSpeed = Vector3.zero;
		}
		
		this.Speed = Vector3.Lerp(this.Speed, perfectSpeed, Time.deltaTime * (this.isDashing ? AccelerationDash : Acceleration));
		this.Speed.y = this.rigidbody.velocity.y;

		if (this.Speed.sqrMagnitude < 0.01f)
		{
			this.Speed = Vector3.zero;
		}
		else
		{
			this.player.OnMove();
			this.transform.forward = this.Speed.normalized;
		}

		this.rigidbody.velocity = this.Speed;
		this.rigidbody.angularVelocity = Vector3.zero;		
	}
	
	void Update ()
	{
		UpdateState();
		UpdateDash();
		UpdateSpeed();
	}

	void OnBumped(BumperTrigger bumper)
	{
		this.Speed += bumper.transform.forward * bumper.Power;
	}
		
	public void OnCollisionEnter(Collision collision)
	{
		PlayerController other = collision.gameObject.GetComponent<PlayerController>();
		if(other != null && this.player.Id < other.player.Id)
		{
			Vector3 prevSpeed = this.Speed;

			if (other.isDashing)
			{
				this.Speed += collision.relativeVelocity.normalized * (BumpPower * other.Speed.magnitude);
				this.gameObject.SendMessage("OnPushed", other, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("OnPush", this, SendMessageOptions.DontRequireReceiver);
			}

			if (this.isDashing)
			{
				other.Speed -= collision.relativeVelocity.normalized * (BumpPower * prevSpeed.magnitude);
				this.gameObject.SendMessage("OnPush", other, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("OnPushed", this, SendMessageOptions.DontRequireReceiver);
			}
		}		
	}
}
