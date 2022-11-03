using UnityEngine;

public class Shockwave : MonoBehaviour {
    [SerializeField] private float range = 10f;
    void Start() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, range);
        foreach (Collider2D collider in colliders) {
            if (collider.TryGetComponent<PlayerTank>(out PlayerTank playerTank)) {
                VCamController.Instance.ShakeCamera(0.8f, 0.5f);
                return;
            }
        }
    }
}