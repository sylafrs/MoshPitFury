using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	private Player player;
	private PlayerBrain Brain;

	void Awake()
	{
		player = this.GetComponent<Player>();
	}

	public float MaxSpeed;
	public float MaxSpeedDash;

	public float Acceleration;
	public float AccelerationDash;

	public float DashDuration;
	public float DashCooldown;
	private float DashTimer;

	private Vector3 Speed;
	public float BumpPower = 2;

	void UpdateDash()
	{
		if (DashTimer > 0)
			DashTimer -= Time.deltaTime;

		// On dashe.
		if (this.player.IsDashing)
		{
			// Fin du dash
			if (this.DashTimer <= 0)
			{
				this.StopsDashing();
			}
		}
		else
		{
			// Fin du cooldown
			if (DashTimer <= 0)
			{
				// Nouveau dash.
				if(this.Brain && this.Brain.WantToDash)
				{
					this.StartDashing();
				}
			}
		}
	}

	void StartDashing()
	{
		this.player.IsDashing = true;
		this.DashTimer = this.DashDuration;
	}

	void StopsDashing()
	{
		this.player.IsDashing = false;
		this.DashTimer = this.DashCooldown;

		if (this.Speed.sqrMagnitude > MaxSpeed * MaxSpeed)
		{
			this.Speed = this.Speed.normalized * MaxSpeed;
		}
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
			else if (this.Brain)
			{
				perfectSpeed = this.Brain.WantedDirection * MaxSpeed;
			}
			else
			{
				perfectSpeed = Vector3.zero;
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

	void UpdateBrain()
	{
		if (Brain == null)
			Brain = this.GetComponent<PlayerBrain>();
		if (Brain != null)
			this.Brain.UpdateState();
	}

	void Update()
	{
		UpdateBrain();
		UpdateDash();
		UpdateSpeed();
	}

	void OnBumped(BumperTrigger bumper)
	{
		this.Speed += bumper.transform.forward * bumper.Power;
	}

	private static void OnPushPlayer(Collision collision, PlayerController pusher, PlayerController pushed)
	{
		PushData push = new PushData();
		push.Collision = collision;
		push.Pushed = pushed.player;
		push.Pusher = pusher.player;

		Vector3 collisionSpeed = collision.relativeVelocity.normalized;
		if(collision.gameObject == pushed.gameObject)		
			collisionSpeed = -collisionSpeed;
		
		pushed.Speed = collisionSpeed * (pusher.BumpPower * pusher.Speed.magnitude);

		Debug.Log("PUSH : " + pusher.player.Id + " on " + pushed.player.Id + " --> " + pushed.Speed.ToString());
		
		pusher.Speed = Vector3.zero;
		pusher.StopsDashing();

		push.Other = pusher.player;
		pushed.gameObject.SendMessage("OnPushed", push, SendMessageOptions.DontRequireReceiver);

		push.Other = pushed.player;
		pusher.gameObject.SendMessage("OnPush", push, SendMessageOptions.DontRequireReceiver);
	}

	private static void OnDoublePush(Collision collision, PlayerController p1, PlayerController p2)
	{
		PushData push = new PushData();
		push.Collision = collision;
		push.Pushed = null;
		push.Pusher = null;

		p1.Speed = Vector3.zero;
		p1.StopsDashing();

		p2.Speed = Vector3.zero;
		p2.StopsDashing();

		push.Other = p2.player;
		p1.gameObject.SendMessage("OnDoublePush", push, SendMessageOptions.DontRequireReceiver);

		push.Other = p1.player;
		p2.gameObject.SendMessage("OnDoublePush", push, SendMessageOptions.DontRequireReceiver);
	}

	public void OnCollisionEnter(Collision collision)
	{
		PlayerController other = collision.gameObject.GetComponent<PlayerController>();
		if (other != null && this.player.Id < other.player.Id)
		{
			Vector3 prevSpeed = this.Speed;

			if (other.player.IsDashing && !this.player.IsDashing)
			{
				OnPushPlayer(collision, other, this);
			}

			if (this.player.IsDashing && !other.player.IsDashing)
			{
				OnPushPlayer(collision, this, other);
			}

			if(this.player.IsDashing && other.player.IsDashing)
			{
				OnDoublePush(collision, this, other);
			}
		}
	}
}

public class PushData
{
	public Player Pushed;
	public Player Pusher;
	public Player Other;
	public Collision Collision;
}
