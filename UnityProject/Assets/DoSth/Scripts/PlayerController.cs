using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private Player player;
	
	void Awake()
	{
		player = this.GetComponent<Player>();
	}

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

	private bool AButton
	{
		get
		{
			return Input.GetButton("P" + player.Id + "_A");
		}
	}

	private Vector3 PadDirection
	{
		get
		{
			const float sqrDeadZone = 0.09f;//0.3f * 0.3f;

			Vector3 padController = new Vector3
			(
				Input.GetAxisRaw("P" + player.Id + "_Vertical"),
				0,
				Input.GetAxisRaw("P" + player.Id + "_Horizontal")
			);

			if (padController.sqrMagnitude < sqrDeadZone)
				padController = Vector3.zero;

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

	void UpdateSpeed()
	{
		Vector3 perfectSpeed;
		if(isDashing)
		{
			perfectSpeed = this.Speed.normalized * MaxSpeedDash;
		}
		else
		{
			perfectSpeed = this.PadDirection.normalized * MaxSpeed;
		}
		
		this.Speed = Vector3.Lerp(this.Speed, perfectSpeed, Time.deltaTime * (this.isDashing ? AccelerationDash : Acceleration));
		this.Speed.y = this.rigidbody.velocity.y;
		this.rigidbody.velocity = this.Speed;
		this.rigidbody.angularVelocity = Vector3.zero;
	}
	
	void Update ()
	{
		UpdateDash();
		UpdateSpeed();
	}
		
	public void OnCollisionEnter(Collision collision)
	{
		PlayerController other = collision.gameObject.GetComponent<PlayerController>();
		if(other != null && this.player.Id < other.player.Id)
		{
			Vector3 prevSpeed = this.Speed;

			if (other.isDashing)
			{
				Debug.Log(this.player.Id + " is pushed");
				this.Speed += collision.relativeVelocity.normalized * (BumpPower * other.Speed.magnitude);
			}

			if (this.isDashing)
			{
				Debug.Log(other.player.Id + " is pushed");
				other.Speed -= collision.relativeVelocity.normalized * (BumpPower * prevSpeed.magnitude);
			}
		}		
	}
}
