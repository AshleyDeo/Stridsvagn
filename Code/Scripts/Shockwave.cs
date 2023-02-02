using UnityEngine;

public class Shockwave : MonoBehaviour {
    [SerializeField] private float range = 10f;
    void Start() {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders) {
            if (collider.TryGetComponent<PlayerTank>(out PlayerTank playerTank)) {
                VCamController.Instance.ShakeCamera(0.8f, 0.5f);
                return;
            }
        }
    }
}