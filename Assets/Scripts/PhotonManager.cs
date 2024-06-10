using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    //���� �Է�
    private readonly string version = "1.0f";
    //����� ���̵� �Է�
    private string userId = "Mary";

    private void Awake()
    {
        //���� ���� �����鿡�� �ڵ����� ���� �ε�
        PhotonNetwork.AutomaticallySyncScene = true;
        //���� ������ �������� ���� ���
        PhotonNetwork.GameVersion = version;
        //���� ���̵� �Ҵ�
        PhotonNetwork.NickName = userId;
        //���� ������ ��� Ƚ�� ����. �ʴ� 30ȸ
        Debug.Log(PhotonNetwork.SendRate);
        //���� ����
        PhotonNetwork.ConnectUsingSettings();
    }

    //���� ������ ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnConnectedToMaster()
    {
        Debug.Log("PhotonNetwork|Connected to Master!");
        Debug.Log($"PhotonNetwork|.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinLobby(); //�κ� ����
    }

    //�κ� ���� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedLobby()
    {
        Debug.Log($"PhotonNetwork|.InLobby = {PhotonNetwork.InLobby}");
        PhotonNetwork.JoinRandomRoom(); //���� ��ġ����ŷ ��� ����
    }

    //������ �� ������ �������� ��� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log($"PhotonNetwork|JoinRandom failed {returnCode}:{message}");

        //���� �Ӽ� ����
        RoomOptions ro = new RoomOptions();
        ro.MaxPlayers = 20;//�ִ������ڼ�
        ro.IsOpen = true;//���ǿ��¿���
        ro.IsVisible = true;//�κ񿡼� �� ��Ͽ� ���� ��ų�� ����

        //�� ����
        PhotonNetwork.CreateRoom("My Room", ro);
    }

    //�� ������ �Ϸ�� �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnCreatedRoom()
    {
        Debug.Log("PhotonNetwork|Created Room");
        Debug.Log($"PhotonNetwork|Room Name = {PhotonNetwork.CurrentRoom.Name}");
    }

    //�뿡 ������ �� ȣ��Ǵ� �ݹ� �Լ�
    public override void OnJoinedRoom()
    {
        Debug.Log($"PhotonNetwork|.InRoom = {PhotonNetwork.InRoom}");
        Debug.Log($"PhotonNetwork|Player Count = {PhotonNetwork.CurrentRoom.PlayerCount}");

        //�뿡 ������ ����� ���� Ȯ��
        foreach(var player in PhotonNetwork.CurrentRoom.Players)
        {
            Debug.Log($"PhotonNetwork|{player.Value.NickName},{player.Value.ActorNumber}");
            //$ => String.Format()
        }

        //ĳ���� ���� ������ �迭�� ����
        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idex = Random.Range(1, points.Length);
        //ĳ���͸� ����
        PhotonNetwork.Instantiate("Player", points[idex].position, points[idex].rotation,0);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
