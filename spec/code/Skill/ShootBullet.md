# ShootBulletクラス設計


# 概要
- シンプルに直線状に進む弾を射出するスキル


# 実装
- SkillBaseを継承する
- ```/spec/gamedesign/skill/shootBullet.md``` を参照すること


# 処理フロー
1. Execute呼び出しが行われたら、BulletのBulder関数をコールする
2. クールタイムの処理をする


# SerializeFieldで設定するprivate変数
```/spec/gamedesign/skill/shootBullet.md``` を参照してデフォルトの値を設定すること
- attackPower: 攻撃力
- bulletSpeed: 弾の速度


# 期待値
- 派生先に倣う


# エッジケース
- なし
