using UnityEngine;
using System.Collections;

namespace FuckGame.View
{
    public class ViewAnimator : MonoBehaviour
	{
		private Animator animator;

		private float speedRemainTime;

		private float animatorSpeed = 1f;

		public virtual void Initialize()
		{
			this.animator = base.GetComponent<Animator>();
		}

		public virtual void SetAniInteger(int stateName, int state)
		{
			this.animator.SetInteger(stateName, state);
		}

		public virtual void SetAniBoolean(int stateName, bool value)
		{
			this.animator.SetBool(stateName, value);
		}

		public virtual void SetAniTrigger(int id)
		{
			this.animator.SetTrigger(id);
		}

		public virtual void ResetAniTrigger(int id)
		{
			this.animator.ResetTrigger(id);
		}

		public virtual void Play()
		{
			this.animatorSpeed = 1f;
		}

		public virtual void Pause()
		{
			this.animatorSpeed = 0f;
		}

		private void Update()
		{
			if (this.speedRemainTime != 0f)
			{
				this.speedRemainTime -= Time.deltaTime;
				if (this.speedRemainTime < 0f)
				{
					this.speedRemainTime = 0f;
					this.animatorSpeed = 1f;
				}
			}
			if (this.animator != null && this.animator.speed != this.animatorSpeed)
			{
				this.animator.speed = this.animatorSpeed;
			}
		}

		public void SetAniSpeed(float speed, float keepTime)
		{
			this.animatorSpeed = speed;
			this.speedRemainTime = keepTime;
		}
	}
}
