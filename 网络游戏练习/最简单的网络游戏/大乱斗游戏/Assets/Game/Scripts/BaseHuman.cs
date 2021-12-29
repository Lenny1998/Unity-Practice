using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 人物基类
/// </summary>
public class BaseHuman : MonoBehaviour
{
    /// <summary>
    /// 是否正在移动
    /// </summary>
    protected bool isMoving = false;

    /// <summary>
    /// 移动目标点
    /// </summary>
    private Vector3 targetPosition;

    /// <summary>
    /// 动画组件
    /// </summary>
    private Animator animator;

    /// <summary>
    /// 移动速度
    /// </summary>
    public float speed = 1.2f;

    /// <summary>
    /// 描述
    /// </summary>
    public string desc = "";

    // Start is called before the first frame update
    protected void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        if (animator == null)
        {
            return;
        }
    }

    // Update is called once per frame
    protected void Update()
    {
        MoveUpdate();
    }

    /// <summary>
    /// 移动到某处
    /// </summary>
    /// <param name="pos"></param>
    public void MoveTo(Vector3 pos)
    {
        targetPosition = pos;
        isMoving = true;
        animator.SetBool("isMoving", true);
    }

    public void MoveUpdate()
    {
        if (isMoving == false)
        {
            return;
        }

        Vector3 pos = transform.position;
        transform.position = Vector3.MoveTowards(pos, targetPosition, speed * Time.deltaTime);
        transform.LookAt(targetPosition);
        if (Vector3.Distance(pos,targetPosition) < 0.05f)
        {
            isMoving = false;
            animator.SetBool("isMoving", false);
        }
    }
}
