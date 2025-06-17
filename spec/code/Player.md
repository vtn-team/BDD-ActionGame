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
1. moveSpeedの長さ、進む方向にダイスのように回転しながら1セル分移動する
	- 移動中は無敵となる
	- 1回の移動で1回転とする

## Skill: スキル処理
1. キーに対応したIDを取得する。IDと対応するskillを取得する。nullなら処理を終了する
2. skillのExecute関数を実行する。
3. skillのNextActionIntervalを取得し、その間は行動できないようにする。


# SerializeFieldで設定するprivate変数
プレイヤーの設定  
- generatorID: 生成者ID。
- hitPoint: HP。0になると負け。

移動まわりの変数  
- moveSpeed: 移動アニメーションの速度
- moveInterval: 移動インターバル

スキルまわりの変数  
- skillList[4]: skillの派生先をSubClassSelectorを使い設定する。スキルは4つ設定できる。
	- エディタ拡張で、「キーとスキルの対応表」を参考に、キーアサインごとに設定できるようにする


# 外部インタフェース
- IHitTargetに倣う
- CheckDead: 死亡判定


## キーとスキルの対応表
- INPUT_01 = 0 //○/Z
- INPUT_02 = 1 //×/X
- INPUT_03 = 2 //△/C
- INPUT_04 = 3 //□/V


# 期待値
- 移動中の無敵時は、Damage関数はfalseを返す