using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerTabs : MonoBehaviour
{
    public List<GameObject> OldTabs = new List<GameObject>();
    public GameObject NewTab;
    public float cooldownTime = 1.5f;

    private bool isCooldown = false;
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandTag") && !isCooldown && !hasTriggered)
        {
            StartCoroutine(SwitchTabsWithCooldown());
            hasTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("HandTag"))
        {
            hasTriggered = false; // Allow re-triggering on next entry
        }
    }

    private IEnumerator SwitchTabsWithCooldown()
    {
        isCooldown = true;

        foreach (var obj in OldTabs)
            obj.SetActive(false);

        NewTab.SetActive(true);

        yield return new WaitForSeconds(cooldownTime);
        isCooldown = false;
    }
}
