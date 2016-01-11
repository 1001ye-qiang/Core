using UnityEngine;
using System.Collections;

public class PrepareUI : MonoBehaviour {

    private UISlider sli;
	// Use this for initialization
	void Start () {
        sli = transform.Find("Slider").GetComponent<UISlider>();
        sli.value = 0;
	}

    private bool isTrue = false;
	// Update is called once per frame
	void Update () {
        sli.value = Mathf.Lerp(sli.value, 1f, Time.deltaTime);
        
        if (sli.value >= 0.99f && isTrue == false)
        {
            isTrue = true;
            Debug.LogError("over");
            // DO something.
            Destroy(gameObject);
        }
	}
}
