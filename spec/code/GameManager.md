# GameManagerクラス設計

# 概要
- ゲームの進行管理


# 実装
- MonoBehaviourを継承する
- ゲーム状態の状態管理


# 処理フロー
1. AwakeでPlayerとEnemyコンポーネントを生成し初期化する
2. Updateで、各コンポーネントのCheckDeadを呼び出し、死んでいたらGameObjectを消す
3. 勝ったプレイヤーをリザルトに表示する


# SerializeFieldされたprivate変数
- plaer: PlayerのPrefab参照
- enemy: EnemyのPrefab参照
- result: リザルト表示


# 外部インタフェース


# 期待値
- 以下の順番でCheckDeadを実行する
	- Enemy
	- Player


# エッジケース
- EnemyとPlayerが同時に死んだ場合、引き分けとする
