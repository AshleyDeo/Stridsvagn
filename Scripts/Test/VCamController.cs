using UnityEngine;
using Cinemachine;
public class VCamController : MonoBehaviour {
    public static VCamController Instance { get; private set; }

    private CinemachineVirtualCamera vCam;
    private float shakeTimer = 0f;
    void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else { Instance = this; }
		vCam = GetComponent<CinemachineVirtualCamera>();
    }
    void Update() {
        if (shakeTimer > 0) {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0) {
				CinemachineBasicMultiChannelPerlin cBMCP = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
				cBMCP.m_AmplitudeGain = 0f;
			}
        }
    }
    public void ShakeCamera(float intensity, float time) {
        CinemachineBasicMultiChannelPerlin cBMCP = vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cBMCP.m_AmplitudeGain = intensity;
        shakeTimer = time;
    }
}