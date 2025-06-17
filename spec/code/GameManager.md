# GameManagerクラス設計

# 概要
- ゲームの進行管理


# 実装
- MonoBehaviourを継承する
- ゲーム状態の状態管理


# 処理フロー
1. AwakeでPlayerとEnemyをシーンから検索する
	1. Enemyは複数対いる
	2. CinemachineTargetGroupコンポーネントをFindして取得、PlayerとすべてのEnemyの参照をリストに入れる
	3. PlayerとEnemyそれぞれのHitPointGaugeを生成する
2. Updateで、各コンポーネントのCheckDeadを呼び出し、死んでいたらGameObjectのActiveをfalseにする
3. 勝敗判定をする
	1. - ```/spec/gamedesign/general.md``` を参照すること
4. 勝ったプレイヤーをリザルトに表示する


# SerializeFieldされたprivate変数
- player: PlayerのP参照
- result: リザルト表示


# 外部インタフェース


# 期待値
- 以下の順番でCheckDeadを実行する
	- Enemy
	- Player


# エッジケース
- EnemyとPlayerが同時に死んだ場合、引き分けとする
