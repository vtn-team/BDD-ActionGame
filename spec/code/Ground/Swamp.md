# Swampクラス設計


# 概要
- 沼地クラス


# 実装
- MonoBehaviourを継承する
- ```/spec/gamedesign/groundcell/swamp.md``` を参照すること


# 処理フロー
- プレイヤー移動時にプレイヤーに行動阻害を与える
	- 行動阻害はプレイヤー側で実装する


# SerializeFieldで設定するprivate変数
- inhibitionTime: 沼地移動時に動けなくなる時間


# 期待値
- プレイヤーの移動を阻害する


