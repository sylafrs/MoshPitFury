using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour 
{
	private Player player;
    private PlayerBrain Brain;
		
	void Awake()
	{
		player  = this.GetComponent<Player>();
	}


	public float	MaxSpeed;
	public float	MaxSpeedDash;

	public float	Acceleration;
	public float	AccelerationDash;

	public float	DashDuration;
	public float	DashCooldown;
	private float	DashTimer;
		
	private Vector3 Speed;
	public float BumpPower = 2;

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
				this.player.IsDashing = this.Brain.WantToDash;

				// Nouveau dash.
				if(this.player.IsDashing)
					this.DashTimer = this.DashDuration;
			}
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
			else
			{
				perfectSpeed = this.Brain.WantedDirection * MaxSpeed;
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
        if(Brain == null)
            Brain = this.GetComponent<PlayerBrain>();
        if(Brain != null)
            this.Brain.UpdateState();		
    }
	
	void Update ()
	{
        UpdateBrain();
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
        if (other != null && this.player.Id < other.player.Id)
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
