/********************************************************************
生成日期:	25:7:2019   9:51
类    名: 	UploadAssets
作    者:	HappLI
描    述:	资源上传到指定的服务器地址
*********************************************************************/

using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace TopGame.ED
{
    public class UploadAssets
    {
        struct UploadData
        {
            public string strFile;
            public string remoteFile;
        }

        List<UploadData> m_vUploadFile = new List<UploadData>();
        public void Upload(string url, string localDir,string directory, List<string> vIngores = null)
        {
            localDir = localDir.Replace("\\", "/");
            if (Directory.Exists(localDir))
            {
                string[] files = Directory.GetFiles(localDir, "*.*", SearchOption.AllDirectories);
                if (localDir[localDir.Length - 1] != '/') localDir += "/";
                for (int i = 0; i < files.Length; ++i)
                {
                    bool bUp = true;
                    if (vIngores != null)
                    {
                        for (int j = 0; j < vIngores.Count; ++j)
                        {
                            if (files[i].Contains(vIngores[j]))
                            {
                                bUp = false;
                                break;
                            }
                        }
                    }
                    if (bUp)
                    {
                        string strRemote = files[i].Replace("\\", "/").Replace(localDir, "");
                        UploadData upData = new UploadData();
                        upData.remoteFile = strRemote;
                        upData.strFile = files[i];
                        m_vUploadFile.Add(upData);
                    }
                }

                if (m_vUploadFile.Count > 0)
                {
                    UnityEditor.EditorUtility.DisplayProgressBar("上传", "", 0);
                    for (int i = 0; i < m_vUploadFile.Count; ++i)
                    {
                        UploadData upData = m_vUploadFile[i];
                        UnityEditor.EditorUtility.DisplayProgressBar("上传", upData.remoteFile, (float)i / (float)m_vUploadFile.Count);

                        WWWForm wwwForm = new WWWForm();
                        wwwForm.AddField("filename", upData.remoteFile);
                        wwwForm.AddField("directory", directory);

                        byte[] bit = File.ReadAllBytes(upData.strFile);                        
                        string filedata = System.Convert.ToBase64String(bit); 
                        wwwForm.AddField("filedata", filedata);
                      
                        var request = UnityWebRequest.Post(url, wwwForm);
                        var asyncOp = request.SendWebRequest();
                        while (!asyncOp.isDone)
                        {
                            if(request.isNetworkError || request.isHttpError)
                            {
                                Debug.LogError(request.error);
                            }
                        }
                        Debug.Log(request.downloadHandler.text);
                    }
                    UnityEditor.EditorUtility.ClearProgressBar();
                }
            }
        }
    }
}