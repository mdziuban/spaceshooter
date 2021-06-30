using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
	private Transform _cameraTransform;
	
	[SerializeField] private float _shakeDuration = 1f;
	[SerializeField] private float _shakeAmount = 0.5f;
	[SerializeField] private float _decreaseFactor = 1.0f;
	
	private Vector3 _originalPos;
    private bool _shakeTheCamera = false;
	
	void Awake()
	{
		if (_cameraTransform == null)
		{
			_cameraTransform = GetComponent<Transform>();
		}
	}
	
	private void Start()
	{
		_originalPos = _cameraTransform.localPosition;
	}

	void Update()
	{
        if (_shakeTheCamera)
        {
            if (_shakeDuration > 0)
            {
                _cameraTransform.localPosition = _originalPos + Random.insideUnitSphere * _shakeAmount;
                
                _shakeDuration -= Time.deltaTime * _decreaseFactor;
            }
            else
            {
                _shakeDuration = 1f;
                _cameraTransform.localPosition = _originalPos;
                _shakeTheCamera = false;
            }
        }
	}

    public void ShakeTheCamera(float shakeTime)
    {
        _shakeDuration = shakeTime;
        _shakeTheCamera = true;
    }
}
