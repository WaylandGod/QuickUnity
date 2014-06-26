using QuickUnity.Config;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class ConfigDataTemplate.
/// </summary>
public class ConfigDataTemplate : ConfigData
{
	/// <summary>
	/// ID
	/// </summary>
	public long id;

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
	/// int值测试
	/// </summary>
	public int intTest;
	
    /// <summary>
    /// Parse the data from configuration file.
    /// </summary>
    /// <param name="kvps">A dictionary to hold key value pair of configuration data.</param>
    public override void ParseData(Dictionary<string, string> kvps)
    {
        base.ParseData(kvps);	

		id = ReadLong("id");
		name = ReadString("name");
		level = ReadShort("level");
		boolTest = ReadBool("boolTest");
		byteTest = ReadByte("byteTest");
		doubleTest = ReadDouble("doubleTest");
		floatTest = ReadFloat("floatTest");
		intTest = ReadInt("intTest");
    }
}