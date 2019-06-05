using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

struct MonsterAtATime
{
	
}
class JsonObject
{
	
	public string Name { get; set; }
	public string Skill { get; set; }
	public int Age { get; set; }
	public override string ToString()
	{
		return string.Format("JsonObject:name:{0},age={1},skill={2}", Name, Age, Skill);
	}
}
public class GameManager : MonoBehaviour {

    
    public GameObject Player;
    public GameObject Door;
	public GameObject[] Monsters;
	JsonData waves;

	Vector3 pos =new Vector3(9.5f, 0.0f, 15.0f);
	Quaternion rot = Quaternion.Euler(0, 180, 0);


	IEnumerator CreateMethod()
	{
		//对于n波怪物
		for (int OnGoingWave = 0; OnGoingWave < waves.Count; OnGoingWave++)
		{
			JsonData monster = waves[OnGoingWave]["monster"];
			float waveTime = (float)waves[OnGoingWave]["time"];
			Debug.Log(waveTime);//这一波怪物持续的时间

			Vector3 pos = new Vector3(9.5f, 0.0f, 15.0f);
			Quaternion rot = Quaternion.Euler(0, 180, 0);
			for (int m=0; m<monster.Count;m++ )
			{
				pos = new Vector3((float)monster[m]["posx"], (float)monster[m]["posy"], (float)monster[m]["posz"]);
				rot = Quaternion.Euler(0, (int)monster[m]["rot"], 0);
				switch ((int)monster[m]["type"])
				{
					case 0:
						
						GameObject monster0Clone = (GameObject)GameObject.Instantiate(Monsters[0], pos, rot);
						monster0Clone.SetActive(true);
						yield return new WaitForSeconds(0.5f);
						break;
					case 1:
						GameObject monster1Clone = (GameObject)GameObject.Instantiate(Monsters[1], pos, rot);
						monster1Clone.SetActive(true);
						yield return new WaitForSeconds(0.5f);
						break;
					default:
						break;
				}
				
			}
			yield return new WaitForSeconds(waveTime);
		}
			
		
	}
	// Use this for initialization
	void Start () {
		string contents = System.IO.File.ReadAllText(@"Assets\waves.json");//json数据文件的相对路径
		//Debug.Log("contents = " + contents);
		waves = JsonMapper.ToObject(contents)["waves"];//得到一个jsondata数组
		//Debug.Log(waves);

		StartCoroutine(CreateMethod());//生成怪物的协程，独立
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
