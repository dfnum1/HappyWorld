using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using System;
using System.Windows.Forms;

public class ExcelUtility
{
    static int BEGIN_ROW = 3;
    /// <summary>
    /// 表格数据集合
    /// </summary>
    private DataSet mResultSet;

	/// <summary>
	/// 构造函数
	/// </summary>
	/// <param name="excelFile">Excel file.</param>
	public ExcelUtility (string excelFile)
	{
        try
        {
            FileStream mStream = File.Open(excelFile, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
            mResultSet = mExcelReader.AsDataSet();
        }
        catch (System.Exception ex)
        {
        	
        }
	}

    bool IsEmptyRow(DataRow row, int col)
    {
        for (int j = 0; j < col; j++)
        {
            string field = row[j].ToString();
            if(!string.IsNullOrEmpty(field))
            {
                return false;
            }    
        }
        return true;
    }
			
	/// <summary>
	/// 转换为实体类列表
	/// </summary>
	public List<T> ConvertToList<T> ()
	{
        if (mResultSet == null)
            return null;
		//判断Excel文件中是否存在数据表
		if (mResultSet.Tables.Count < 1)
			return null;
		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];
			
		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return null;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;
				
		//准备一个列表以保存全部数据
		List<T> list = new List<T> ();
				
		//读取数据
		for (int i=1; i<rowCount; i++) 
		{
			//创建实例
			Type t = typeof(T);
			ConstructorInfo ct = t.GetConstructor (System.Type.EmptyTypes);
			T target = (T)ct.Invoke (null);
			for (int j=0; j<colCount; j++) 
			{
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				object value = mSheet.Rows [i] [j];
				//设置属性值
				SetTargetProperty (target, field, value);
			}
					
			//添加至列表
			list.Add (target);
		}
				
		return list;
	}

	/// <summary>
	/// 转换为Json
	/// </summary>
	/// <param name="JsonPath">Json文件路径</param>
	/// <param name="Header">表头行数</param>
	public void ConvertToJson (string JsonPath, Encoding encoding)
	{
        if (mResultSet == null)
            return;
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//准备一个列表存储整个表的数据
		List<Dictionary<string, object>> table = new List<Dictionary<string, object>> ();

		//读取数据
		for (int i = 1; i < rowCount; i++) {
            if (i >= BEGIN_ROW && IsEmptyRow(mSheet.Rows[i], colCount))
                continue;
			//准备一个字典存储每一行的数据
			Dictionary<string, object> row = new Dictionary<string, object> ();
			for (int j = 0; j < colCount; j++) {
				//读取第1行数据作为表头字段
				string field = mSheet.Rows [0] [j].ToString ();
				//Key-Value对应
				row [field] = mSheet.Rows [i] [j];
			}

			//添加到表数据中
			table.Add (row);
		}

		//生成Json字符串
		string json = JsonConvert.SerializeObject (table, Newtonsoft.Json.Formatting.Indented);
		//写入文件
		using (FileStream fileStream=new FileStream(JsonPath,FileMode.Create,FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
				textWriter.Write (json);
			}
		}
	}

    /// <summary>
	/// 转换为lua
	/// </summary>
	/// <param name="luaPath">lua文件路径</param>
	public void ConvertToLua(string luaPath, Encoding encoding)
    {
        if (mResultSet == null)
            return;
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("local datas = {");
        stringBuilder.Append("\r\n");

        //读取数据表
        foreach (DataTable mSheet in mResultSet.Tables)
        {
            //判断数据表内是否存在数据
            if (mSheet.Rows.Count < 1)
                continue;

            //读取数据表行数和列数
            int rowCount = mSheet.Rows.Count;
            int colCount = mSheet.Columns.Count;

            //准备一个列表存储整个表的数据
            List<Dictionary<string, object>> table = new List<Dictionary<string, object>>();

            //读取数据
            for (int i = 1; i < rowCount; i++)
            {
                if (i >= BEGIN_ROW && IsEmptyRow(mSheet.Rows[i], colCount))
                    continue;
                //准备一个字典存储每一行的数据
                Dictionary<string, object> row = new Dictionary<string, object>();
                for (int j = 0; j < colCount; j++)
                {
                    //读取第1行数据作为表头字段
                    string field = mSheet.Rows[0][j].ToString();
                    //Key-Value对应
                    row[field] = mSheet.Rows[i][j];
                }
                //添加到表数据中
                table.Add(row);
            }
            stringBuilder.Append(string.Format("\t\"{0}\" = ", mSheet.TableName));
            stringBuilder.Append("{\r\n");
            foreach (Dictionary<string, object> dic in table)
            {
                stringBuilder.Append("\t\t{\r\n");
                foreach (string key in dic.Keys)
                {
                    if (dic[key].GetType().Name == "String")
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = \"{1}\",\r\n", key, dic[key]));
                    else
                        stringBuilder.Append(string.Format("\t\t\t\"{0}\" = {1},\r\n", key, dic[key]));
                }
                stringBuilder.Append("\t\t},\r\n");
            }
            stringBuilder.Append("\t}\r\n");
        }

        stringBuilder.Append("}\r\n");
        stringBuilder.Append("return datas");

        //写入文件
        using (FileStream fileStream = new FileStream(luaPath, FileMode.Create, FileAccess.Write))
        {
            using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
            {
                textWriter.Write(stringBuilder.ToString());
            }
        }
    }

    void ConvertToCsv(StringBuilder stringBuilder, DataTable mSheet, int rowStart = 0)
    {
        if (mResultSet == null)
            return;
        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 1)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        string[] dataTypes = new string[colCount];
        for (int i = 0; i < colCount; ++i)
        {
            string dataType = mSheet.Rows[1][i].ToString().Trim();
            if (string.IsNullOrEmpty(dataType))
            {
                dataTypes[i] = "";
                //MessageBox.Show(Path.GetFileNameWithoutExtension(strFile) + "表:数据类型行不能有空类型!");
                continue;
            }

            dataTypes[i] = dataType;
        }

        string[] defaults = new string[colCount];
        string[] externLines = new string[colCount];
        for (int i = 0; i < colCount; ++i)
        {
            string dataType = mSheet.Rows[2][i].ToString().Trim();
            if (string.IsNullOrEmpty(dataType))
            {
                externLines[i] = "";
                defaults[i] = "";
                //MessageBox.Show(Path.GetFileNameWithoutExtension(strFile) + "表:数据类型行不能有空类型!");
                continue;
            }

            externLines[i] = dataType;

            int defaultLeft = dataType.IndexOf('#');
            int defaultRight = dataType.LastIndexOf('#');
            if (defaultLeft < defaultRight && defaultRight > 0)
            {
                externLines[i] = dataType.Substring(0, defaultLeft);
                defaults[i] = dataType.Substring(defaultLeft + 1, defaultRight - defaultLeft - 1);
            }
            else
                defaults[i] = "";
        }

        //读取数据
        for (int i = rowStart; i < rowCount; i++)
        {
            if (i >= BEGIN_ROW && IsEmptyRow(mSheet.Rows[i], colCount))
                continue;
            for (int j = 0; j < colCount; j++)
            {
                if (string.IsNullOrEmpty(dataTypes[j])) continue;
                string strLabel = mSheet.Rows[i][j].ToString();
                //使用","分割每一个数值
                if (i >= 4)
                {
                    if (!string.IsNullOrEmpty(defaults[j]) && string.IsNullOrEmpty(strLabel))
                        strLabel = defaults[j];
                    //data zooms
                    if (dataTypes[j].ToLower().Contains("bit|"))
                    {
                        int value = 0;
                        string[] vals = strLabel.Split('|');
                        for (int b = 0; b < vals.Length; ++b)
                        {
                            string temp = vals[b].Trim();
                            int tempVal = 0;
                            if (temp.Length > 0 && int.TryParse(temp, out tempVal))
                            {
                                value |= 1 << tempVal;
                            }
                        }
                        strLabel = value.ToString();
                    }
                }
                else if (i == 1) //data type
                {
                    strLabel = dataTypes[j].Trim();
                    if (strLabel.Trim().Contains("bit|"))
                        strLabel = strLabel.Replace("bit|", "");
                    else if (strLabel.Trim().Contains("json"))
                        strLabel = strLabel.Replace("json", "string");
                }
                else if (i == 2) //extern desc
                {
                    strLabel = externLines[j].Trim();
                }
                else if (i == 3) //property name
                {
                    strLabel = strLabel.Trim();
                }
                if (i>=4 && dataTypes[j].ToLower().CompareTo("string") == 0)
                    stringBuilder.Append("\""+ strLabel + "\",");
                else if (i >= 4 && dataTypes[j].ToLower().CompareTo("json") == 0)
                    stringBuilder.Append("\"" + strLabel.Replace("\"", "\"\"") +"\",");
                else
                    stringBuilder.Append(strLabel + ",");
            }
            //使用换行符分割每一行
            stringBuilder.Append("\r\n");
        }
    }

    /// <summary>
    /// 转换为CSV
    /// </summary>
    public void ConvertToCSV (string CSVPath, Encoding encoding, bool bMulSheet)
	{
        if (mResultSet == null)
            return;
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
			return;

        StringBuilder stringBuilder = new StringBuilder();
        if (bMulSheet)
        {
            for(int i =0; i < mResultSet.Tables.Count; ++i)
                ConvertToCsv(stringBuilder, mResultSet.Tables[i], i <=0?0:4);
        }
        else
            ConvertToCsv(stringBuilder, mResultSet.Tables[0]);

        //写入文件
        using (FileStream fileStream = new FileStream(CSVPath, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream, encoding)) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

	/// <summary>
	/// 导出为Xml
	/// </summary>
	public void ConvertToXml (string XmlFile)
	{
        if (mResultSet == null)
            return;
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
			return;

		//默认读取第一个数据表
		DataTable mSheet = mResultSet.Tables [0];

		//判断数据表内是否存在数据
		if (mSheet.Rows.Count < 1)
			return;

		//读取数据表行数和列数
		int rowCount = mSheet.Rows.Count;
		int colCount = mSheet.Columns.Count;

		//创建一个StringBuilder存储数据
		StringBuilder stringBuilder = new StringBuilder ();
		//创建Xml文件头
		stringBuilder.Append ("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
		stringBuilder.Append ("\r\n");
		//创建根节点
		stringBuilder.Append ("<Table>");
		stringBuilder.Append ("\r\n");
		//读取数据
		for (int i = 1; i < rowCount; i++) {

            if (i >= BEGIN_ROW && IsEmptyRow(mSheet.Rows[i], colCount))
                continue;
            //创建子节点
            stringBuilder.Append ("  <Row>");
			stringBuilder.Append ("\r\n");
			for (int j = 0; j < colCount; j++) {
				stringBuilder.Append ("   <" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append (mSheet.Rows [i] [j].ToString ());
				stringBuilder.Append ("</" + mSheet.Rows [0] [j].ToString () + ">");
				stringBuilder.Append ("\r\n");
			}
			//使用换行符分割每一行
			stringBuilder.Append ("  </Row>");
			stringBuilder.Append ("\r\n");
		}
		//闭合标签
		stringBuilder.Append ("</Table>");
		//写入文件
		using (FileStream fileStream = new FileStream(XmlFile, FileMode.Create, FileAccess.Write)) {
			using (TextWriter textWriter = new StreamWriter(fileStream,Encoding.GetEncoding("utf-8"))) {
				textWriter.Write (stringBuilder.ToString ());
			}
		}
	}

    void WriteStringToBytes(List<byte> vBys, string strVale)
    {
        vBys.AddRange(new List<byte>(BitConverter.GetBytes((ushort)strVale.Length)));
        if (strVale.Length <= 0) return;
        vBys.AddRange(new List<byte>(System.Text.Encoding.UTF8.GetBytes(strVale)));
    }

    public void ConvertToBinary(string strFile)
    {
        if (mResultSet == null)
            return;
        //判断Excel文件中是否存在数据表
        if (mResultSet.Tables.Count < 1)
            return;

        //默认读取第一个数据表
        DataTable mSheet = mResultSet.Tables[0];

        //判断数据表内是否存在数据
        if (mSheet.Rows.Count < 4)
            return;

        //读取数据表行数和列数
        int rowCount = mSheet.Rows.Count;
        int colCount = mSheet.Columns.Count;

        List<byte> vHeads = new List<byte>();

        bool[] bValids = new bool[colCount];
        int validColCount = 0;
        int headLen = 0;
        for (int i = 0; i < colCount; ++i)
        {
            string dataType = mSheet.Rows[1][i].ToString();
            if (string.IsNullOrEmpty(dataType))
            {
                bValids[i] = false;
                //MessageBox.Show(Path.GetFileNameWithoutExtension(strFile) + "表:数据类型行不能有空类型!");
                continue;
            }

            if (dataType.Trim().Contains("bit|"))
                dataType = dataType.Replace("bit|", "");

            WriteStringToBytes(vHeads, dataType);
            validColCount++;
            bValids[i] = true;
        }
        string keyFun = mSheet.Rows[2][0].ToString();
        WriteStringToBytes(vHeads, keyFun);
        //!mapping
        for (int i = 1; i < colCount; ++i)
        {
            if (!bValids[i]) continue;
            string temp = mSheet.Rows[2][i].ToString();
            WriteStringToBytes(vHeads, temp);
        }
        //字段名
        for (int i = 0; i < colCount; ++i)
        {
            if (!bValids[i]) continue;
            string temp = mSheet.Rows[3][i].ToString();
            if (string.IsNullOrEmpty(temp))
            {
                MessageBox.Show(string.Format(Path.GetFileNameWithoutExtension(strFile) + "表:第{0}列名称字段有问题!", i));
                return;
            }
            WriteStringToBytes(vHeads, temp);
        }

        FileStream fs = new FileStream(strFile, FileMode.OpenOrCreate);
        BinaryWriter writer = new BinaryWriter(fs, System.Text.Encoding.UTF8);

        int valueRow = 0;
        for (int i = 4; i < rowCount; i++)
        {
            if (!bValids[0]) continue;
            string value = mSheet.Rows[i][0].ToString();
            if (string.IsNullOrEmpty(value))
                continue;
            valueRow++;
        }
        writer.Write(0);//version
        writer.Write((ushort)vHeads.Count);
        writer.Write(valueRow);
        writer.Write((int)validColCount);
        writer.Write(vHeads.ToArray());
        
        if(valueRow>0)
        {
            //读取数据
            for (int i = 4; i < rowCount; i++)
            {
                if (IsEmptyRow(mSheet.Rows[i], colCount))
                    continue;
                for (int j = 0; j < colCount; j++)
                {
                    if (!bValids[j]) continue;
                    string value = mSheet.Rows[i][j].ToString();
                    if (j == 0 && string.IsNullOrEmpty(value))
                        continue;
                    string dataType = mSheet.Rows[1][j].ToString();
                    if (!WriteData(writer, dataType, value))
                    {
                        MessageBox.Show(string.Format(Path.GetFileNameWithoutExtension(strFile) + "表:第{0}列类型{1}有问题!", j, mSheet.Rows[1][j].ToString()));
                        writer.Close();
                        return;
                    }
                }

            }
        }

        writer.Close();
    }

    void WriteStringToWritter(BinaryWriter writter, string strValue)
    {
        byte[] text = System.Text.Encoding.UTF8.GetBytes(strValue);
        writter.Write((ushort)text.Length);
        if (strValue.Length <= 0) return;
        writter.Write(text);
    }

    private bool WriteData(BinaryWriter writter, string strLabel, string strValue)
    {
        if (strLabel.Contains("enum|"))
        {
            int ret;
            if (!int.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.Contains("bit|"))
        {
            int ret = 0;
            string[] temps = strValue.Split('|');
            for(int i = 0; i < temps.Length; ++i)
            {
                int tempValue = 0;
                if (int.TryParse(temps[i], out tempValue))
                    ret = 1 << tempValue;
            }
            writter.Write(ret);
            return true;
        }
        char splitChar = '|';
        int pos = strLabel.IndexOf('|');
        if(pos>0 && pos < strLabel.Length)
        {
            string spec = strLabel.Substring(pos+1);
            strLabel = strLabel.Substring(0, pos);
            if (spec.Length>0)
                splitChar = spec[0];
        }
        if (strLabel.Length <= 0)
            return false;
        strLabel = strLabel.ToLower();
        if (strLabel.CompareTo("string") == 0)
        {
            WriteStringToWritter(writter, strValue);
            return true;
        }
        if (strLabel.CompareTo("bool") == 0)
        {
            writter.Write((strValue.ToLower()=="true" || strValue.ToLower() == "1"));
            return true;
        }
        if (strLabel.CompareTo("char") == 0)
        {
            char ret;
            if (!char.TryParse(strValue, out ret))
                ret = '0';
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("byte") == 0)
        {
            byte ret;
            if (!byte.TryParse(strValue, out ret))
                ret = 0;

            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("short") == 0)
        {
            short ret;
            if (!short.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("ushort") == 0)
        {
            ushort ret;
            if (!ushort.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("int") == 0)
        {
            int ret;
            if (!int.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("uint") == 0)
        {
            uint ret;
            if (!uint.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("long") == 0)
        {
            long ret;
            if (!long.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("ulong") == 0)
        {
            ulong ret;
            if (!ulong.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("float") == 0)
        {
            float ret;
            if (!float.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("double") == 0)
        {
            double ret;
            if (!double.TryParse(strValue, out ret))
                ret = 0;
            writter.Write(ret);
            return true;
        }
        if (strLabel.CompareTo("vector2") == 0 || strLabel.CompareTo("vec2") == 0)
        {
            if(string.IsNullOrEmpty(strValue))
            {
                writter.Write(0f);
                writter.Write(0f);
                return true;
            }
            else
            {
                string[] vals = strValue.Split(splitChar);
                float x, y;
                if (vals.Length == 2 && float.TryParse(vals[0], out x) && float.TryParse(vals[1], out y))
                {
                    writter.Write(x);
                    writter.Write(y);
                }
                else
                {
                    writter.Write(0f);
                    writter.Write(0f);
                }
            }
            return true;
        }
        if (strLabel.CompareTo("vector3") == 0 || strLabel.CompareTo("vec3") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write(0f);
                writter.Write(0f);
                writter.Write(0f);
                return true;
            }
            else
            {
                string[] vals = strValue.Split(splitChar);
                float x, y, z;
                if (vals.Length == 3 && float.TryParse(vals[0], out x) && float.TryParse(vals[1], out y) && float.TryParse(vals[2], out z))
                {
                    writter.Write(x);
                    writter.Write(y);
                    writter.Write(z);
                }
                else
                {
                    writter.Write(0f);
                    writter.Write(0f);
                    writter.Write(0f);
                }
            }
            return true;
        }
        if (strLabel.CompareTo("vector4") == 0 || strLabel.CompareTo("vec4") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write(0f);
                writter.Write(0f);
                writter.Write(0f);
                writter.Write(0f);
                return true;
            }
            else
            {
                string[] vals = strValue.Split(splitChar);
                float x, y, z, w;
                if (vals.Length == 4 && float.TryParse(vals[0], out x) && float.TryParse(vals[1], out y) && float.TryParse(vals[2], out z) && float.TryParse(vals[3], out w))
                {
                    writter.Write(x);
                    writter.Write(y);
                    writter.Write(z);
                    writter.Write(w);
                }
                else
                {
                    writter.Write(0f);
                    writter.Write(0f);
                    writter.Write(0f);
                    writter.Write(0f);
                }
            }
            return true;

        }
        if (strLabel.CompareTo("vector2int") == 0 || strLabel.CompareTo("vec2int") == 0 || strLabel.CompareTo("vec2i") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write(0);
                writter.Write(0);
                return true;
            }
            else
            {
                string[] vals = strValue.Split(splitChar);
                int x, y;
                if (vals.Length == 2 && int.TryParse(vals[0], out x) && int.TryParse(vals[1], out y))
                {
                    writter.Write(x);
                    writter.Write(y);
                    
                }
                else
                {
                    writter.Write(0);
                    writter.Write(0);
                }
            }
            return true;
        }
        if (strLabel.CompareTo("vector3int") == 0 || strLabel.CompareTo("vec3int") == 0 || strLabel.CompareTo("vec3i") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write(0);
                writter.Write(0);
                writter.Write(0);
                return true;
            }
            else
            {
                string[] vals = strValue.Split(splitChar);
                int x, y, z;
                if (vals.Length == 3 && int.TryParse(vals[0], out x) && int.TryParse(vals[1], out y) && int.TryParse(vals[2], out z))
                {
                    writter.Write(x);
                    writter.Write(y);
                    writter.Write(z);
                }
                else
                {
                    writter.Write(0);
                    writter.Write(0);
                    writter.Write(0);
                }
            }
            return true;
        }
        if (strLabel.CompareTo("byte[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for(int i = 0; i < vals.Length; ++i)
            {
                byte va;
                if (!byte.TryParse(vals[i], out va))
                    va = 0;
                writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("int[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                int va;
                if (!int.TryParse(vals[i], out va))
                    va = 0;
                writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("uint[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);

            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                uint va;
                if (!uint.TryParse(vals[i], out va))
                    va = 0;
                    writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("float[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                float va;
                if (!float.TryParse(vals[i], out va))
                    va = 0;
                writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("short[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                short va;
                if (!short.TryParse(vals[i], out va))
                    va = 0;
                writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("ushort[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                ushort va;
                if (!ushort.TryParse(vals[i], out va))
                    va = 0;
                writter.Write(va);
            }
            return true;
        }
        if (strLabel.CompareTo("vector2[]") == 0 || strLabel.CompareTo("vec2[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            if(vals.Length>0 && vals.Length%2 == 0)
            {
                writter.Write((ushort)(vals.Length/2));
                for (int i = 0; i < vals.Length; i+=2)
                {
                    float va;
                    if (!float.TryParse(vals[i + 0], out va)) va = 0;
                    writter.Write(va);
                    if (!float.TryParse(vals[i + 1], out va)) va = 0;
                    writter.Write(va);
                }
            }
            else
                writter.Write((ushort)0);
            return true;
        }
        if (strLabel.CompareTo("vector3[]") == 0 || strLabel.CompareTo("vec3[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            if (vals.Length > 0 && vals.Length % 3 == 0)
            {
                writter.Write((ushort)(vals.Length / 3));
                for (int i = 0; i < vals.Length; i += 3)
                {
                    float va;
                    if (!float.TryParse(vals[i + 0], out va)) va = 0;
                    writter.Write(va);
                    if (!float.TryParse(vals[i + 1], out va)) va = 0;
                    writter.Write(va);
                    if (!float.TryParse(vals[i + 2], out va)) va = 0;
                    writter.Write(va);
                }
            }
            else
                writter.Write((ushort)0);
            return true;
        }
        if (strLabel.CompareTo("vector2int[]") == 0 || strLabel.CompareTo("vec2int[]") == 0 || strLabel.CompareTo("vec2i[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            if (vals.Length > 0 && vals.Length % 2 == 0)
            {
                writter.Write((ushort)(vals.Length / 2));
                for (int i = 0; i < vals.Length; i += 2)
                {
                    int va;
                    if (!int.TryParse(vals[i + 0], out va)) va = 0;
                    writter.Write(va);
                    if (!int.TryParse(vals[i + 1], out va)) va = 0;
                    writter.Write(va);
                }
            }
            else
                writter.Write((ushort)0);
            return true;
        }
        if (strLabel.CompareTo("vector3int[]") == 0 || strLabel.CompareTo("vec3int[]") == 0 || strLabel.CompareTo("vec3i[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            if (vals.Length > 0 && vals.Length % 3 == 0)
            {
                writter.Write((ushort)(vals.Length / 3));
                for (int i = 0; i < vals.Length; i += 3)
                {
                    int va;
                    if (!int.TryParse(vals[i + 0], out va)) va = 0;
                    writter.Write(va);
                    if (!int.TryParse(vals[i + 1], out va)) va = 0;
                    writter.Write(va);
                    if (!int.TryParse(vals[i + 2], out va)) va = 0;
                    writter.Write(va);
                }
            }
            else
                writter.Write((ushort)0);
            return true;
        }
        if (strLabel.CompareTo("string[]") == 0)
        {
            if (string.IsNullOrEmpty(strValue))
            {
                writter.Write((ushort)0);
                return true;
            }
            string[] vals = strValue.Split(splitChar);
            writter.Write((ushort)vals.Length);
            for (int i = 0; i < vals.Length; ++i)
            {
                WriteStringToWritter(writter, strValue);
            }
            return true;
        }

        MessageBox.Show("不存在这样的数据类型:" + strLabel);
        return false;
    }

    /// <summary>
    /// 设置目标实例的属性
    /// </summary>
    private void SetTargetProperty (object target, string propertyName, object propertyValue)
	{
		//获取类型
		Type mType = target.GetType ();
		//获取属性集合
		PropertyInfo[] mPropertys = mType.GetProperties ();
		foreach (PropertyInfo property in mPropertys) {
			if (property.Name == propertyName) {
				property.SetValue (target, Convert.ChangeType (propertyValue, property.PropertyType), null);
			}
		}
	}
}

