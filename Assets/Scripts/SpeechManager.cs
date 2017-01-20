using UnityEngine;
using System.Collections;

public class SpeechManager : MonoBehaviour
{
	// Sentence sentence; 当回合要说的话，每个成员是一个音节
	// int nextWordIndex;
	// float timer; 用于控制显示这句话的计时器

	void Start ()
	{
		// 初始化
	}

	void Update ()
	{
		// 赋值 timer
		// 判断下一个单词的出现时机和 timer 
		// Y: ShowWord(nextWord)
		// N: pass

		// 如果已经显示了最后一个单词，则告知 GameManager 结束

	}

	void ShowWord (Word word)
	{
		// 显示单词 播放动画
	}
}
