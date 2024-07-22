/********************************************************************
生成日期:	1:11:2020 13:16
类    名: 	ProjectileHitUtil
作    者:	HappLI
描    述:	飞行道具管理器
*********************************************************************/
using Framework.Core;

namespace TopGame.Core
{
    public class ProjectileHitUtil
    {
        public static void OnProjectHitCallback(InstanceOperiaon pOp)
        {
            if (pOp.GetAble() == null) return;
            AInstanceAble able = pOp.GetAble();
            if (pOp.HasData<Variable3>(0)) able.SetPosition((pOp.GetUserData<Variable3>(0)).ToVector3());
            if(pOp.HasData<Variable3>(1)) pOp.GetAble().SetForward(pOp.GetUserData<Variable3>(1).ToVector3());
            if (pOp.HasData<Variable3>(2)) able.SetScale(pOp.GetUserData<Variable3>(2).ToVector3());
            if(pOp.HasData<Variable1>(3))
            {
                float life = pOp.GetUserData<Variable1>(3).floatVal;
                if(life>0)
                {
                    if (able is ParticleController)
                    {
                        ParticleController ctl = able as ParticleController;
                        ctl.SetLifeTime(life);
                    }
                }
            }
        }
    }
}

