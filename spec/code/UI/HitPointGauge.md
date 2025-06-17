# HitPointGaugeクラス設計


# 概要
- 対象のIHitTargetのHitPointを可視化する


# 実装
- MonoBehaviourを継承する
- Imageを使用する
- Builder関数があり、GameManagerから呼び出される
	- IHitTargetを引数に受け取る
	- ResourcesからHitPointGaugeを読み込み複製する
	- Canvas直下に配置する
		- Awake時にCanvasをFindしキャッシュしておく


# 処理フロー
1. Updateで以下をループ
	1. 対象のオブジェクトのHeadPosを取得し、射影変換して2D座標にし、自分の位置をそれに合わせる
	2. 対象のオブジェクトのHP割合を取得し、ImageのFillAmountを更新する。

