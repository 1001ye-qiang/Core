using System;
using System.Collections.Generic;
using UnityEngine;

public class ViewTrigger<T> : MonoBehaviour
{
    public delegate void OnTrigger(ViewTrigger<T> trigger);

    public delegate bool OnColidder(ViewTrigger<T> trigger, GameObject other, Vector3 pos);

    public ViewTrigger<T>.OnTrigger onTriggerEnable;

    public ViewTrigger<T>.OnTrigger onTriggerDisable;

    public ViewTrigger<T>.OnTrigger onTriggerUpdate;

    protected ViewTrigger<T>.OnColidder onColliderEnter;

    protected ViewTrigger<T>.OnColidder onColliderExit;

    private HashSet<Animator> hasHitTarget = new HashSet<Animator>();

    public float distance
    {
        get
        {
            BoxCollider component = base.GetComponent<BoxCollider>();
            if (component != null)
            {
                return component.center.z + component.size.z * 0.5f + base.transform.localPosition.z;
            }
            SphereCollider component2 = base.GetComponent<SphereCollider>();
            if (component2 != null)
            {
                return component2.center.z + component2.radius + base.transform.localPosition.z;
            }
            return 0f;
        }
    }

    public void Init(ViewTrigger<T>.OnColidder onColliderEnter, ViewTrigger<T>.OnColidder onColliderExit)
    {
        this.onColliderEnter = onColliderEnter;
        this.onColliderExit = onColliderExit;
    }

    public virtual void OnEnable()
    {
        if (this.onTriggerEnable != null)
        {
            this.onTriggerEnable(this);
        }
        this.hasHitTarget.Clear();
    }

    public virtual void OnDisable()
    {
        if (this.onTriggerDisable != null)
        {
            this.onTriggerDisable(this);
        }
    }

    public virtual void Update()
    {
        if (this.onTriggerUpdate != null)
        {
            this.onTriggerUpdate(this);
        }
    }

    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (this.onColliderEnter != null)
        {
            this.onColliderEnter(this, other.gameObject, other.gameObject.transform.position);
        }
    }

    public virtual void OnTriggerExit2D(Collider2D other)
    {
        if (this.onColliderExit != null)
        {
            this.onColliderExit(this, other.gameObject, other.gameObject.transform.position);
        }
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != base.gameObject.layer)
        {
            return;
        }
        Animator animator = other.gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            animator = other.gameObject.GetComponentInParent<Animator>();
        }
        if (animator != null)
        {
            if (this.hasHitTarget.Contains(animator))
            {
                return;
            }
            this.hasHitTarget.Add(animator);
        }
        if (this.onColliderEnter != null)
        {
            this.onColliderEnter(this, other.gameObject, ViewTrigger<T>.GetHitPos(base.gameObject, other.gameObject));
        }
    }

    public virtual void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer != base.gameObject.layer)
        {
            return;
        }
        if (this.onColliderExit != null)
        {
            this.onColliderExit(this, other.gameObject, ViewTrigger<T>.GetHitPos(base.gameObject, other.gameObject));
        }
    }

    protected static Vector3 GetHitPos(GameObject a, GameObject b)
    {
        Vector3 vector = Vector3.zero;
        if (b.GetComponent<BoxCollider>() != null)
        {
            vector = b.GetComponent<BoxCollider>().center;
        }
        else
        {
            vector = b.GetComponent<SphereCollider>().center;
        }
        Vector3 vector2 = b.transform.position + vector - a.transform.position;
        Ray ray = new Ray(a.transform.position, vector2.normalized);
        RaycastHit[] array = Physics.RaycastAll(ray, vector2.magnitude, LayerMask.GetMask(new string[]
		{
			LayerMask.LayerToName(a.layer)
		}));
        RaycastHit[] array2 = array;
        for (int i = 0; i < array2.Length; i++)
        {
            RaycastHit raycastHit = array2[i];
            if (raycastHit.collider.gameObject == b)
            {
                return raycastHit.point;
            }
        }
        return b.transform.position;
    }
}
