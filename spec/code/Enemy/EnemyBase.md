# EnemyBaseクラス設計


# 概要
- 敵の行動制御をするベースクラス


# 実装
- MonoBehaviourを継承する
- IHitTargetを実装する
- このクラスを派生させてエネミーを表現するため、このクラスはabstract classである


# SerializeFieldで設定するprivate変数
- hitPoint: HP


# 外部インタフェース
CheckDead: 死亡判定


# 処理フロー
- 派生先に倣う


# 期待値
- 派生先に倣う


# エッジケース
- なし
