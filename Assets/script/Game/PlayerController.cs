using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using Game;
//using GameFramework.Network.Movement;
using Unity.Netcode;
using UnityEngine;
[RequireComponent(typeof(CharacterController))]
public class PlayerController : NetworkBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _turnSpeed;
    [SerializeField] private Vector2 _minMaxRotationX;
    [SerializeField] private Transform _camTransform;
    //[SerializeField] private NetworkMovementComponent _playerMovement;
    //[SerializeField] private float _interactDistance;
    //[SerializeField] private LayerMask _interactionLayer;
    private CharacterController _cc;
    private PlayerControll _playerControl;
    private float _cameraAngle;
    [SerializeField] private Transform spwanedObjPrefab;
    private Transform spwanedObjTransform;


    public override void OnNetworkSpawn()
    {
        CinemachineVirtualCamera cvm = _camTransform.gameObject.GetComponent<CinemachineVirtualCamera>();
        if (IsOwner)
        {
            cvm.Priority = 1;
        }
        else
        {
            cvm.Priority = 0;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        _cc = GetComponent<CharacterController>();
        _playerControl = new PlayerControll();
        _playerControl.Enable();
        Cursor.lockState = CursorLockMode.Locked;
    }
    // Update is called once per frame
    void Update()
    {
        //Vector2 movementInput = _playerControl.Player.Move.ReadValue<Vector2>();
        //Vector2 lookInput = _playerControl.Player.Look.ReadValue<Vector2>();
        //if (IsClient && IsLocalPlayer)
        //{
        //    _playerMovement.ProcessLocalPlayerMovement(movementInput, lookInput);
        //}
        //else
        //{
        //    _playerMovement.ProcessSimulatedPlayerMovement();
        //}
        if (IsLocalPlayer)
        {
            if (_playerControl.Player.Move.inProgress)
            {
                Vector2 movementInput = _playerControl.Player.Move.ReadValue<Vector2>();
                Vector3 movement = movementInput.x * _camTransform.right + movementInput.y * _camTransform.forward;
                movement.y = 0;
                _cc.Move(movement * _speed * Time.deltaTime);
            }
            if (_playerControl.Player.Look.inProgress)
            {
                Vector2 lookInput = _playerControl.Player.Look.ReadValue<Vector2>();
                transform.RotateAround(transform.position, transform.up, lookInput.x * _turnSpeed * Time.deltaTime);
                RotateCamera(lookInput.y);
            }
            if (Input.GetKey(KeyCode.T)) {
            spawnBallServerRPC();
            }
        }
        
        //if (!IsOwner) return;
        ///*if (Input.GetKey(KeyCode.T)) {
        //    randomNumber.Value = Random.Range(0, 100);
        //}*/
        //Vector3 moveDir = new Vector3(0, 0, 0);
        //if (Input.GetKey(KeyCode.W)) moveDir.z = +1f;
        //if (Input.GetKey(KeyCode.S)) moveDir.z = -1f;
        //if (Input.GetKey(KeyCode.A)) moveDir.x = -1f;
        //if (Input.GetKey(KeyCode.D)) moveDir.x = +1f;
        //float moveSpeed = 3f;
        //transform.position += moveDir * moveSpeed * Time.deltaTime;
    }
    //[ServerRpc]
    //private void UseButtonServerRpc()
    //{
    //    if (Physics.Raycast(_camTransform.position, _camTransform.forward, out RaycastHit hit, _interactDistance, _interactionLayer))
    //    {
    //        if (hit.collider.TryGetComponent<ButtonDoor>(out ButtonDoor buttonDoor))
    //        {
    //            buttonDoor.Activate();
    //        }
    //    }
    //}
    private void RotateCamera(float lookInputY)
    {
        _cameraAngle = Vector3.SignedAngle(transform.forward, _camTransform.forward, _camTransform.right);
        float cameraRotationAmount = lookInputY * _turnSpeed * Time.deltaTime;
        float newCameraAngle = _cameraAngle - cameraRotationAmount;
        if (newCameraAngle <= _minMaxRotationX.x && newCameraAngle >= _minMaxRotationX.y)
        {
            _camTransform.RotateAround(_camTransform.position, _camTransform.right, -lookInputY * _turnSpeed * Time.deltaTime);
        }
    }
    private void spawnBall(){
        spwanedObjTransform = Instantiate(spwanedObjPrefab);
        spwanedObjTransform.position = transform.position;
        spwanedObjTransform.GetComponent<NetworkObject>().Spawn();
    }

    [ServerRpc]
    private void spawnBallServerRPC(){
        spawnBall();
    }
}