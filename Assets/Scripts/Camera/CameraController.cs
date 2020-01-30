using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public bool isFollowPlayer { get; set; }
    public float MinimumZoom = 5;
    public float MaximumZoom = 10f;
    public Transform Target;
    public PlatformController TargetController;

    //快移时摄像机往前移
    public float HorizentalLookDistance;
    public Vector3 CameraOffset;




    public bool PixelPerfect = false;

    public float LookAheadTrigger = 0.1f;
    public float ZoomSpeed = 0.4f;
    public float ResetSpeed = 0.5f;

    public float CameraSpeed = 0.3f;


    protected Bounds _levelBounds;

    protected float _currentZoom;
    protected Camera _camera;

    protected float _xMin;
    protected float _xMax;
    protected float _yMin;
    protected float _yMax;

    protected Vector3 _currentVelocity;
    protected Vector3 _lastTargetPosition;
    protected Vector3 _lookAheadPos;
    protected float _offsetZ;

    protected Vector3 _lookDirectionModifier = Vector3.zero;


    //shake
    protected float _shakeDuration;
    protected float _shakeIntensity;
    public float _shakeDecay;

    // Start is called before the first frame update
    public virtual void Init(ActPlayerController controller)
    {
        _camera = GetComponent<Camera>();
        isFollowPlayer = true;
        _currentZoom = MinimumZoom;

        if(controller.Pawn == null)
        {
            return;
        }
        AssignTarget(controller.Pawn);

        //
        if(OldGameMain.GetInstance().gameMode != null)
        {
            _levelBounds = OldGameMain.GetInstance().gameMode.GetLevelBound();
        }

        _lastTargetPosition = Target.position;
        _offsetZ = (transform.position - Target.position).z;
        transform.parent = null;
        if (PixelPerfect)
        {

        }
        else
        {
            Zoom();
        }
    }

    protected virtual void AssignTarget(ActCharacterNew pawn)
    {
        Target = pawn.transform;
        if(pawn.platformer == null)
        {
            return;
        }
        TargetController = pawn.platformer;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        GetLevelBounds();

        if(!isFollowPlayer || TargetController == null)
        {
            return;
        }
        if (!PixelPerfect)
        {
            Zoom();
        }
        FollowPlayer();
    }


    public virtual void Shake(Vector3 shakeParameters)
    {

    }

    protected virtual void FollowPlayer()
    {
        float xMoveDelta = (Target.position - _lastTargetPosition).x;

        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > LookAheadTrigger;

        if (updateLookAheadTarget)
        {
            _lookAheadPos = HorizentalLookDistance * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
            _lookAheadPos = Vector3.MoveTowards(_lookAheadPos,Vector3.zero,Time.deltaTime*ResetSpeed);
        }

        Vector3 aheadTargetPos = Target.position + _lookAheadPos + Vector3.forward * _offsetZ + _lookDirectionModifier + CameraOffset;
        Vector3 newCameraPosition = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref _currentVelocity, CameraSpeed);
        Vector3 shakeFactorPosition = Vector3.zero;

        if(_shakeDuration > 0)
        {
            shakeFactorPosition = Random.insideUnitSphere * _shakeIntensity * _shakeDuration;
            _shakeDuration -= _shakeDecay * Time.deltaTime;
        }

        newCameraPosition = newCameraPosition + shakeFactorPosition;

        //must be true
        if(_camera.orthographic == true)
        {
            float posX, posY,posZ;

            if(_levelBounds.size != Vector3.zero)
            {
                posX = Mathf.Clamp(newCameraPosition.x,_xMin,_xMax);
                posY = Mathf.Clamp(newCameraPosition.y,_yMin,_yMax);
            }
            else
            {
                posX = newCameraPosition.x;
                posY = newCameraPosition.y;
            }
            posZ = newCameraPosition.z;
            transform.position = new Vector3(posX,posY,posZ);
        }
        else
        {
            transform.position = newCameraPosition;
        }

        _lastTargetPosition = Target.position;

    }



    protected virtual void Zoom()
    {
        if (PixelPerfect)
        {
            return;
        }

        float charaterSped = Mathf.Abs(TargetController.Speed.x);
        float currentVolocity = 0f;

        _currentZoom = Mathf.SmoothDamp(_currentZoom,(charaterSped/10)*(MaximumZoom - MinimumZoom) + MinimumZoom, ref currentVolocity, ZoomSpeed);
        _camera.orthographicSize = _currentZoom;
    }

    protected virtual void GetLevelBounds()
    {
        if(_levelBounds.size == Vector3.zero)
        {
            return;
        }

        //problem
        float cameraHeight = Camera.main.orthographicSize * 2f;
        float cameraWidth = cameraHeight * Camera.main.aspect;

        _xMin = _levelBounds.min.x + (cameraWidth/2);
        _xMax = _levelBounds.min.x - (cameraWidth / 2);
        _yMin = _levelBounds.min.x + (cameraHeight / 2);
        _yMax = _levelBounds.min.x - (cameraHeight / 2);

        if(_levelBounds.max.x - _levelBounds.min.x <= cameraWidth)
        {
            _xMin = _levelBounds.center.x;
            _xMax = _levelBounds.center.x;
        }

        if (_levelBounds.max.y - _levelBounds.min.y <= cameraHeight)
        {
            _yMin = _levelBounds.center.x;
            _yMax = _levelBounds.center.x;
        }

    }

    public virtual void TeleportCameraToTarget()
    {
        this.transform.position = Target.position + _lookAheadPos + Vector3.forward * _offsetZ + _lookDirectionModifier + CameraOffset;
    }
}
