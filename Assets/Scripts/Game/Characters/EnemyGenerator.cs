using UnityEngine;
using QFramework;

namespace TheUnfairDice
{
    public partial class EnemyGenerator : ViewController
    {
        private float mCurrentGenerateSec = 0;
        public float GenerateSec = 1f;
        public int MaxEnemyCount = 50;
        public static BindableProperty<int> TotalEnemyCount = new(0);
        public static BindableProperty<int> CurrentEnemyCount = new(0);

        private bool mAttackFortress = false;

        private void Start()
        {
            CurrentEnemyCount.Register(enemyCount =>
            {
                if (enemyCount >= 20)
                    mAttackFortress = true;
                else
                    mAttackFortress = false;

            }).UnRegisterWhenGameObjectDestroyed(gameObject);
        }

        private void Update()
        {
            if(TotalEnemyCount.Value >= MaxEnemyCount)
            {
                if (CurrentEnemyCount.Value == 0)
                {
                    UIKit.OpenPanel<UIGamePassPanel>();
                }
            }
            else
            {
                mCurrentGenerateSec += Time.deltaTime;

                if (mCurrentGenerateSec >= GenerateSec)
                {
                    mCurrentGenerateSec = 0;

                    if (Player.Default)
                    {
                        Vector2 pos = Vector2.zero;

                        float ldx = CameraController.LDTrans.position.x;    // ��� ���µ� X
                        float ldy = CameraController.LDTrans.position.y;    // ��� ���µ� Y
                        float rux = CameraController.RUTrans.position.x;    // ��� ���ϵ� X
                        float ruy = CameraController.RUTrans.position.y;    // ��� ���ϵ� Y

                        int xOry = RandomUtility.Choose(-1, 1);

                        // �����ɵ��λ�þ���Ҫ��С�� 8 ʱ����Ҫ���¼���
                        while (true)
                        {
                            if (xOry > 0)
                            {
                                // ��߻��ұ�
                                pos.x = RandomUtility.Choose(ldx, rux);
                                pos.y = Random.Range(ldy, ruy);
                            }
                            else
                            {
                                // �ϱ߻��±�
                                pos.x = Random.Range(ldx, rux);
                                pos.y = RandomUtility.Choose(ldy, ruy);
                            }

                            // ����Ҫ������ 8 ʱ������ѭ��
                            if (Vector2.Distance(pos, Fortress.Default.Position()) > 8f) break;
                        }

                        Enemy.InstantiateWithParent(this)
                            .Position(pos)
                            .Self(self =>
                            {
                                if (mAttackFortress)
                                    self.IsTargetFortress = true;
                            })
                            .Show();

                        TotalEnemyCount.Value++;
                    }
                }
            }
        }
    }
}