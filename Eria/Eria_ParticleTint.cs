using System;
using System.Collections;
using System.Collections.Generic;
using HarmonyLib;
using UnityEngine;

namespace Eria
{
    // Recolors skill particles to match each skill's paint color. Same mechanism as RedMist_ParticleTint.
    internal static class EriaParticleTint
    {
        private static readonly Color Red = new Color(0.86f, 0.12f, 0.12f, 1f);
        private static readonly Color Blue = new Color(0.20f, 0.40f, 0.95f, 1f);
        private static readonly Color Yellow = new Color(1.00f, 0.85f, 0.20f, 1f);
        private static readonly Color Purple = new Color(0.55f, 0.20f, 0.75f, 1f);
        private static readonly Color Orange = new Color(1.00f, 0.50f, 0.10f, 1f);
        private static readonly Color Green = new Color(0.30f, 0.80f, 0.30f, 1f);
        // Per-skill shades within each color
        private static readonly Color LightRed = new Color(1.00f, 0.62f, 0.60f, 1f);     // Coup de Grâce
        private static readonly Color SkyBlue = new Color(0.45f, 0.75f, 1.00f, 1f);      // Cieux Bleus
        private static readonly Color Turquoise = new Color(0.25f, 0.85f, 0.80f, 1f);    // La Rivière
        private static readonly Color Wheat = new Color(0.92f, 0.58f, 0.02f, 1f);        // Récolte d'Automne: deep golden wheat (heal flares lighten it, so it starts darker)
        private static readonly Color LightYellow = new Color(1.00f, 0.95f, 0.58f, 1f);  // Hirondelle Sacrée
        private static readonly Color Black = new Color(0.19f, 0.19f, 0.22f, 1f); // ink black, still visible on additive particles
        internal const int ReapplyFrames = 24;

        // Particle/flare_lv1 layers (from output_log): circle, core, dust, dust2, flare1, flare2, glow, sw.
        // Every layer stays visible; the big ones (center glow, streak, burst, shockwave) get a weaker tint so the colored heal isn't glaring.
        private static readonly Dictionary<string, float> FlareLayerAlpha = new Dictionary<string, float>
        {
            { "glow", 0.4f },
            { "flare1", 0.35f },
            { "flare2", 0.4f },
            { "sw", 0.4f },
            { "circle", 0.7f },
            { "flare_lv1(Clone)", 0.7f }, // the root layer is also a ring (circle material)
            { "core", 0.7f },
        };
        private const float FlareLighten = 0.45f;

        // Putréfaction (Particle/Enemy/KitchenMaid_1) layers (from output_log): Smoke2, dust, effect/ShockWave (1), effect/WatesSplash,
        // effect/blood_fog, effect/distortion, effect/flash (1). The smoke/fog/glow layers left a strong colored haze when fully tinted,
        // so like flare_lv1 they keep their shape but get a weaker tint.
        private static readonly HashSet<string> HazeSkills = new HashSet<string> { "S_Eria_RedBlue" };
        private static readonly Dictionary<string, float> HazeLayerAlpha = new Dictionary<string, float>
        {
            { "dust", 0.5f },
            { "flash (1)", 0.5f },
        };
        // Attack particles (e.g. VioletSlash, Public_5, ShadowPriest_7 from output_log) have big additive glow layers that turn into a
        // large colored halo when fully tinted, so those layers get a weaker tint on every other tinted skill.
        private static readonly Dictionary<string, float> GlowLayerAlpha = new Dictionary<string, float>
        {
            { "glow", 0.35f },
            { "glow 2", 0.35f },
            { "flash (1)", 0.4f },
            // Particle/Buff (Hirondelle Sacrée etc.): glowBall root, flash, light and dustGlow make the big glow
            { "Buff(Clone)", 0.35f },
            { "flash", 0.4f },
            { "light", 0.35f },
            { "dustGlow", 0.5f },
        };

        // The fog that lingers after the hit is hidden entirely
        private static readonly string[] HazeHiddenLayers = { "blood_fog", "Smoke2" };
        private static readonly HashSet<string> LoggedParticles = new HashSet<string>();

        private static readonly Dictionary<string, Color> Tints = new Dictionary<string, Color>
        {
            { "S_Eria_1", Red },
            { "S_Eria_2", Red },
            { "S_Eria_3", LightRed },
            { "S_Eria_4", Blue },
            { "S_Eria_5", SkyBlue },
            { "S_Eria_6", Turquoise },
            { "S_Eria_7", Yellow },
            { "S_Eria_8", Wheat },
            { "S_Eria_9", LightYellow },
            { "S_Eria_10_0", Red },
            { "S_Eria_10_1", Blue },
            { "S_Eria_10_2", Yellow },
            { "S_Eria_BlueBlue", Blue },
            { "S_Eria_YellowYellow", Yellow },
            { "S_Eria_RedBlue", Purple },
            { "S_Eria_RedYellow", Orange },
            { "S_Eria_BlueYellow", Green },
            { "S_Eria_RedBlueYellow", Black },
        };

        public static bool TryGetTint(Skill skill, out Color tint)
        {
            string skillKey = skill?.MySkill?.KeyID;
            if (skillKey != null && Tints.TryGetValue(skillKey, out tint))
            {
                return true;
            }
            tint = Color.white;
            return false;
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

        public static void ApplyToParticle(SkillParticle particle)
        {
            Color tint;
            if (particle == null || !TryGetTint(particle.SkillData, out tint))
            {
                return;
            }

            bool flare = IsFlare(particle);
            bool haze = HazeSkills.Contains(particle.SkillData.MySkill.KeyID);
            LogLayers(particle);

            // The effect's own color controller (GAP ParticleSystemController) isn't used: it recolors every layer the same way,
            // which would undo the per-layer strengths below.

            if (flare)
            {
                // Heal flares use a lighter, pastel version of the color so they aren't too sharp
                tint = Color.Lerp(tint, Color.white, FlareLighten);
            }
            Color baseTint = tint;
            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                tint = baseTint;
                if (haze && Array.IndexOf(HazeHiddenLayers, particleSystem.name) >= 0)
                {
                    var hidden = particleSystem.GetComponent<ParticleSystemRenderer>();
                    if (hidden != null)
                    {
                        hidden.enabled = false;
                    }
                    continue;
                }
                float alpha;
                if ((flare && FlareLayerAlpha.TryGetValue(particleSystem.name, out alpha))
                    || (haze && HazeLayerAlpha.TryGetValue(particleSystem.name, out alpha))
                    || (!flare && !haze && GlowLayerAlpha.TryGetValue(particleSystem.name, out alpha)))
                {
                    tint.a = alpha;
                }

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

                var renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                if (renderer != null)
                {
                    SetMaterialColor(renderer.material, tint);
                }
            }

            tint = baseTint;
            foreach (SpriteRenderer spriteRenderer in particle.GetComponentsInChildren<SpriteRenderer>(true))
            {
                spriteRenderer.color = tint;
                SetMaterialColor(spriteRenderer.material, tint);
            }

            foreach (TrailRenderer trailRenderer in particle.GetComponentsInChildren<TrailRenderer>(true))
            {
                trailRenderer.startColor = tint;
                trailRenderer.endColor = tint;
                SetMaterialColor(trailRenderer.material, tint);
            }
        }

        // Particles reset some of their colors over the first frames, so keep reapplying for a short while
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
            for (int i = 0; i < frames; i++)
            {
                ApplyToSkill(skill);
                yield return null;
            }
        }

        private static bool IsFlare(SkillParticle particle)
        {
            return particle.name.IndexOf("flare_lv1", StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private static string GetTransformPath(Transform current, Transform root)
        {
            List<string> path = new List<string>();
            for (Transform walker = current; walker != null; walker = walker.parent)
            {
                path.Add(walker.name);
                if (walker == root)
                {
                    break;
                }
            }
            path.Reverse();
            return string.Join("/", path.ToArray());
        }

        // Writes each layer of the particle to output_log.txt once, to see which layers are worth hiding
        private static void LogLayers(SkillParticle particle)
        {
            if (!LoggedParticles.Add(particle.name))
            {
                return;
            }
            foreach (ParticleSystem particleSystem in particle.GetComponentsInChildren<ParticleSystem>(true))
            {
                ParticleSystemRenderer renderer = particleSystem.GetComponent<ParticleSystemRenderer>();
                Material m = renderer != null ? renderer.sharedMaterial : null;
                Debug.Log("[EriaTint] " + particle.name + " PS '" + GetTransformPath(particleSystem.transform, particle.transform) + "' Material='" + MaterialName(m) + "' Shader='" + (m != null && m.shader != null ? m.shader.name : "None") + "'");
            }
            foreach (SpriteRenderer spriteRenderer in particle.GetComponentsInChildren<SpriteRenderer>(true))
            {
                Debug.Log("[EriaTint] " + particle.name + " SR '" + spriteRenderer.name + "' Material='" + MaterialName(spriteRenderer.sharedMaterial) + "'");
            }
        }

        private static string MaterialName(Material material)
        {
            return material == null ? "None" : material.name.Replace(" (Instance)", string.Empty).Trim();
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
        }
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new[] { typeof(Skill), typeof(BattleChar), typeof(BattleChar) })]
    internal static class EriaParticleTintSinglePatch
    {
        private static void Postfix(Skill skill, SkillParticle __instance)
        {
            Color tint;
            if (!EriaParticleTint.TryGetTint(skill, out tint))
            {
                return;
            }

            EriaParticleTint.ApplyToParticle(__instance);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(EriaParticleTint.ReapplyTint(__instance, EriaParticleTint.ReapplyFrames));
        }
    }

    [HarmonyPatch(typeof(SkillParticle), "init", new[] { typeof(Skill), typeof(BattleChar), typeof(List<BattleChar>) })]
    internal static class EriaParticleTintMultiPatch
    {
        private static void Postfix(Skill skill, SkillParticle __instance)
        {
            Color tint;
            if (!EriaParticleTint.TryGetTint(skill, out tint))
            {
                return;
            }

            EriaParticleTint.ApplyToParticle(__instance);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(EriaParticleTint.ReapplyTint(__instance, EriaParticleTint.ReapplyFrames));
        }
    }

    [HarmonyPatch(typeof(BattleChar), "ParticleOutEnd", new[] { typeof(Skill), typeof(List<BattleChar>), typeof(Vector3) })]
    internal static class EriaParticleTintParticleOutEndPatch
    {
        private static void Postfix(Skill __0)
        {
            Color tint;
            if (!EriaParticleTint.TryGetTint(__0, out tint))
            {
                return;
            }

            EriaParticleTint.ApplyToSkill(__0);
            if (BattleSystem.instance != null) BattleSystem.instance.StartCoroutine(EriaParticleTint.ApplyToSkillRepeated(__0, EriaParticleTint.ReapplyFrames));
        }
    }
}
