using UnityEngine;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;

    [SyncVar(hook = "OnIsBallSet")] private bool _isBall;
    [SerializeField] private Vector3 _offset;

    private void OnIsBallSet(bool isBall)
    {
        transform.FindChild("Ball").gameObject.SetActive(isBall);
        transform.FindChild("Box").gameObject.SetActive(!isBall);
    }

    [Command]
    private void CmdSetIsBall(bool isBall)
    {
        _isBall = isBall;
    }

    private void Start()
    {
        OnIsBallSet(_isBall);

        if (!isLocalPlayer)
        {
            return;
        }

        CmdSetIsBall(GlobalState.IsBall);
    }

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        var x = Input.GetAxis("Horizontal") * -0.1f;
        var z = Input.GetAxis("Vertical") * -0.1f;

        transform.Translate(x, 0, z);

        transform.Rotate(0, Input.GetAxis("Mouse X"), 0);

        FindObjectOfType<Camera>().transform.position = transform.position - (Quaternion.Euler(0, transform.eulerAngles.y, 0) * _offset);

        FindObjectOfType<Camera>().transform.LookAt(transform);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    [Command]
    private void CmdFire()
    {
        var bullet = (GameObject)Instantiate(_bulletPrefab, transform.position - transform.forward, Quaternion.identity);
        
        bullet.GetComponent<Rigidbody>().velocity = -transform.forward * 8;

        NetworkServer.Spawn(bullet);
        
        Destroy(bullet, 3.0f);
    }
}