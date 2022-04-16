using UnityEngine;
using UnityEngine.Analytics;

public class AnalyticsEventManager : MonoBehaviour
{
    public void OnEvent(string Event)
    {
        Analytics.CustomEvent(Event);
    }
}
