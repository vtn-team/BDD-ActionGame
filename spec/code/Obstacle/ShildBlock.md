# ShildBlockクラス設計


# 概要
- ステージ上に置かれる弾が当たる障害物


# 実装
- MonoBehaviourを継承する
- IHitTargetを実装する


# SerializeFieldで設定するprivate変数
- hitPoint: シールドのHP


# 外部インタフェース
- IHitTargetに倣う
- CheckDead: 死亡判定。hitPointが0以下の場合trueを返す。
