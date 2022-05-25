using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoPioche : MonoBehaviour
{
    [SerializeField] private GameObject interdit;
    [SerializeField] private GameObject pioche;
    private bool isBlinking;
    public void StartBlinking(){
        if(!isBlinking){
            StartCoroutine(blinkForbidden());
        }
    }

    private IEnumerator blinkForbidden() {
        isBlinking = true;
        for (int i = 0; i < 4; i++)
        {
            interdit.SetActive(true);
            pioche.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            interdit.SetActive(false);
            pioche.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        isBlinking = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
