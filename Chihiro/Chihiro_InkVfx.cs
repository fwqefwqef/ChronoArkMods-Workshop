using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using HarmonyLib;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Chihiro
{
    internal static class ChihiroInkVfx
    {
        private static readonly Color InkTint = new Color(0.19f, 0.19f, 0.22f, 1f);
        private static readonly Color NishikiWhiteTint = new Color(0.90f, 0.90f, 0.90f, 1f);
        private static readonly Color NeutralOriginalTint = new Color(1f, 1f, 1f, 1f);
        private static readonly Color NishikiRedTint = new Color(0.82f, 0.15f, 0.12f, 1f);
        private static readonly Color NishikiBlackTint = new Color(0.08f, 0.08f, 0.08f, 1f);
        private static readonly Color MagatsumiGreenBlackTint = new Color(0.22f, 0.10f, 0.24f, 1f);
        private static readonly Color MagatsumiShadowTint = new Color(0.10f, 0.04f, 0.12f, 1f);
        private const bool DebugInkVfx = true;
        private static readonly HashSet<string> DebugSkillKeys = new HashSet<string>
        {
            "S_Chihiro_2",
            "S_Chihiro_2_0",
            "S_Chihiro_3",
            "S_Chihiro_6_0",
            "S_Chihiro_8_0",
            "S_Chihiro_R1",
            "S_Chihiro_R2_0",
            "S_Chihiro_R2_1",
            "S_Chihiro_R2_2",
            "S_Chihiro_R2_3_0",
            "S_Chihiro_R2_4",
            "S_Chihiro_7"
        };
        private static readonly HashSet<int> LoggedParticles = new HashSet<int>();
        private static readonly HashSet<string> DebugMaterialNames = new HashSet<string>
        {
            "SwordSlash8 (Instance)",
            "glowCore (Instance)",
            "DistortionSmoke18 (Instance)",
            "shockwave-front-yellow (Instance)",
            "flareGray_add (Instance)"
        };
        private static readonly HashSet<string> LoggedMaterials = new HashSet<string>();
        private static readonly HashSet<string> LoggedSuppressions = new HashSet<string>();
        private static int FirstCastTraceUntilFrame = -1;
        private static BattleChar FirstCastTraceUser;
        private static string FirstCastTraceSkillKey;
        private static readonly HashSet<int> TracedParticleInits = new HashSet<int>();
        private static readonly Dictionary<string, HashSet<string>> SuppressedMaterialNamesBySkill = new Dictionary<string, HashSet<string>>
        {
            {
                "S_Chihiro_1_0",
                new HashSet<string>
                {
                    "Mat_fx_Hit&Slash_add"
                }
            },
            {
                "S_Chihiro_2",
                new HashSet<string>
                {
                    "glowCore",
                    "flareGray_Multy",
                    "particle-blue-1",
                    "SwordSlash_Ilya",
                    "DistortionSmoke18",
                    "distCircle2M",
                    "flareGray_add",
                    "M_ky_flare02_4x4",
                    "M_ky_dust08_4x4i",
                    "glow-white",
                    "shockwave"
                }
            },
            {
                "S_Chihiro_2_0",
                new HashSet<string>
                {
                    "glowCore",
                    "flareGray_Multy",
                    "particle-blue-1",
                    "SwordSlash_Ilya",
                    "DistortionSmoke18",
                    "distCircle2M",
                    "flareGray_add",
                    "M_ky_flare02_4x4",
                    "M_ky_dust08_4x4i",
                    "glow-white",
                    "shockwave"
                }
            },
            {
                "S_Chihiro_3",
                new HashSet<string>
                {
                    "DistortionSmoke18",
                    "shockwave-front-yellow",
                    "flareGray_add"
                }
            },
        };
        private static readonly Dictionary<string, HashSet<string>> SuppressedShaderNamesBySkill = new Dictionary<string, HashSet<string>>
        {
            {
                "S_Chihiro_1_0",
                new HashSet<string>
                {
                    "VFX_Klaus/Fx_Hit&Slash_add"
                }
            },
            {
                "S_Chihiro_2",
                new HashSet<string>
                {
                    "Custom/Particle",
                    "Legacy Shaders/Particles/Multiply",
                    "Hovl/Particles/SwordSlash",
                    "Hovl/Particles/Distortion",
                    "DistortionNonNormal",
                    "KY/add",
                    "KY/add_hilight"
                }
            },
            {
                "S_Chihiro_2_0",
                new HashSet<string>
                {
                    "Custom/Particle",
                    "Legacy Shaders/Particles/Multiply",
                    "Hovl/Particles/SwordSlash",
                    "Hovl/Particles/Distortion",
                    "DistortionNonNormal",
                    "KY/add",
                    "KY/add_hilight"
                }
            },
            {
                "S_Chihiro_3",
                new HashSet<string>
                {
                    "Hovl/Particles/Distortion",
                    "Custom/Particle"
                }
            },
        };
        private static readonly Dictionary<string, string[]> SuppressedPathFragmentsBySkill = new Dictionary<string, string[]>
        {
            {
                "S_Chihiro_1_0",
                new[]
                {
                    "/highlight"
                }
            },
            {
                "S_Chihiro_2",
                new[]
                {
                    "/dust",
                    "/dust (1)",
                    "/flash",
                    "/flash (1)",
                    "/particles",
                    "/Sword Slash 1",
                    "/Distortion(Delete for Mobile)",
                    "/shockWave",
                    "/dust_hilight"
                }
            },
            {
                "S_Chihiro_2_0",
                new[]
                {
                    "/dust",
                    "/dust (1)",
                    "/flash",
                    "/flash (1)",
                    "/particles",
                    "/Sword Slash 1",
                    "/Distortion(Delete for Mobile)",
                    "/shockWave",
                    "/dust_hilight"
                }
            },
            {
                "S_Chihiro_3",
                new[]
                {
                    "/shockwave-front",
                    "/flash (1)",
                    "/Distortion(Delete for Mobile)"
                }
            },
        };
        private static readonly Dictionary<string, string[]> SuppressedBranchFragmentsBySkill = new Dictionary<string, string[]>
        {
            {
                "S_Chihiro_1_0",
                new[]
                {
                    "FX_hit_12/left_up/highlight",
                    "FX_hit_12/left_down/highlight",
                    "FX_hit_12/right_up/highlight",
                    "FX_hit_12/right_down/highlight"
                }
            },
            {
                "S_Chihiro_2",
                new[]
                {
                    "Ilya_6/flash",
                    "Ilya_6/Sword Slash 1",
                    "Ilya_6/Sword Slash 1/Distortion(Delete for Mobile)",
                    "Ilya_6/Sword Slash 1/Distortion(Delete for Mobile) (1)",
                    "glow",
                    "shockwave",
                    "Snow/shockWave",
                    "Snow/dust_hilight",
                    "Snow/Distortion(Delete for Mobile)",
                    "Snow/Sparks (3)"
                }
            },
            {
                "S_Chihiro_2_0",
                new[]
                {
                    "Ilya_6/flash",
                    "Ilya_6/Sword Slash 1",
                    "Ilya_6/Sword Slash 1/Distortion(Delete for Mobile)",
                    "Ilya_6/Sword Slash 1/Distortion(Delete for Mobile) (1)",
                    "glow",
                    "shockwave",
                    "Snow/shockWave",
                    "Snow/dust_hilight",
                    "Snow/Distortion(Delete for Mobile)",
                    "Snow/Sparks (3)"
                }
            },
            {
                "S_Chihiro_3",
                new[]
                {
                    "Hit/dust",
                    "flash (1)"
                }
            },
            {
                "S_Chihiro_R2_4",
                new[]
                {
                    "FlashParticle/Flash",
                    "FlashParticle/TargetIlya/LightningCore",
                    "FlashParticle/TargetIlya/BackgroundSnow",
                    "FlashParticle/TargetIlya/Snow_IlyaTrail",
                    "FlashParticle/TargetIlya/thander",
                    "FlashParticle/TargetIlya/thander (1)",
                    "FlashParticle/TargetIlya/Distortion(Delete for Mobile)",
                    "FlashParticle/Sparks",
                    "GroundCrack"
                }
            },
        };

        public static void ApplyToSkill(Skill skill)
        {
            if (skill == null || BattleSystem.instance == null || BattleSystem.instance.G_Particle == null)
            {
                return;
            }

            foreach (SkillParticle particle in BattleSystem.instance.G_Particle)
            {
                if (particle == null || particle.SkillData != skill)
                {
                    continue;
                }

                if (ShouldSuppressUntintableSubeffects(skill))
                {
                    SuppressUntintableSubeffects(skill, particle);
                }

                if (ShouldApplyBaseTint(skill.MySkill.KeyID))
                {
                    ApplyToParticle(particle, GetTintForSkill(skill), GetPlaybackSpeedForSkill(skill));
                }
                ApplyAccentOverrides(skill.MySkill.KeyID, particle);

                if (ShouldLogSkill(skill) && LoggedParticles.Add(particle.GetInstanceID()))
                {
                    LogParticleState(skill, particle);
                }
            }
        }

        private static void ApplyToParticle(SkillParticle particle, Color tint, float playbackSpeed)
        {
            var controller = particle.GetComponentInChildren<GAP_ParticleSystemController.ParticleSystemController>(true);
            if (controller != null)
            {
                controller.changeColor = true;
                controller.newMinColor = tint;
                controller.newMaxColor = tint;
                controller.ChangeColorOnly();
            }

            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                var main = particleSystem.main;
                main.simulationSpeed = playbackSpeed;
                main.startColor = new ParticleSystem.MinMaxGradient(tint);

                var colorOverLifetime = particleSystem.colorOverLifetime;
                if (colorOverLifetime.enabled)
                {
                    colorOverLifetime.color = new ParticleSystem.MinMaxGradient(tint);
                }

                var colorBySpeed = particleSystem.colorBySpeed;
                if (colorBySpeed.enabled)
                {
                    colorBySpeed.color = new ParticleSystem.MinMaxGradient(tint);
                }

                var trails = particleSystem.trails;
                if (trails.enabled)
                {
                    trails.colorOverLifetime = new ParticleSystem.MinMaxGradient(tint);
                    trails.colorOverTrail = new ParticleSystem.MinMaxGradient(tint);
                }

                var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer != null && renderer.material != null)
                {
                    SetMaterialColor(renderer.material, tint);
                }

                if (renderer != null && renderer.sharedMaterial != null)
                {
                    SetMaterialColor(renderer.sharedMaterial, tint);
                }
            }

            foreach (SpriteRenderer spriteRenderer in particle.GetComponentsInChildren<SpriteRenderer>(true))
            {
                spriteRenderer.color = tint;

                if (spriteRenderer.material != null)
                {
                    SetMaterialColor(spriteRenderer.material, tint);
                }
            }

            foreach (TrailRenderer trailRenderer in particle.GetComponentsInChildren<TrailRenderer>(true))
            {
                trailRenderer.startColor = tint;
                trailRenderer.endColor = tint;

                if (trailRenderer.material != null)
                {
                    SetMaterialColor(trailRenderer.material, tint);
                }
            }
        }

        private static void ApplyAccentOverrides(string skillKey, SkillParticle particle)
        {
            if (particle == null || string.IsNullOrEmpty(skillKey))
            {
                return;
            }

            if (skillKey == "S_Chihiro_6_0")
            {
                ApplyTintToBranch(particle, "blood_1", NishikiRedTint);
                ApplyTintToBranch(particle, "hit-slash", NishikiBlackTint);
                ApplyTintToBranch(particle, "dust", NishikiBlackTint);
                return;
            }

            if (skillKey == "S_Chihiro_8_0")
            {
                ApplyTintToBranch(particle, "slash", NishikiBlackTint);
                ApplyTintToBranch(particle, "slash/distortion", NishikiBlackTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Smoke", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Sparks", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Hit", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Hit/SparksCore", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Hit/Sparks", NishikiWhiteTint);
                ApplyTintToBranch(particle, "slash/Sword Slash 1/Sparks/Sparks", NishikiWhiteTint);

                ApplyTintToBranch(particle, "blood_1", NishikiRedTint);
                ApplyTintToBranch(particle, "slash/flash", NishikiRedTint);
                ApplyTintToBranch(particle, "slash/dust_hilight", NishikiRedTint);
                ApplyTintToBranch(particle, "WatesSplash", NishikiBlackTint);
                ApplyTintToBranch(particle, "slash/ShockWave (1)", NishikiBlackTint);
                ApplyTintToBranch(particle, "hit-slash", NishikiBlackTint);
                return;
            }

            if (skillKey == "S_Chihiro_R2_4")
            {
                ApplyTintToBranch(particle, "Slash", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/slash", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/flash", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/ShockWave (1)", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/Sparks (3)", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/smoke", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/smoke (1)", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/Sparks (4)", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/RayLightning", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/SparksCore", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "Slash/SparksCore (1)", MagatsumiGreenBlackTint);
                ApplyTintToBranch(particle, "slash", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "slash (2)", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2/Shadow (1)", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2/SCG_ilya_8_2 (4)", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2 (1)", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2 (2)", MagatsumiShadowTint);
                ApplyTintToBranch(particle, "SCG_ilya_8_2 (3)", MagatsumiShadowTint);
                return;
            }

            if (skillKey != "S_Chihiro_7")
            {
                return;
            }

            ApplyTintToBranch(particle, "slash/ray", NishikiRedTint);
            ApplyTintToBranch(particle, "slash/hit", NishikiRedTint);
            ApplyTintToBranch(particle, "slash/shine-ray -stretched-", NishikiWhiteTint);
            ApplyTintToBranch(particle, "slash/halo", NishikiRedTint);
            ApplyTintToBranch(particle, "slash/blood_1", NishikiWhiteTint);
        }

        private static void ApplyTintToBranch(SkillParticle particle, string branchSuffix, Color tint)
        {
            foreach (Transform child in particle.GetComponentsInChildren<Transform>(true))
            {
                string path = GetTransformPath(child, particle.transform);
                if (!path.EndsWith(branchSuffix, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                ApplyTintToTransform(child, tint);
            }
        }

        private static void ApplyTintToTransform(Transform transform, Color tint)
        {
            ParticleSystem particleSystem = transform.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(tint);

                var colorOverLifetime = particleSystem.colorOverLifetime;
                if (colorOverLifetime.enabled)
                {
                    colorOverLifetime.color = new ParticleSystem.MinMaxGradient(tint);
                }

                var colorBySpeed = particleSystem.colorBySpeed;
                if (colorBySpeed.enabled)
                {
                    colorBySpeed.color = new ParticleSystem.MinMaxGradient(tint);
                }

                var trails = particleSystem.trails;
                if (trails.enabled)
                {
                    trails.colorOverLifetime = new ParticleSystem.MinMaxGradient(tint);
                    trails.colorOverTrail = new ParticleSystem.MinMaxGradient(tint);
                }
            }

            ParticleSystemRenderer particleRenderer = transform.GetComponent<ParticleSystemRenderer>();
            if (particleRenderer != null)
            {
                if (particleRenderer.material != null)
                {
                    SetMaterialColor(particleRenderer.material, tint);
                }

                if (particleRenderer.sharedMaterial != null)
                {
                    SetMaterialColor(particleRenderer.sharedMaterial, tint);
                }
            }

            SpriteRenderer spriteRenderer = transform.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                spriteRenderer.color = tint;
                if (spriteRenderer.material != null)
                {
                    SetMaterialColor(spriteRenderer.material, tint);
                }
            }

            TrailRenderer trailRenderer = transform.GetComponent<TrailRenderer>();
            if (trailRenderer != null)
            {
                trailRenderer.startColor = tint;
                trailRenderer.endColor = tint;
                if (trailRenderer.material != null)
                {
                    SetMaterialColor(trailRenderer.material, tint);
                }
            }
        }

        private static void SetMaterialColor(Material material, Color tint)
        {
            if (material == null)
            {
                return;
            }

            if (material.HasProperty("_TintColor"))
            {
                material.SetColor("_TintColor", tint);
            }

            if (material.HasProperty("_Color"))
            {
                material.SetColor("_Color", tint);
            }

            if (material.HasProperty("_BaseColor"))
            {
                material.SetColor("_BaseColor", tint);
            }

            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", tint);
            }

            SetShaderDefinedColorProperties(material, tint);
        }

        private static void SetShaderDefinedColorProperties(Material material, Color tint)
        {
            foreach (string colorPropertyName in GetShaderPropertyNames(material, "Color"))
            {
                if (material.HasProperty(colorPropertyName))
                {
                    material.SetColor(colorPropertyName, tint);
                }
            }

            foreach (string vectorPropertyName in GetShaderPropertyNames(material, "Vector"))
            {
                if (material.HasProperty(vectorPropertyName) && LooksLikeColorProperty(vectorPropertyName))
                {
                    material.SetVector(vectorPropertyName, new Vector4(tint.r, tint.g, tint.b, tint.a));
                }
            }
        }

        private static Color GetTintForSkill(Skill skill)
        {
            if (skill == null || skill.MySkill == null)
            {
                return InkTint;
            }

            return GetTintForSkillKey(skill.MySkill.KeyID);
        }

        private static Color GetTintForSkillKey(string skillKey)
        {
            switch (skillKey)
            {
                case "S_Chihiro_R2_0":
                case "S_Chihiro_R2_1":
                case "S_Chihiro_R2_2":
                case "S_Chihiro_R2_3_0":
                case "S_Chihiro_R2_4":
                    return MagatsumiGreenBlackTint;
                case "S_Chihiro_6_0":
                    return NishikiWhiteTint;
                case "S_Chihiro_8_0":
                    return NeutralOriginalTint;
                case "S_Chihiro_7":
                    return NishikiBlackTint;
                default:
                    return InkTint;
            }
        }

        private static float GetPlaybackSpeedForSkill(Skill skill)
        {
            if (skill == null || skill.MySkill == null)
            {
                return 1f;
            }

            return GetPlaybackSpeedForSkillKey(skill.MySkill.KeyID);
        }

        private static float GetPlaybackSpeedForSkillKey(string skillKey)
        {
            switch (skillKey)
            {
                case "S_Chihiro_6_0":
                    return 0.72f;
                case "S_Chihiro_R2_4":
                    return 1.4f;
                default:
                    return 1f;
            }
        }

        private static bool ShouldApplyBaseTint(string skillKey)
        {
            return skillKey != "S_Chihiro_8_0" && skillKey != "S_Chihiro_R2_4";
        }

        private static IEnumerable<string> GetShaderPropertyNames(Material material, string propertyTypeName)
        {
            if (material == null || material.shader == null)
            {
                yield break;
            }

            MethodInfo getPropertyCount = typeof(Shader).GetMethod("GetPropertyCount", Type.EmptyTypes);
            MethodInfo getPropertyName = typeof(Shader).GetMethod("GetPropertyName", new[] { typeof(int) });
            MethodInfo getPropertyType = typeof(Shader).GetMethod("GetPropertyType", new[] { typeof(int) });
            if (getPropertyCount == null || getPropertyName == null || getPropertyType == null)
            {
                yield break;
            }

            int propertyCount = (int)getPropertyCount.Invoke(material.shader, null);
            for (int i = 0; i < propertyCount; i++)
            {
                object propertyType = getPropertyType.Invoke(material.shader, new object[] { i });
                if (propertyType == null || propertyType.ToString() != propertyTypeName)
                {
                    continue;
                }

                string propertyName = getPropertyName.Invoke(material.shader, new object[] { i }) as string;
                if (!string.IsNullOrEmpty(propertyName))
                {
                    yield return propertyName;
                }
            }
        }

        private static bool LooksLikeColorProperty(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                return false;
            }

            string lowered = propertyName.ToLowerInvariant();
            return lowered.Contains("color") || lowered.Contains("tint");
        }

        private static bool ShouldLogSkill(Skill skill)
        {
            return DebugInkVfx && skill != null && skill.MySkill != null && DebugSkillKeys.Contains(skill.MySkill.KeyID);
        }

        private static bool ShouldSuppressUntintableSubeffects(Skill skill)
        {
            if (skill == null || skill.MySkill == null)
            {
                return false;
            }

            string skillKey = skill.MySkill.KeyID;
            return SuppressedMaterialNamesBySkill.ContainsKey(skillKey)
                || SuppressedShaderNamesBySkill.ContainsKey(skillKey)
                || SuppressedPathFragmentsBySkill.ContainsKey(skillKey)
                || SuppressedBranchFragmentsBySkill.ContainsKey(skillKey);
        }

        private static void SuppressUntintableSubeffects(Skill skill, SkillParticle particle)
        {
            string skillKey = skill.MySkill.KeyID;
            HashSet<string> suppressedMaterials = SuppressedMaterialNamesBySkill.ContainsKey(skillKey)
                ? SuppressedMaterialNamesBySkill[skillKey]
                : new HashSet<string>();
            HashSet<string> suppressedShaders = SuppressedShaderNamesBySkill.ContainsKey(skillKey)
                ? SuppressedShaderNamesBySkill[skillKey]
                : new HashSet<string>();
            string[] suppressedPaths = SuppressedPathFragmentsBySkill.ContainsKey(skillKey)
                ? SuppressedPathFragmentsBySkill[skillKey]
                : Array.Empty<string>();
            string[] suppressedBranches = SuppressedBranchFragmentsBySkill.ContainsKey(skillKey)
                ? SuppressedBranchFragmentsBySkill[skillKey]
                : Array.Empty<string>();

            SuppressBranches(particle, suppressedBranches);

            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer == null)
                {
                    continue;
                }

                string path = GetTransformPath(particleSystem.transform, particle.transform);
                string materialName = NormalizeMaterialName(renderer.material);
                string shaderName = GetShaderName(renderer.material);
                bool pathMatches = suppressedPaths.Any(fragment => path.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0);
                bool materialMatches = suppressedMaterials.Contains(materialName);
                bool shaderMatches = suppressedShaders.Contains(shaderName);
                if (!materialMatches && !(shaderMatches && pathMatches))
                {
                    continue;
                }

                renderer.enabled = false;
                particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                particleSystem.Clear(true);
                particleSystem.gameObject.SetActive(false);

                string suppressionKey = path + "::" + materialName + "::" + shaderName;
                if (LoggedSuppressions.Add(suppressionKey))
                {
                    //Debug.Log($"[ChihiroInkVfx] SUPPRESS Path='{path}' Material='{materialName}' Shader='{shaderName}'");
                }
            }
        }

        public static void ApplyToSkillKey(string skillKey)
        {
            if (string.IsNullOrEmpty(skillKey) || BattleSystem.instance == null || BattleSystem.instance.G_Particle == null)
            {
                return;
            }

            foreach (SkillParticle particle in BattleSystem.instance.G_Particle)
            {
                if (particle == null || particle.SkillData == null || particle.SkillData.MySkill == null)
                {
                    continue;
                }

                if (particle.SkillData.MySkill.KeyID != skillKey)
                {
                    continue;
                }

                if (ShouldSuppressUntintableSubeffects(particle.SkillData))
                {
                    SuppressUntintableSubeffects(particle.SkillData, particle);
                }

                if (ShouldApplyBaseTint(skillKey))
                {
                    ApplyToParticle(particle, GetTintForSkillKey(skillKey), GetPlaybackSpeedForSkillKey(skillKey));
                }
                ApplyAccentOverrides(skillKey, particle);
            }
        }

        public static IEnumerator ApplyToSkillRepeated(Skill skill, int frameCount)
        {
            if (skill == null)
            {
                yield break;
            }

            for (int i = 0; i < frameCount; i++)
            {
                ApplyToSkill(skill);
                yield return null;
            }
        }

        public static IEnumerator ApplyToSkillKeyRepeated(string skillKey, int frameCount)
        {
            if (string.IsNullOrEmpty(skillKey))
            {
                yield break;
            }

            for (int i = 0; i < frameCount; i++)
            {
                ApplyToSkillKey(skillKey);
                yield return null;
            }
        }

        public static int GetRepeatFrameCount(Skill skill)
        {
            if (skill == null || skill.MySkill == null)
            {
                return 0;
            }

            switch (skill.MySkill.KeyID)
            {
                case "S_Chihiro_2":
                case "S_Chihiro_2_0":
                    return 24;
                case "S_Chihiro_1_0":
                case "S_Chihiro_R1":
                case "S_Chihiro_R2_0":
                case "S_Chihiro_R2_1":
                case "S_Chihiro_R2_2":
                case "S_Chihiro_R2_3_0":
                case "S_Chihiro_R2_4":
                case "S_Chihiro_6_0":
                case "S_Chihiro_8_0":
                case "S_Chihiro_7":
                case "S_Chihiro_3":
                    return 12;
                default:
                    return 0;
            }
        }

        public static bool ShouldForceImmediateInk(Skill skill)
        {
            return GetRepeatFrameCount(skill) > 0;
        }

        public static void BeginFirstCastTrace(Skill skill)
        {
            if (skill == null || skill.MySkill == null || skill.MySkill.KeyID != "S_Chihiro_2")
            {
                return;
            }

            FirstCastTraceUntilFrame = Time.frameCount + 90;
            FirstCastTraceUser = skill.Master;
            FirstCastTraceSkillKey = skill.MySkill.KeyID;
            TracedParticleInits.Clear();
            //Debug.Log($"[ChihiroInkVfx] TRACE_START Skill='{skill.MySkill.KeyID}' Frame='{Time.frameCount}'");
        }

        public static void BeginFirstCastTraceFromParticle(Skill skill)
        {
            if (skill == null || skill.MySkill == null || skill.MySkill.KeyID != "S_Chihiro_2")
            {
                return;
            }

            FirstCastTraceUntilFrame = Time.frameCount + 90;
            FirstCastTraceUser = skill.Master;
            FirstCastTraceSkillKey = skill.MySkill.KeyID;
        }

        public static bool ShouldTraceFirstCastParticle(BattleChar user, Skill skill)
        {
            return DebugInkVfx
                && FirstCastTraceUntilFrame >= Time.frameCount
                && (
                    (FirstCastTraceUser != null && user == FirstCastTraceUser)
                    || (skill != null && skill.MySkill != null && skill.MySkill.KeyID == FirstCastTraceSkillKey)
                );
        }

        public static void TraceSpawnedParticle(Skill skill, SkillParticle particle)
        {
            if (particle == null || !TracedParticleInits.Add(particle.GetInstanceID()))
            {
                return;
            }

            string skillKey = skill != null && skill.MySkill != null ? skill.MySkill.KeyID : "NULL";
            //Debug.Log($"[ChihiroInkVfx] TRACE_INIT Skill='{skillKey}' Particle='{particle.name}' ParticleName='{particle.ParticleName}'");
            if (skill != null)
            {
                LogParticleState(skill, particle);
            }
        }

        private static void SuppressBranches(SkillParticle particle, string[] suppressedBranches)
        {
            if (suppressedBranches == null || suppressedBranches.Length == 0)
            {
                return;
            }

            foreach (Transform child in particle.GetComponentsInChildren<Transform>(true))
            {
                string path = GetTransformPath(child, particle.transform);
                if (!suppressedBranches.Any(fragment => path.EndsWith(fragment, StringComparison.OrdinalIgnoreCase)))
                {
                    continue;
                }

                child.gameObject.SetActive(false);

                string suppressionKey = "BRANCH::" + path;
                if (LoggedSuppressions.Add(suppressionKey))
                {
                    //Debug.Log($"[ChihiroInkVfx] SUPPRESS_BRANCH Path='{path}'");
                }
            }
        }

        private static void LogParticleState(Skill skill, SkillParticle particle)
        {
            //Debug.Log($"[ChihiroInkVfx] Skill='{skill.MySkill.KeyID}' Particle='{particle.name}' ParticleName='{particle.ParticleName}'");

            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                //Debug.Log($"[ChihiroInkVfx] PS Path='{GetTransformPath(particleSystem.transform, particle.transform)}' Material='{GetMaterialName(renderer != null ? renderer.material : null)}' Shader='{GetShaderName(renderer != null ? renderer.material : null)}'");
                LogMaterialColorProperties(renderer != null ? renderer.material : null, particleSystem.name);
            }

            foreach (SpriteRenderer spriteRenderer in particle.GetComponentsInChildren<SpriteRenderer>(true))
            {
                //Debug.Log($"[ChihiroInkVfx] SR Path='{GetTransformPath(spriteRenderer.transform, particle.transform)}' Material='{GetMaterialName(spriteRenderer.material)}' Shader='{GetShaderName(spriteRenderer.material)}' Color='{spriteRenderer.color}'");
                LogMaterialColorProperties(spriteRenderer.material, spriteRenderer.name);
            }

            foreach (TrailRenderer trailRenderer in particle.GetComponentsInChildren<TrailRenderer>(true))
            {
                //Debug.Log($"[ChihiroInkVfx] TR Path='{GetTransformPath(trailRenderer.transform, particle.transform)}' Material='{GetMaterialName(trailRenderer.material)}' Shader='{GetShaderName(trailRenderer.material)}' StartColor='{trailRenderer.startColor}' EndColor='{trailRenderer.endColor}'");
                LogMaterialColorProperties(trailRenderer.material, trailRenderer.name);
            }
        }

        private static void LogMaterialColorProperties(Material material, string ownerName)
        {
            if (material == null)
            {
                return;
            }

            string[] commonColorKeys = { "_TintColor", "_Color", "_BaseColor", "_EmissionColor" };
            foreach (string colorKey in commonColorKeys)
            {
                if (material.HasProperty(colorKey))
                {
                    //Debug.Log($"[ChihiroInkVfx] MAT Owner='{ownerName}' Property='{colorKey}' Value='{material.GetColor(colorKey)}'");
                }
            }

            foreach (string colorPropertyName in GetShaderPropertyNames(material, "Color"))
            {
                if (Array.IndexOf(commonColorKeys, colorPropertyName) >= 0 || !material.HasProperty(colorPropertyName))
                {
                    continue;
                }

                //Debug.Log($"[ChihiroInkVfx] MAT Owner='{ownerName}' ShaderColorProperty='{colorPropertyName}' Value='{material.GetColor(colorPropertyName)}'");
            }

            if (ShouldLogMaterialDetails(material, ownerName))
            {
                LogAllShaderProperties(material, ownerName);
            }
        }

        private static string GetTransformPath(Transform current, Transform root)
        {
            List<string> path = new List<string>();
            Transform walker = current;
            while (walker != null)
            {
                path.Add(walker.name);
                if (walker == root)
                {
                    break;
                }

                walker = walker.parent;
            }

            path.Reverse();
            return string.Join("/", path);
        }

        private static string GetMaterialName(Material material)
        {
            return material == null ? "None" : material.name;
        }

        private static string NormalizeMaterialName(Material material)
        {
            string materialName = GetMaterialName(material);
            if (materialName == "None")
            {
                return materialName;
            }

            return materialName.Replace(" (Instance)", string.Empty).Trim();
        }

        private static string GetShaderName(Material material)
        {
            return material == null || material.shader == null ? "None" : material.shader.name;
        }

        private static bool ShouldLogMaterialDetails(Material material, string ownerName)
        {
            if (!DebugInkVfx || material == null)
            {
                return false;
            }

            string materialName = GetMaterialName(material);
            if (!DebugMaterialNames.Contains(materialName))
            {
                return false;
            }

            string logKey = ownerName + "::" + materialName;
            return LoggedMaterials.Add(logKey);
        }

        private static void LogAllShaderProperties(Material material, string ownerName)
        {
            if (material == null || material.shader == null)
            {
                return;
            }

            MethodInfo getPropertyCount = typeof(Shader).GetMethod("GetPropertyCount", Type.EmptyTypes);
            MethodInfo getPropertyName = typeof(Shader).GetMethod("GetPropertyName", new[] { typeof(int) });
            MethodInfo getPropertyType = typeof(Shader).GetMethod("GetPropertyType", new[] { typeof(int) });
            if (getPropertyCount == null || getPropertyName == null || getPropertyType == null)
            {
                return;
            }

            int propertyCount = (int)getPropertyCount.Invoke(material.shader, null);
            for (int i = 0; i < propertyCount; i++)
            {
                string propertyName = getPropertyName.Invoke(material.shader, new object[] { i }) as string;
                object propertyType = getPropertyType.Invoke(material.shader, new object[] { i });
                if (string.IsNullOrEmpty(propertyName) || propertyType == null)
                {
                    continue;
                }

                string typeName = propertyType.ToString();
                string value = GetMaterialPropertyValue(material, propertyName, typeName);
                //Debug.Log($"[ChihiroInkVfx] MATDETAIL Owner='{ownerName}' Material='{material.name}' Shader='{material.shader.name}' Property='{propertyName}' Type='{typeName}' Value='{value}'");
            }
        }

        private static string GetMaterialPropertyValue(Material material, string propertyName, string propertyTypeName)
        {
            try
            {
                switch (propertyTypeName)
                {
                    case "Color":
                        return material.GetColor(propertyName).ToString();
                    case "Vector":
                        return material.GetVector(propertyName).ToString();
                    case "Float":
                    case "Range":
                        return material.GetFloat(propertyName).ToString();
                    case "Texture":
                        Texture texture = material.GetTexture(propertyName);
                        return texture == null ? "None" : texture.name;
                    default:
                        return "Unsupported";
                }
            }
            catch (Exception ex)
            {
                return "Error: " + ex.Message;
            }
        }
    }


    [HarmonyPatch(typeof(BattleChar), "ParticleOutEnd", new Type[] { typeof(Skill), typeof(List<BattleChar>), typeof(Vector3) })]
    internal static class ChihiroInkVfxParticlePatch
    {
        private static void Postfix(Skill __0)
        {
            if (!ChihiroInkVfx.ShouldForceImmediateInk(__0))
            {
                return;
            }

            int frameCount = ChihiroInkVfx.GetRepeatFrameCount(__0);
            string skillKey = __0 != null && __0.MySkill != null ? __0.MySkill.KeyID : null;
            ChihiroInkVfx.BeginFirstCastTraceFromParticle(__0);

            ChihiroInkVfx.ApplyToSkill(__0);
            ChihiroInkVfx.ApplyToSkillKey(skillKey);
            BattleSystem.DelayInput(ChihiroInkVfx.ApplyToSkillRepeated(__0, frameCount));
            BattleSystem.DelayInput(ChihiroInkVfx.ApplyToSkillKeyRepeated(skillKey, frameCount));
        }
    }

    [HarmonyPatch(typeof(BattleChar), "ParticleOut", new Type[] { typeof(Skill), typeof(BattleChar) })]
    internal static class ChihiroInkVfxFirstCastTraceSinglePatch
    {
        private static void Prefix(Skill skill)
        {
            ChihiroInkVfx.BeginFirstCastTrace(skill);
        }
    }

    [HarmonyPatch(typeof(BattleChar), "ParticleOut", new Type[] { typeof(Skill), typeof(List<BattleChar>) })]
    internal static class ChihiroInkVfxFirstCastTraceMultiPatch
    {
        private static void Prefix(Skill skill)
        {
            ChihiroInkVfx.BeginFirstCastTrace(skill);
        }
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new Type[] { typeof(Skill), typeof(BattleChar), typeof(BattleChar) })]
    internal static class ChihiroInkVfxSkillParticleInitSinglePatch
    {
        private static void Postfix(Skill skill, BattleChar User, SkillParticle __instance)
        {
            if (!ChihiroInkVfx.ShouldTraceFirstCastParticle(User, skill))
            {
                return;
            }

            ChihiroInkVfx.TraceSpawnedParticle(skill, __instance);
        }
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new Type[] { typeof(Skill), typeof(BattleChar), typeof(List<BattleChar>) })]
    internal static class ChihiroInkVfxSkillParticleInitMultiPatch
    {
        private static void Postfix(Skill skill, BattleChar User, SkillParticle __instance)
        {
            if (!ChihiroInkVfx.ShouldTraceFirstCastParticle(User, skill))
            {
                return;
            }

            ChihiroInkVfx.TraceSpawnedParticle(skill, __instance);
        }
    }
}
