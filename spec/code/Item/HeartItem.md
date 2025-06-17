# HeartItemクラス設計


# 概要
- 体力回復アイテム


# 実装
-ItemBaseを継承する
- ```/spec/gamedesign/item/heart.md``` を参照すること


# 処理フロー
1. プレイヤーとの当たり判定処理をOnTriggerEnterで持つ
	1. タグがPlayerでIHitTargetがあるかどうかを確認し、あればHealメソッドを呼び出す


## Healメソッド
- 対象に-healAmountのDamageを渡す
	- ダメージがマイナスなので回復という扱いになる


# SerializeFieldで設定するprivate変数
- ```/spec/gamedesign/item/heart.md``` を参照してデフォルトの値を設定すること
- healAmount: 回復力


# 期待値
- MaxHitPointを超えて回復しない


# エッジケース
- なし
