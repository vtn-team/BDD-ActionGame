# ThreeLineShootクラス設計


# 概要
- 3列直線状に進む弾を射出するスキル


# 実装
- SkillBaseを継承する
- ```/spec/gamedesign/skill/threeLineShoot.md``` を参照すること


# 処理フロー
1. Execute呼び出しが行われたら、BulletのBulder関数を3回コールする
	1. スキル発動者の位置、スキル発動者の位置のひとつ上、スキル発動者の位置のひとつ下の発射位置で呼び出す
2. クールタイムの処理をする


# SerializeFieldで設定するprivate変数
```/spec/gamedesign/skill/shootBullet.md``` を参照してデフォルトの値を設定すること
- attackPower: 攻撃力
- bulletSpeed: 弾の速度(1秒間に進む速度)


# 期待値
- 派生先に倣う


# エッジケース
- なし
