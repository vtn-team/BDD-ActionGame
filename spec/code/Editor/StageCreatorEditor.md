# StageCreatorEditorクラス設計


# 概要
- ステージの生成を行う


# 実装
- Editorを継承する
- ステージを生成する
- このスクリプトはエディタ上でのみ動作する
- アイテムは配置しない


# 処理フロー

## ステージ全体生成
1. エディタ拡張の「Generate Stage」が押される
2. PlayerクラスをFindしてplayerRefに参照を入れる
3. ステージ生成処理を実行する
4. 敵を生成する

## フィールドのみ再生成
1. エディタ拡張の「Regenerate Fields Only」が押される
2. 既存のフィールド（GroundBase派生）のみを削除する
3. 新しいフィールドレイアウトを生成する
4. 敵とプレイヤーは移動・削除しない


## ステージ生成処理
1. StageCreatorが生成するGameObjectを削除する
	1. StageCreatorのGameObjectの子供を全部消去する
	2. すべてのEnemyを削除する
	3. CinemachineTargetGroupのリストも同様に削除する
- ステージの0,0を原点とし、-stageWidth/2～stageWidth/2と-stageHeight/2～stageHeight/2の大きさのステージを生成する
	- セル生成ルールを参考にすること
	- セルのyは-0.5(プレイヤーの高さの半分)にすること
- 敵を生成し、指定位置に配置する
	- 敵の向きをENUMを参考に指定する
	- 向きは90度間隔とする(Yの回転は90の倍数でしかありえない)
	- プレイヤーの向きに向く場合、X軸にあたる方向を向く
- アイテムはここでは配置しない
- プレイヤーを指定位置に移動、配置する


# 変数
すべてStageCreatorで宣言/定義されたものを参照する


# エディタUIボタン機能

## Generate Stage ボタン
- 完全なステージ再生成（敵・プレイヤー・フィールド全て）
- 設定検証とリトライ機能付き（最大3回）
- 実行前に確認ダイアログ表示

## Regenerate Fields Only ボタン  
- フィールド（地面タイル）のみ再生成
- 敵とプレイヤーの位置は保持
- リトライ機能付き（最大3回）
- 実行前に確認ダイアログ表示

## Clear Stage ボタン
- 全ステージオブジェクトの削除
- 実行前に確認ダイアログ表示

## Validate Settings ボタン
- 位置設定の検証（境界・重複・衝突チェック）
- 結果をダイアログで表示

# 期待値
- セル生成ルールによりうまくステージ生成ができない場合は処理を中止する
	- 作り直しは3回までとする
- 敵の位置が重複している場合は処理をしない
	- エラーを出力する
- フィールド再生成時は敵・プレイヤー位置を保持する
- ステージ生成はエディタ拡張で行う