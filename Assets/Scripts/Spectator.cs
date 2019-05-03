using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

using Photon.Pun;

public class Spectator : MonoBehaviourPun
{
    public Text arenaNumber;

    public Camera playerCamera;

    public int currentBounds;

    public List<BoxCollider> spectatorBounds;

    private void Update()
    {
        transform.position += GetDirection() * Time.deltaTime * 10;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (currentBounds == 0) { transform.position = GetPositionInBounds(spectatorBounds[spectatorBounds.Count - 1].bounds); currentBounds = spectatorBounds.Count - 1; }
            else { transform.position = GetPositionInBounds(spectatorBounds[currentBounds - 1].bounds);  currentBounds--; }       
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentBounds == spectatorBounds.Count - 1) { transform.position = GetPositionInBounds(spectatorBounds[0].bounds); currentBounds = 0; }
            else { transform.position = GetPositionInBounds(spectatorBounds[currentBounds + 1].bounds); currentBounds++; }
        }
        arenaNumber.text = "Arena #" + (currentBounds + 1);
    }

    Vector3 GetDirection()
    {
        Vector3 direction = new Vector3();
        if (Input.GetKey(KeyCode.W) && spectatorBounds[currentBounds].bounds.max.z > transform.position.z)
        {
            direction += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S) && spectatorBounds[currentBounds].bounds.min.z < transform.position.z )
        {
            direction += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A) && spectatorBounds[currentBounds].bounds.min.x < transform.position.x)
        {
            direction += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D) && spectatorBounds[currentBounds].bounds.max.x > transform.position.x)
        {
            direction += Vector3.right;
        }
        return direction;
    }

    public void Warp()
    {
        spectatorBounds = UnitSpawner.find.spectatorSpawns;
        playerCamera.transform.eulerAngles = new Vector3(90f, 0f, 0f);
        transform.position = GetPositionInBounds(spectatorBounds[0].bounds);
        currentBounds = 0;
    }

    public Vector3 GetPositionInBounds(Bounds spawnBounds)
    {
        return new Vector3(UnityEngine.Random.Range(spawnBounds.min.x, spawnBounds.max.x), spawnBounds.center.y, UnityEngine.Random.Range(spawnBounds.min.z, spawnBounds.max.z));
    }

    public void ExitGame()
    {
        GetComponent<PlayerController>().enabled = true;
        this.photonView.RPC("PhotonDestroy", RpcTarget.All);
        Application.Quit();
    }
}
