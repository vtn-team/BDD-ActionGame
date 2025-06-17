# StageCreatorクラス設計


# 概要
- ステージの生成を行う


# 実装
- MonoBehaviourを継承する
- ステージを生成する
- このスクリプトはエディタ上でのみ動作する


# 処理フロー
このクラスはゲーム中何もしない


# 外部インタフェース
- StageSize: ステージの幅と高さを返す


# SerializeFieldされたprivate変数
- stageWidth: ステージの幅
- stageHeight: ステージの高さ
- cellPrefabList: セルのPrefabリスト。セルIDのENUM分指定する必要がある
- playerPos: プレイヤー位置の[x,y]を指定する。
- enemyList: 敵生成構造体のリスト
- cellRuleList: セル生成ルール構造体のリスト

## 敵生成構造体
- enemyPos: 敵の生成位置
- enemyDir: 敵の向き(敵の方向ENUMを参照)
- enemyPrefab: どの敵を生成するか

## セル生成ルール構造体
- cellID: セルIDのENUM
- needNum: 何枚必要か

## 敵の方向ENUM
- ENEMY_DIR_RANDOM: ランダム
- ENEMY_DIR_PLAYER: プレイヤーの方を向く
- ENEMY_DIR_LEFT: 左を向く
- ENEMY_DIR_UP: 上を向く
- ENEMY_DIR_DOWN: 下を向く
- ENEMY_DIR_RIGHT: 右を向く

# その他のprivate変数
- playerRef: プレイヤーの参照


# 期待値
- セル生成ルールによりうまくステージ生成ができない場合は処理を中止する
	- 作り直しは3回までとする
- 敵の位置が重複している場合は処理をしない
	- エラーを出力する
- ステージ生成はエディタ拡張で行う