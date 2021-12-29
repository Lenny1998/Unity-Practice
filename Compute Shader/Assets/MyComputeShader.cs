using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyComputeShader : MonoBehaviour
{
    public ComputeShader computeShader;

    private struct data
    {
        public float a;
        public float b;
        public float c;
    };

    private data[] inputDatas; 
    private data[] outputDatas;

    private void Start()
    {
        InitData();
        ToComputeShader(ref inputDatas, ref outputDatas);
    }

    private void InitData()
    {
        inputDatas = new data[3];
        outputDatas = new data[3];

        Debug.Log("--------GPU输入----------");
        for (int i = 0; i < inputDatas.Length; i++)
        {
            inputDatas[i].a = i + 1;
            inputDatas[i].b = i + 2;
            inputDatas[i].c = i + 3;
            Debug.Log(inputDatas[i].a + " , " + inputDatas[i].b + " , " + inputDatas[i].c);
        }
    }

    private void ToComputeShader(ref data[] input, ref data[] output)
    {
        //data数据里面有3个float，一个float为4个字节，所以3*4 = 12
        ComputeBuffer inputBuffer = new ComputeBuffer(input.Length, 12);
        ComputeBuffer outputBuffer = new ComputeBuffer(output.Length, 12);

        //拿到核心
        int k = computeShader.FindKernel("CSMain");

        inputBuffer.SetData(input);

        //写入GPU
        computeShader.SetBuffer(k, "inputDatas", inputBuffer);
        computeShader.SetBuffer(k, "outputDatas", outputBuffer);

        computeShader.Dispatch(k, output.Length, 1, 1);

        outputBuffer.GetData(output);

        Debug.Log("--------GPU输出----------");
        for (int i = 0; i < output.Length; i++)
        {
            Debug.Log(outputDatas[i].a + " , " + outputDatas[i].b + " , " + outputDatas[i].c);
        }

        inputBuffer.Release();
        outputBuffer.Release();
    }
}
