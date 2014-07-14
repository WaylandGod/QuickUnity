using QuickUnity.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ConfigData of Example.
/// </summary>
public class Example : ConfigData
{
	/// <summary>
	/// ID
	/// </summary>
	public int id;

	/// <summary>
	/// 名称
	/// </summary>
	public string name;

	/// <summary>
	/// 等级
	/// </summary>
	public short level;

	/// <summary>
	/// bool值测试
	/// </summary>
	public bool boolTest;

	/// <summary>
	/// byte值测试
	/// </summary>
	public byte byteTest;

	/// <summary>
	/// double值测试
	/// </summary>
	public double doubleTest;

	/// <summary>
	/// float值测试
	/// </summary>
	public float floatTest;

	/// <summary>
	/// long值测试
	/// </summary>
	public long longTest;
	
    /// <summary>
    /// Parse the waterHeightData from configuration file.
    /// </summary>
    /// <param name="kvps">A dictionary to hold key value pair of configuration waterHeightData.</param>
    public override void ParseData(Dictionary<string, string> kvps)
    {
        base.ParseData(kvps);

		id = ReadInt("id");
		name = ReadString("name");
		level = ReadShort("level");
		boolTest = ReadBool("boolTest");
		byteTest = ReadByte("byteTest");
		doubleTest = ReadDouble("doubleTest");
		floatTest = ReadFloat("floatTest");
		longTest = ReadLong("longTest");
    }
}