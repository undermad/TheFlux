using UnityEngine;

namespace TheFlux.Core.Scripts.Services.Helpers
{
    public class SpinnerView : MonoBehaviour
    {
        public void SetActive(bool active)
        {
            gameObject.SetActive(active);
        }
    }
}