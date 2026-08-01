using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace RedMist
{
    internal static class RedMistParticleTint
    {
        private static readonly Color SpearTint = new Color(0.86f, 0.12f, 0.12f, 1f);
        private static readonly string[] SuppressedPathFragments = { "/glow", "/halo", "/shockwave-front" };
        private static readonly string[] SuppressedBranchSuffixes = { "glow", "halo", "shockwave-front" };
        private static readonly string[] SuppressedMaterialNames = { "glowCore", "glow-white", "flareGray_add", "shockwave-front-yellow" };
        private static readonly string[] SuppressedShaderNames = { "KY/add", "KY/add_hilight", "Custom/Particle" };
        private static readonly string[] SRedMist9SuppressedPathFragments = { "/smoke2", "/blood_fog" };
        private static readonly string[] SRedMist9SuppressedBranchSuffixes = { "Smoke2", "blood_fog" };
        private static readonly string[] SRedMist9SuppressedMaterialNames = { "smoke_kai_add", "smoke" };
        private static readonly string[] SRedMist9SuppressedShaderNames = { "Legacy Shaders/Particles/Additive" };
        internal const int ReapplyFrames = 24;

        public static bool ShouldTint(Skill skill)
        {
            string skillKey = skill?.MySkill?.KeyID;
            return skillKey == "S_RedMist_1" || skillKey == "S_RedMist_1_0" || skillKey == "S_RedMist_5" || skillKey == "S_RedMist_8" || skillKey == "S_RedMist_9" || skillKey == "S_RedMist_10";
        }

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

                ApplyToParticle(particle);
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
                if (particle == null || particle.SkillData?.MySkill == null)
                {
                    continue;
                }

                if (particle.SkillData.MySkill.KeyID != skillKey)
                {
                    continue;
                }

                ApplyToParticle(particle);
            }
        }

        public static void ApplyToParticle(SkillParticle particle)
        {
            if (particle == null)
            {
                return;
            }

            string skillKey = particle.SkillData?.MySkill?.KeyID;
            HideGlowSubeffects(particle, skillKey);

            var controller = particle.GetComponentInChildren<GAP_ParticleSystemController.ParticleSystemController>(true);
            if (controller != null)
            {
                controller.changeColor = true;
                controller.newMinColor = SpearTint;
                controller.newMaxColor = SpearTint;
                controller.ChangeColorOnly();
            }

            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                var main = particleSystem.main;
                main.startColor = new ParticleSystem.MinMaxGradient(SpearTint);

                var colorOverLifetime = particleSystem.colorOverLifetime;
                if (colorOverLifetime.enabled)
                {
                    colorOverLifetime.color = new ParticleSystem.MinMaxGradient(SpearTint);
                }

                var colorBySpeed = particleSystem.colorBySpeed;
                if (colorBySpeed.enabled)
                {
                    colorBySpeed.color = new ParticleSystem.MinMaxGradient(SpearTint);
                }

                var trails = particleSystem.trails;
                if (trails.enabled)
                {
                    trails.colorOverLifetime = new ParticleSystem.MinMaxGradient(SpearTint);
                    trails.colorOverTrail = new ParticleSystem.MinMaxGradient(SpearTint);
                }

                var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                {
                    SetMaterialColor(renderer.material, SpearTint);
                    SetMaterialColor(renderer.sharedMaterial, SpearTint);
                }
            }

            foreach (SpriteRenderer spriteRenderer in particle.GetComponentsInChildren<SpriteRenderer>(true))
            {
                spriteRenderer.color = spriteRenderer.enabled ? SpearTint : WithAlpha(SpearTint, 0f);
                SetMaterialColor(spriteRenderer.material, SpearTint);
                SetMaterialColor(spriteRenderer.sharedMaterial, SpearTint);
            }

            foreach (TrailRenderer trailRenderer in particle.GetComponentsInChildren<TrailRenderer>(true))
            {
                Color trailTint = trailRenderer.enabled ? SpearTint : WithAlpha(SpearTint, 0f);
                trailRenderer.startColor = trailTint;
                trailRenderer.endColor = trailTint;
                SetMaterialColor(trailRenderer.material, SpearTint);
                SetMaterialColor(trailRenderer.sharedMaterial, SpearTint);
            }
        }

        public static IEnumerator ReapplyTint(SkillParticle particle, int frames)
        {
            for (int i = 0; i < frames; i++)
            {
                yield return null;
                if (particle == null)
                {
                    yield break;
                }

                ApplyToParticle(particle);
            }
        }

        public static IEnumerator ApplyToSkillRepeated(Skill skill, int frames)
        {
            if (skill == null)
            {
                yield break;
            }

            for (int i = 0; i < frames; i++)
            {
                ApplyToSkill(skill);
                yield return null;
            }
        }

        public static IEnumerator ApplyToSkillKeyRepeated(string skillKey, int frames)
        {
            if (string.IsNullOrEmpty(skillKey))
            {
                yield break;
            }

            for (int i = 0; i < frames; i++)
            {
                ApplyToSkillKey(skillKey);
                yield return null;
            }
        }

        private static void HideGlowSubeffects(SkillParticle particle, string skillKey)
        {
            string[] suppressedBranchSuffixes = GetSuppressedBranchSuffixes(skillKey);
            string[] suppressedPathFragments = GetSuppressedPathFragments(skillKey);
            string[] suppressedMaterialNames = GetSuppressedMaterialNames(skillKey);
            string[] suppressedShaderNames = GetSuppressedShaderNames(skillKey);

            foreach (Transform child in particle.GetComponentsInChildren<Transform>(true))
            {
                string path = GetTransformPath(child, particle.transform);
                if (ShouldKeepSRedMist9Branch(skillKey, path))
                {
                    continue;
                }

                if (!MatchesAnySuffix(path, suppressedBranchSuffixes))
                {
                    continue;
                }

                HideBranchVisuals(child);
            }

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
                bool pathMatches = ContainsAnyFragment(path, suppressedPathFragments);
                bool materialMatches = MatchesAnyExact(materialName, suppressedMaterialNames);
                bool shaderMatches = MatchesAnyExact(shaderName, suppressedShaderNames);
                if (ShouldKeepSRedMist9Particle(skillKey, path, materialName))
                {
                    continue;
                }

                if (!materialMatches && !(shaderMatches && pathMatches))
                {
                    continue;
                }

                HideParticleRenderer(particleSystem, renderer);
            }
        }

        private static string[] GetSuppressedPathFragments(string skillKey)
        {
            return skillKey == "S_RedMist_9"
                ? CombineArrays(SuppressedPathFragments, SRedMist9SuppressedPathFragments)
                : SuppressedPathFragments;
        }

        private static string[] GetSuppressedBranchSuffixes(string skillKey)
        {
            return skillKey == "S_RedMist_9"
                ? CombineArrays(SuppressedBranchSuffixes, SRedMist9SuppressedBranchSuffixes)
                : SuppressedBranchSuffixes;
        }

        private static string[] GetSuppressedMaterialNames(string skillKey)
        {
            return skillKey == "S_RedMist_9"
                ? CombineArrays(SuppressedMaterialNames, SRedMist9SuppressedMaterialNames)
                : SuppressedMaterialNames;
        }

        private static string[] GetSuppressedShaderNames(string skillKey)
        {
            return skillKey == "S_RedMist_9"
                ? CombineArrays(SuppressedShaderNames, SRedMist9SuppressedShaderNames)
                : SuppressedShaderNames;
        }

        private static string[] CombineArrays(string[] first, string[] second)
        {
            if (first == null || first.Length == 0)
            {
                return second ?? Array.Empty<string>();
            }

            if (second == null || second.Length == 0)
            {
                return first;
            }

            string[] combined = new string[first.Length + second.Length];
            Array.Copy(first, combined, first.Length);
            Array.Copy(second, 0, combined, first.Length, second.Length);
            return combined;
        }

        private static bool ShouldKeepSRedMist9Branch(string skillKey, string path)
        {
            if (skillKey != "S_RedMist_9" || string.IsNullOrEmpty(path))
            {
                return false;
            }

            return path.EndsWith("/dust", StringComparison.OrdinalIgnoreCase);
        }

        private static bool ShouldKeepSRedMist9Particle(string skillKey, string path, string materialName)
        {
            if (skillKey != "S_RedMist_9" || string.IsNullOrEmpty(path))
            {
                return false;
            }

            if (path.EndsWith("/dust", StringComparison.OrdinalIgnoreCase) && string.Equals(materialName, "glowCore", StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            return false;
        }

        private static void HideBranchVisuals(Transform root)
        {
            foreach (ParticleSystem particleSystem in root.GetComponentsInChildren<ParticleSystem>(true))
            {
                HideParticleRenderer(particleSystem, particleSystem.GetComponent<ParticleSystemRenderer>());
            }

            foreach (SpriteRenderer spriteRenderer in root.GetComponentsInChildren<SpriteRenderer>(true))
            {
                spriteRenderer.enabled = false;
                spriteRenderer.color = WithAlpha(spriteRenderer.color, 0f);
                SetMaterialTransparent(spriteRenderer.material);
                SetMaterialTransparent(spriteRenderer.sharedMaterial);
            }

            foreach (TrailRenderer trailRenderer in root.GetComponentsInChildren<TrailRenderer>(true))
            {
                trailRenderer.enabled = false;
                trailRenderer.startColor = WithAlpha(trailRenderer.startColor, 0f);
                trailRenderer.endColor = WithAlpha(trailRenderer.endColor, 0f);
                SetMaterialTransparent(trailRenderer.material);
                SetMaterialTransparent(trailRenderer.sharedMaterial);
            }
        }

        private static void HideParticleRenderer(ParticleSystem particleSystem, ParticleSystemRenderer renderer)
        {
            if (particleSystem == null)
            {
                return;
            }

            var main = particleSystem.main;
            main.startColor = new ParticleSystem.MinMaxGradient(WithAlpha(SpearTint, 0f));

            var colorOverLifetime = particleSystem.colorOverLifetime;
            if (colorOverLifetime.enabled)
            {
                colorOverLifetime.color = new ParticleSystem.MinMaxGradient(WithAlpha(SpearTint, 0f));
            }

            var colorBySpeed = particleSystem.colorBySpeed;
            if (colorBySpeed.enabled)
            {
                colorBySpeed.color = new ParticleSystem.MinMaxGradient(WithAlpha(SpearTint, 0f));
            }

            var trails = particleSystem.trails;
            if (trails.enabled)
            {
                trails.colorOverLifetime = new ParticleSystem.MinMaxGradient(WithAlpha(SpearTint, 0f));
                trails.colorOverTrail = new ParticleSystem.MinMaxGradient(WithAlpha(SpearTint, 0f));
            }

            if (renderer != null)
            {
                renderer.enabled = false;
                SetMaterialTransparent(renderer.material);
                SetMaterialTransparent(renderer.sharedMaterial);
            }
        }

        private static void SetMaterialColor(Material material, Color tint)
        {
            if (material == null)
            {
                return;
            }

            string[] colorKeys = { "_TintColor", "_Color", "_BaseColor" };
            foreach (string colorKey in colorKeys)
            {
                if (material.HasProperty(colorKey))
                {
                    material.SetColor(colorKey, tint);
                }
            }

            if (material.HasProperty("_EmissionColor"))
            {
                material.SetColor("_EmissionColor", Color.black);
            }

            material.DisableKeyword("_EMISSION");
        }

        private static void SetMaterialTransparent(Material material)
        {
            if (material == null)
            {
                return;
            }

            string[] colorKeys = { "_TintColor", "_Color", "_BaseColor", "_EmissionColor" };
            foreach (string colorKey in colorKeys)
            {
                if (material.HasProperty(colorKey))
                {
                    material.SetColor(colorKey, Color.clear);
                }
            }

            material.DisableKeyword("_EMISSION");
        }

        private static Color WithAlpha(Color color, float alpha)
        {
            color.a = alpha;
            return color;
        }

        private static bool ContainsAnyFragment(string text, string[] fragments)
        {
            if (string.IsNullOrEmpty(text) || fragments == null)
            {
                return false;
            }

            foreach (string fragment in fragments)
            {
                if (!string.IsNullOrEmpty(fragment) && text.IndexOf(fragment, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesAnySuffix(string text, string[] suffixes)
        {
            if (string.IsNullOrEmpty(text) || suffixes == null)
            {
                return false;
            }

            foreach (string suffix in suffixes)
            {
                if (!string.IsNullOrEmpty(suffix) && text.EndsWith(suffix, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static bool MatchesAnyExact(string text, string[] values)
        {
            if (string.IsNullOrEmpty(text) || values == null)
            {
                return false;
            }

            foreach (string value in values)
            {
                if (string.Equals(text, value, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
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
            return string.Join("/", path.ToArray());
        }

        private static string NormalizeMaterialName(Material material)
        {
            string materialName = material == null ? "None" : material.name;
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
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new[] { typeof(Skill), typeof(BattleChar), typeof(BattleChar) })]
    internal static class RedMistParticleTintSinglePatch
    {
        private static void Postfix(Skill skill, SkillParticle __instance)
        {
            if (!RedMistParticleTint.ShouldTint(skill))
            {
                return;
            }

            RedMistParticleTint.ApplyToParticle(__instance);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(RedMistParticleTint.ReapplyTint(__instance, RedMistParticleTint.ReapplyFrames));
        }
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new[] { typeof(Skill), typeof(BattleChar), typeof(List<BattleChar>) })]
    internal static class RedMistParticleTintMultiPatch
    {
        private static void Postfix(Skill skill, SkillParticle __instance)
        {
            if (!RedMistParticleTint.ShouldTint(skill))
            {
                return;
            }

            RedMistParticleTint.ApplyToParticle(__instance);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(RedMistParticleTint.ReapplyTint(__instance, RedMistParticleTint.ReapplyFrames));
        }
    }

    [HarmonyPatch(typeof(BattleChar), "ParticleOutEnd", new[] { typeof(Skill), typeof(List<BattleChar>), typeof(Vector3) })]
    internal static class RedMistParticleTintParticleOutEndPatch
    {
        private static void Postfix(Skill __0)
        {
            if (!RedMistParticleTint.ShouldTint(__0))
            {
                return;
            }

            string skillKey = __0?.MySkill?.KeyID;
            RedMistParticleTint.ApplyToSkill(__0);
            RedMistParticleTint.ApplyToSkillKey(skillKey);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(RedMistParticleTint.ApplyToSkillRepeated(__0, RedMistParticleTint.ReapplyFrames));
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(RedMistParticleTint.ApplyToSkillKeyRepeated(skillKey, RedMistParticleTint.ReapplyFrames));
        }
    }
}







