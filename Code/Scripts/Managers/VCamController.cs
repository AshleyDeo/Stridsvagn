using UnityEngine;
using Cinemachine;
public class VCamController : MonoBehaviour {
    public static VCamController Instance { get; private set; }
    [SerializeField] Logger _log;
    [SerializeField] bool _showLogs = false;
    private CinemachineVirtualCamera vCam;
    private float shakeTimer = 0f;

    void Awake() {
		if (Instance != null && Instance != this) {
			Destroy(this);
		}
		else { Instance = this; }
		vCam = GetComponent<CinemachineVirtualCamera>();
	}
    void Start() {
	    vCam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>().m_AmplitudeGain = 0;
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
    public void SetFollow(Transform target) {
        _log.Log("New VCam Target", this);
        vCam.LookAt = target;
        vCam.Follow = target;
	}
	protected void Log (string message) {
		if (!_showLogs) return;
		if (_log) _log.Log(message, this);
		else Debug.Log(message);
	}
}