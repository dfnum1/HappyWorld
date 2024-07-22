using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

namespace TopGame.ED
{
	public class AvatarMaskGenerator
    {
		const string _sUpporSpineBoneName = "Bip001 Spine1";

		public static void GeneratorAvatarMask(ModelImporter modelImporter, string avatarMaskAssetPath, string spineBoneName = _sUpporSpineBoneName)
		{
			if (modelImporter == null)
				return;

			if (string.IsNullOrEmpty(avatarMaskAssetPath))
				return;

			AvatarMask avatarMask = new AvatarMask();

			var refTransformsPath = modelImporter.transformPaths;
			
			avatarMask.transformCount = refTransformsPath.Length;

			for (int i = 0; i < refTransformsPath.Length; i++)
			{
				avatarMask.SetTransformPath(i, refTransformsPath[i]);
				avatarMask.SetTransformActive(i, refTransformsPath[i].Contains(spineBoneName));
			}

			AssetDatabase.CreateAsset(avatarMask, avatarMaskAssetPath);
		}

		public static void GeneratorAnimatorController(List<Motion> motionClips, string controllerAssetPath)
		{
			if (string.IsNullOrEmpty(controllerAssetPath))
				return;

			if (motionClips == null || motionClips.Count == 0)
				return;

			var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerAssetPath);

			controller.AddLayer("Attack Layer");

			var baseLayer = controller.layers[0];
			var skillLayer = controller.layers[1];

			var sm = baseLayer.stateMachine;

			for (int i = 0; i < motionClips.Count; ++i)
			{
				var clip = motionClips[i];
				AnimatorState state = sm.AddState(clip.name);
				state.motion = clip;
			}

			sm = skillLayer.stateMachine;

			sm.AddState("empty");

			for (int i = 0; i < motionClips.Count; ++i)
			{
				var clip = motionClips[i];

				if (!clip.name.StartsWith("attack"))
					continue;
				AnimatorState state = sm.AddState(clip.name);
				state.motion = clip;
			}
		}
	}
}
