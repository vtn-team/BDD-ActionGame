# Playerクラス設計


# 概要
- プレイヤーの挙動を統括する


# 実装
- MonoBehaviourを継承し、インスペクタからInputSystemの設定ができるようにする
- IHitTargetを実装する
- BoxColliderを使用する
- Ridigbodyを使用する。ただし物理挙動は使わず、当たり判定のトリガーとしてのみ機能させる。
- InputSystemを使用して操作系は管理する
- キーとふるまいは```/spec/usecase/gameplay.md```を参考にすること


# 処理フロー
1. Awakeでコンポーネントの初期化とコールバック登録
2. インプットシステムからの入力を受け取り、入力があれば対応する処理を実行する
	1. ふるまいは```/spec/usecase/gameplay.md```を参考にすること
	2. それぞれの処理にクールタイムと移動インターバルを考慮すること
	3. 移動は入力中でも受け付ける(連続移動ができるようにする)


## Move: 移動処理
1. canMoveがfalseの場合処理をしない
2. StageCreatorからステージのサイズを取得し、ステージの範囲外に出る入力を無視する
3. 移動する
	1. transform.forwardが入力した移動方向となるように回転する
	2. moveSpeedの長さ、transform.forward方向にダイスのように回転しながら1セル分移動する
	3. 移動後のセルの情報を取得し、影響を受ける
- 移動中は無敵となる
- 1回の移動で1回転とする
- 右またはDキー入力するとX+に向かい移動する

## Skill: スキル処理
1. キーに対応したIDを取得する。IDと対応するskillを取得する。nullなら処理を終了する
2. skillのExecute関数を実行する。
3. skillのNextActionIntervalを取得し、その間は行動できないようにする。


# SerializeFieldで設定するprivate変数
プレイヤーの設定  
- canMove: 移動できるかどうか
- generatorID: 生成者ID。
- maxHitPoint: 最大HP。

移動まわりの変数  
- moveSpeed: 移動アニメーションの速度(この秒数でアニメーションが完結する)
- moveInterval: 移動インターバル

スキルまわりの変数  
- skillList[4]: skillの派生先をSubClassSelectorを使い設定する。スキルは4つ設定できる。
	- エディタ拡張で、「キーとスキルの対応表」を参考に、キーアサインごとに設定できるようにする

# private変数
- hitPoint: HP。0になると負け。AwakeでmaxHitPointが代入される。


# 外部インタフェース
- IHitTargetに倣う
- CheckDead: 死亡判定。hitPointが0以下の場合trueを返す。


## キーとスキルの対応表
- INPUT_01 = 0 //○/Z
- INPUT_02 = 1 //×/X
- INPUT_03 = 2 //△/C
- INPUT_04 = 3 //□/V


# 期待値
- 移動中の無敵時は、Damage関数はfalseを返す