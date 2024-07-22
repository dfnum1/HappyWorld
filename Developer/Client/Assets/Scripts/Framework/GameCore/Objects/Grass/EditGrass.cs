/********************************************************************
生成日期:	1:11:2020 13:07
类    名: 	EditGrass
作    者:	HappLI
描    述:	编辑模式下的草
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace TopGame.Core
{
    public class EditGrass : IGrass
    {
        private const int GRASS_GRID = 8;
        private const int GRASS_CELLS = 1024;
        private const int GRASS_FRAMES = 64;
        private static Stack<int> ms_CatchRemove = new Stack<int>();
        private static List<Blade> ms_Blades = new List<Blade>();
        private static RenderVertex ms_RenderVertex = new RenderVertex();
        int m_nFrame = 0;
        int m_nSpawn =0;
        int m_nTriangles = 0;
        int m_nNumDips = 0;
        Material m_pMaterial = null;

        GrassBaker m_pWorld = null;

        public int variation{ get; private set; }
        public int numTextures { get; private set; }
        public float distanceScale { get; private set; }

        public float minVisibleDistance { get; private set; }
        public float maxVisibleDistance { get; private set; }
        public float minFadeDistance { get; private set; }
        public float maxFadeDistance { get; private set; }

        public float sizeX { get; private set; }
        public float sizeY { get; private set; }
        public float step { get; private set; }
        public float density { get; private set; }
        public float threshold { get; private set; }
        public float angle { get; private set; }

        public Vector4 scaleMean { get; private set; }
        public Vector4 scaleSpread { get; private set; }
        public Vector4 aspectSpread { get; private set; }
        public Vector4 aspectMean { get; private set; }
        public Vector4 heightSpread { get; private set; }
        public Vector4 heightMean { get; private set; }
        public Vector4 offsetMean { get; private set; }
        public Vector4 offsetSpread { get; private set; }

        struct Cell
        {
            public int id;
            public int frame;
            public Vector4 density;
            public Vector3 center;
            public float distance;
            public int num_vertex;
            public Bounds bound_box;

            public List<int> meshs;
        }

        struct Blade
        {
            public Vector3 position;
            public Vector3 normal;
            public Vector3 scale;
            public float density;
            public int index;
        }

        struct RenderVertex
        {
            public List<Vector3> vertexs;
            public List<Vector3> normals;
            //   public byte[] normal;
            //     public byte[] tangent;
            public List<Vector4> texcoords;

            public List<int> indices;
            public void Clear()
            {
                if (vertexs == null) vertexs = new List<Vector3>();
                else vertexs.Clear();
                if (texcoords == null) texcoords = new List<Vector4>();
                else texcoords.Clear();
                if (indices == null) indices = new List<int>();
                else indices.Clear();
                if (normals == null) normals = new List<Vector3>();
                else normals.Clear();
            }
        }

        List<GrassBladeData> m_BakerBlades = new List<GrassBladeData>();

        class MeshData
        {
            public Mesh mesh;
        }

        static Stack<Mesh> ms_vPoolMesh = new Stack<Mesh>();
        List<Mesh>   m_vMesh;
        Dictionary<int, Cell>   m_Cells;
        List<Cell>              m_VisibleCells;

        Matrix4x4 m_Transform = Matrix4x4.identity;
        Matrix4x4 m_InvTransform = Matrix4x4.identity;
        Vector3 m_Direction =  Vector3.forward;

        Bounds m_BoundBounds = new Bounds();
        public EditGrass(GrassBaker world)
        {
            m_pWorld = world;
            m_Cells = new Dictionary<int, Cell>();
            m_vMesh = new List<Mesh>();
            m_VisibleCells = new List<Cell>();
            setVariation(0);
            setNumTextures(4);

            setMinVisibleDistance(Mathf.NegativeInfinity);
            setMinFadeDistance(0.0f);

            setMaxVisibleDistance(Mathf.Infinity);
            setMaxFadeDistance(0.0f);

            setSizeX(1.0f);
            setSizeY(1.0f);
            setStep(1.0f);
            setDensity(1.0f);
            setThreshold(0.5f);
            setAngle(0.0f);

            setScale(new Vector4(4.0f, 4.0f, 4.0f, 4.0f), new Vector4(1.0f, 1.0f, 1.0f, 1.0f));
            setAspect(new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
            setHeight(new Vector4(1.0f, 1.0f, 1.0f, 1.0f), new Vector4(0.5f, 0.5f, 0.5f, 0.5f));
            setOffset(new Vector4(0.0f, 0.0f, 0.0f, 0.0f), new Vector4(0.0f, 0.0f, 0.0f, 0.0f));

            SetTransform(Matrix4x4.identity);
        }
        //------------------------------------------------------
        ~EditGrass()
        {

        }
        //------------------------------------------------------
        public void Clear()
        {
            ms_RenderVertex.Clear();
            foreach (var db in m_vMesh)
            {
                db.Clear();
                if(!ms_vPoolMesh.Contains(db)) ms_vPoolMesh.Push(db);
            }
            m_vMesh.Clear();
            // clear cells
            m_Cells.Clear();

            // update bounds
            UpdateBounds();

            m_BakerBlades.Clear();
        }
        //------------------------------------------------------
        public void Update(Camera camera)
        {
            m_nFrame++;

            m_nSpawn = 1;

            ////! delete old cells
            //foreach (var db in m_Cells)
            //{
            //    if(m_nFrame - db.Value.frame > GRASS_FRAMES)
            //    {
            //        if (db.Value.mesh) db.Value.mesh.Clear();
            //        ms_CatchRemove.Push(db.Key);
            //    }
            //}
            //if(ms_CatchRemove.Count>0)
            //{
            //    foreach (var db in ms_CatchRemove)
            //    {
            //        m_Cells.Remove(db);
            //    }
            //    ms_CatchRemove.Clear();

            //    // update bounds
            //    UpdateBounds();
            //}
        }
        //------------------------------------------------------
        void UpdateBounds()
        {
            Vector4 aspect = (aspectMean + aspectSpread) * 0.5f;
            Vector4 height = (heightMean + heightSpread) * 0.5f;
            Vector4 width = new Vector4(height.x * aspect.x, height.y * aspect.y, height.z * aspect.z, height.w * aspect.w);
            float w = Mathf.Max(Mathf.Max(width.x, width.y), Mathf.Max(width.z, width.w));
            float h = Mathf.Max(Mathf.Max(height.x, height.y), Mathf.Max(height.z, height.w));

            m_BoundBounds.SetMinMax( new Vector3(-sizeX,0, -sizeY), new Vector3(sizeX+w, 0, sizeY+h) );
            m_BoundBounds.center = m_Transform.MultiplyPoint(m_BoundBounds.center);

            foreach (var db in m_Cells)
            {
                m_BoundBounds.Expand(db.Value.bound_box.size);
            }
        }
        //------------------------------------------------------
        Vector3 CreateBlade( List<Blade> blades, Vector3 position, int index, float offsetFactor )
        {
            float dens = 1f;

            Vector3 tposition = position;
            Vector3 normal = m_Direction;

            if (m_pWorld!=null)
            {
                dens = m_pWorld.GetDensity(position, index);
                if (dens < Mathf.Epsilon || dens < threshold) return Vector3.zero;

                float fHeight = 0;
                Vector3 pos = m_Transform.MultiplyPoint(position);
                if (!m_pWorld.IsInZoom(pos + new Vector3(0, 500, 0), ref fHeight, ref normal)) return Vector3.zero;
                tposition = position;// m_Transform.MultiplyPoint(position);
                tposition.y = fHeight;
            }
            else
            {
                tposition = position;// m_Transform.MultiplyPoint(position);
            }

            Blade blade = new Blade();
            blade.position = tposition;
            blade.normal = normal;
            blade.density = dens;
            blade.index = index;

            madVector3(ref blade.position, blade.normal, offsetFactor, blade.position);
            blades.Add(blade);

            return blade.position;
        }
        //------------------------------------------------------
        int CheckMesh(bool bForceOver=false)
        {
            if (ms_RenderVertex.vertexs.Count > 63000 || (bForceOver && ms_RenderVertex.vertexs.Count>0))
            {
                Mesh pMesh;
                if (ms_vPoolMesh.Count > 0)
                {
                    pMesh = ms_vPoolMesh.Pop();
                    pMesh.Clear();
                }
                else pMesh = new Mesh();

                pMesh.SetVertices(ms_RenderVertex.vertexs);
                pMesh.SetNormals(ms_RenderVertex.normals);
                pMesh.SetUVs(0, ms_RenderVertex.texcoords);
                pMesh.SetIndices(ms_RenderVertex.indices, MeshTopology.Triangles, 0);

                ms_RenderVertex.Clear();

                m_vMesh.Add(pMesh);
                return m_vMesh.Count - 1;
            }
            return -1;
        }
        //------------------------------------------------------
        void CreateCell(ref Cell c, Vector3 min, Vector3 max)
        {
            c.meshs = new List<int>();
            int meshid = CheckMesh();
            if(meshid>=0) c.meshs.Add(meshid);
            c.num_vertex = 0;
            
            //! generate blades
            int offset = 0;
            Vector3 size = max - min;
            Vector3 step = size / GRASS_GRID;

            //! density
            float fdes = density * sizeX * sizeY;
            c.density = new Vector4(fdes, fdes, fdes, fdes);
            if (m_pWorld != null)
            {
                Vector4 dens = m_pWorld.GetDensity(min, max);
                c.density.x = c.density.x * dens.x;
                c.density.y = c.density.y * dens.y;
                c.density.z = c.density.z * dens.z;
                c.density.w = c.density.w * dens.w;
            }

            //! allocate arrays
            ms_Blades.Clear();
            int Intdensity = (int)(c.density[0] + c.density[1] + c.density[2] + c.density[3]);

            Vector3 min_bounds = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 max_bounds = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //! grenrate blades
            for (int i = 0; i < 4; ++i)
            {
         //       UnityEngine.Random.InitState(c.id * 413711 + i * 113);

                //! generate cell baldes
                int num_blades = ms_Blades.Count;
                for(float d = 0; d < c.density[i]; d += GRASS_GRID*GRASS_GRID)
                {
                    for(int z = 0; z < GRASS_GRID; ++z)
                    {
                        for(int x = 0; x < GRASS_GRID; ++x)
                        {
                            Vector3 position = min;
                            position.x += step.x * x + UnityEngine.Random.Range(0f, step.x);
                            position.z += step.z * z + UnityEngine.Random.Range(0, step.z);

                            float offsetFactor = offsetMean[i] + UnityEngine.Random.Range(-offsetSpread[i], offsetSpread[i]);
                            //! create blade
                            Vector3 pos = CreateBlade(ms_Blades, position, i, offsetFactor);

                            min_bounds = Vector3.Min(min_bounds, pos);
                            max_bounds = Vector3.Max(max_bounds, pos);

                            if ( (ms_Blades.Count - num_blades)*4 >= 65536 / 4 ) break;
                        }
                        if ((ms_Blades.Count - num_blades) * 4 >= 65536 / 4) break;
                    }
                    if ((ms_Blades.Count - num_blades) * 4 >= 65536 / 4) break;
                }

                //! randomize cell baldes
                for(int j = num_blades; j < ms_Blades.Count; j+=2)
                {
                    int k = UnityEngine.Random.Range(num_blades, ms_Blades.Count);
                    int l = UnityEngine.Random.Range(num_blades, ms_Blades.Count);
                    Blade kb = ms_Blades[k];
                    Blade lb = ms_Blades[l];
                    ms_Blades[k] = lb;
                    ms_Blades[l] = kb;
                }

                //! remove old cell baldes
                int num = (int)c.density[i];
                if (ms_Blades.Count > offset + num)
                    ms_Blades.RemoveRange(offset + num, ms_Blades.Count - offset - num);
                offset += num;
            }

            // randomize all baldes
            for(int i =0; i < ms_Blades.Count; i+=2)
            {
                int k = UnityEngine.Random.Range(0, ms_Blades.Count);
                int l = UnityEngine.Random.Range(0, ms_Blades.Count);
                Blade kb = ms_Blades[k];
                Blade lb = ms_Blades[l];
                ms_Blades[k] = lb;
                ms_Blades[l] = kb;
            }

            //! cell bounds
            c.bound_box.SetMinMax(min_bounds, max_bounds);

            //! generate vertices
            int num_vertexs = 0;
            float radius = 0f;
            Vector3 tangent = Vector3.zero, binormal = Vector3.zero, iposition = Vector3.zero;
            for(int i = 0; i < ms_Blades.Count; ++i)
            {
                Blade b = ms_Blades[i];

                float angle = UnityEngine.Random.Range(-Mathf.PI, Mathf.PI);
                tangent.x = Mathf.Sin(angle);
                tangent.z = Mathf.Cos(angle);
                tangent.y = 0;
                tangent = m_Transform.MultiplyVector(tangent);
                binormal = b.normal;
                tangent.Normalize();
                binormal.Normalize();

                float aspect;
                do
                {
                    aspect = aspectMean[b.index] + UnityEngine.Random.Range(-aspectSpread[b.index], aspectSpread[b.index]);
                } while (aspect < Mathf.Epsilon);

                float height;
                do
                {
                    height = heightMean[b.index] + UnityEngine.Random.Range(-heightSpread[b.index], heightSpread[b.index]);
                } while (height < Mathf.Epsilon);

                //! blade size
                height *= Mathf.Clamp01( (scaleMean[b.index]+ UnityEngine.Random.Range(-scaleSpread[b.index], scaleSpread[b.index]))*b.density);
                float width = aspect * height;

                //! blade scale
                float scale = Mathf.Max((float)width, (float)height);
                float iscale = 1f / scale;

                float uv_w = UnityEngine.Random.Range(0, numTextures);

                float binormal_w = Mathf.Clamp01(width * iscale * 0.5f + 0.5f);
                float tangent_w = Mathf.Clamp01(height * iscale * 0.5f + 0.5f);
                //s_attribute_0.w -- vertex scale
                //s_attribute_1 --normal
                //s_attribute_2 --tangle
                //s_attribute_3 --uv
                float c_width = (binormal_w * 2.0f - 1.0f) * scale;
                float c_height = (tangent_w * 2.0f - 1.0f) * scale;
                Vector4 uv = Vector4.zero;
                uv.z = b.index;
                uv.w = uv_w;

                b.scale = new Vector3(c_width, c_height, 1);

                int verCnt = ms_RenderVertex.vertexs.Count;
                //! balde position
                {
                    Vector3 pos = b.position;
                    pos -= tangent * (0-0.5f) * c_width;
                    pos += binormal * 0 * c_height;

                    uv.x = 0;
                    uv.y = 0;   
                    ms_RenderVertex.vertexs.Add(pos);
                    ms_RenderVertex.normals.Add(new Vector3(0, uv.z, uv.w));
                    ms_RenderVertex.texcoords.Add(uv);
                }
                {
                    Vector3 pos = b.position;
                    pos -= tangent * (1 - 0.5f) * c_width;
                    pos += binormal * 0 * c_height;
                    uv.x = 1;
                    uv.y = 0;     
                    ms_RenderVertex.vertexs.Add(pos);
                    ms_RenderVertex.normals.Add(new Vector3(0, uv.z, uv.w));
                    ms_RenderVertex.texcoords.Add(uv);
                }
                {
                    Vector3 pos = b.position;
                    pos -= tangent * (1 - 0.5f) * c_width;
                    pos += binormal * 1 * c_height;
                    uv.x = 1;
                    uv.y = 1;       
                    ms_RenderVertex.vertexs.Add(pos);
                    ms_RenderVertex.normals.Add(new Vector3(c_height, uv.z, uv.w));
                    ms_RenderVertex.texcoords.Add(uv);
                }
                {
                    Vector3 pos = b.position;
                    pos -= tangent * (0 - 0.5f) * c_width;
                    pos += binormal * 1 * c_height;
                    uv.x = 0;
                    uv.y = 1;       
                    ms_RenderVertex.vertexs.Add(pos);
                    ms_RenderVertex.normals.Add(new Vector3(c_height, uv.z, uv.w));
                    ms_RenderVertex.texcoords.Add(uv);
                }

                ms_RenderVertex.indices.Add(verCnt + 0);
                ms_RenderVertex.indices.Add(verCnt + 1);
                ms_RenderVertex.indices.Add(verCnt + 2);
                ms_RenderVertex.indices.Add(verCnt + 2);
                ms_RenderVertex.indices.Add(verCnt + 3);
                ms_RenderVertex.indices.Add(verCnt + 0);

                radius = Mathf.Max(radius, width * width / 4 + height * height);

                num_vertexs += 4;

                GrassBladeData bData = new GrassBladeData();
                bData.normal = binormal;
                bData.position = b.position;
                bData.tangent = tangent;
                bData.texCol = (byte)b.index;
                bData.texRow = (byte)uv_w;
                bData.scale = new Vector3(c_width, c_height, 1);
                m_BakerBlades.Add(bData);
            }
            c.num_vertex = num_vertexs;

            meshid = Mathf.Max(0, CheckMesh());
            if (meshid >= 0) c.meshs.Add(meshid);
            ms_Blades.Clear();
        }
        //------------------------------------------------------
        void UpdateCell(Vector3 camera)
        {
            while(true)
            {
                int isize_x = Mathf.Clamp(Mathf.CeilToInt(sizeX / step), 1, GRASS_CELLS);
                int isize_y = Mathf.Clamp(Mathf.CeilToInt(sizeY / step), 1, GRASS_CELLS);
                float step_x = sizeX / (float)isize_x;
                float step_y = sizeY / (float)isize_y;

                Vector3 icamera = m_InvTransform.MultiplyPoint(camera);
                int x = (int)(icamera.x / step_x);
                int y = (int)(icamera.z / step_y);
                int radius = Mathf.CeilToInt((maxVisibleDistance + maxFadeDistance) / Mathf.Min(step_x, step_y)) + 1;

                int x0 = Mathf.Max(x - radius, 0);
                int y0 = Mathf.Max(y - radius, 0);
                int x1 = Mathf.Min(x + radius + 1, isize_x);
                int y1 = Mathf.Min(y + radius + 1, isize_y);

                // find new cell
                int cell_x = -1;
                int cell_y = -1;
                int identifier = -1;
                int distance = 1000000;
                for (int Y = y0; Y < y1; Y++)
                {
                    for (int X = x0; X < x1; X++)
                    {
                        int d = (X - x) * (X - x) + (Y - y) * (Y - y);
                        if (d < radius * radius)
                        {
                            int id = Y * isize_x + X;
                            Cell cell;
                            if (!m_Cells.TryGetValue(id, out cell))
                            {
                                if (distance > d)
                                {
                                    distance = d;
                                    identifier = id;
                                    cell_x = X;
                                    cell_y = Y;
                                }
                            }
                            else
                            {
                                cell.frame = m_nFrame;
                                //DDDD
                            }
                        }
                    }
                }
                // create new cell
                if (/*m_nSpawn-- > 0 && */identifier != -1)
                {
                    Cell c = new Cell();
                    c.id = identifier;
                    c.frame = m_nFrame;

                    // create cell
                    Vector3 offset = new Vector3(step_x * cell_x, 0, step_y * cell_y);
                    CreateCell(ref c, offset, offset + new Vector3(step_x, 0, step_y));

                    m_Cells.Add(c.id, c);

                    // update bounds
                    UpdateBounds();
                }
                else
                    break;
            }
            // distance to cells
            foreach (var db in m_Cells)
            {
                Cell cell = db.Value;
                cell.distance = Vector3.SqrMagnitude(db.Value.bound_box.center - camera);
            }
        }
        //------------------------------------------------------
        void madVector3(ref Vector3 ret, Vector3 v0,float v1, Vector3 v2)
        {
            ret.x = v0.x * v1 + v2.x;
            ret.y = v0.y * v1 + v2.y;
            ret.z = v0.z * v1 + v2.z;
        }
        //------------------------------------------------------
        public void Destroy()
        {
            m_Cells.Clear();
        }
        //------------------------------------------------------
        public void SetMaterial(Material material)
        {
            m_pMaterial = material;
        }
        //------------------------------------------------------
        public void GetMeshs(List<Mesh> vMeshs)
        {
            for (int i = 0; i < m_vMesh.Count; ++i)
                vMeshs.Add(m_vMesh[i]);
        }
        //------------------------------------------------------
        public void SetDatas(GrassBladeData[] datas)
        {
            m_BakerBlades = new List<GrassBladeData>(datas);
        }
        //------------------------------------------------------
        public void GetDatas(List<GrassBladeData> vBlads)
        {
            for (int i = 0; i < m_BakerBlades.Count; ++i)
                vBlads.Add(m_BakerBlades[i]);
        }
        //------------------------------------------------------
        public void Bake(Vector3 camraPos)
        {
            Clear();
            UpdateCell(camraPos);
            CheckMesh(true);
        }
        //------------------------------------------------------
        public void Render(Camera camera )
        {
            if (camera == null) return;
   //         Bake(camera.transform.position);

            m_VisibleCells.Clear();
            float distance = maxVisibleDistance + maxFadeDistance;
            foreach (var db in m_Cells)
            {
                Cell cell = db.Value;
                if (cell.meshs == null || cell.meshs.Count<=0) continue;
        //        if (cell.distance > distance) continue;
                //if()
                m_VisibleCells.Add(cell);
            }

            RenderCell(camera.transform.position);
        }
        //------------------------------------------------------
        void RenderCell(Vector3 camraPos)
        {
            if (m_pMaterial == null) return;
            m_nNumDips = 0;
            m_nTriangles = 0;
            for(int i =0; i < m_vMesh.Count; ++i)
            {
                Graphics.DrawMesh(m_vMesh[i], m_Transform, m_pMaterial, 0);
            }
        }
        //------------------------------------------------------
        public void setVariation(int v)
        {
            variation = v;
            Clear();
        }
        //------------------------------------------------------
        public void setNumTextures(int v)
        {
            numTextures = Mathf.Max(1,v);
            Clear();
        }
        //------------------------------------------------------
        public void setMinVisibleDistance(float distance)
        {
            minVisibleDistance = distance;
        }
        //------------------------------------------------------
        public void setMinFadeDistance(float distance)
        {
            minFadeDistance = distance;
        }
        //------------------------------------------------------
        public void setMaxVisibleDistance(float distance)
        {
            maxVisibleDistance = distance;
        }
        //------------------------------------------------------
        public void setMaxFadeDistance(float distance)
        {
            maxFadeDistance = distance;
        }
        //------------------------------------------------------
        public void setSizeX(float size)
        {
            if (sizeX == size) return;
            sizeX = Mathf.Max(size, 0.0f);
            Clear();
            UpdateBounds();
        }
        //------------------------------------------------------
        public void setSizeY(float size)
        {
            if (sizeY == size) return;
            sizeY = Mathf.Max(size, 0.0f);
            Clear();
            UpdateBounds();
        }
        //------------------------------------------------------
        public void setStep(float size)
        {
            if (step == size) return;
            step = Mathf.Max(size, Mathf.Epsilon);
            Clear();
        }
        //------------------------------------------------------
        public void setDensity(float d)
        {
            if (density == d) return;
            density = Mathf.Max(d, 0);
            Clear();
        }
        //------------------------------------------------------
        public void setThreshold(float d)
        {
            if (threshold == d) return;
            threshold = Mathf.Clamp01(d);
            Clear();
        }
        //------------------------------------------------------
        public void setAngle(float d)
        {
            if (angle == d) return;
            angle = Mathf.Clamp01(d);
            Clear();
        }
        //------------------------------------------------------
        public void setScale(Vector4 mean, Vector4 spread)
        {
            if (scaleMean == mean && spread == scaleSpread) return;
            scaleMean = Vector4.Max(mean, Vector4.one*Vector4.kEpsilon);
            scaleSpread = spread;
            Clear();
        }
        //------------------------------------------------------
        public void setAspect(Vector4 mean, Vector4 spread)
        {
            if (aspectMean == mean && aspectSpread == scaleSpread) return;
            aspectMean = Vector4.Max(mean, Vector4.one * Vector4.kEpsilon);
            aspectSpread = spread;
            Clear();
        }
        //------------------------------------------------------
        public void setHeight(Vector4 mean, Vector4 spread)
        {
            if (heightMean == mean && heightSpread == scaleSpread) return;
            heightMean = Vector4.Max(mean, Vector4.one * Vector4.kEpsilon);
            heightSpread = spread;
            Clear();
        }
        //------------------------------------------------------
        public void setOffset(Vector4 mean, Vector4 spread)
        {
            if (offsetMean == mean && offsetSpread == scaleSpread) return;
            offsetMean = Vector4.Max(mean, Vector4.one * Vector4.kEpsilon);
            offsetSpread = spread;
            Clear();
        }
        //------------------------------------------------------
        public void SetTransform(Matrix4x4 t)
        {
            Matrix4x4 old_itransform = m_InvTransform;

            m_Transform = t;
            m_InvTransform = t.inverse;
            m_Direction = t.MultiplyVector(Vector3.up);

//             Matrix4x4 rtransform = m_Transform * old_itransform;
//             foreach (var c in m_Cells)
            {
            //    c.Value.bound_box.center
            }

            UpdateBounds();
        }
        //------------------------------------------------------
        public int GetNumTriangles()
        {
            return 0;
        }
        //------------------------------------------------------
        public int GetDumDips()
        {
            return 0;
        }
    }
}

