using System.Threading;
using BTD_Mod_Helper;
using BTD_Mod_Helper.Api.Components;
using BTD_Mod_Helper.Api.Display;
using BTD_Mod_Helper.Api.Towers;
using BTD_Mod_Helper.Extensions;
using HarmonyLib;
using Il2Cpp;
using Il2CppAssets.Scripts;
using Il2CppAssets.Scripts.Models;
using Il2CppAssets.Scripts.Models.Effects;
using Il2CppAssets.Scripts.Models.GenericBehaviors;
using Il2CppAssets.Scripts.Models.Map;
using Il2CppAssets.Scripts.Models.Profile;
using Il2CppAssets.Scripts.Models.Towers;
using Il2CppAssets.Scripts.Models.Towers.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Abilities.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Attack.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.Behaviors.Emissions;
using Il2CppAssets.Scripts.Models.Towers.Filters;
using Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors;
using Il2CppAssets.Scripts.Models.Towers.TowerFilters;
using Il2CppAssets.Scripts.Models.Towers.Weapons.Behaviors;
using Il2CppAssets.Scripts.Models.TowerSets;
using Il2CppAssets.Scripts.Simulation.Objects;
using Il2CppAssets.Scripts.Simulation.SimulationBehaviors;
using Il2CppAssets.Scripts.Simulation.SMath;
using Il2CppAssets.Scripts.Simulation.Towers;
using Il2CppAssets.Scripts.Simulation.Towers.Behaviors;
using Il2CppAssets.Scripts.Simulation.Towers.Projectiles;
using Il2CppAssets.Scripts.Simulation.Towers.Weapons;
using Il2CppAssets.Scripts.Unity;
using Il2CppAssets.Scripts.Unity.Display;
using Il2CppAssets.Scripts.Unity.Towers;
using Il2CppAssets.Scripts.Unity.Towers.Projectiles;
using Il2CppAssets.Scripts.Unity.Towers.Weapons;
using Il2CppAssets.Scripts.Unity.UI_New.InGame;
using Il2CppAssets.Scripts.Utils;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppNewtonsoft.Json.Utilities;
using MelonLoader;
using MonkeyBarack;
using Monkeys;
using UnityEngine;
using UnityEngine.Playables;
using weapondisplays;
using static Il2CppNinjaKiwi.NKMulti.IO.MessageReader;

[assembly: MelonInfo(typeof(MonkeyBarack.MonkeyBarack), ModHelperData.Name, ModHelperData.Version, ModHelperData.RepoOwner)]
[assembly: MelonGame("Ninja Kiwi", "BloonsTD6")]
namespace weapondisplays
{
    public class DisplayBlank : ModDisplay
    {
        public override string BaseDisplay => Generic2dDisplay;

        public override void ModifyDisplayNode(UnityDisplayNode node)
        {
            Set2DTexture(node, "BananaDisplay");
        }
    }
}
namespace MonkeyBarack
{

    public class MonkeyBarack : BloonsTD6Mod
    {

        public override void OnApplicationStart()
        {
            ModHelper.Msg<MonkeyBarack>("MonkeyBarack loaded!");
        }
        public class MonkeyBarackTower : ModTower
        {
            public override TowerSet TowerSet => TowerSet.Military;
            public override string BaseTower => TowerType.DartMonkey;
            public override int Cost => 800;
            public override string DisplayName => "Monkey Barack";
            public override string Name => "MonkeyBarack";
            public override int TopPathUpgrades => 5;
            public override int MiddlePathUpgrades => 5;
            public override int BottomPathUpgrades => 5;
            public override string Description => "The Barack Monkey send monkeys to the battle that walk along the path and rush into Bloons.";
            public override string Icon => "000";
            public override string Portrait => "000";

            public override ParagonMode ParagonMode => ParagonMode.Base000;
            public override bool IsValidCrosspath(int[] tiers) =>
       ModHelper.HasMod("UltimateCrosspathing") || base.IsValidCrosspath(tiers);

            public override void ModifyBaseTowerModel(TowerModel towerModel)
            {
                towerModel.RemoveBehaviors<NecromancerZoneModel>();
                towerModel.RemoveBehaviors<AttackModel>();
                towerModel.display = Game.instance.model.GetTower(TowerType.MonkeyVillage).display;
                towerModel.GetBehavior<DisplayModel>().display = Game.instance.model.GetTower(TowerType.MonkeyVillage).display;
                towerModel.radius = Game.instance.model.GetTower(TowerType.MonkeyVillage).radius;
                var agemodel = Game.instance.model.GetTowerFromId("SpikeFactory").GetAttackModel().weapons[0].projectile.GetBehavior<AgeModel>().Duplicate();
                var summon = Game.instance.model.GetTowerFromId("WizardMonkey-004").GetAttackModel(2).Duplicate();
                summon.weapons[0].projectile.name = "AttackModel_Summon3_";
                summon.weapons[0].emission = new NecromancerEmissionModel("BarackSummon_", 99999999, 999999, 1, 1, 1, 50, 0, null, null, null, 1, 100, 1, 1, 2);
                summon.weapons[0].rate = 12f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().disableRotateWithPathDirection = false;
                summon.name = "AttackModel_Summon_";
                summon.weapons[0].projectile.AddBehavior(new ClearHitBloonsModel("Clearhit", 1.5f));
                summon.weapons[0].projectile.GetDamageModel().damage = 2;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartMonkey").display;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.Lead;
                summon.weapons[0].projectile.pierce = 6;
                summon.range = 100000;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().lifespanFrames = 99999;
                summon.weapons[0].projectile.RemoveBehavior<CreateEffectOnExhaustedModel>();
                agemodel.lifespanFrames = 99999;
                agemodel.lifespan = 99999;
                agemodel.rounds = 9999;
                towerModel.range = 20;
                summon.weapons[0].projectile.AddBehavior(agemodel);
                towerModel.AddBehavior(summon);
            }

        }

        public class NinjaArmy : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => TOP;
            public override int Tier => 1;
            public override int Cost => 600;
            public override string Icon => "100";

            public override string DisplayName => "Ninja Army";
            public override string Description => "Spawn in a ninja monkey every few seconds.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 7f;
                summon.name = "AttackModel_Ninja_";
                summon.weapons[0].projectile.GetDamageModel().damage = 1;
                summon.weapons[0].projectile.pierce = 8;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("NinjaMonkey").display;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.75f;
                towerModel.AddBehavior(summon);

            }
        }
        public class FustyParty : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => TOP;
            public override int Tier => 2;
            public override int Cost => 1000;
            public override string Icon => "200";

            public override string DisplayName => "Fusty Party";
            public override string Description => "Fusty love party. But when the Bloons are here, it squashing time";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 30f;
                summon.name = "AttackModel_Fusty_";
                summon.weapons[0].projectile.GetDamageModel().damage = 20;
                summon.weapons[0].projectile.pierce = 3;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("PatFusty").display;
                summon.weapons[0].projectile.scale = 0.9f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.6f;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.AddBehavior(summon);

            }
        }
        public class TruckNotRotorDisplay : ModDisplay
        {
            public override PrefabReference BaseDisplayReference => Game.instance.model.GetTower(TowerType.HeliPilot, 0, 4, 0).GetBehavior<AirUnitModel>().display;
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                node.RemoveBone("MonkeyHeliRig:Top_Rotor");
                node.RemoveBone("MonkeyHeliRig:Top_RotorExtra");
            }
        }
        public class ReinforcedBarack : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => TOP;
            public override int Tier => 3;
            public override int Cost => 5000;
            public override string Icon => "300";

            public override string DisplayName => "Reinforced Barack";
            public override string Description => "Now send deadly trucks on the track. When they die, it unleash a big explosion and spawn 2 Officers";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 28;
                summon.name = "AttackModel_Truck_";
                summon.weapons[0].projectile.GetDamageModel().damage = 100;
                summon.weapons[0].projectile.pierce = -1;
                summon.weapons[0].projectile.maxPierce = 1;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.3f;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                summon.weapons[0].projectile.name = "ProjectileModel_MonkeySpawnTruck_";

                summon.weapons[0].projectile.ApplyDisplay<TruckNotRotorDisplay>();
                var projectile = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var explosion = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                var effect = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var sound = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                var CreateTower = Game.instance.model.GetTowerFromId("EngineerMonkey-200").Duplicate();

                explosion.projectile.GetDamageModel().damage = 35;
                explosion.projectile.pierce = 20;
                explosion.projectile.radius *= 2.5f;
                effect.effectModel.scale *= 2.5f;
                projectile.projectile = CreateTower.GetAttackModel(1).weapons[0].projectile;
                projectile.projectile.RemoveBehavior<AgeModel>();
                projectile.projectile.RemoveBehavior<ArriveAtTargetModel>();
                projectile.projectile.AddBehavior(new TravelStraitModel("TravelModel", 100, 0.07f));
                projectile.projectile.ApplyDisplay<DisplayBlank>();
                projectile.projectile.RemoveBehavior<CreateTowerModel>();
                projectile.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<Officer>().Duplicate(), 0f, false, false, false, false, true));
                projectile.emission = new ArcEmissionModel("Arc", 2, 0, 360, null, false, false);

                summon.weapons[0].projectile.AddBehavior(projectile);
                summon.weapons[0].projectile.AddBehavior(explosion);
                summon.weapons[0].projectile.AddBehavior(effect);
                summon.weapons[0].projectile.AddBehavior(sound);
                towerModel.AddBehavior(summon);

            }
        }
        public class MilitarySquad : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => TOP;
            public override int Tier => 4;
            public override int Cost => 12500;
            public override string Icon => "400";

            public override string DisplayName => "Military Squad";
            public override string Description => "The truck is now armored, dealing more explosion damage and now spawn 1 Officer and 1 Soldier that deal good damage.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Truck_");
                summon.weapons[0].rate = 23;
                summon.weapons[0].projectile.GetDamageModel().damage = 300;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.1f;

                var projectile = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>();
                var projectile2 = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var explosion = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
                var effect = summon.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>();
                var sound = summon.weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>();

                explosion.projectile.GetDamageModel().damage = 75;
                explosion.projectile.pierce = 50;
                explosion.projectile.radius *= 1.4f;
                effect.effectModel.scale *= 1.4f;
                projectile.emission = new RandomArcEmissionModel("Arc", 3, 0, 360, 360, 0, null);
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.range += 5;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().range += 5;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].rate *= 0.85f;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 3;
                projectile.projectile.GetBehavior<TravelStraitModel>().lifespan = 0.12f;
                projectile2.emission = new ArcEmissionModel("Arc", 1, 0, 360, null, false, false);
                projectile2.projectile.GetBehavior<TravelStraitModel>().lifespan = 0.03f;
                projectile2.projectile.RemoveBehavior<CreateTowerModel>();
                projectile2.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<Soldier>().Duplicate(), 0f, false, false, false, false, true));

                summon.weapons[0].projectile.RemoveBehavior<CreateProjectileOnExhaustFractionModel>();

                projectile2.projectile.AddBehavior(projectile);
                summon.weapons[0].projectile.AddBehavior(projectile2);
            }
        }
        public class ArmmoredTruck : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => TOP;
            public override int Tier => 5;
            public override int Cost => 115000;
            public override string Icon => "500";

            public override string DisplayName => "Armored Truck";
            public override string Description => "Strongest military army...";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Truck_");
                summon.weapons[0].rate = 20;
                summon.weapons[0].projectile.GetDamageModel().damage = 800;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.25f;

                var projectile = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>();
                var projectile2 = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>();
                var projectile3 = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var explosion = summon.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>();
                var effect = summon.weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>();
                var sound = summon.weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>();

                explosion.projectile.GetDamageModel().damage = 400;
                explosion.projectile.pierce = 150;
                explosion.projectile.radius *= 1.2f;
                effect.effectModel.scale *= 1.2f;
                projectile.emission = new ArcEmissionModel("Arc", 2, 0, 360, null, false, false);
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.range += 4;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.name = "Better Officer";
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().range += 4;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].rate *= 0.65f;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 8;
                projectile.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Bad", 3, 0, false, false));
                projectile.projectile.GetBehavior<TravelStraitModel>().lifespan = 0.14f;
                projectile2.emission = new ArcEmissionModel("Arc", 2, 0, 360, null, false, false);
                projectile2.projectile.GetBehavior<TravelStraitModel>().lifespan = 0.09f;
                projectile2.projectile.RemoveBehavior<CreateTowerModel>();
                projectile2.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<Soldier>().Duplicate(), 0f, false, false, false, false, true));
                projectile2.projectile.GetBehavior<CreateTowerModel>().tower.range += 8;
                projectile2.projectile.GetBehavior<CreateTowerModel>().tower.name = "Better Soldier";
                projectile2.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().range += 8;
                projectile2.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].rate *= 0.5f;
                projectile2.projectile.GetBehavior<CreateTowerModel>().tower.GetAttackModel().weapons[0].projectile.GetDamageModel().damage += 70;

                projectile3.emission = new ArcEmissionModel("Arc", 1, 0, 360, null, false, false);
                projectile3.projectile.GetBehavior<TravelStraitModel>().lifespan = 0.01f;
                projectile3.projectile.RemoveBehavior<CreateTowerModel>();
                projectile3.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<FlameThrower>().Duplicate(), 0f, false, false, false, false, true));

                summon.weapons[0].projectile.RemoveBehavior<CreateProjectileOnExhaustFractionModel>();
                projectile2.projectile.AddBehavior(projectile);
                projectile3.projectile.AddBehavior(projectile2);
                summon.weapons[0].projectile.AddBehavior(projectile3);

                var dart = towerModel.GetAttackModel("AttackModel_Summon_");
                dart.weapons[0].rate /= 2;
                dart.weapons[0].projectile.GetDamageModel().damage += 28;
                dart.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartMonkey-202").display;
                dart.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                dart.weapons[0].projectile.pierce += 20;

                var ninja = towerModel.GetAttackModel("AttackModel_Ninja_");
                ninja.weapons[0].rate /= 2;
                ninja.weapons[0].projectile.GetDamageModel().damage += 15;
                ninja.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("NinjaMonkey-302").display;
                ninja.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                ninja.weapons[0].projectile.pierce += 60;

                var fusty = towerModel.GetAttackModel("AttackModel_Fusty_");
                fusty.weapons[0].rate /= 1.75f;
                fusty.weapons[0].projectile.GetDamageModel().damage += 130;
                fusty.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("PatFusty 10").display;
                fusty.weapons[0].projectile.pierce += 6;
            }
        }
        public class PiercingTraining : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => MIDDLE;
            public override int Tier => 1;
            public override int Cost => 750;
            public override string Icon => "010";
            public override string DisplayName => "Piercing Training";
            public override string Description => "Troops gain more pierce";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                foreach (var attacks in towerModel.GetAttackModels())
                {

                    if (attacks.name.Contains("Summon"))
                    {
                        attacks.weapons[0].projectile.pierce += 3;
                    }
                }

            }
        }
        public class FasterTraining : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => MIDDLE;
            public override int Tier => 2;
            public override int Cost => 1250;
            public override string Icon => "020";

            public override string DisplayName => "Faster Training";
            public override string Description => "Troops spawn faster";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                foreach (var attacks in towerModel.GetAttackModels())
                {
                    if (attacks.name.Contains("Summon"))
                    {
                        attacks.weapons[0].rate *= 0.8f;
                    }

                }

            }

        }
        public class SuperArmy : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => MIDDLE;
            public override int Tier => 3;
            public override int Cost => 2500;
            public override string Icon => "030";

            public override string DisplayName => "Super Army";
            public override string Description => "Now send Super Monkey at a fast rate.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 0.75f;
                summon.name = "AttackModel_Super_";
                summon.weapons[0].projectile.GetDamageModel().damage = 2;
                summon.weapons[0].projectile.pierce = 7;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("SuperMonkey").display;
                summon.weapons[0].projectile.scale *= 0.8f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.8f;
                towerModel.AddBehavior(summon);

            }
        }
        public class AdvancedTechBarack : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => MIDDLE;
            public override int Tier => 4;
            public override int Cost => 14500;
            public override string Icon => "040";

            public override string DisplayName => "Advanced Tech Barack";
            public override string Description => "Every Bloons in his range move slower. Missile Barrage: Send a burst of deadly missiles that destroy most Bloons on screen.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var abilityModel = Game.instance.model.GetTowerFromId("Ezili 3").GetAbility().Duplicate();
                abilityModel.RemoveBehavior<ActivateAttackModel>();
                abilityModel.name = "AbilityModel_MissileBarrage";
                abilityModel.icon = GetSpriteReference<MonkeyBarack>("040");
                abilityModel.displayName = "Missile Barrage";
                abilityModel.description = "Send a burst of deadly missiles that destroy most Bloons on screen.";
                abilityModel.cooldown = 60f;
                abilityModel.RemoveBehaviors<CreateSoundOnAbilityModel>();
                abilityModel.RemoveBehaviors<CreateEffectOnAbilityModel>();


                var activateAttackModel = new ActivateAttackModel("ActivateAttackModel_MissileBarrage", 5, true, new Il2CppReferenceArray<AttackModel>(1), true, false, false, false, false);
                abilityModel.AddBehavior(activateAttackModel);

                var attackModel = activateAttackModel.attacks[0] = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 5).GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();

                attackModel.attackThroughWalls = true;
                attackModel.range = 2000;
                activateAttackModel.AddChildDependant(attackModel);
                towerModel.range += 20;
                towerModel.AddBehavior(new SlowBloonsZoneModel("SlowZone", 40, "Ice:Regular:ArcticWind", true, new Il2CppReferenceArray<FilterModel>(new FilterModel[] { new FilterInvisibleModel("Camo", true, false) }), 0.5f, 0, true, 5, "", false));
                var weaponModel = attackModel.weapons[0];
                weaponModel.rate = 0.01f;
                weaponModel.emission = new RandomTargetSpreadModel("Spread", 200, null, null);
                weaponModel.SetProjectile(Game.instance.model.GetTower(TowerType.MonkeySub, 0, 3).GetAttackModel(1).weapons[0].projectile.Duplicate());
                var projectileModel = weaponModel.projectile;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 25;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.radius *= 3;
                projectileModel.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId = Game.instance.model.GetTower(TowerType.MortarMonkey, 4).GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.collisionPasses = new[] { -1, 0 };
                var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModel>().Duplicate();
                var stuntag = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModifierForTagModel>().Duplicate();
                var moabstun = Game.instance.model.GetTowerFromId("BombShooter-500").GetDescendant<SlowModel>().Duplicate();
                moabstun.lifespan = 0.2f;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stuntag);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(moabstun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                towerModel.AddBehavior(abilityModel);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);


            }

        }
        public class OperationBloonsDestruction : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => MIDDLE;
            public override int Tier => 5;
            public override int Cost => 200000;
            public override string Icon => "050";

            public override string DisplayName => "Operation: Bloons Destruction";
            public override string Description => "Map Annihilator: Destroy every Plastic piece on the Map.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var abilityModel = towerModel.GetAbility();
                abilityModel.RemoveBehavior<ActivateAttackModel>();
                abilityModel.name = "AbilityModel_MapAnnihilator";
                abilityModel.displayName = "Map Annihilator";
                abilityModel.description = "Destroy every Plastic piece on the Map.";
                abilityModel.cooldown = 120;
                abilityModel.icon = GetSpriteReference<MonkeyBarack>("050");
                abilityModel.RemoveBehavior<ActivateAttackModel>();

                var activateAttackModel = new ActivateAttackModel("ActivateAttackModel_MapAnnihilator", 10, true, new Il2CppReferenceArray<AttackModel>(1), true, false, false, false, false);
                abilityModel.AddBehavior(activateAttackModel);

                var attackModel = activateAttackModel.attacks[0] = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 5).GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();

                attackModel.attackThroughWalls = true;
                attackModel.range = 2000;
                activateAttackModel.AddChildDependant(attackModel);
                towerModel.range += 10;

                var weaponModel = attackModel.weapons[0];
                weaponModel.rate = 0.0035f;
                weaponModel.emission = new RandomTargetSpreadModel("Spread", 200, null, null);
                weaponModel.SetProjectile(Game.instance.model.GetTower(TowerType.MonkeySub, 0, 3).GetAttackModel(1).weapons[0].projectile.Duplicate());
                var projectileModel = weaponModel.projectile;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 175;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Bad", 2, 0, false, false));
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Ceramic", 2, 0, false, false));
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.radius *= 5;
                projectileModel.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId = Game.instance.model.GetTower(TowerType.MortarMonkey, 5).GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.collisionPasses = new[] { -1, 0 };
                var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModel>().Duplicate();
                var stuntag = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModifierForTagModel>().Duplicate();
                var moabstun = Game.instance.model.GetTowerFromId("BombShooter-500").GetDescendant<SlowModel>().Duplicate();
                var superbrittle = Game.instance.model.GetTowerFromId("IceMonkey-500").GetDescendant<AddBonusDamagePerHitToBloonModel>();
                superbrittle.perHitDamageAddition = 8;
                moabstun.lifespan = 1f;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stuntag);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(moabstun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(superbrittle);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                var summon = towerModel.GetAttackModel("AttackModel_Super_");
                summon.weapons[0].rate = 0.45f;
                summon.weapons[0].projectile.GetDamageModel().damage = 30;
                summon.weapons[0].projectile.pierce = 40;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("SuperMonkey-300").display;
                summon.weapons[0].projectile.scale *= 0.8f;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.3f;



            }

        }
        public class DoubleForces : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => BOTTOM;
            public override int Tier => 1;
            public override int Cost => 750;
            public override string Icon => "001";

            public override string DisplayName => "Double Forces";
            public override string Description => "Spawn 1 extra dart monkey.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.name = "AttackModel_Summon_2";
                towerModel.AddBehavior(summon);
            }
        }
        public class ExplosionMadness : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => BOTTOM;
            public override int Tier => 2;
            public override int Cost => 1500;
            public override string Icon => "002";

            public override string DisplayName => "Explosion Madness";
            public override string Description => "Now send explosive troops.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 25f;
                summon.name = "AttackModel_Bomb_";
                summon.weapons[0].projectile.GetDamageModel().damage = 1;
                summon.weapons[0].projectile.pierce = 8;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter").display;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.75f;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                var bomb = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                var fx = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var sound = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                bomb.projectile.GetDamageModel().damage = 2;
                summon.weapons[0].projectile.AddBehavior(bomb);
                summon.weapons[0].projectile.AddBehavior(fx);
                summon.weapons[0].projectile.AddBehavior(sound);
                towerModel.AddBehavior(summon);
            }
        }
        public class SupportingUnit : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => BOTTOM;
            public override int Tier => 3;
            public override int Cost => 3500;
            public override string Icon => "003";

            public override string DisplayName => "Supporting Unit";
            public override string Description => "Every few seconds, spawn a banana farm that when die, give some money. Also give a small amount of money when it hit a Bloon.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_").Duplicate();
                summon.weapons[0].rate = 30f;
                summon.name = "AttackModel_Money_";
                summon.weapons[0].projectile.GetDamageModel().damage = 2;
                summon.weapons[0].projectile.pierce = 2;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BananaFarm").display;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.9f;
                var projectile = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                projectile.projectile = Game.instance.model.GetTowerFromId("BananaFarm-400").GetAttackModel().weapons[0].projectile.Duplicate();
                projectile.projectile.RemoveBehavior<ArriveAtTargetModel>();
                projectile.projectile.scale *= 10;
                projectile.projectile.GetBehavior<CashModel>().minimum = 300;
                projectile.projectile.GetBehavior<CashModel>().maximum = 300;

                projectile.projectile.AddBehavior(new TravelStraitModel("TravelModel", 0, 15));
                var money = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                money.projectile = Game.instance.model.GetTowerFromId("BananaFarm-003").GetAttackModel().weapons[0].projectile.Duplicate();
                money.projectile.GetBehavior<CashModel>().minimum = 25;
                money.projectile.GetBehavior<CashModel>().maximum = 25;
                summon.weapons[0].projectile.AddBehavior(projectile);
                summon.weapons[0].projectile.AddBehavior(money);
                towerModel.AddBehavior(summon);

            }
        }
        public class MachineGun : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => BOTTOM;
            public override int Tier => 4;
            public override int Cost => 7500;
            public override string Icon => "004";

            public override string DisplayName => "Machine Gun";
            public override string Description => "The barrack is equipped with a machine gun.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var machineGun = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                machineGun.weapons[0].rate = 0.06f;
                machineGun.range = 999999;
                machineGun.GetDescendant<RotateToTargetModel>().rotateTower = false;

                machineGun.weapons[0].animation = -200;
                machineGun.name = "MachineGun";
                machineGun.weapons[0].projectile = Game.instance.model.GetTower(TowerType.DartlingGunner).GetWeapon().projectile.Duplicate();
                machineGun.weapons[0].projectile.GetDamageModel().damage = 1;
                machineGun.weapons[0].projectile.pierce = 4;
                machineGun.weapons[0].ejectX = 0;
                machineGun.weapons[0].ejectY = 0;
                machineGun.weapons[0].ejectZ = 0;
                Il2CppReferenceArray<ThrowMarkerOffsetModel> array;
                array = new[] { new ThrowMarkerOffsetModel("offset1", 2, 0, 0, 0), new ThrowMarkerOffsetModel("offset2", -2, 0, 0, 100) };

                machineGun.weapons[0].emission = new EmissionWithOffsetsModel("None", array, 1, false, null, 0);
                towerModel.AddBehavior(machineGun);
            }
        }
        public class MilitarySupport : ModUpgrade<MonkeyBarackTower>
        {

            public override int Path => BOTTOM;
            public override int Tier => 5;
            public override int Cost => 50000;
            public override string Icon => "005";

            public override string DisplayName => "Military Support";
            public override string Description => "This military can support very well in the battlefield.";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Money_");
                var machineGun = towerModel.GetAttackModel("MachineGun");
                machineGun.weapons[0].rate /= 1.25f;

                machineGun.weapons[0].projectile.pierce += 1;
                machineGun.weapons[0].projectile.GetDamageModel().damage += 2;

                summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<CashModel>().minimum = 1200;
                summon.weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().projectile.GetBehavior<CashModel>().maximum = 1200;
                summon.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CashModel>().minimum = 75;
                summon.weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().projectile.GetBehavior<CashModel>().maximum = 75;
                summon.weapons[0].projectile.pierce += 3;
                towerModel.AddBehavior(machineGun);

                var tack = Game.instance.model.GetTowerFromId("TackShooter-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();

                tack.range = 1000.0f;
                tack.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenModel_"));
                tack.weapons[0].emission.Cast<ArcEmissionModel>().count = 6;
                tack.weapons[0].projectile.GetDamageModel().damage = 2;
                tack.weapons[0].projectile.pierce /= 2;
                tack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan = 0.4f;
                tack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                towerModel.AddBehavior(tack);
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            }
        }
        public class UnionOfMonkeySocialistRepublics : ModParagonUpgrade<MonkeyBarackTower>
        {
            public override int Cost => 2000000;
            public override string DisplayName => "Union of Monkey Socialist Republics";
            public override string Description => "Peak of Capitalism.";
            public override string Icon => "Paragon";
            public override void ApplyUpgrade(TowerModel towerModel)
            {
                var summon = towerModel.GetAttackModel("AttackModel_Summon_");
                summon.weapons[0].rate = 15;
                summon.weapons[0].projectile.GetDamageModel().damage = 2500;
                summon.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("DartMonkey-050").display;
                summon.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                summon.weapons[0].projectile.pierce = 60;
                summon.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.5f;

                var summon2 = summon.Duplicate();
                summon2.weapons[0].rate = 100;
                summon2.weapons[0].projectile.GetDamageModel().damage = 125000;
                summon2.weapons[0].projectile.pierce = 120;
                summon2.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.2f;
                summon2.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("MonkeyAce-Paragon").GetBehavior<AirUnitModel>().display;
                towerModel.AddBehavior(summon2);

                var summon3 = summon.Duplicate();
                summon3.weapons[0].rate = 8f;
                summon3.weapons[0].projectile.GetDamageModel().damage = 2000;
                summon3.weapons[0].projectile.pierce = 90;
                summon3.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("NinjaMonkey-040").display;
                summon3.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 2.5f;
                towerModel.AddBehavior(summon3);

                var summon4 = summon.Duplicate();
                summon4.weapons[0].rate = 30f;
                summon4.weapons[0].projectile.GetDamageModel().damage = 8000;
                summon4.weapons[0].projectile.pierce = 35;
                summon4.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("PatFusty 20").display;
                summon4.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.5f;
                towerModel.AddBehavior(summon4);

                var summon5 = summon.Duplicate();
                summon5.weapons[0].rate = 25;
                summon5.weapons[0].projectile.GetDamageModel().damage = 10000;
                summon5.weapons[0].projectile.pierce = -1;
                summon5.weapons[0].projectile.maxPierce = 1;
                summon5.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.3f;
                summon5.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                summon5.weapons[0].projectile.ApplyDisplay<TruckNotRotorDisplay>();
                var projectile = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                var projectile2 = projectile.Duplicate();
                var explosion = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                var effect = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var sound = Game.instance.model.GetTowerFromId("BombShooter-200").GetAttackModel().weapons[0].projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                var CreateTower = Game.instance.model.GetTowerFromId("EngineerMonkey-200").Duplicate();

                explosion.projectile.GetDamageModel().damage = 20000;
                explosion.projectile.pierce = 1000;
                explosion.projectile.radius *= 5f;
                effect.effectModel.scale *= 5f;
                projectile.projectile = CreateTower.GetAttackModel(1).weapons[0].projectile;
                projectile.projectile.RemoveBehavior<AgeModel>();
                projectile.projectile.RemoveBehavior<ArriveAtTargetModel>();
                projectile.projectile.AddBehavior(new TravelStraitModel("TravelModel", 100, 0.07f));
                projectile.projectile.ApplyDisplay<DisplayBlank>();
                projectile.projectile.RemoveBehavior<CreateTowerModel>();
                projectile.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<UMSRUnit01>().Duplicate(), 0f, false, false, false, false, true));
                projectile.emission = new ArcEmissionModel("Arc", 1, 0, 360, null, false, false);
                projectile2.emission = new ArcEmissionModel("Arc", 3, 0, 360, null, false, false);
                projectile2.projectile.AddBehavior(new TravelStraitModel("TravelModel", 100, 0.07f));
                projectile2.projectile.RemoveBehavior<CreateTowerModel>();
                projectile2.projectile.AddBehavior(new CreateTowerModel("AlchTower", GetTowerModel<UMSRUnit02>().Duplicate(), 0f, false, false, false, false, true));

                projectile.projectile.AddBehavior(projectile2);
                summon5.weapons[0].projectile.AddBehavior(projectile);
                summon5.weapons[0].projectile.AddBehavior(explosion);
                summon5.weapons[0].projectile.AddBehavior(effect);
                summon5.weapons[0].projectile.AddBehavior(sound);
                towerModel.AddBehavior(summon5);

                var summon6 = summon.Duplicate();
                summon6.weapons[0].rate = 0.8f;

                summon6.name = "AttackModel_Super_";
                summon6.weapons[0].projectile.GetDamageModel().damage = 1000;
                summon6.weapons[0].projectile.pierce = 30;
                summon6.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("SuperMonkey-205").display;
                summon6.weapons[0].projectile.scale *= 0.8f;
                summon6.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.5f;
                towerModel.AddBehavior(summon6);

                var abilityModel = Game.instance.model.GetTowerFromId("Ezili 3").GetAbility().Duplicate();
                abilityModel.RemoveBehavior<ActivateAttackModel>();
                abilityModel.name = "AbilityModel_CommunismTaste";
                abilityModel.displayName = "Communism Taste";
                abilityModel.description = "With this great treasure i summon...";
                abilityModel.cooldown = 160f;
                abilityModel.RemoveBehaviors<CreateSoundOnAbilityModel>();
                abilityModel.RemoveBehaviors<CreateEffectOnAbilityModel>();


                var activateAttackModel = new ActivateAttackModel("ActivateAttackModel_CommunismTaste", 15, true, new Il2CppReferenceArray<AttackModel>(1), true, false, false, false, false);
                abilityModel.AddBehavior(activateAttackModel);

                var attackModel = activateAttackModel.attacks[0] = Game.instance.model.GetTower(TowerType.MortarMonkey, 0, 5).GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();

                attackModel.attackThroughWalls = true;
                attackModel.range = 2000;
                activateAttackModel.AddChildDependant(attackModel);
                towerModel.range += 10;

                var weaponModel = attackModel.weapons[0];
                weaponModel.rate = 0.000001f;
                weaponModel.emission = new RandomTargetSpreadModel("Spread", 200, null, null);
                weaponModel.SetProjectile(Game.instance.model.GetTower(TowerType.MonkeySub, 0, 3).GetAttackModel(1).weapons[0].projectile.Duplicate());
                var projectileModel = weaponModel.projectile;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().damage = 2250;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Bad", 2, 0, false, false));
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Ceramic", 2, 0, false, false));
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.radius *= 15;
                projectileModel.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId = Game.instance.model.GetTower(TowerType.MortarMonkey, 5).GetAttackModel().weapons[0].projectile.GetBehavior<Il2CppAssets.Scripts.Models.Towers.Projectiles.Behaviors.CreateEffectOnExpireModel>().assetId;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.collisionPasses = new[] { -1, 0 };
                var stun = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModel>().Duplicate();
                var stuntag = Game.instance.model.GetTowerFromId("BombShooter-400").GetDescendant<SlowModifierForTagModel>().Duplicate();
                var moabstun = Game.instance.model.GetTowerFromId("BombShooter-500").GetDescendant<SlowModel>().Duplicate();
                var superbrittle = Game.instance.model.GetTowerFromId("IceMonkey-500").GetDescendant<AddBonusDamagePerHitToBloonModel>();
                superbrittle.perHitDamageAddition = 12;
                moabstun.lifespan = 3f;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(stuntag);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(moabstun);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.AddBehavior(superbrittle);
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                projectileModel.GetBehavior<CreateProjectileOnExpireModel>().GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);

                towerModel.AddBehavior(abilityModel);

                var summon7 = summon.Duplicate();
                summon7.weapons[0].rate = 25f;
                summon7.weapons[0].projectile.GetDamageModel().damage = 2500;
                summon7.weapons[0].projectile.pierce = 10000000;
                summon7.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BombShooter-004").display;
                summon7.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 0.6f;
                summon7.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;
                var bomb = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                var fx = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateEffectOnContactModel>().Duplicate();
                var sound2 = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateSoundOnProjectileCollisionModel>().Duplicate();
                bomb.projectile.GetDamageModel().damage = 1500;
                summon7.weapons[0].projectile.AddBehavior(bomb);
                summon7.weapons[0].projectile.AddBehavior(fx);
                summon7.weapons[0].projectile.AddBehavior(sound2);
                towerModel.AddBehavior(summon7);

                var summon8 = summon.Duplicate();
                summon8.weapons[0].rate = 30f;
                summon8.name = "AttackModel_Money_";
                summon8.weapons[0].projectile.GetDamageModel().damage = 250;
                summon8.weapons[0].projectile.pierce = 10;
                summon8.weapons[0].projectile.display = Game.instance.model.GetTowerFromId("BananaFarm-520").display;
                summon8.weapons[0].projectile.GetBehavior<TravelAlongPathModel>().speedFrames *= 1.1f;
                var projectile3 = Game.instance.model.GetTowerFromId("MortarMonkey").GetAttackModel().weapons[0].projectile.GetBehavior<CreateProjectileOnExhaustFractionModel>().Duplicate();
                projectile3.projectile = Game.instance.model.GetTowerFromId("BananaFarm-400").GetAttackModel().weapons[0].projectile.Duplicate();
                projectile3.projectile.RemoveBehavior<ArriveAtTargetModel>();
                projectile3.projectile.scale *= 10;
                projectile3.projectile.GetBehavior<CashModel>().minimum = 3500;
                projectile3.projectile.GetBehavior<CashModel>().maximum = 3500;

                projectile3.projectile.AddBehavior(new TravelStraitModel("TravelModel", 0, 15));
                var money = Game.instance.model.GetTower(TowerType.BombShooter).GetWeapon().projectile.GetBehavior<CreateProjectileOnContactModel>().Duplicate();
                money.projectile = Game.instance.model.GetTowerFromId("BananaFarm-003").GetAttackModel().weapons[0].projectile.Duplicate();
                money.projectile.GetBehavior<CashModel>().minimum = 350;
                money.projectile.GetBehavior<CashModel>().maximum = 350;
                summon8.weapons[0].projectile.AddBehavior(projectile3);
                summon8.weapons[0].projectile.AddBehavior(money);
                towerModel.AddBehavior(summon8);

                var machineGun = Game.instance.model.GetTowerFromId("DartMonkey").GetAttackModel().Duplicate();
                machineGun.weapons[0].rate = 0.01f;
                machineGun.range = 999999;
                machineGun.GetDescendant<RotateToTargetModel>().rotateTower = false;

                machineGun.weapons[0].animation = -200;
                machineGun.weapons[0].projectile = Game.instance.model.GetTower(TowerType.DartlingGunner).GetWeapon().projectile.Duplicate();
                machineGun.weapons[0].projectile.GetDamageModel().damage = 100;
                machineGun.weapons[0].projectile.pierce = 100;
                machineGun.weapons[0].ejectX = 0;
                machineGun.weapons[0].ejectY = 0;
                machineGun.weapons[0].ejectZ = 0;
                Il2CppReferenceArray<ThrowMarkerOffsetModel> array;
                array = new[] { new ThrowMarkerOffsetModel("offset1", 4, 0, 0, 0), new ThrowMarkerOffsetModel("offset1", 2, 0, 0, 0), new ThrowMarkerOffsetModel("offset1", 0, 0, 0, 0), new ThrowMarkerOffsetModel("offset2", -2, 0, 0, 100), new ThrowMarkerOffsetModel("offset1", -4, 0, 0, 0) };

                machineGun.weapons[0].emission = new EmissionWithOffsetsModel("None", array, 1, false, null, 0);
                towerModel.AddBehavior(machineGun);
                var tack = Game.instance.model.GetTowerFromId("TackShooter-050").GetAbility().GetBehavior<ActivateAttackModel>().attacks[0].Duplicate();

                tack.range = 1000.0f;
                tack.weapons[0].projectile.AddBehavior(new ExpireProjectileAtScreenEdgeModel("ExpireProjectileAtScreenModel_"));
                tack.weapons[0].emission.Cast<ArcEmissionModel>().count = 12;
                tack.weapons[0].projectile.GetDamageModel().damage = 550;
                tack.weapons[0].projectile.pierce = 200;
                tack.weapons[0].projectile.GetBehavior<TravelStraitModel>().lifespan = 0.6f;
                tack.weapons[0].projectile.GetDamageModel().immuneBloonProperties = BloonProperties.None;

                abilityModel.icon = GetSpriteReference<MonkeyBarack>("050");

                towerModel.AddBehavior(tack);
                towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            }
        }
        public class ParagonDisplay : ModTowerDisplay<MonkeyBarackTower>
        {
            public override string BaseDisplay => GetDisplay(TowerType.MonkeyVillage, 0, 3);

            public override bool UseForTower(int[] tiers)
            {
                return IsParagon(tiers);
            }

            public override int ParagonDisplayIndex => 0;

            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
            }
        }
        [HarmonyPatch(typeof(Il2CppAssets.Scripts.Simulation.SimulationBehaviors.NecroData), nameof(NecroData.RbePool))]
        internal static class Necro_RbePool
        {
            [HarmonyPrefix]
            private static bool Postfix(NecroData __instance, ref int __result)
            {
                var tower = __instance.tower;
                if (tower.towerModel.name.Contains("Barack"))
                {
                    __result = 9999;
                }
                return false;
            }
        }
    }

}
namespace Monkeys
{
    public class Officer : ModTower
    {
        public override string Portrait => "Officer";
        public override string Name => "Officer";
        public override TowerSet TowerSet => TowerSet.Military;
        public override string BaseTower => TowerType.SniperMonkey;

        public override bool DontAddToShop => true;
        public override int Cost => 0;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;


        public override string DisplayName => "Officer";
        public override string Description => "";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetBehavior<AttackModel>();
            var weapons = attackModel.weapons[0];
            var projectile = weapons.projectile;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 30f, 3, false, false));
            weapons.rate = 0.4f;
            projectile.GetDamageModel().damage = 5;
            towerModel.range = 42;
            attackModel.range = 42;
            towerModel.radius = 0;
            towerModel.isGlobalRange = false;
            var Pops = Game.instance.model.GetTowerFromId("Sentry").GetBehavior<CreditPopsToParentTowerModel>().Duplicate();
            towerModel.AddBehavior(Pops);



        }
        public class OfficerDisplay : ModTowerDisplay<Officer>
        {
            public override float Scale => .7f;
            public override string BaseDisplay => GetDisplay(TowerType.EngineerMonkey, 0, 0, 2);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
                foreach (var renderer in node.genericRenderers)
                {
                    renderer.material.mainTexture = GetTexture("OfficerDisplay");
                }
            }
        }

    }
    public class Soldier : ModTower
    {
        public override string Portrait => "Soldier";
        public override string Name => "Soldier";
        public override TowerSet TowerSet => TowerSet.Military;
        public override string BaseTower => TowerType.SniperMonkey;

        public override bool DontAddToShop => true;
        public override int Cost => 0;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;


        public override string DisplayName => "Soldier";
        public override string Description => "";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetBehavior<AttackModel>();
            var weapons = attackModel.weapons[0];
            var projectile = weapons.projectile;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 40f, 3, false, false));
            weapons.rate = 1.05f;
            projectile.GetDamageModel().damage = 40;
            towerModel.range = 59;
            attackModel.range = 59;
            towerModel.radius = 0;
            towerModel.isGlobalRange = false;
            var Pops = Game.instance.model.GetTowerFromId("Sentry").GetBehavior<CreditPopsToParentTowerModel>().Duplicate();
            towerModel.AddBehavior(Pops);
        }
        public class SoldierDisplay : ModTowerDisplay<Soldier>
        {
            public override float Scale => .7f;
            public override string BaseDisplay => GetDisplay(TowerType.SniperMonkey, 0, 0, 3);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
        }

    }
    public class FlameThrower : ModTower
    {
        public override string Portrait => "FlameThrower";
        public override string Name => "FlameThrower";
        public override TowerSet TowerSet => TowerSet.Military;
        public override string BaseTower => TowerType.DartMonkey;

        public override bool DontAddToShop => true;
        public override int Cost => 0;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;


        public override string DisplayName => "Flame Thrower";
        public override string Description => "";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetBehavior<AttackModel>();
            var weapons = attackModel.weapons[0];
            var projectile = weapons.projectile;
            towerModel.isSubTower = true;
            weapons.SetProjectile(Game.instance.model.GetTower(TowerType.Gwendolin).GetWeapon().projectile.Duplicate());
            towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 25f, 3, false, false));
            weapons.rate = 0.075f;
            projectile = weapons.projectile;
            projectile.GetBehavior<TravelStraitModel>().speed /= 2;
            projectile.GetBehavior<TravelStraitModel>().lifespan *= 2;
            projectile.pierce = 10;
            projectile.GetDamageModel().damage = 5;
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Ceramic", 0, 12, false, false));
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Fortified", 2, 1, false, false));
            projectile.AddBehavior(new DamageModifierForTagModel("DamageModif", "Ddt", 2, 0, false, false));
            towerModel.range = 55;
            attackModel.range = 55;
            towerModel.radius = 0;
            towerModel.GetDescendants<FilterInvisibleModel>().ForEach(model => model.isActive = false);
            towerModel.isGlobalRange = false;
            var Pops = Game.instance.model.GetTowerFromId("Sentry").GetBehavior<CreditPopsToParentTowerModel>().Duplicate();
            towerModel.AddBehavior(Pops);
        }
        public class FlameThrowerDisplay : ModTowerDisplay<FlameThrower>
        {
            public override float Scale => .7f;
            public override string BaseDisplay => GetDisplay(TowerType.Gwendolin);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
        }

    }
    public class UMSRUnit01 : ModTower
    {
        public override string Portrait => "Unit01";
        public override string Name => "UMSR Unit 01";
        public override TowerSet TowerSet => TowerSet.Military;
        public override string BaseTower => TowerType.SniperMonkey;

        public override bool DontAddToShop => true;
        public override int Cost => 0;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;


        public override string DisplayName => "UMSR Unit 01";
        public override string Description => "";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetBehavior<AttackModel>();
            var weapons = attackModel.weapons[0];
            var projectile = weapons.projectile;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 45f, 3, false, false));
            weapons.rate = 1f;
            projectile.GetDamageModel().damage = 5000;
            towerModel.range = 50;
            attackModel.range = 50;
            towerModel.radius = 0;
            towerModel.isGlobalRange = false;
            var Pops = Game.instance.model.GetTowerFromId("Sentry").GetBehavior<CreditPopsToParentTowerModel>().Duplicate();
            towerModel.AddBehavior(Pops);
        }
        public class UMSRUnit01Display : ModTowerDisplay<UMSRUnit01>
        {
            public override float Scale => .8f;
            public override string BaseDisplay => GetDisplay(TowerType.SniperMonkey, 2, 0, 4);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
            }
        }

    }
    public class UMSRUnit02 : ModTower
    {
        public override string Portrait => "Unit02";
        public override string Name => "UMSR Unit 02";
        public override TowerSet TowerSet => TowerSet.Military;
        public override string BaseTower => TowerType.DartMonkey;

        public override bool DontAddToShop => true;
        public override int Cost => 0;

        public override int TopPathUpgrades => 0;
        public override int MiddlePathUpgrades => 0;
        public override int BottomPathUpgrades => 0;


        public override string DisplayName => "UMSR Unit 02";
        public override string Description => "";

        public override void ModifyBaseTowerModel(TowerModel towerModel)
        {
            var attackModel = towerModel.GetBehavior<AttackModel>();
            var weapons = attackModel.weapons[0];
            var projectile = weapons.projectile;
            towerModel.isSubTower = true;
            towerModel.AddBehavior(new TowerExpireModel("ExpireModel", 38f, 3, false, false));
            weapons.rate = 0.4f;
            projectile.GetDamageModel().damage = 300;
            projectile.pierce = 30;
            towerModel.range = 40;
            attackModel.range = 40;
            towerModel.radius = 0;
            projectile.display = Game.instance.model.GetTower(TowerType.DartlingGunner, 3).GetAttackModel().weapons[0].projectile.display;
            var Pops = Game.instance.model.GetTowerFromId("Sentry").GetBehavior<CreditPopsToParentTowerModel>().Duplicate();
            towerModel.AddBehavior(Pops);
        }
        public class UMSRUnit02Display : ModTowerDisplay<UMSRUnit02>
        {
            public override float Scale => .8f;
            public override string BaseDisplay => GetDisplay(TowerType.EngineerMonkey, 0, 0, 2);

            public override bool UseForTower(int[] tiers)
            {
                return true;
            }
            public override void ModifyDisplayNode(UnityDisplayNode node)
            {
            }
        }

    }
}
