/********************************************************************
生成日期:	2022-3-9
类    名: 	AssetsImporter
作    者:	zdq
描    述:	资源导入控制
*********************************************************************/

#if UNITY_EDITOR
using UnityEditor;

namespace TopGame.ED
{
    public class AssetsImporter : AssetPostprocessor
    {

        class MyTexturePostprocessor : AssetPostprocessor
        {
            /// <summary>
            /// 图片导入到 包含 Datas/Texture 路径底下时,设置为 Sprite
            /// </summary>
            void OnPreprocessTexture()
            {
                if (assetPath.Contains("Datas/Texture") || assetPath.Contains("UI/Textures"))
                {
                    TextureImporter textureImporter = (TextureImporter)assetImporter;
                    textureImporter.textureType = TextureImporterType.Sprite;
                    textureImporter.streamingMipmaps = false;
                }
            }
        }
    }
}
#endif
/*
 
AssetPostprocessor
class in UnityEditor
描述
AssetPostprocessor 允许您挂接到导入管线并在导入资源前后运行脚本。

在模型导入期间，系统将按以下顺序调用函数： OnPreprocessModel 是在最开始时调用的，您可以覆盖在整个模型导入过程中使用的 ModelImporter 设置。 导入网格和材质后，从导入的节点创建游戏对象层级视图。使用 OnPostprocessMeshHierarchy 来更改层级视图。表示导入节点的每个游戏对象都被赋予相应的 MeshFilter、MeshRenderer 和 MeshCollider 组件。向 MeshRenderer 分配材质之前，系统会调用 OnAssignMaterialModel 函数。 在游戏对象初始化 MeshRenderer 之后且存在“userdata”时，系统会调用 OnPostprocessGameObjectWithUserProperties。这发生在生成子游戏对象之前。 如果在先前的阶段中未禁用动画生成（请参阅 ModelImporter.generateAnimations），则会生成带蒙皮的网格和动画。如果可能，系统还会创建化身并优化游戏对象层级。之后，系统将为根游戏对象调用 OnPostprocessModel。 系统会在 SpeedTree 资源（.spm 文件）上调用 OnPreprocessSpeedTree 和 OnPostprocessSpeedTree，与调用 OnPreprocessModel 和 OnPostprocessModel 的方式相同，区别在于 SpeedTreeImporter 类型是 assetImporter。

在制作流程中，AssetPostprocessor 应始终放置在项目中预先构建的 dll 中，而非脚本中。 AssetPostprocessor 会更改导入的资源的输出，因此，如果其中一个脚本出现编译错误，就会导致资源以不同的方式导入。 在制作流程中操作时，这是一个严重的问题。通过对 AssetPostprocessor 使用 dll，您可以确保在脚本出现编译错误时，也能够始终执行 AssetPostprocessor。 通过此方法，您可以覆盖导入设置中的默认值，也可以修改纹理或网格之类的导入数据。

OnAssignMaterialModel	提供源材质。
OnPostprocessAllAssets	在完成任意数量的资源导入后（当资源进度条到达末尾时）调用此函数。
OnPostprocessAnimation	当 AnimationClip 已完成导入时调用此函数。
OnPostprocessAssetbundleNameChanged	将资源分配给其他资源捆绑包时调用的处理程序。
OnPostprocessAudio	将此函数添加到一个子类中，以在音频剪辑完成导入时获取通知。
OnPostprocessCubemap	将此函数添加到一个子类中，以在立方体贴图纹理完成导入之前获取通知。
OnPostprocessGameObjectWithAnimatedUserProperties	当自定义属性的动画曲线已完成导入时调用此函数。
OnPostprocessGameObjectWithUserProperties	为每个在导入文件中至少附加了一个用户属性的游戏对象调用此函数。
OnPostprocessMaterial	将此函数添加到一个子类中，以在材质资源完成导入时获取通知。
OnPostprocessMeshHierarchy	当变换层级视图已完成导入时调用此函数。
OnPostprocessModel	将此函数添加到一个子类中，以在模型完成导入时获取通知。
OnPostprocessPrefab	Gets a notification when a Prefab completes importing.
OnPostprocessSpeedTree	将此函数添加到一个子类中，以在 SpeedTree 资源完成导入时获取通知。
OnPostprocessSprites	将此函数添加到一个子类中，以在精灵的纹理完成导入时获取通知。
OnPostprocessTexture	将此函数添加到一个子类中，以在纹理刚完成导入之前获取通知。
OnPreprocessAnimation	将此函数添加到一个子类中，以在导入模型（.fbx、.mb 文件等）中的动画之前获取通知。
OnPreprocessAsset	将此函数添加到一个子类中，以在导入所有资源之前获取通知。
OnPreprocessAudio	将此函数添加到一个子类中，以在导入音频剪辑之前获取通知。
OnPreprocessMaterialDescription	将此函数添加到一个子类中，以在材质从 Model Importer 导入时接收通知。
OnPreprocessModel	将此函数添加到一个子类中，以在导入模型（.fbx、.mb 文件等）之前获取通知。
OnPreprocessSpeedTree	将此函数添加到一个子类中，以在导入 SpeedTree 资源（.spm 文件）之前获取通知。
OnPreprocessTexture	将此函数添加到一个子类中，以在纹理导入器运行之前获取通知。
 */

