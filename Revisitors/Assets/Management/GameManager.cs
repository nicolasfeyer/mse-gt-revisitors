using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private Vector2 cameraOffset;
    [SerializeField] private RectTransform panel;

    public UnityEvent onPlayerDead;

    private Camera cam;
    private PlayerCtrl player;
    private AllCheckpoint allCheckpoints;
    private int checkpointId = 0;
    public static GameManager instance;
    Vector3 offset;

    bool isPause = false;

    private void Awake()
    {
        Cursor.visible = false;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        offset = new Vector3(cameraOffset.x, cameraOffset.y, -20);
    }


    private void Start()
    {
        panel.gameObject.SetActive(true);

        cam = Camera.main;
        checkpointId = 0;
        allCheckpoints = FindObjectOfType<AllCheckpoint>();
        //GameObject playerGO = Instantiate(playerPrefab, allCheckpoints.GetCheckpoint(0).SpawnPoint.position, Quaternion.identity);

        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            if (PlayerCtrl.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject playerGO = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
                player = playerGO.GetComponent<PlayerCtrl>();
                //player.IsGoingRight = allCheckpoints.GetCheckpoint(0).GoRight;
                player.SetDirectionRight(allCheckpoints.GetCheckpoint(0).GoRight);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }


        }
    }

    public void ChangeCheckpoint(int id)
    {
        checkpointId = id;
    }

    public void RespawnPlayer()
    {
        onPlayerDead.Invoke();
        player.transform.position = allCheckpoints.GetCheckpoint(checkpointId).SpawnPoint.position;
        //player.IsGoingRight = allCheckpoints.GetCheckpoint(checkpointId).GoRight;
        player.SetDirectionRight(allCheckpoints.GetCheckpoint(checkpointId).GoRight);
    }

    public void EndGame()
    {
        player.EndGame();

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    private void LateUpdate()
    {
        if (cam != null && player != null)
        {
            cam.transform.position = player.transform.position + new Vector3(0f, 0f, -10f);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Pause();
        }
    }

    public void Pause()
    {
        isPause = !isPause;
        if (isPause)
        {
            Time.timeScale = 0f;
            panel.gameObject.SetActive(true);
            Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            panel.gameObject.SetActive(false);
            Cursor.visible = false;
        }
    }

    private void OnDestroy()
    {
        Cursor.visible = true;
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Cursor.visible = true;
        SceneManager.LoadScene(0);
    }
}
