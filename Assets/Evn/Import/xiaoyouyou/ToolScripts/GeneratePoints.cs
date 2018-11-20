using UnityEngine;
using System.Collections;
using System.IO;

public class GeneratePoints : MonoBehaviour {
	public string file_name = string.Empty;

	[ContextMenu("Export Server File")]
	void ExportFile(){

		if (file_name == string.Empty) {
			Debug.Log (name + " need set filename!!!");
			return;
		}

		FileStream fs = new FileStream(Application.dataPath + "/" + file_name + ".py", FileMode.Create);
		StreamWriter sw = new StreamWriter(fs,System.Text.Encoding.GetEncoding("UTF-8"));
		//开始写入
		sw.WriteLine ("# -*- coding: utf-8 -*-");
		sw.WriteLine("DATAS = [");


		Transform[] objs = GetComponentsInChildren<Transform> (true);
		int i = 0, len = objs.Length;
		foreach (Transform child in objs)  
		{  
			i++;
			if(child.gameObject == this.gameObject) continue;

			Debug.Log(child.name + ":" + child.position);
			string str_point = "    ";
			if(i < len)
				str_point = str_point + child.position + ",";
			else
				str_point = str_point + child.position;

			sw.WriteLine(str_point);
		}  
		sw.WriteLine("]");
		//清空缓冲区
		sw.Flush();
		//关闭流
		sw.Close();
		fs.Close();

		Debug.LogError ("如果布点有变动,请把这个文件(" + Application.dataPath + "/" + file_name + ".py) 提交给服务器开发人员!!!");
	}
}
