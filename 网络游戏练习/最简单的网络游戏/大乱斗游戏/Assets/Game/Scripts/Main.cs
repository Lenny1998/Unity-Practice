using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main : MonoBehaviour
{

	//人物模型预设
	public GameObject humanPrefab;
	//人物列表
	public BaseHuman myHuman;
	public Dictionary<string, BaseHuman> otherHumans = new Dictionary<string, BaseHuman>();

	void Start()
	{
		//网络模块
		NetManager.AddListener("Enter", OnEnter);
		NetManager.AddListener("List", OnList);
		NetManager.AddListener("Move", OnMove);
		NetManager.AddListener("Leave", OnLeave);
		NetManager.AddListener("Attack", OnAttack);
		NetManager.AddListener("Die", OnDie);
		NetManager.Connect("192.168.1.104", 8888);
		//添加一个角色
		GameObject obj = (GameObject)Instantiate(humanPrefab);
		float x = Random.Range(-5, 5);
		float z = Random.Range(-5, 5);
		obj.transform.position = new Vector3(x, 0, z);
		myHuman = obj.AddComponent<CtrlHuman>();
		myHuman.desc = NetManager.GetDesc();
		//发送协议
		Vector3 pos = myHuman.transform.position;
		Vector3 eul = myHuman.transform.eulerAngles;
		string sendStr = "Enter|";
		sendStr += NetManager.GetDesc() + ",";
		sendStr += pos.x + ",";
		sendStr += pos.y + ",";
		sendStr += pos.z + ",";
		sendStr += eul.y + ",";
		NetManager.Send(sendStr);
		NetManager.Send("List|");
	}

	void Update()
	{
		NetManager.Update();
	}

	void OnEnter(string msgArgs)
	{
		Debug.Log("OnEnter " + msgArgs);
		//解析参数
		string[] split = msgArgs.Split(',');
		string desc = split[0];
		float x = float.Parse(split[1]);
		float y = float.Parse(split[2]);
		float z = float.Parse(split[3]);
		float eulY = float.Parse(split[4]);
		//是自己
		if (desc == NetManager.GetDesc())
			return;
		//添加一个角色
		GameObject obj = (GameObject)Instantiate(humanPrefab);
		obj.transform.position = new Vector3(x, y, z);
		obj.transform.eulerAngles = new Vector3(0, eulY, 0);
		BaseHuman h = obj.AddComponent<SyncHuman>();
		h.desc = desc;
		otherHumans.Add(desc, h);
	}

	void OnList(string msgArgs)
	{
		Debug.Log("OnList " + msgArgs);
		//解析参数
		string[] split = msgArgs.Split(',');
		int count = (split.Length - 1) / 6;
		for (int i = 0; i < count; i++)
		{
			string desc = split[i * 6 + 0];
			float x = float.Parse(split[i * 6 + 1]);
			float y = float.Parse(split[i * 6 + 2]);
			float z = float.Parse(split[i * 6 + 3]);
			float eulY = float.Parse(split[i * 6 + 4]);
			int hp = int.Parse(split[i * 6 + 5]);
			//是自己
			if (desc == NetManager.GetDesc())
				continue;
			//添加一个角色
			GameObject obj = (GameObject)Instantiate(humanPrefab);
			obj.transform.position = new Vector3(x, y, z);
			obj.transform.eulerAngles = new Vector3(0, eulY, 0);
			BaseHuman h = obj.AddComponent<SyncHuman>();
			h.desc = desc;
			otherHumans.Add(desc, h);
		}
	}

	void OnMove(string msgArgs)
	{
		Debug.Log("OnMove " + msgArgs);
		//解析参数
		string[] split = msgArgs.Split(',');
		string desc = split[0];
		float x = float.Parse(split[1]);
		float y = float.Parse(split[2]);
		float z = float.Parse(split[3]);
		//移动
		if (!otherHumans.ContainsKey(desc))
			return;
		BaseHuman h = otherHumans[desc];
		Vector3 targetPos = new Vector3(x, y, z);
		h.MoveTo(targetPos);

	}

	void OnLeave(string msgArgs)
	{
		Debug.Log("OnLeave " + msgArgs);
		//解析参数
		string[] split = msgArgs.Split(',');
		string desc = split[0];
		//删除
		if (!otherHumans.ContainsKey(desc))
			return;
		BaseHuman h = otherHumans[desc];
		Destroy(h.gameObject);
		otherHumans.Remove(desc);
	}

	void OnAttack(string msgArgs)
	{
		//Debug.Log("OnAttack " + msgArgs);
		////解析参数
		//string[] split = msgArgs.Split(',');
		//string desc = split[0];
		//float eulY = float.Parse(split[1]);
		////攻击动作
		//if (!otherHumans.ContainsKey(desc))
		//	return;
		//SyncHuman h = (SyncHuman)otherHumans[desc];
		//h.SyncAttack(eulY);
	}

	void OnDie(string msgArgs)
	{
		Debug.Log("OnAttack " + msgArgs);
		//解析参数
		string[] split = msgArgs.Split(',');
		string attDesc = split[0];
		string hitDesc = split[0];
		//自己死了
		if (hitDesc == myHuman.desc)
		{
			Debug.Log("Game Over");
			return;
		}
		//死了
		if (!otherHumans.ContainsKey(hitDesc))
			return;
		SyncHuman h = (SyncHuman)otherHumans[hitDesc];
		h.gameObject.SetActive(false);

	}

}
