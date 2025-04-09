using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace TimeStar.DigitalPlant
{
    public class RuntimeVolumeApplicator : MonoBehaviour
    {
        public VolumeProfileSettings settings = new VolumeProfileSettings(); // 在 Inspector 中创建实例 
        public bool generateNewProfile = true; // 是否产生新的profile

       
        private Volume m_Volume;

        void Start()
        {
            ApplyVolumeProfile();
        }

        void ApplyVolumeProfile()
        {
            // 获取 Volume 组件
            m_Volume = GetComponent<Volume>();
            if (m_Volume == null)
            {
                if (generateNewProfile)
                {
                    settings.CreateNewProfile();
                }
            }

            // 应用设置

            //检查Volume的Profile是不是和Setting里的一致
            if (m_Volume.profile != settings.volumeProfile)
            {
                m_Volume.profile = settings.volumeProfile;
            }

            ApplyBloomSettings(m_Volume.profile, settings.bloomSetting);
            ApplyColorAdjustmentsSettings(m_Volume.profile, settings.colorAdjustmentsSetting);
            ApplyTonemappingSettings(m_Volume.profile, settings.tonemappingSetting);
            ApplyVignetteSettings(m_Volume.profile, settings.vignetteSetting);
            ApplyLiftGammaGainSettings(m_Volume.profile, settings.liftGammaGainSetting);
        }

        void ApplyBloomSettings(VolumeProfile profile, BloomSetting bloomSetting)
        {
            if (!bloomSetting.overrideProfile) return;

            Bloom bloom;
            if (!profile.TryGet(out bloom))
            {
                bloom = profile.Add<Bloom>();
            }

            // 修正：直接设置 active 属性
            bloom.active = bloomSetting.active;
            Debug.Log($"Bloom active set to {bloomSetting.active}");
            if (bloomSetting.active)
            {
                bloom.highQualityFiltering.value = bloomSetting.highQualityFiltering;
                bloom.threshold.overrideState = bloomSetting.threshold_state;
                bloom.threshold.value = bloomSetting.threshold;
                bloom.intensity.overrideState = bloomSetting.intensity_state;
                bloom.intensity.value = bloomSetting.intensity;
                bloom.scatter.overrideState = bloomSetting.scatter_state;
                bloom.scatter.value = bloomSetting.scatter;
                bloom.clamp.overrideState = bloomSetting.clamp_state;
                bloom.clamp.value = bloomSetting.clamp;
                bloom.tint.value = bloomSetting.tint;
            }
        }

        void ApplyColorAdjustmentsSettings(VolumeProfile profile, ColorAdjustmentsSetting colorAdjustmentsSetting)
        {
            if (!colorAdjustmentsSetting.overrideProfile) return;

            ColorAdjustments colorAdjustments;
            if (!profile.TryGet(out colorAdjustments))
            {
                colorAdjustments = profile.Add<ColorAdjustments>();
            }

            // 修正：直接设置 active 属性
            colorAdjustments.active = colorAdjustmentsSetting.active;
            Debug.Log($"ColorAdjustments active set to {colorAdjustmentsSetting.active}");
            if (colorAdjustmentsSetting.active)
            {
                colorAdjustments.postExposure.overrideState = colorAdjustmentsSetting.postExposure_state;
                colorAdjustments.postExposure.value = colorAdjustmentsSetting.postExposure;

                colorAdjustments.contrast.overrideState = colorAdjustmentsSetting.contrast_state;
                colorAdjustments.contrast.value = colorAdjustmentsSetting.contrast;

                colorAdjustments.colorFilter.overrideState = colorAdjustmentsSetting.colorFilter_state;
                colorAdjustments.colorFilter.value = colorAdjustmentsSetting.colorFilter;

                colorAdjustments.hueShift.overrideState = colorAdjustmentsSetting.hueShift_state;
                colorAdjustments.hueShift.value = colorAdjustmentsSetting.hueShift;

                colorAdjustments.saturation.overrideState = colorAdjustmentsSetting.saturation_state;
                colorAdjustments.saturation.value = colorAdjustmentsSetting.saturation;
            }
        }

        void ApplyTonemappingSettings(VolumeProfile profile, TonemappingSetting tonemappingSetting)
        {
            if (!tonemappingSetting.overrideProfile) return;

            Tonemapping tonemapping;
            if (!profile.TryGet(out tonemapping))
            {
                tonemapping = profile.Add<Tonemapping>();
            }

            tonemapping.active = tonemappingSetting.active;
            Debug.Log($"Tonemapping active set to {tonemappingSetting.active}");
            if (tonemappingSetting.active)
            {
                tonemapping.mode.overrideState = tonemappingSetting.mode_state;
                tonemapping.mode.value = tonemappingSetting.mode;
            }
        }

        void ApplyVignetteSettings(VolumeProfile profile, VignetteSetting vignetteSetting)
        {
            if (!vignetteSetting.overrideProfile) return;

            Vignette vignette;
            if (!profile.TryGet(out vignette))
            {
                vignette = profile.Add<Vignette>();
            }

            vignette.active = vignetteSetting.active;
            Debug.Log($"Vignette active set to {vignetteSetting.active}");
            if (vignetteSetting.active)
            {
                vignette.intensity.overrideState = vignetteSetting.intensity_state;
                vignette.intensity.value = vignetteSetting.intensity;
                vignette.smoothness.overrideState = vignetteSetting.smoothness_state;
                vignette.smoothness.value = vignetteSetting.smoothness;
                vignette.rounded.overrideState = vignetteSetting.round_state;
                vignette.rounded.value = vignetteSetting.round;

            }
        }

        void ApplyLiftGammaGainSettings(VolumeProfile profile, LiftGammaGainSetting liftGammaGainSetting)
        {
            if (!liftGammaGainSetting.overrideProfile) return;

            LiftGammaGain liftGammaGain;
            if (!profile.TryGet(out liftGammaGain))
            {
                liftGammaGain = profile.Add<LiftGammaGain>();
            }

            liftGammaGain.active = liftGammaGainSetting.active;
            Debug.Log($"LiftGammaGain active set to {liftGammaGainSetting.active}");
            if (liftGammaGainSetting.active)
            {
                liftGammaGain.lift.overrideState = liftGammaGainSetting.lift_state;
                liftGammaGain.lift.value = liftGammaGainSetting.lift;
                liftGammaGain.gamma.overrideState = liftGammaGainSetting.gamma_state;
                liftGammaGain.gamma.value = liftGammaGainSetting.gamma;
                liftGammaGain.gain.overrideState = liftGammaGainSetting.gain_state;
                liftGammaGain.gain.value = liftGammaGainSetting.gain;
            }
        }
    }

    [Serializable]
    public class BloomSetting
    {
        public bool overrideProfile = true;
        public bool active = true;
        public bool highQualityFiltering = true;
        public float threshold = 0f;
        public bool threshold_state = false;
        public float intensity = 0f;
        public bool intensity_state = false;
        public float scatter = 0f;
        public bool scatter_state = false;
        public float clamp = 0f;
        public bool clamp_state = false;
        public Color tint = Color.white;
    }

    [Serializable]
    public class ColorAdjustmentsSetting
    {
        public bool overrideProfile = true;
        public bool active = true;
        public float postExposure = 0f;
        public bool postExposure_state = false;
        public float contrast = 0f;
        public bool contrast_state = false;
        public Color colorFilter = Color.white;
        public bool colorFilter_state = false;
        public float hueShift = 0f;
        public bool hueShift_state = false;
        public float saturation = 0f;
        public bool saturation_state = false;
    }

    [Serializable]
    public class TonemappingSetting
    {
        public bool overrideProfile = true;
        public bool active = true;
        public TonemappingMode mode = TonemappingMode.ACES;
        public bool mode_state = false;
    }

    [Serializable]
    public class VignetteSetting
    {
        public bool overrideProfile = true;
        public bool active = true;
        public float intensity = 0f;
        public bool intensity_state = false;
        public float smoothness = 0f;
        public bool smoothness_state = false;
        public bool round = false;
        public bool round_state = false;

    }

    [Serializable]
    public class LiftGammaGainSetting
    {
        public bool overrideProfile = true;
        public bool active = true;
        public Vector4 lift = Vector4.one;
        public bool lift_state = false;
        public Vector4 gamma = Vector4.one;
        public bool gamma_state = false;
        public Vector4 gain = Vector4.one;
        public bool gain_state = false;
    }

    [Serializable]
    public class VolumeProfileSettings
    {
        public VolumeProfile volumeProfile;  // 可以为 Volume 组件已有的 Profile， 也可以Runtime生成

        public BloomSetting bloomSetting = new BloomSetting();
        public ColorAdjustmentsSetting colorAdjustmentsSetting = new ColorAdjustmentsSetting();
        public TonemappingSetting tonemappingSetting = new TonemappingSetting();
        public VignetteSetting vignetteSetting = new VignetteSetting();
        public LiftGammaGainSetting liftGammaGainSetting = new LiftGammaGainSetting();

        public void CreateNewProfile()
        {
            volumeProfile = ScriptableObject.CreateInstance<VolumeProfile>();
        }
    }
}