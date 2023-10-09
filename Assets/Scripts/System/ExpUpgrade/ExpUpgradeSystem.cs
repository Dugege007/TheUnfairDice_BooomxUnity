﻿using UnityEngine;
using QFramework;
using System.Collections.Generic;
using System.Linq;

namespace TheUnfairDice
{
    public class ExpUpgradeSystem : AbstractSystem
    {
        public static EasyEvent OnCoinUpgradeSystemChanged = new EasyEvent();

        public List<ExpUpgradeItem> Items { get; } = new();

        private AbilityConfig[] mAbilityConfigs;

        protected override void OnInit()
        {
            mAbilityConfigs = ConfigManager.Default.AbilityConfigs;

            ResetData();

            Global.Level.Register(_ =>
            {
                Roll();
            });
        }

        public ExpUpgradeItem Add(ExpUpgradeItem item)
        {
            Items.Add(item);
            return item;
        }

        public void ResetData()
        {
            Items.Clear();

            // 水
            AddNewExpUpgradeItem("", mAbilityConfigs[0]);
            // 火
            AddNewExpUpgradeItem("", mAbilityConfigs[1]);
            //// 树
            //AddNewExpUpgradeItem("", mAbilityConfigs[2]);
            //// 剑
            //AddNewExpUpgradeItem("", mAbilityConfigs[3]);
            //// 地
            //AddNewExpUpgradeItem("", mAbilityConfigs[4]);
            //// 光
            //AddNewExpUpgradeItem("", mAbilityConfigs[5]);
        }

        private void AddNewExpUpgradeItem(string key, AbilityConfig abilityConfig)
        {
            Add(new ExpUpgradeItem()
                .WithKey(key)
                .WithName(abilityConfig.Name)
                .WithDescription(lv =>
                {
                    return UpgradeDiscription(lv, abilityConfig);
                })
                .WithMaxLevel(abilityConfig.Powers.Count)
                .OnUpgrade((_, lv) =>
                {
                    UpgradePowerValue(lv, abilityConfig);
                }));
        }

        private string UpgradeDiscription(int lv, AbilityConfig abilityConfig)
        {
            if (lv == 1 && abilityConfig.IsWeapon)
            {
                return $"{abilityConfig.Name} Lv1" + "\n" + abilityConfig.Description;
            }

            for (int i = 1; i < abilityConfig.Powers.Count + 1; i++)
            {
                if (lv == i)
                    return abilityConfig.Powers[lv - 1].GetPowerUpInfo(abilityConfig.Name);
            }

            return "未知等级";   // 不加这行会报错
        }

        private void UpgradePowerValue(int lv, AbilityConfig abilityConfig)
        {


            // 解锁能力
            if (lv == 1)
            {
                if (abilityConfig.Name == mAbilityConfigs[0].Name)
                    Global.HolyWaterUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[1].Name)
                    Global.HolyFireUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[2].Name)
                    Global.HolyTreeUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[3].Name)
                    Global.HolySwordUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[4].Name)
                    Global.HolyLandUnlocked.Value = true;
                else if (abilityConfig.Name == mAbilityConfigs[5].Name)
                    Global.HolyLightUnlocked.Value = true;
            }

            // 升级能力
            for (int i = 1; i < abilityConfig.Powers.Count + 1; i++)
            {
                if (lv == i)
                {
                    Debug.Log("当前升级：" + abilityConfig.Name + abilityConfig.Powers[lv - 1].Lv);

                    foreach (PowerData powerData in abilityConfig.Powers[lv - 1].PowerDatas)
                    {
                        // 水
                        if (abilityConfig.Name == mAbilityConfigs[0].Name)
                        {
                            switch (powerData.Type)
                            {
                                case AbilityPower.PowerType.Damage:
                                    HolyWater.Damage.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.CDTime:
                                    HolyWater.CDTime.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Duration:
                                    HolyWater.Duration.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Range:
                                    HolyWater.Range.Value += powerData.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        // 火
                        if (abilityConfig.Name == mAbilityConfigs[1].Name)
                        {
                            switch (powerData.Type)
                            {
                                case AbilityPower.PowerType.Damage:
                                    HolyFire.Damage.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.CDTime:
                                    HolyFire.CDTime.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Duration:
                                    HolyFire.Duration.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Range:
                                    HolyFire.Range.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Speed:
                                    HolyFire.Speed.Value += powerData.Value;
                                    break;
                                case AbilityPower.PowerType.Count:
                                    HolyFire.Count.Value += (int)powerData.Value;
                                    break;
                                case AbilityPower.PowerType.AttackCount:
                                    HolyFire.AttackCount.Value += (int)powerData.Value;
                                    break;
                                default:
                                    break;
                            }
                        }

                        Debug.Log($"升级{powerData.Type} " + powerData.Value);
                    }
                }
            }
        }

        public void Roll()
        {
            if (Items.Count >= 0)
            {
                foreach (ExpUpgradeItem expUpgradeItem in Items)
                {
                    expUpgradeItem.Visible.Value = false;
                }
            }

            // 随机取几个可升级的能力
            List<ExpUpgradeItem> list = Items.Where(item => !item.UpgradeFinish).ToList();
            if (list.Count >= 3)
            {
                list.GetAndRemoveRandomItem().Visible.Value = true;
                list.GetAndRemoveRandomItem().Visible.Value = true;
                list.GetAndRemoveRandomItem().Visible.Value = true;
            }
            else if (list.Count > 0)
            {
                foreach (ExpUpgradeItem item in list)
                {
                    item.Visible.Value = true;
                }
            }
            else
            {
                Debug.Log("没有可用的升级项");
                return;
            }
        }
    }
}
