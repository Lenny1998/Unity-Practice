using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 游戏对象
/// </summary>
public class GameActor : MonoBehaviour
{
    private Transform m_Transform;
    // Start is called before the first frame update
    void Start()
    {
        m_Transform = gameObject.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Jump()
    {
        Debug.Log("跳");
    }

    public void FireGun()
    {
        Debug.Log("开火");
    }

    public void SwapWeapon()
    {
        Debug.Log("换武器");
    }

    public void Squat()
    {
        Debug.Log("蹲");
    }

    public void MoveTo(Vector3 pos)
    {
        Debug.Log("移动");
        m_Transform.Translate(pos);
    }
}
