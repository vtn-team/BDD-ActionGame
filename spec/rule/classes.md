# クラスの関係図や役割を定義する

## GameManager
ゲームの進行や変数を管理する

## Player
プレイヤーに関連する処理を統括する  
プレイヤーの入力系統を処理する  
Skillを4つまで所持できる

## Enemy
敵の処理のベースクラス
それぞれの個性的な敵は派生クラスで表現する


# プロジェクト全体の詳細設計
- 個々の詳細な設計は、code以下にクラス名と同じmdを記載し仕様化する
  - 個々の仕様が無いものは、このページの情報をもとに生成する事
