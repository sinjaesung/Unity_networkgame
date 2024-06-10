using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Cinemachine;

public class Movement : MonoBehaviour
{
    private CharacterController controller;
    private new Transform transform;
    private Animator animator;
    private new Camera camera;

    // ������ Plane�� ����ĳ�����ϱ� ���� ����
    private Plane plane;
    private Ray ray;
    private Vector3 hitPoint;

    //�̵� �ӵ�
    public float moveSpeed = 10.0f;

    private PhotonView pv;
    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        transform = GetComponent<Transform>();
        animator = GetComponent<Animator>();
        camera = Camera.main;

        pv = GetComponent<PhotonView>();
        virtualCamera = GameObject.FindObjectOfType<CinemachineVirtualCamera>();

        //�ڽ��� ĳ������ ��� �ó׸ӽ� ī�޶� ����
        if (pv.IsMine)
        {
            virtualCamera.Follow = transform;
            virtualCamera.LookAt = transform;
        }

        //������ �ٴ��� �������� ���ΰ��� ��ġ�� ����
        plane = new Plane(transform.up, transform.position);
    }

    private void Update()
    {
        //�ڽ��� ĳ����(��Ʈ�p ��ü)�� ��Ʈ��
        if (pv.IsMine)
        {
            Move();
            //Turn();
        }   
    }

    float h => Input.GetAxis("Horizontal");
    float y => Input.GetAxis("Vertical");

    private void Move()
    {
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0.0f;
        cameraRight.y = 0.0f;

        Debug.Log("CameraForward,Right:" + cameraForward + "," + cameraRight);

        //�̵��� ���� ���� ���
        Vector3 moveDir = (cameraForward * y) + (cameraRight * h);
        moveDir.Set(moveDir.x, 0.0f, moveDir.z);
        //ĳ���� �̵� ó��
        controller.SimpleMove(moveDir * moveSpeed);

        //ĳ���� �ִϸ��̼� ó��
        float forward = Vector3.Dot(moveDir, transform.forward);//�յڹ��⳻��ũ��,�¿���⳻��ũ��
        float strafe = Vector3.Dot(moveDir, transform.right);//���ͳ������ؼ� ���͵鳢���� ũ�ⷮamount���ϴ� ����.
        animator.SetFloat("Forward", forward);
        animator.SetFloat("Strafe", strafe);
    }

    //ȸ�� ó���ϴ� �Լ�
    void Turn()
    {
        //���콺�� 2���� ��ǩ���� �̿��� 3���� ���̸� ����
        ray = camera.ScreenPointToRay(Input.mousePosition);
        float enter = 0.0f;

        //������ �ٴڿ� ray�� �߻��� �浹 ������ �Ÿ��� enter ������ ��ȯ
        plane.Raycast(ray, out enter);
        //������ �ٴڿ� ���̰� �浹�� ��ǩ���� ����
        hitPoint = ray.GetPoint(enter);

        //ȸ���ؾ� �� ������ ���͸� ���
        Vector3 lookDir = hitPoint - transform.position;
        lookDir.y = 0;
        //ĳ������ ȸ���� ����
        transform.localRotation = Quaternion.LookRotation(lookDir);
    }
}
