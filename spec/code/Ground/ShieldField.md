# ShieldFieldクラス設計


# 概要
- シールドクラス


# 実装
- MonoBehaviourを継承する
- ```/spec/gamedesign/groundcell/shiled.md``` を参照すること

# 処理フロー
- Start時、自分の位置のy=0.5にShiledBlockを生成する
	- ShiledBlockはResourcesフォルダにあるものを読み込んで複製する


# 期待値
- プレイヤーはこのセルに移動できない


