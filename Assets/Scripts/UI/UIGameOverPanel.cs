using UnityEngine;
using UnityEngine.UI;
using QFramework;
using UnityEngine.SceneManagement;
using QAssetBundle;

namespace TheUnfairDice
{
    public class UIGameOverPanelData : UIPanelData
    {
    }
    public partial class UIGameOverPanel : UIPanel
    {
        protected override void OnInit(IUIData uiData = null)
        {
            mData = uiData as UIGameOverPanelData ?? new UIGameOverPanelData();
            // please add init code here

            Time.timeScale = 0;

            BackToHomeBtn.onClick.AddListener(() =>
            {
                AudioKit.PlaySound(Sfx.CLICK);

                this.CloseSelf();
                Global.ResetData();
                SceneManager.LoadScene("GameStart");
            });
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
