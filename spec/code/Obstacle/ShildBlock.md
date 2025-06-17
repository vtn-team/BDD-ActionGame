# ShildBlockクラス設計


# 概要
- ステージ上に置かれる弾が当たる障害物


# 実装
- MonoBehaviourを継承する
- IHitTargetを実装する


# SerializeFieldで設定するprivate変数
- hitPoint: シールドのHP



# 外部インタフェース
- NextActionInterval: 行動制限時間を返す
- CoolTime: クールタイム中の場合、再使用までの時間を返す
- CheckExecute: スキルが実行可能かを確認する
- Execute: スキルを実行する。処理するGameObjectを引数に渡す。

