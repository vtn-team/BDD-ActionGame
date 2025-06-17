# EnemyBaseクラス設計


# 概要
- 敵の行動制御をするベースクラス


# 実装
- MonoBehaviourを継承する
- IHitTargetを実装する
- Ridigbodyを使用する。ただし物理挙動は使わず、当たり判定のトリガーとしてのみ機能させる。
- このクラスを派生させてエネミーを表現するため、このクラスはabstract classである


# SerializeFieldで設定するprivate変数
- maxHitPoint: 最大HP。

# private変数
- hitPoint: HP。0になると負け。AwakeでmaxHitPointが代入される。

# 外部インタフェース
CheckDead: 死亡判定。hitPointが0以下の場合trueを返す。


# 処理フロー
- 派生先に倣う


# 期待値
- 派生先に倣う


# エッジケース
- なし
