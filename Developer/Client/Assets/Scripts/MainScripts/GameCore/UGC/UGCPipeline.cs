/********************************************************************
生成日期:	1:11:2020 10:06
类    名: 	UGCPipeline
作    者:	HappLI
描    述:	UGC 执行管线
*********************************************************************/
using System;
using UnityEngine;
using System.Collections.Generic;
using TopGame.Core;
using Framework.Data;

namespace Framework.UGC
{
    public class UGCPipeline
    {
        private Dictionary<int, UGCBrick> m_vBricks;
        public static UGCPipeline Load(string strFile)
        {
            int dataSize = 0;
            byte[] bytes = JniPlugin.ReadFileAllBytes(strFile, ref dataSize, true);
            if (bytes != null && dataSize > 0)
            {
                BinaryUtil binaryer = new BinaryUtil();
                binaryer.Set(bytes, dataSize);
                binaryer.SeekBegin();
                try
                {
                    Dictionary<int, UGCBrick> bricks = null;
                    int version = (int)binaryer.ToByte();
                    int cnt = (int)binaryer.ToUshort();
                    for (int i = 0; i < cnt; ++i)
                    {
                        int brickType = binaryer.ToInt32();
                        UGCBrick brick = null;
                        if (brick == null) return null;

                        brick.Read(ref binaryer);
                        if (bricks == null) bricks = new Dictionary<int, UGCBrick>(cnt);
                        bricks.Add(brick.GetID(), brick);
                    }

                    UGCPipeline pipeline = new UGCPipeline();
                    pipeline.m_vBricks = bricks;
                    return pipeline;
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }
        //------------------------------------------------------
        public static void Write(UGCPipeline pipeline, string strFile)
        {
            BinaryUtil binaryer = new BinaryUtil();
            binaryer.WriteByte(0);//version
            if(pipeline.m_vBricks!=null)
            {
                binaryer.WriteUshort((ushort)pipeline.m_vBricks.Count);
                foreach(var db in pipeline.m_vBricks)
                {
                    db.Value.Write(ref binaryer);
                }
            }
            else
            {
                binaryer.WriteUshort(0);
            }
            binaryer.SaveTo(strFile);
        }
    }

}

