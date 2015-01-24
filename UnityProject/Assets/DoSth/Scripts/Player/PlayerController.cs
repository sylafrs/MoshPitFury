using UnityEngine;
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
		
	private Vector3 Speed;
	public float BumpPower = 2;

	private void UpdateState()
	{
		this.PadState = GamePad.GetState((PlayerIndex)(this.player.Id - 1));
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

	void OnDeath()
	{
		if (PadState.IsConnected)
			StartCoroutine(TimeVibration(3, 0.2f, 0.2f));

	}
	void OnDestroy()
	{
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), 0, 0);
	}

	IEnumerator TimeVibration(float time, float left, float right)
	{
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), left, right);
		yield return new WaitForSeconds(time);
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), 0, 0);
	}

	void UpdateDash()
	{
		if(DashTimer > 0)
			DashTimer -= Time.deltaTime;
		
		// On dashe.
		if(this.player.IsDashing)
		{
			// Fin du dash
			if(this.DashTimer <= 0)
			{
				this.player.IsDashing = false;
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
				this.player.IsDashing = this.AButton;

				// Nouveau dash.
				if(this.player.IsDashing)
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
			if (this.player.IsDashing)
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
		
		this.Speed = Vector3.Lerp(this.Speed, perfectSpeed, Time.deltaTime * (this.player.IsDashing ? AccelerationDash : Acceleration));
		this.Speed.y = this.rigidbody.velocity.y;

		if (this.Speed.sqrMagnitude < 0.01f)
		{
			this.player.gameObject.SendMessage("OnIdle");
			this.Speed = Vector3.zero;
		}
		else
		{
			this.player.gameObject.SendMessage("OnMove");
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

			if (other.player.IsDashing)
			{
				PushData push = new PushData();
				push.Collision = collision;
				push.Pushed = this.player;
				push.Pusher = other.player;

				this.Speed += collision.relativeVelocity.normalized * (BumpPower * other.Speed.magnitude);
				this.gameObject.SendMessage("OnPushed", push, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("OnPush", push, SendMessageOptions.DontRequireReceiver);
			}

			if (this.player.IsDashing)
			{
				PushData push = new PushData();
				push.Collision = collision;
				push.Pushed = other.player;
				push.Pusher = this.player;

				other.Speed -= collision.relativeVelocity.normalized * (BumpPower * prevSpeed.magnitude);
				this.gameObject.SendMessage("OnPush", push, SendMessageOptions.DontRequireReceiver);
				other.gameObject.SendMessage("OnPushed", push, SendMessageOptions.DontRequireReceiver);
			}
		}		
	}
}

public class PushData
{
	public Player Pushed;
	public Player Pusher;
	public Collision Collision;
}
