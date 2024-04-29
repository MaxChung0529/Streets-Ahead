using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : MonoBehaviour
{
    [SerializeField] private float speed;
    private float direction;
    private int moveForSec = 1;
    private float movementSpeed = 0;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        movementSpeed = speed * Time.deltaTime * direction;
        StartCoroutine(MoveForward());
    }

    public void SetDirection(float _direction)
    {
        direction = _direction;
        gameObject.SetActive(true);

        float localScaleX = transform.localScale.x;
        if (Mathf.Sign(localScaleX) != _direction)
        {
            localScaleX = -localScaleX;
        }

        transform.localScale = new Vector3(localScaleX, transform.localScale.y, transform.localScale.z);
    }

    IEnumerator MoveForward()
    {
        transform.Translate(movementSpeed, 0, 0);
        yield return new WaitForSeconds(moveForSec);
        gameObject.SetActive(false);
    }

}
