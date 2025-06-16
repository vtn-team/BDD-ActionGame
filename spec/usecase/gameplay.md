# インゲーム中に期待される振る舞い
インゲームはInGameシーンを使用する  

## Feature: プレイヤー入力

#### Scenario: 移動入力の処理
  Given いつでも  
  When WASDまたは方向キーが押される  
  Then PlayerがMoveイベントを処理する  

#### Scenario: 入力の先行受付
  Given プレイヤーが[アクション]中である  
  When 行動終了のN秒以内にキー入力があった  
  Then 入力をキューに積み、行動終了時にイベントを発火させる  
  
#### Scenario: スキルの処理
  Given プレイヤーが[アイドル状態]  
  When Z/X/C/V、または○/×/△/□ボタンが押される  
  Then PlayerがSkillイベントを実行、押されたボタンの情報を引数として渡す

#### Scenario: 入力の無効化
  Given ゲームが[ポーズ状態]である  
  When なんらかのキー入力があった  
  Then 入力を処理しない  


## Feature: ゲームの勝利条件と敗北条件
  Given 常に  
  When すべての敵がいなくなった
  Then プレイヤーの勝ち

  Given 常に
  When PlayerのHPが0になった
  Then ゲームオーバー  


