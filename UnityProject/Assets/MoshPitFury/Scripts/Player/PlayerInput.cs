using System.Collections;
using UnityEngine;
using XInputDotNetPure;

public class PlayerInput : PlayerBrain
{
	private GamePadState PadState;

	IEnumerator TimeVibration(float time, float left, float right)
	{
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), left, right);
		yield return new WaitForSeconds(time);
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), 0, 0);
	}

	void OnDeath()
	{
		if (PadState.IsConnected)
			StartCoroutine(TimeVibration(3, 0.2f, 0.2f));

	}

	Vector3 CameraRelatedDirection(Vector3 padDir)
	{
		Transform t = Camera.main.transform;

		Vector3 dir = Quaternion.Euler(0, t.eulerAngles.y, 0) * padDir;

		return dir.normalized;
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
			else
			{
				padController.x = Input.GetAxis("P" + this.player.Id + "_Horizontal");
				padController.z = Input.GetAxis("P" + this.player.Id + "_Vertical");
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

	void OnDestroy()
	{
		if (PadState.IsConnected)
			GamePad.SetVibration((PlayerIndex)(this.player.Id - 1), 0, 0);
	}

	public override void UpdateState()
	{
		this.PadState = GamePad.GetState((PlayerIndex)(this.player.Id - 1));
	}

	private bool AButton
	{
		get
		{
			if (this.PadState.IsConnected)
				return this.PadState.Buttons.A == ButtonState.Pressed;

			return Input.GetButton("P" + this.player.Id + "_A");
		}
	}

	public bool StartButton
	{
		get
		{
			if (this.PadState.IsConnected)
				return this.PadState.Buttons.Start == ButtonState.Pressed;
			return false;
		}
	}

	public override Vector3 WantedDirection
	{
		get { return CameraRelatedDirection(this.PadDirection); }
	}

	public override bool WantToDash
	{
		get { return this.AButton; }
	}

}
