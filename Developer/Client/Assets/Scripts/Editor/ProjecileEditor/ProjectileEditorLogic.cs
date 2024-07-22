/********************************************************************
生成日期:	25:7:2019   14:35
类    名: 	ProjectileEditorLogic
作    者:	HappLI
描    述:	飞行道具编辑器
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using System.Linq;
using System.IO;
using TopGame.Data;
using TopGame.Core;
using UnityEditor.IMGUI.Controls;
using Framework.Core;
using Framework.ED;
using Framework.Data;
using ExternEngine;

namespace TopGame.ED
{
    public class ProjectileEditorLogic
    {
        public class ProjectTrack
        {
            public List<Vector3> vTracks;
            public Projectile pProjectile;

            private bool m_bInited = false;
            public Vector3 backupSpeed = Vector3.zero;
            public Vector3 backupPosition = Vector3.zero;
            public Vector3 backupAcceleration = Vector3.zero;

            public Color trackColor = Color.white;
            public ProjectTrack(Projectile pProj)
            {
                pProjectile = pProj;
                vTracks = new List<Vector3>();

                backupPosition = pProjectile.position;
                backupSpeed = pProjectile.speed;
                backupAcceleration = pProjectile.acceleration;
            }
            public void Destroy()
            {
                if(pProjectile!=null) pProjectile.Destroy();
            }
            public void TestTrack(Vector3 vStart, Vector3 vEnd)
            {
                if (pProjectile == null) return;
                if (pProjectile.projectile == null)
                    return;
                Color color = Handles.color;
                Handles.color = trackColor;

                if (pProjectile.projectile.type == EProjectileType.TrackPath)
                {
                    pProjectile.BuildTrackPathKeyframe(vStart, vEnd, pProjectile.projectile.speedLerp.x);
                    pProjectile.DrawTrackPath(trackColor);
                }
                else
                {
                    int trackCnt = 99;
                    vTracks.Clear();
                    vTracks.Add(vStart);
                    pProjectile.position = backupPosition;
                    pProjectile.speed = backupSpeed;
                    pProjectile.acceleration = backupAcceleration;
                    pProjectile.remain_life_time = pProjectile.projectile.life_time;
                    pProjectile.delta = (pProjectile.projectile.launch_delay + 0.0666f);
                    pProjectile.speed = backupSpeed;
                    pProjectile.track_begin = false;
                    pProjectile.track_end = false;
                    pProjectile.SetBounceTypeCount((int)pProjectile.projectile.speedLerp.x);
                    while (pProjectile.remain_life_time >= 0 && trackCnt >= 0)
                    {
                        if (vTracks.Count <= 0 || (vTracks[vTracks.Count - 1] - pProjectile.position).sqrMagnitude > 0.05f)
                            vTracks.Add(pProjectile.position);
                        pProjectile.Update(FFloat.FRAMEFPS30);
                        trackCnt--;
                    }


                    float len = 0;
                    for (int i = 0; i < vTracks.Count - 1; ++i)
                    {
                        Handles.DrawLine(vTracks[i], vTracks[i + 1]);

                        Vector3 toDir = vTracks[i + 1] - vTracks[i];
                        if (toDir.sqrMagnitude <= 0) toDir = Vector3.forward;
                        len += toDir.magnitude;
                        if (len >= 2)
                        {
                            Quaternion qt = Quaternion.LookRotation(toDir.normalized);
                            Handles.ArrowHandleCap(0, vTracks[i], qt, HandleUtility.GetHandleSize(vTracks[i]), EventType.Repaint);
                            len = 0;
                        }
                    }
                }
                Handles.color = color;
            }
        }
        public class ProjectileItem : TreeAssetView.ItemData
        {
            public int editorID = 0;
            public ProjectileData pData;
            public override Color itemColor()
            {
                return Color.white;
            }
        }

        EditorModule m_pGameModuel;
        ProjectileEditor m_pEditor;

        ProjectileData m_pCurItem;
        float m_PlayRuntime = 0;

        Vector2 m_Scroll = Vector2.zero;

        bool m_bExpandSpeedAcc = false;
        UnityEditor.IMGUI.Controls.TreeViewState m_TreeState;
        UnityEditor.IMGUI.Controls.MultiColumnHeaderState m_AssetListMCHState;
        TreeAssetView m_pTreeView = null;

        List<ProjectTrack> m_TestProjectile = new List<ProjectTrack>();
        GameObject m_pPreveObject = null;
        Actor m_pSimulateActor = null;
        Actor m_pTargetActor = null;
        GameObject m_pWaringObj = null;

        bool m_bDirtyRefresh = false;
        //-----------------------------------------------------
        public void OnEnable(ProjectileEditor pEditor, EditorModule moude)
        {
            m_pEditor = pEditor;
            m_pGameModuel = moude;

            m_TestProjectile = new List<ProjectTrack>();
            {
                MultiColumnHeaderState.Column[] colums = new MultiColumnHeaderState.Column[] 
                {
                new MultiColumnHeaderState.Column(),
                new MultiColumnHeaderState.Column()
                };

                colums[0].headerContent = new GUIContent("ID", "");
                colums[0].minWidth = 100;
                colums[0].width = 100;
                colums[0].maxWidth = 300;
                colums[0].headerTextAlignment = TextAlignment.Left;
                colums[0].canSort = true;
                colums[0].autoResize = true;

                colums[1].headerContent = new GUIContent("描述", "");
                colums[1].minWidth = 100;
                colums[1].width = pEditor.position.width - 85;
                colums[1].maxWidth = pEditor.position.width - 85;
                colums[1].headerTextAlignment = TextAlignment.Left;
                colums[1].canSort = false;
                colums[1].autoResize = true;

                var headerState = new MultiColumnHeaderState(colums);
                m_AssetListMCHState = headerState;

                m_TreeState = new UnityEditor.IMGUI.Controls.TreeViewState();
                m_pTreeView = new TreeAssetView(m_TreeState, m_AssetListMCHState);
                m_pTreeView.Reload();

                m_pTreeView.OnItemDoubleClick = OnSelect;
                m_pTreeView.OnSelectChange = OnSelect;
                m_pTreeView.OnCellDraw += OnCellGUI;

                RefreshList();
            }

            CsvData_Player csvPlayer = DataEditorUtil.GetTable<CsvData_Player>(true);
            if (csvPlayer!=null)
            {
                DataEditorUtil.MappingTable(csvPlayer);
                foreach (var db in csvPlayer.datas)
                {
                    if (db.Value.Models_nModelID_data == null || string.IsNullOrEmpty(db.Value.Models_nModelID_data.strFile)) continue;
                    if (AssetDatabase.LoadAssetAtPath<GameObject>(db.Value.Models_nModelID_data.strFile) == null) continue;
                    m_pSimulateActor = moude.world.CreateNode(EActorType.Player, db.Value) as Actor;
                    m_pTargetActor = moude.world.CreateNode(EActorType.Player, db.Value) as Actor;
                    break;
                }
            }

            m_pTargetActor.SetDirection(-Vector3.forward);
            m_pTargetActor.SetPosition(Vector3.forward * 10);
            m_pSimulateActor.SetDirection(Vector3.forward);
            m_pSimulateActor.SetPosition(Vector3.zero);


            //             foreach (var db in Data.DataManager.getInstance().Projectile.datas)
            //             {
            //                 db.Value.aabb_min = new Vector3(db.Value.aabb_min.z, db.Value.aabb_min.y, db.Value.aabb_min.x);
            //                 db.Value.aabb_max = new Vector3(db.Value.aabb_max.z, db.Value.aabb_max.y, db.Value.aabb_max.x);
            //                 if(db.Value.speeds != null)
            //                 {
            //                     for(int i=0; i < db.Value.speeds.Length; ++i)
            //                         db.Value.speeds[i] = new Vector3(db.Value.speeds[i].z, db.Value.speeds[i].y, db.Value.speeds[i].x);
            //                 }
            //                 if (db.Value.accelerations != null)
            //                 {
            //                     for (int i = 0; i < db.Value.accelerations.Length; ++i)
            //                         db.Value.accelerations[i] = new Vector3(db.Value.accelerations[i].z, db.Value.accelerations[i].y, db.Value.accelerations[i].x);
            //                 }
            //                 db.Value.track_target_offset = new Vector3(db.Value.track_target_offset.z, db.Value.track_target_offset.y, db.Value.track_target_offset.x);
            //             }
            //             Data.DataManager.getInstance().Projectile.Save();

            //foreach (var db in Data.DataManager.getInstance().Projectile.datas)
            //{
            //    db.Value.speedLerp = Vector2.one * 10;
            //    List<Vector3> maxSpeeds = db.Value.speedMaxs != null ? new List<Vector3>(db.Value.speedMaxs) : new List<Vector3>();
            //    List<Vector3> lowSpeeds = db.Value.speedLows != null ? new List<Vector3>(db.Value.speedLows) : new List<Vector3>();
            //    List<Vector3> upperSpeeds = db.Value.speedUppers != null ? new List<Vector3>(db.Value.speedUppers) : new List<Vector3>();
            //    List<Vector3> acces = db.Value.accelerations != null ? new List<Vector3>(db.Value.accelerations) : new List<Vector3>();
            //    for (int i = 0; i < lowSpeeds.Count; ++i)
            //    {
            //        maxSpeeds.Add(Vector3.zero);
            //    }
            //    if (db.Value.type == EProjectileType.Track)
            //    {
            //        for (int i = 0; i < acces.Count; ++i)
            //        {
            //            maxSpeeds[i] = new Vector3(acces[i].y, acces[i].y, acces[i].y);
            //            acces[i] = new Vector3(acces[i].z, acces[i].z, acces[i].z);
            //        }
            //    }
            //    db.Value.speedMaxs = maxSpeeds.ToArray();
            //    db.Value.speedLows = lowSpeeds.ToArray();
            //    db.Value.speedUppers = upperSpeeds.ToArray();
            //    db.Value.accelerations = acces.ToArray();
            //}
            //Data.DataManager.getInstance().Projectile.Save();
        }
        //-----------------------------------------------------
        public void OnDisable()
        {
            Clear();
            if (m_pSimulateActor != null)
            {
                m_pSimulateActor.Destroy();
            }
            if (m_pTargetActor != null)
            {
                m_pTargetActor.Destroy();
            }
            m_pGameModuel.Destroy();
        }
        //-----------------------------------------------------
        public void Clear()
        {
            ClearTarget();
        }
        //-----------------------------------------------------
        void ClearTarget()
        {
            if(m_pGameModuel!=null && m_pGameModuel.GetProjectileManager()!=null) m_pGameModuel.GetProjectileManager().StopAllProjectiles();
            if (m_pWaringObj) GameObject.DestroyImmediate(m_pWaringObj);
            m_pWaringObj = null;
            if (m_pPreveObject != null) GameObject.DestroyImmediate(m_pPreveObject);
            m_pPreveObject = null;
            m_pCurItem = null;
        }
        //-----------------------------------------------------
        public void Load()
        {

        }
        //-----------------------------------------------------
        public void Play(bool bPlay)
        {
            m_PlayRuntime = 0;
            if (bPlay)
            {
                if(m_pCurItem != null)
                {
                    CsvData_Projectile projectile = DataEditorUtil.GetTable<CsvData_Projectile>(false);
                    if (projectile != null)
                        m_pGameModuel.GetProjectileManager().SetDatas(projectile.datas);

                    if (m_pSimulateActor != null) m_pSimulateActor.SetPosition(Vector3.zero);
                     Transform trackTrans = null;
                    if (m_pTargetActor != null && m_pTargetActor.GetObjectAble()!=null)
                        trackTrans = m_pTargetActor.GetObjectAble().GetTransorm();

                    RefreshTestProject(m_pSimulateActor, m_pCurItem, m_pTargetActor);
                    m_pGameModuel.GetProjectileManager().LaunchProjectile((uint)m_pCurItem.id, m_pSimulateActor, null, null,0, Vector3.up, Vector3.forward, m_pTargetActor, 0, 0, trackTrans);
                }
            }
        }
        //-----------------------------------------------------
        public bool isPlay()
        {
            return false;
        }
        //-----------------------------------------------------
        void OnSelect(TreeAssetView.ItemData data)
        {
            ProjectileItem item = data as ProjectileItem;
            OnChangeSelect(item.pData);
        }
        //-----------------------------------------------------
        void RefreshTestProject(AWorldNode pOwnerActor, ProjectileData pData, AWorldNode targetNode)
        {
            for(int i = 0; i < m_TestProjectile.Count; ++i)
            {
                m_TestProjectile[i].Destroy();
            }

            Transform pTrackTransform = null;
            if (targetNode != null && targetNode.GetObjectAble() != null)
                pTrackTransform = m_pTargetActor.GetObjectAble().GetTransorm();

            m_TestProjectile.Clear();
            FVector3 up = FVector3.up;
            FVector3 vDirection = pOwnerActor.GetDirection();
            FVector3 vPosition = pOwnerActor.GetPosition();
            FVector3 vRight = FVector3.Cross(vDirection, up);
            FFloat damage_power = FFloat.zero;
            uint track_frame_id = 0xffffffff;
            uint track_body_id = 0xffffffff;
            Transform pTrackSlot = null;
            FVector3 trackOffset = FVector3.zero;
            m_pGameModuel.GetProjectileManager().TrackCheck(targetNode, vPosition, pData, pTrackTransform, ref pTrackSlot, ref damage_power, ref track_frame_id, ref track_body_id, ref trackOffset);

            if (pData.speedLows != null && pData.speedLows.Length > 0)
            {
                for (int i = 0; i < pData.speedLows.Length; ++i)
                {
                    Projectile pProjectile = new Projectile(m_pGameModuel);
                    pProjectile.SetContextData(pData);

                    pProjectile.Reset();
                    pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.InitEffect(false);
                    pProjectile.SetSpeed(vPosition, vDirection, i);
                    pProjectile.speed = vDirection * pData.speedLows[i].z + up * pData.speedLows[i].y + vRight * pData.speedLows[i].x;
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.delta = (pData.launch_delay + 0.0666f);


                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);

                    ProjectTrack trackProj = new ProjectTrack(pProjectile);
                    trackProj.trackColor = Color.red;
                    m_TestProjectile.Add(trackProj);
                }
                for (int i = 0; i < pData.speedUppers.Length; ++i)
                {
                    Projectile pProjectile = new Projectile(m_pGameModuel);
                    pProjectile.SetContextData(pData);

                    pProjectile.Reset();
                    pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                    pProjectile.InitEffect(false);
                    pProjectile.SetSpeed(vPosition, vDirection, i);
                    pProjectile.speed = vDirection * pData.speedUppers[i].z + up * pData.speedUppers[i].y + vRight * pData.speedUppers[i].x;
                    pProjectile.SetTrack(pTrackSlot, trackOffset);
                    pProjectile.delta = (pData.launch_delay + 0.0666f);


                    pProjectile.SetVisible(true);
                    pProjectile.SetActived(true);
                    pProjectile.EnableLogic(true);
                    pProjectile.SetSpatial(true);
                    pProjectile.SetCollectAble(false);

                    ProjectTrack trackProj = new ProjectTrack(pProjectile);
                    trackProj.trackColor = Color.yellow;
                    m_TestProjectile.Add(trackProj);
                }
            }
            else if (pData.life_time > 0)
            {
                Projectile pProjectile = new Projectile(m_pGameModuel);
                pProjectile.SetContextData(pData);

                pProjectile.Reset();

                pProjectile.SetData(pData, pOwnerActor, targetNode, vPosition, vDirection, damage_power, track_frame_id, track_body_id);
                pProjectile.InitEffect(false);
                pProjectile.delta = (pData.launch_delay + 0.0666f);
                pProjectile.SetTrack(pTrackSlot, trackOffset);


                pProjectile.SetVisible(true);
                pProjectile.SetActived(true);
                pProjectile.EnableLogic(true);
                pProjectile.SetSpatial(true);
                pProjectile.SetCollectAble(false);
                //! call back
                m_TestProjectile.Add(new ProjectTrack(pProjectile));
            }
        }
        //-----------------------------------------------------
        void OnChangeSelect(ProjectileData  item)
        {
            if (item == m_pCurItem)
                return;
            ClearTarget();
            m_pCurItem = item;

            if (m_pCurItem == null) return;
            //creat test projectile
            RefreshTestProject(m_pSimulateActor, item, m_pTargetActor);

            GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.effect);
            if(pObj!=null)
            {
                m_pPreveObject = GameObject.Instantiate<GameObject>(pObj);
                Base.Util.ResetGameObject(m_pPreveObject, Base.EResetType.All);
            }
            pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.waring_effect);
            if (pObj)
            {
                m_pWaringObj = GameObject.Instantiate<GameObject>(pObj);
                Base.Util.ResetGameObject(m_pWaringObj, Base.EResetType.All);
            }
        }
        //-----------------------------------------------------
        public void Update(float fFrameTime)
        {
            if (fFrameTime >= 0.03333) fFrameTime = 0.03333f;
            m_PlayRuntime += fFrameTime;
           // UpdateParticle();
           if(m_bDirtyRefresh)
           {
                m_bDirtyRefresh = false;
                if(m_pTreeView!=null)
                {
                    int selectId = m_pCurItem != null ? (int)m_pCurItem.id : -1;
                    RefreshList(false);
                    if (selectId >= 0)
                        m_pTreeView.SetSelection(new List<int>(new int[] { (int)m_pCurItem.id }));
                }
            }
        }
        //-----------------------------------------------------
        void UpdateParticle()
        {
            if (Application.isPlaying) return;
            ParticleSystem[] systems = GameObject.FindObjectsOfType<ParticleSystem>();
            if (systems == null) return;
            for (int i = 0; i < systems.Length; ++i)
            {
                systems[i].Play();
                systems[i].Simulate(m_PlayRuntime);
            }
        }
        //-----------------------------------------------------
        public void OnEvent(Event evt)
        {

        }
        //-----------------------------------------------------
        public void SaveData()
        {
            CsvData_Projectile csv = DataEditorUtil.GetTable<CsvData_Projectile>();
            if (csv != null) csv.Save();
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
        }
        //-----------------------------------------------------
        public void Realod()
        {
            CsvData_Projectile csv = DataEditorUtil.GetTable<CsvData_Projectile>();
            DataEditorUtil.MappingTable(csv);
            RefreshList();
        }
        //-----------------------------------------------------
        void RefreshList(bool bRealod = true)
        {
            CsvData_Projectile projectile = DataEditorUtil.GetTable<CsvData_Projectile>(bRealod);
            if (projectile == null) return;
            DataEditorUtil.MappingTable(projectile);
            m_pTreeView.BeginTreeData();
            foreach (var db in  projectile.datas)
            {
                ProjectileItem item = new ProjectileItem();
                item.pData = db.Value;
                item.id = (int)db.Key;
                item.editorID = item.id;
                item.name = db.Value.desc + db.Value.classify + db.Value.id;
                m_pTreeView.AddData(item);

//                 if (db.Value.type == EProjectileType.Projectile ||
//                     db.Value.type == EProjectileType.TrackPoint ||
//                     db.Value.type == EProjectileType.Projectile)
//                 {
//                     if (db.Value.speedLows != null)
//                     {
//                         for (int i = 0; i < db.Value.speedLows.Length; ++i)
//                             db.Value.speedLows[i] *= 1.5f;
//                     }
//                     if (db.Value.speedUppers != null)
//                     {
//                         for (int i = 0; i < db.Value.speedUppers.Length; ++i)
//                             db.Value.speedUppers[i] *= 1.5f;
//                     }
//                     if(db.Value.accelerations !=null)
//                     {
//                         for (int i = 0; i < db.Value.accelerations.Length; ++i)
//                             db.Value.accelerations[i] *= 1.5f;
//                     }
//                 }
//                 if (db.Value.type == EProjectileType.TrackPath)
//                     db.Value.speedLerp *= 1.5f;
// 
//                 if (ProjectileData.IsTrack(db.Value.type))
//                 {
//                     db.Value.speedLerp.x *= 1.5f;
//                 }
            }
            projectile.Save();
            m_pGameModuel.GetProjectileManager().SetDatas(projectile.datas);
            m_pTreeView.EndTreeData();
        }
        //-----------------------------------------------------
        bool OnCellGUI(Rect cellRect, TreeAssetView.TreeItemData item, int column, bool bSelected, bool focused)
        {
            ProjectileItem data = item.data as ProjectileItem;
            data.name = data.pData.desc + data.pData.classify + data.id;
            if (column == 0)
            {
                int.TryParse(GUI.TextField(new Rect(0, cellRect.y, cellRect.width - 40, cellRect.height), data.editorID.ToString()), out data.editorID);
                if(data.editorID != data.pData.id)
                {
                    if(GUI.Button(new Rect(cellRect.width - 40, cellRect.y, 40, cellRect.height), "使用"))
                    {
                        CsvData_Projectile projectile = DataEditorUtil.GetTable<CsvData_Projectile>();
                        if(projectile.datas.ContainsKey((uint)data.editorID))
                        {
                            EditorUtility.DisplayDialog("提示", "该id 已存在", "晓得");
                        }
                        else
                        {
                            projectile.datas.Remove(data.pData.id);
                            data.pData.id = (uint)data.editorID;
                            projectile.datas[data.pData.id] = data.pData;
                            m_bDirtyRefresh = true;
                        }
                    }
                }
            }
            else if (column == 1)
            {
                GUI.Label(cellRect, data.pData.desc);
            }
            return true;
        }
        //-----------------------------------------------------
        public void OnGUI()
        {
        }
        //-----------------------------------------------------
        public void DrawPreview(Rect rc)
        {
            
        }
        //-----------------------------------------------------
        public void OnSceneGUI()
        {
#if !UNITY_5_1
            UnityEngine.Rendering.CompareFunction zTest = Handles.zTest;
            Handles.zTest = UnityEngine.Rendering.CompareFunction.LessEqual;
#endif
            foreach (var db in m_pGameModuel.GetProjectileManager().GetRunningProjectile())
            {
                Vector3 vSpeedDirection = db.Value.speed;
                if (vSpeedDirection.sqrMagnitude > 0.01f)
                    vSpeedDirection.Normalize();
                else
                    vSpeedDirection = db.Value.direction;
                RenderVolumeByColor(ref db.Value.projectile.aabb_min, ref db.Value.projectile.aabb_max, db.Value.position, Quaternion.LookRotation(vSpeedDirection), EditorKits.GetVolumeToColor(EVolumeType.Attack), 1.0f, false);
            }

            if (m_pCurItem != null)
            {
                Vector3 position = Vector3.zero;
                Quaternion rotation = Quaternion.identity;
                if(m_pPreveObject != null)
                {
                    position = m_pPreveObject.transform.position;
                    rotation = m_pPreveObject.transform.rotation;
                }
                if (m_pCurItem.collisionType == EActorCollisionType.BOX)
                    RenderVolumeByColor(ref m_pCurItem.aabb_min, ref m_pCurItem.aabb_max, position, rotation, EditorKits.GetVolumeToColor(EVolumeType.Attack), 1.0f, true);
                else if (m_pCurItem.collisionType == EActorCollisionType.CAPSULE)
                    RenderSphereByColor(ref m_pCurItem.aabb_min.x, position, rotation, EditorKits.GetVolumeColor(EVolumeType.Attack), 1.0f, true);

                m_pSimulateActor.SetPosition(Handles.DoPositionHandle(m_pSimulateActor.GetPosition(), Quaternion.identity));
                m_pTargetActor.SetPosition(Handles.DoPositionHandle(m_pTargetActor.GetPosition(), Quaternion.identity));
                for(int i = 0; i < m_TestProjectile.Count; ++i)
                {
                    m_TestProjectile[i].TestTrack(m_pSimulateActor.GetPosition(), m_pTargetActor.GetPosition());
                }

                if (m_pCurItem.explode_range > 0)
                {
                    Framework.Core.CommonUtility.DrawBoundingBox(Vector3.zero, Vector3.one * m_pCurItem.explode_range, Matrix4x4.identity, Color.cyan, false);
                }
               // TestDrawLineTrace(m_pCurItem, m_pSimulateActor.GetPosition(), m_pSimulateActor.GetDirection(), 1/30f);

                if(m_pCurItem.type == EProjectileType.TrackPath)
                {
                    if(m_pCurItem.speedMaxs!=null && m_pCurItem.speedMaxs.Length>0 && m_pCurItem.accelerations.Length>0)
                    {
                        EProjectileParabolicType parabolicType = (EProjectileParabolicType)m_pCurItem.accelerations[0].y;
                        if (parabolicType == EProjectileParabolicType.StartEnd)
                        {
                            if (m_pCurItem.speedMaxs.Length == 2)
                            {
                                Handles.color = Color.green;
                                Vector3 leftCenter =  m_pCurItem.speedMaxs[0] + m_pSimulateActor.GetPosition();
                                m_pCurItem.speedUppers[0] = Handles.DoPositionHandle(m_pCurItem.speedUppers[0] + leftCenter, Quaternion.identity) - leftCenter;
                                Handles.DrawLine(leftCenter, m_pCurItem.speedUppers[0] + leftCenter);
                                if (m_pCurItem.speedUppers[0].sqrMagnitude > 0)
                                    Handles.ArrowHandleCap(0, m_pCurItem.speedUppers[0] + leftCenter, Quaternion.LookRotation(m_pCurItem.speedUppers[0]), 0.1f, EventType.Repaint);

                                Handles.color = Color.red;
                                Vector3 rightCenter = m_pCurItem.speedMaxs[1] + m_pTargetActor.GetPosition();
                                m_pCurItem.speedLows[1] = Handles.DoPositionHandle(m_pCurItem.speedLows[1] + rightCenter, Quaternion.identity) - rightCenter;
                                Handles.DrawLine(rightCenter, m_pCurItem.speedLows[1] + rightCenter);
                                if (m_pCurItem.speedLows[1].sqrMagnitude > 0)
                                    Handles.ArrowHandleCap(0, m_pCurItem.speedLows[1] + rightCenter, Quaternion.LookRotation(m_pCurItem.speedLows[1]), 0.1f, EventType.Repaint);
                            }
                        }
                        else
                        {
                            for (int i = 0; i < m_pCurItem.speedMaxs.Length; ++i)
                            {
                                Vector3 center = m_pCurItem.speedMaxs[i] + position;
                                float handleSize = Mathf.Min(1, HandleUtility.GetHandleSize(m_pCurItem.speedMaxs[i] + position));
                                Handles.SphereHandleCap(0, center, Quaternion.identity, handleSize, EventType.Repaint);
                                if (Event.current.shift)
                                {
                                    if(i >0)
                                    {
                                        Handles.color = Color.green;
                                        m_pCurItem.speedLows[i] = Handles.DoPositionHandle(m_pCurItem.speedLows[i] + center, Quaternion.identity) - center;
                                        Handles.DrawLine(center, m_pCurItem.speedLows[i] + center);
                                        if (m_pCurItem.speedLows[i].sqrMagnitude > 0)
                                            Handles.ArrowHandleCap(0, m_pCurItem.speedLows[i] + center, Quaternion.LookRotation(m_pCurItem.speedLows[i]), 0.1f, EventType.Repaint);
                                    }

                                    if (i < m_pCurItem.speedMaxs.Length-1)
                                    {
                                        Handles.color = Color.red;
                                        m_pCurItem.speedUppers[i] = Handles.DoPositionHandle(m_pCurItem.speedUppers[i] + center, Quaternion.identity) - center;
                                        Handles.DrawLine(center, m_pCurItem.speedUppers[i] + center);
                                        if (m_pCurItem.speedUppers[i].sqrMagnitude > 0)
                                            Handles.ArrowHandleCap(0, m_pCurItem.speedUppers[i] + center, Quaternion.LookRotation(m_pCurItem.speedUppers[i]), 0.1f, EventType.Repaint);
                                    }
                                }
                                else
                                {
                                    m_pCurItem.speedMaxs[i] = Handles.PositionHandle(center, Quaternion.identity) - position;
                                }
                            }
                            if (!Event.current.shift)
                            {
                                for (int i = 0; i < m_pCurItem.speedMaxs.Length; ++i)
                                {
                                    Vector3 point = m_pCurItem.speedMaxs[i] + position;
                                    Vector2 position2 = HandleUtility.WorldToGUIPoint(point);
                                    {
                                        GUILayout.BeginArea(new Rect(position2.x, position2.y, 100, 25));
                                        GUILayout.BeginHorizontal();
                                        GUILayout.Label("point[" + i + "]");
                                        if (GUILayout.Button("移除"))
                                        {
                                            m_pCurItem.RemoveTrackPoint(i);
                                            break;
                                        }
                                        GUILayout.EndHorizontal();
                                        GUILayout.EndArea();
                                    }
                                    if (m_pCurItem.speedMaxs.Length > 1)
                                    {
                                        Vector3 insert = (m_pCurItem.speedMaxs[(i + 1) % m_pCurItem.speedMaxs.Length] + m_pCurItem.speedMaxs[i]) / 2 + position;
                                        {
                                            Vector2 insertgui = HandleUtility.WorldToGUIPoint(insert);
                                            GUILayout.BeginArea(new Rect(insertgui.x, insertgui.y, 40, 25));
                                            if (GUILayout.Button("插入"))
                                            {
                                                m_pCurItem.InsertTrackPoint((i + 1) % m_pCurItem.speedMaxs.Length, insert - position, FVector3.zero, FVector3.zero);
                                                break;
                                            }
                                            GUILayout.EndArea();
                                        }
                                    }
                                }
                            }
                        }
                        
                    }
                }
            }
#if !UNITY_5_1
            Handles.zTest = zTest;
#endif
        }
        //-----------------------------------------------------
        static string[] SPEED_AXIS_NAME = {"X轴速度", "Y轴速度", "Z轴速度" };
        static string[] ROTATE_AXIS_NAME = { "X轴旋转", "Y轴旋转", "Z轴旋转" };
        public void OnDrawInspecPanel(Vector2 size)
        {
            if (m_pCurItem == null) return;
            if(GUILayout.Button("删除", new GUILayoutOption[] { GUILayout.Width(size.x-6) }))
            {
                if(EditorUtility.DisplayDialog("确认", "是否确定删除?", "确定", "再想想"))
                {
                    CsvData_Projectile projectileCsv = DataEditorUtil.GetTable<CsvData_Projectile>();
                    var selects = m_pTreeView.GetSelection();
                    if (selects != null && selects.Count > 1)
                    {
                        for (int i = 0; i < selects.Count; ++i)
                        {
                            var data = m_pTreeView.GetItem(selects[i]) as ProjectileItem;
                            if (data != null)
                            {
                                projectileCsv.datas.Remove(data.pData.id);
                                if(m_pCurItem == data.pData)
                                {
                                    OnChangeSelect(null);
                                }
                            }
                        }
                    }
                    else
                    {
                        projectileCsv.datas.Remove((uint)m_pCurItem.id);
                        OnChangeSelect(null);
                    }

                    RefreshList(false);
                    return;
                }

            }
            if (m_pCurItem!=null && m_pSimulateActor!=null && m_pTargetActor!=null && GUILayout.Button("刷新预览曲线", new GUILayoutOption[] { GUILayout.Width(size.x - 6) }))
            {
                RefreshTestProject(m_pSimulateActor, m_pCurItem, m_pTargetActor);
            }

            m_Scroll = EditorGUILayout.BeginScrollView(m_Scroll);

            ProjectileData projectile = m_pCurItem;
            projectile.desc = EditorGUILayout.TextField("名称描述",projectile.desc);
     //       projectile.type = (Core.EProjectileType)EditorGUILayout.EnumPopup("类型", projectile.type);
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "type");

            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "classify");

            float labelWidth = EditorGUIUtility.labelWidth;
            if(projectile.type != EProjectileType.Trap)
            {
                if(projectile.type == EProjectileType.TrackPath)
                {
                    List<Vector3> vInTags = (projectile.speedLows != null) ? new List<Vector3>(projectile.speedLows) : new List<Vector3>();
                    List<Vector3> vOutTags = (projectile.speedUppers != null) ? new List<Vector3>(projectile.speedUppers) : new List<Vector3>();
                    List<Vector3> vPoint = (projectile.speedMaxs != null) ? new List<Vector3>(projectile.speedMaxs) : new List<Vector3>();
                    List<Vector3> vAccSpeed = (projectile.accelerations != null) ? new List<Vector3>(projectile.accelerations) : new List<Vector3>();
                    m_bExpandSpeedAcc = EditorGUILayout.Foldout(m_bExpandSpeedAcc, "路径点");
                    if(m_bExpandSpeedAcc)
                    {
                        float width = size.x - 40;
                        GUILayoutOption[] layOp = new GUILayoutOption[] { GUILayout.Width(width / 4) };
                        GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(width) });
                        GUILayout.Label("index", layOp);
                        GUILayout.Label("点", layOp);
                        GUILayout.Label("速度", layOp);
                        GUILayout.Label("", layOp);
                        GUILayout.EndHorizontal();

                        for (int i = 0; i < vPoint.Count; ++i)
                        {
                            Vector3 inTag = vInTags[i];
                            Vector3 outTag = vOutTags[i];
                            Vector3 point = vPoint[i];
                            Vector3 accSpeed = vAccSpeed[i];

                            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(width) });
                            EditorGUILayout.LabelField((i + 1).ToString(), layOp);
                            point = EditorGUILayout.Vector3Field("",point, layOp);
                            accSpeed.x = Mathf.Max(0.1f, EditorGUILayout.FloatField("", accSpeed.x, layOp));
                            if (GUILayout.Button("删除", layOp))
                            {
                                vInTags.RemoveAt(i);
                                vOutTags.RemoveAt(i);
                                vPoint.RemoveAt(i);
                                vAccSpeed.RemoveAt(i);
                                break;
                            }
                            GUILayout.EndHorizontal();

                            vInTags[i] = inTag;
                            vOutTags[i] = outTag;
                            vPoint[i] = point;
                            vAccSpeed[i] = accSpeed;
                        }
                        if (GUILayout.Button("新建组"))
                        {
                            vInTags.Add(Vector3.zero);
                            vOutTags.Add(Vector3.zero);
                            if (vPoint.Count >= 2) vPoint.Add(vPoint[vPoint.Count - 1] + (vPoint[vPoint.Count - 1] - vPoint[vPoint.Count - 2]).normalized * 5);
                            else if (vPoint.Count > 1) vPoint.Add(vPoint[vPoint.Count - 1] + Vector3.forward * 5);
                            else vPoint.Add(Vector3.zero);
                            vAccSpeed.Add(Vector3.one);
                        }

                        if (projectile.accelerations != null && projectile.accelerations.Length > 0)
                        {
                            EProjectileParabolicType preType = (EProjectileParabolicType)projectile.accelerations[0].y;
                            projectile.accelerations[0].y = HandleUtilityWrapper.PopEnum("构建方式", (int)projectile.accelerations[0].y, typeof(EProjectileParabolicType));
                            if (preType != (EProjectileParabolicType)projectile.accelerations[0].y)
                            {
                                vInTags.Clear();
                                vOutTags.Clear();
                                vPoint.Clear();
                                vAccSpeed.Clear();

                                vInTags.Add(Vector3.zero);
                                vOutTags.Add(Vector3.zero);
                                vPoint.Add(Vector3.zero);
                                vAccSpeed.Add(new Vector3(1, projectile.accelerations[0].y, 0));
                                if ((EProjectileParabolicType)projectile.accelerations[0].y == EProjectileParabolicType.StartEnd)
                                {
                                    vInTags.Add(Vector3.zero);
                                    vOutTags.Add(Vector3.zero);
                                    vPoint.Add(Vector3.zero);
                                    vAccSpeed.Add(new Vector3(1, 0, 0));
                                }
                            }
                        }
                        projectile.speedLows = vInTags.ToArray();
                        projectile.speedUppers = vOutTags.ToArray();
                        projectile.speedMaxs = vPoint.ToArray();
                        projectile.accelerations = vAccSpeed.ToArray();
                    }
                    projectile.speedLerp = EditorGUILayout.Vector2Field("速度区间", projectile.speedLerp);
                }
                else
                {
                    List<Vector3> vLowerSpeeds = (projectile.speedLows != null) ? new List<Vector3>(projectile.speedLows) : new List<Vector3>();
                    List<Vector3> vUpperSpeeds = (projectile.speedUppers != null) ? new List<Vector3>(projectile.speedUppers) : new List<Vector3>();
                    List<Vector3> vMaxSpeeds = (projectile.speedMaxs != null) ? new List<Vector3>(projectile.speedMaxs) : new List<Vector3>();
                    List<Vector3> vAccelerations = (projectile.accelerations != null) ? new List<Vector3>(projectile.accelerations) : new List<Vector3>();
                    m_bExpandSpeedAcc = EditorGUILayout.Foldout(m_bExpandSpeedAcc, "速度组");
                    if (m_bExpandSpeedAcc)
                    {
                        float width = size.x - 40;
                        GUILayoutOption[] layOp = new GUILayoutOption[] { GUILayout.Width(width / 4) };
                        for (int i = 0; i < vLowerSpeeds.Count; ++i)
                        {
                            Vector3 lower = vLowerSpeeds[i];
                            Vector3 upper = vUpperSpeeds[i];
                            Vector3 maxer = vMaxSpeeds[i];
                            Vector3 acc = vAccelerations[i];
                            GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(width) });
                            GUILayout.Label("轴向", layOp);
                            GUILayout.Label("随机区间", layOp);
                            GUILayout.Label("最大速度(0为无限制)", layOp);
                            if (GUILayout.Button("删除", layOp))
                            {
                                vLowerSpeeds.RemoveAt(i);
                                vUpperSpeeds.RemoveAt(i);
                                vMaxSpeeds.RemoveAt(i);
                                vAccelerations.RemoveAt(i);
                                break;
                            }
                            GUILayout.EndHorizontal();

                            GUILayout.BeginVertical();
                            for (int j = 0; j < 3; ++j)
                            {
                                GUILayout.BeginHorizontal();
                                EditorGUILayout.LabelField(SPEED_AXIS_NAME[j], layOp);

                                GUILayout.BeginHorizontal(layOp);
                                float lowSpeed = EditorGUILayout.FloatField(lower[j]);
                                EditorGUIUtility.labelWidth = 20;
                                EditorGUILayout.LabelField("到", new GUILayoutOption[] { GUILayout.Width(20) });
                                EditorGUIUtility.labelWidth = labelWidth;
                                float upperSpeed = EditorGUILayout.FloatField(upper[j]);
                                GUILayout.EndHorizontal();

                                lower[j] = Mathf.Min(lowSpeed, upperSpeed);
                                upper[j] = Mathf.Max(lowSpeed, upperSpeed);

                                maxer[j] = EditorGUILayout.FloatField(maxer[j], layOp);
                                GUILayout.EndHorizontal();
                            }

                            GUILayout.EndVertical();

                            acc = EditorGUILayout.Vector3Field("加速度", acc, new GUILayoutOption[] { GUILayout.Width(width) });
                            vLowerSpeeds[i] = lower;
                            vUpperSpeeds[i] = upper;
                            vMaxSpeeds[i] = maxer;
                            vAccelerations[i] = acc;
                        }
                        if (GUILayout.Button("新建组"))
                        {
                            vLowerSpeeds.Add(Vector3.zero);
                            vUpperSpeeds.Add(Vector3.zero);
                            vMaxSpeeds.Add(Vector3.zero);
                            vAccelerations.Add(Vector3.zero);
                        }
                        projectile.speedLows = vLowerSpeeds.ToArray();
                        projectile.speedUppers = vUpperSpeeds.ToArray();
                        projectile.speedMaxs = vMaxSpeeds.ToArray();
                        projectile.accelerations = vAccelerations.ToArray();
                    }
                }
            }
            if (ProjectileData.IsTrack(projectile.type))
            {
                projectile.speedLerp.x = EditorGUILayout.FloatField("弹道朝目标过度快慢", projectile.speedLerp.x);
                projectile.speedLerp.y = EditorGUILayout.FloatField("飞离多远朝向目标", projectile.speedLerp.y);
            }
            else if(projectile.type == EProjectileType.Bounce)
            {
                projectile.speedLerp.x = EditorGUILayout.IntField("弹跳次数(<=0不限制)", (int)projectile.speedLerp.x);
                projectile.speedLerp.y = EditorGUILayout.FloatField("弹力衰减值", projectile.speedLerp.y);
            }
            {
                float width = size.x - 40;
                GUILayoutOption[] layOp = new GUILayoutOption[] { GUILayout.Width(width / 3) };
                GUILayout.BeginHorizontal(new GUILayoutOption[] { GUILayout.Width(width) });
                GUILayout.Label("旋转随机区间", layOp);
                GUILayout.Label("最小区间", layOp);
                GUILayout.Label("最大区间", layOp);
                GUILayout.EndHorizontal();
                GUILayout.BeginVertical();
                for (int j = 0; j < 3; ++j)
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(ROTATE_AXIS_NAME[j], layOp);

                    GUILayout.BeginHorizontal(layOp);
                    float lowValue = EditorGUILayout.FloatField(projectile.minRotate[j]);
                    EditorGUIUtility.labelWidth = 20;
                    EditorGUILayout.LabelField("到", new GUILayoutOption[] { GUILayout.Width(20) });
                    EditorGUIUtility.labelWidth = labelWidth;
                    float upperValue = EditorGUILayout.FloatField(projectile.maxRotate[j]);
                    GUILayout.EndHorizontal();

                    projectile.minRotate[j] = Mathf.Min(lowValue, upperValue);
                    projectile.maxRotate[j] = Mathf.Max(lowValue, upperValue);
                    GUILayout.EndHorizontal();
                }

                GUILayout.EndVertical();
            }
           
            EditorGUIUtility.labelWidth = labelWidth;
            projectile.life_time = EditorGUILayout.FloatField("生命时长", projectile.life_time);
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "launch_flag");

            if (ProjectileData.IsTrack(projectile.type))
            {
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "track_target_slot");
                projectile.track_target_offset = EditorGUILayout.Vector3Field("追踪目标绑点偏移", projectile.track_target_offset);
            }

            projectile.collisionType = (EActorCollisionType)EditorGUILayout.EnumPopup("碰撞体类型", projectile.collisionType);
            if(projectile.collisionType == EActorCollisionType.BOX)
            {
                projectile.aabb_min = EditorGUILayout.Vector3Field("包围盒-Min", projectile.aabb_min);
                projectile.aabb_max = EditorGUILayout.Vector3Field("包围盒-Max", projectile.aabb_max);
            }
            else if (projectile.collisionType == EActorCollisionType.CAPSULE)
            {
                projectile.aabb_min.x = EditorGUILayout.FloatField("半径大小", projectile.aabb_min.x);
            }
            projectile.penetrable = EditorGUILayout.Toggle("是否可穿透", projectile.penetrable);
            projectile.counteract = EditorGUILayout.Toggle("是否可抵消", projectile.counteract);
            projectile.explode_range = EditorGUILayout.FloatField("爆炸范围", projectile.explode_range);
            if(projectile.explode_range>0)
            {
                projectile.explode_effect = EditorKits.DrawUIObjectByPath<GameObject>("爆炸击中特效", projectile.explode_effect);
                projectile.explode_effect_offset = EditorGUILayout.Vector3Field("爆炸击中特效偏移", projectile.explode_effect_offset);
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "explode_damage_id");
            }

            projectile.launch_delay = EditorGUILayout.FloatField("延迟发射", projectile.launch_delay);
            projectile.externLogicSpeed = EditorGUILayout.Toggle("附加酷跑/站位速速", projectile.externLogicSpeed);
            string preEffectPrefab = projectile.effect;
            projectile.effectSpeed = EditorGUILayout.FloatField("特效播放速度", projectile.effectSpeed);
            projectile.effect = EditorKits.DrawUIObjectByPath<GameObject>("特效资源", projectile.effect);
            projectile.target_effect_hit = EditorKits.DrawUIObjectByPath<GameObject>("击中特效", projectile.target_effect_hit);
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "effect_hit_slot", null, "击中特效绑点");
            projectile.target_effect_hit_offset = EditorGUILayout.Vector3Field("击中特效位置偏移", projectile.target_effect_hit_offset);
            projectile.target_effect_hit_scale = EditorGUILayout.FloatField("击中特效缩放", projectile.target_effect_hit_scale);

            string preWaringEffectPrefab = projectile.waring_effect;
            projectile.waring_duration = EditorGUILayout.FloatField("预警时长", projectile.waring_duration);
            projectile.waring_effect = EditorKits.DrawUIObjectByPath<GameObject>("预警特效", projectile.waring_effect);

            if (projectile.sound_launch_id <= 0)
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "sound_launch");
            if (string.IsNullOrEmpty(projectile.sound_launch))
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "sound_launch_id");

            if (projectile.sound_hit_id <= 0)
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "sound_hit",null, "击中音效(路径)");
            if (string.IsNullOrEmpty(projectile.sound_hit))
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "sound_hit_id",null, "击中音效(id)");


            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "damage");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "attack_type_filer");

            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "scale");
            if (preEffectPrefab!=null && preEffectPrefab.CompareTo(projectile.effect) != 0)
            {
                if (m_pPreveObject) GameObject.DestroyImmediate(m_pPreveObject);
                GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.effect);
                if (pObj != null)
                {
                    m_pPreveObject = GameObject.Instantiate<GameObject>(pObj);
                    Base.Util.ResetGameObject(m_pPreveObject, Base.EResetType.All);
                }
            }
            //             if (preWaringEffectPrefab != null && preWaringEffectPrefab.CompareTo(projectile.waring_effect) != 0)
            //             {
            //                 if (m_pWaringObj) GameObject.DestroyImmediate(m_pWaringObj);
            //                 GameObject pObj = AssetDatabase.LoadAssetAtPath<GameObject>(m_pCurItem.waring_effect);
            //                 if (pObj != null)
            //                 {
            //                     m_pWaringObj = GameObject.Instantiate<GameObject>(pObj);
            //                     Base.Util.ResetGameObject(m_pWaringObj, Base.EResetType.All);
            //                 }
            //             }
            projectile.unSceneTest = EditorGUILayout.Toggle("忽略场景地表检测", projectile.unSceneTest);
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "stuck_time_hit");

            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_hit_flag");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_action_hit_ground");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_action_hit_air");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_property_hit_ground");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_property_hit_air");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "hit_rate_base");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_direction_postion");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "target_duration_hit");

            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "AttackEventID");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "HitEventID");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "OverEventID");

            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bImmedate");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "fEventStepGap");
            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "StepEventID");

            projectile.hit_count = (byte)EditorGUILayout.IntField("攻击次数", (int)projectile.hit_count);
            projectile.max_oneframe_hit = (byte)EditorGUILayout.IntField("同一帧最大可攻击次数", (int)projectile.max_oneframe_hit);
            projectile.hit_step = EditorGUILayout.FloatField("击中后攻击间隔", projectile.hit_step);
            projectile.bound_count = EditorGUILayout.IntField("弹射次数", projectile.bound_count);
            if (projectile.bound_count > 0)
            {
                projectile.bound_range = EditorGUILayout.FloatField("弹射范围", projectile.bound_range);
                HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_flag");

                if (projectile.bound_sound_launch_id <= 0)
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_sound_launch", null, "弹射发射音效(路径)");
                if (string.IsNullOrEmpty(projectile.bound_sound_launch))
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_sound_launch_id", null, "弹射发射音效(id)");

                if (projectile.bound_hit_sound_id <= 0)
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_hit_sound", null, "弹射击中音效(路径)");
                if (string.IsNullOrEmpty(projectile.bound_hit_sound))
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_hit_sound_id", null, "弹射击中音效(id)");

                projectile.bound_effectSpeed = EditorGUILayout.FloatField("弹射特效播放速度", projectile.bound_effectSpeed);
                projectile.bound_effect = EditorKits.DrawUIObjectByPath<GameObject>("弹射特效资源", projectile.bound_effect);
                projectile.bound_hit_effect = EditorKits.DrawUIObjectByPath<GameObject>("弹射击中特效", projectile.bound_hit_effect);

                if (projectile.bound_range > 0)
                {
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_damage_id");
                    projectile.bound_speed = EditorGUILayout.Vector3Field("弹射速度", projectile.bound_speed);
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_buffs");
                    HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_type");
                    if (projectile.bound_lock_type != ELockHitType.None)
                    {
                        HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_num");
                        HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_conditions");
                        if (projectile.bound_lock_conditions!=null && projectile.bound_lock_conditions.Length > 0)
                        {
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_param1");
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_param2");
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_param3");
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lock_rode");

                            projectile.bound_lock_param1 = CheckConditionParam(projectile.bound_lock_conditions.Length, projectile.bound_lock_param1);
                            projectile.bound_lock_param2 = CheckConditionParam(projectile.bound_lock_conditions.Length, projectile.bound_lock_param2);
                            projectile.bound_lock_param3 = CheckConditionParam(projectile.bound_lock_conditions.Length, projectile.bound_lock_param3);
                            projectile.bound_lock_rode = CheckConditionParam(projectile.bound_lock_conditions.Length, projectile.bound_lock_rode);
                        }
                        HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_lockHeight");
                        if (projectile.bound_lockHeight)
                        {
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_minLockHeight");
                            HandleUtilityWrapper.DrawPropertyByFieldName(projectile, "bound_maxLockHeight");
                        }
                    }
                }
            }

            EditorGUILayout.EndScrollView();
        }
        //-----------------------------------------------------
        T[] CheckConditionParam<T>(int num, T[] param)
        {
            if(num<=0)
            {
                return null;
            }
            if ((param != null && num != param.Length) || param == null)
            {
                List<T> vList = new List<T>();
                for (int i = 0; i < num; ++i)
                {
                    if (param!=null && i < param.Length) vList.Add(param[i]);
                    else vList.Add((T)System.Activator.CreateInstance(typeof(T)));
                }
                return vList.ToArray();
            }

            return param;
        }
        //-----------------------------------------------------
        public void New()
        {
            CsvData_Projectile projectileCsv = DataEditorUtil.GetTable<CsvData_Projectile>();
            uint id = 0;
            foreach(var db in projectileCsv.datas)
            {
                id = (uint)Mathf.Max(db.Key, id);
            }
            id++;

            ProjectileData newData = new ProjectileData();
            newData.id = id;
            projectileCsv.datas.Add(id, newData);

            RefreshList(false);
            List<int> sels = new List<int>();
            sels.Add((int)id);
            m_pTreeView.SetSelection(sels);
            OnChangeSelect(newData);
        }
        //-----------------------------------------------------
        public void OnDrawLayerPanel(Vector2 size)
        {
            if (m_pTreeView == null) return;
            m_pTreeView.searchString = EditorGUILayout.TextField("搜索", m_pTreeView.searchString, new GUILayoutOption[] { GUILayout.Height(25) });
            m_pTreeView.OnGUI(new Rect(0,30, size.x, size.y-30));
        }
        //------------------------------------------------------
        void RenderVolumeByColor(ref Vector3 minVolume, ref Vector3 maxVolume, Vector3 position, Quaternion rotation, Color dwColor, float fScale, bool bEditor = false)
        {
                Vector3 vCenter;
                Vector3 vHalf;
                vCenter = (minVolume + maxVolume) * 0.5f;
                vHalf = maxVolume - vCenter;

            Framework.Core.CommonUtility.DrawBoundingBox(vCenter, vHalf, Matrix4x4.TRS(position, rotation, Vector3.one), dwColor, false);
            if (bEditor)
            {
                minVolume = Handles.DoPositionHandle(minVolume + position, rotation) - position;
                maxVolume = Handles.DoPositionHandle(maxVolume + position, rotation) - position;
                minVolume = Vector3.Min(minVolume, maxVolume);
                maxVolume = Vector3.Max(minVolume, maxVolume);
            }
        }
        //------------------------------------------------------
        void TestDrawLineTrace(ProjectileData data, Vector3 actorPos, Vector3 actorDir, float fStep)
        {
            Color color = Handles.color;
            if (data.life_time <= 0 || data.speedLows == null)
            {
                Handles.color = color;
                return;
            }
            Vector3 vRight = Vector3.Cross(actorDir, Vector3.up);
            Vector3 testPos = m_pTargetActor.GetPosition();
            {
                Handles.color = Color.yellow;
                for (int i = 0; i < data.speedLows.Length; ++i)
                {
                    Vector3 max_speed = data.speedMaxs[i];
                    Vector3 speed = actorDir * data.speedLows[i].z + Vector3.up * data.speedLows[i].y + vRight * data.speedLows[i].x;
                    Vector3 acceleration = actorDir * data.accelerations[i].z + Vector3.up * data.accelerations[i].y + vRight * data.accelerations[i].x ;
                    float fDuration = 0;
                    Vector3 pos = actorPos;
                    Vector3 duration_speed = speed;
                    bool bInit = false;
                    Vector3 toDir = Vector3.zero;
                    Vector3 curForward = actorDir;
                    Vector3 curPos = actorPos;
                    float trackSpeedLerp = data.speedLerp.x;
                    float trackDiff = data.speedLerp.y;
                    float fRemainDuration = data.life_time;
                    while (fDuration <= data.life_time)
                    {
                        duration_speed.x += acceleration.x * fStep;
                        duration_speed.y += acceleration.y * fStep;
                        duration_speed.z += acceleration.z * fStep;

                        if (max_speed.x > 0) duration_speed.x = Mathf.Min(duration_speed.x, max_speed.x);
                        if (max_speed.y > 0) duration_speed.y = Mathf.Min(duration_speed.y, max_speed.y);
                        if (max_speed.z > 0) duration_speed.z = Mathf.Min(duration_speed.z, max_speed.z);

                        if (trackDiff>0 &&(curPos- actorPos).sqrMagnitude >= trackDiff * trackDiff)
                        {
                            trackDiff = 0;
                        }
                        bool bTrackEnd = false;
                        if (ProjectileData.IsTrack(data.type))
                        {
                            curForward = (testPos - curPos).normalized;
                            float sqrtDiff = curForward.sqrMagnitude;
                            if (sqrtDiff > 0.1f)
                            {
                                if (trackDiff <= 0)
                                {
                                    float magnite = duration_speed.magnitude;
                                    if (data.life_time > 0)
                                    {
                                        float scaleLerp = 1 - Mathf.Clamp01(fRemainDuration / data.life_time);
                                        if (curPos.z >= testPos.z) trackSpeedLerp += fStep * 10;
                                        duration_speed = Vector3.Lerp(duration_speed, curForward * magnite, scaleLerp * trackSpeedLerp);
                                    }
                                    else
                                        duration_speed = curForward * magnite;

                                    float fSqrGap = Mathf.Max(1, duration_speed.sqrMagnitude * fStep * fStep);
                                    if ((testPos - curPos).sqrMagnitude <= fSqrGap)
                                    {
                                        bTrackEnd = true;
                                    }
                                }
                            }
                        }

                        curPos = pos + duration_speed * fStep;
                        Handles.DrawLine(pos, curPos);
                        fDuration += fStep;
                        fRemainDuration -= fStep;
                        if (!bInit)
                        {
                            toDir = curPos - pos;
                            bInit = true;
                        }
                        pos = curPos;

                        if (bTrackEnd) break;
                    }
                    if (toDir.sqrMagnitude <= 0) toDir = Vector3.forward;
                    Quaternion qt = Quaternion.LookRotation(toDir.normalized);
                    Handles.ArrowHandleCap(0, actorPos, qt, HandleUtility.GetHandleSize(actorPos), EventType.Repaint);
                }
            }
            {
                Handles.color = Color.red;
                for (int i = 0; i < data.speedUppers.Length; ++i)
                {
                    Vector3 max_speed = data.speedMaxs[i];
                    Vector3 speed = actorDir * data.speedUppers[i].z + Vector3.up * data.speedUppers[i].y + vRight * data.speedUppers[i].x;
                    Vector3 acceleration = actorDir * data.accelerations[i].z + Vector3.up * data.accelerations[i].y + vRight * data.accelerations[i].x;
                    float fDuration = 0;
                    Vector3 pos = actorPos;
                    Vector3 duration_speed = speed;
                    bool bInit = false;
                    Vector3 toDir = Vector3.zero;
                    Vector3 curForward = actorDir;
                    Vector3 curPos = actorPos;
                    float trackSpeedLerp = data.speedLerp.x;
                    float trackDiff = data.speedLerp.y;
                    float fRemainDuration = data.life_time;
                    while (fDuration <= data.life_time)
                    {
                        duration_speed.x += acceleration.x * fStep;
                        duration_speed.y += acceleration.y * fStep;
                        duration_speed.z += acceleration.z * fStep;

                        if (max_speed.x > 0) duration_speed.x = Mathf.Min(duration_speed.x, max_speed.x);
                        if (max_speed.y > 0) duration_speed.y = Mathf.Min(duration_speed.y, max_speed.y);
                        if (max_speed.z > 0) duration_speed.z = Mathf.Min(duration_speed.z, max_speed.z);

                        if (trackDiff > 0 && (curPos - actorPos).sqrMagnitude >= trackDiff * trackDiff)
                        {
                            trackDiff = 0;
                        }
                        bool bTrackEnd = false;
                        if (ProjectileData.IsTrack(data.type))
                        {
                            curForward = (testPos - curPos).normalized;
                            float sqrtDiff = curForward.sqrMagnitude;
                            if (sqrtDiff > 0.1f)
                            {
                                if(trackDiff <=0)
                                {
                                    float magnite = duration_speed.magnitude;
                                    if (data.life_time > 0)
                                    {
                                        float scaleLerp = 1 - Mathf.Clamp01(fRemainDuration / data.life_time);
                                        if (curPos.z >= testPos.z) trackSpeedLerp += fStep * 10;
                                        duration_speed = Vector3.Lerp(duration_speed, curForward * magnite, scaleLerp * trackSpeedLerp);
                                    }
                                    else
                                        duration_speed = curForward * magnite;

                                    float fSqrGap = Mathf.Max(1, duration_speed.sqrMagnitude * fStep * fStep);
                                    if ((testPos - curPos).sqrMagnitude <= fSqrGap)
                                    {
                                        bTrackEnd = true;
                                    }
                                }
                            }
                        }

                        curPos = pos + duration_speed * fStep;
                        Handles.DrawLine(pos, curPos);
                        fDuration += fStep;
                        fRemainDuration -= fStep;
                        if (!bInit)
                        {
                            toDir = curPos - pos;
                            bInit = true;
                        }
                        pos = curPos;
                        if (bTrackEnd) break;
                    }
                    if (toDir.sqrMagnitude <= 0) toDir = Vector3.forward;
                    Quaternion qt = Quaternion.LookRotation(toDir.normalized);
                    Handles.ArrowHandleCap(0, actorPos, qt, HandleUtility.GetHandleSize(actorPos), EventType.Repaint);
                }
            }

            Handles.color = color;
        }
        //------------------------------------------------------
        void RenderSphereByColor(ref float fRadius, Vector3 position, Quaternion rotation, string strColor, float fScale, bool bEditor = false)
        {
            Color dwColor;
#if !UNITY_5_1
            if (!ColorUtility.TryParseHtmlString(strColor, out dwColor))
                return;
#else
            dwColor = Color.white;
#endif
            Color color = Handles.color;
            Handles.color = dwColor;

#if !UNITY_5_1
            Handles.SphereHandleCap(0, position, rotation, fRadius, EventType.Repaint);
#else
             //   Handles.CubeCap()
#endif

            if (bEditor)
            {
                fRadius = Handles.ScaleSlider(fRadius, position, rotation*Vector3.forward, rotation, HandleUtility.GetHandleSize(position), 0.1f);
            }

            Handles.color = color;
        }
    }
}