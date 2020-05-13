using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour {
    public int moveSpeed;
    public Transform enemy;
    private PathFinding pathMgr;
    private Grid grid;
	// Use this for initialization
	void Start () {
        enemy = GameObject.Find("enemy").transform;
        pathMgr = GameObject.Find("A*").GetComponent<PathFinding>();
        grid = GameObject.Find("A*").GetComponent<Grid>();
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKey(KeyCode.W))
        {
            this.transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Translate(Vector3.left * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            this.transform.Translate(Vector3.back * moveSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            this.transform.Translate(Vector3.right * moveSpeed * Time.deltaTime);
        }
    }

    public void StartMove()
    {
        Vector3 startPos = transform.position;
        Vector3 endPos = enemy.position;
        pathMgr.PathFind(startPos, endPos);
        StartCoroutine(Move());
    }

    IEnumerator Move()
    {
        foreach (Node n in grid.path)
        {
            this.transform.SetPositionAndRotation(new Vector3(n.worldPos.x, 1, n.worldPos.z), new Quaternion());
            yield return new WaitForSeconds(0.5f);
        }
    }
}
