# StageCreatorクラス設計


# 概要
- ステージの生成を行う


# 実装
- MonoBehaviourを継承する
- ステージを生成する
- このスクリプトはエディタ上でのみ動作する


# 処理フロー
1. エディタ拡張の「ステージ生成」が押される
2. PlayerクラスをFindしてplayerRefに参照を入れる
3. ステージ生成処理を実行する
4. 敵を生成する
5. 敵の参照を、CinemachineTargetGroupのリストに入れる


## ステージ生成処理
- このGameObjectの子供を全部消去し、CinemachineTargetGroupのリストも同様に削除する
- ステージの0,0を原点とし、-stageWidth/2～stageWidth/2と-stageHeight/2～stageHeight/2の大きさのステージを生成する
	- セル生成ルールを参考にすること
- 敵を生成し、指定位置に配置する
- プレイヤーを指定位置に移動、配置する


# SerializeFieldされたprivate変数
- stageWidth: ステージの幅
- stageHeight: ステージの高さ
- cellPrefabList: セルのPrefabリスト。セルIDのENUM分指定する必要がある
- playerPos: プレイヤー位置の[x,y]を指定する。
- enemyList: 敵生成構造体のリスト
- cellRuleList: セル生成ルール構造体のリスト

## 敵生成構造体
- enemyPos: 敵の生成位置
- enemyPrefab: どの敵を生成するか

## セル生成ルール構造体
- cellID: セルIDのENUM
- needNum: 何枚必要か


# その他のprivate変数
- playerRef: プレイヤーの参照


# 期待値
- セル生成ルールによりうまくステージ生成ができない場合は処理を中止する
	- 作り直しは3回までとする
- 敵の位置が重複している場合は処理をしない
	- エラーを出力する
- ステージ生成はエディタ拡張で行う