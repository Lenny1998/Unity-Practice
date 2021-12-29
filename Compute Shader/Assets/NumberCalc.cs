using System.Diagnostics;
using UnityEngine;

public class NumberCalc : MonoBehaviour
{
    public ComputeShader ComputeShader;
    private ComputeBuffer buffer;
    struct MyInt
    {
        public int val;
        public int index;
    };

    void Start()
    {
        CSFib();
    }

    public void CSFib()
    {
        MyInt[] total = new MyInt[32];
        buffer = new ComputeBuffer(32, 8);
        int kernel = ComputeShader.FindKernel("Fibonacci");
        ComputeShader.SetBuffer(kernel, "buffer", buffer);
        ComputeShader.Dispatch(kernel, 1, 1, 1);
        buffer.GetData(total);
        for (int i = 0; i < total.Length; i++)
        {
            UnityEngine.Debug.Log(total[i].val);
        }

    }

    private void OnDestroy()
    {
        buffer.Release();
    }

}
