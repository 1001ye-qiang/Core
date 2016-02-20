
using UnityEngine;

public class ResAttach : MonoBehaviour
{
    public delegate void OnResUpdate();

    public delegate void OnResDestroy();

    public ResAttach.OnResUpdate onResUpdate;

    public ResAttach.OnResDestroy onResDestroy;

    public void Update()
    {
        if (this.onResUpdate != null)
        {
            this.onResUpdate();
        }
    }

    public void OnDestroy()
    {
        if (this.onResDestroy != null)
        {
            this.onResDestroy();
        }
    }
}