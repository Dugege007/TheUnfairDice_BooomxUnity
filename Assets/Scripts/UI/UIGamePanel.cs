using UnityEngine;
using UnityEngine.UI;
using QFramework;

namespace TheUnfairDice
{
    public class UIGamePanelData : UIPanelData
    {
    }
    public partial class UIGamePanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGamePanelData ?? new UIGamePanelData();
            // please add init code here

            Global.HP.RegisterWithInitValue(hp =>
            {
                PlayerHPText.text = "���HP��" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Fortress.HP.RegisterWithInitValue(hp =>
            {
                FortressrHPText.text = "Ҫ��HP��" + hp;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Fortress.CurrentHumanCount.RegisterWithInitValue(currentHumanCount =>
            {
                HumanCountText.text = "���ࣺ" + currentHumanCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            EnemyGenerator.EnemyCount.RegisterWithInitValue(enemyCount =>
            {
                EnemyCountText.text = "���ˣ�" + enemyCount;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            Global.CurrentSec.RegisterWithInitValue(curerntSec =>
            {
                // ÿ 10 ֡����һ��
                if (Time.frameCount % 10 == 0)
                {
                    int currentSecondsInt = Mathf.FloorToInt(curerntSec);
                    int seconds = currentSecondsInt % 60;
                    int minutes = currentSecondsInt / 60;

                    CurrentTimeText.text = "ʱ�䣺" + $"{minutes:00}:{seconds:00}";
                }

            }).UnRegisterWhenGameObjectDestroyed(gameObject);

            // ע���ʱ������
            ActionKit.OnUpdate.Register(() =>
            {
                Global.CurrentSec.Value += Time.deltaTime;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        protected override void OnOpen(IUIData uiData = null)
        {
        }

        protected override void OnShow()
        {
        }

        protected override void OnHide()
        {
        }

        protected override void OnClose()
        {
        }
    }
}