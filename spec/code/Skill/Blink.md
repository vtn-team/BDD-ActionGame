# Blinkクラス設計

## 概要
前方3マスに瞬間移動するルール破壊型スキル

## 実装
- SkillBaseを継承する
- ```/spec/gamedesign/skill/blink.md``` を参照すること

## 処理フロー
1. プレイヤーの前方向(transform.forward)を取得
2. 3マス前方の目標地点を計算
3. 移動経路上の障害物を段階的にチェック（1マス目→2マス目→3マス目）
4. 障害物がある場合はその手前、ない場合は3マス前方に瞬間移動
5. 移動完了後に地形効果を適用

## SerializeFieldで設定するprivate変数
- _actionInterval: 0.3f（瞬間移動演出時間）
- _coolTime: 6f（クールタイム）
- _blinkDistance: 3（移動距離）

## 実装すべきメソッド
- Execute(GameObject executor): スキル実行処理
  - executorのtransform.forwardを取得
  - 目標地点計算とステージ境界チェック
  - 移動経路の障害物チェック
  - 最適な移動先決定
  - 瞬間移動実行

## 障害物チェック詳細
- Physics.OverlapSphereを使用してオブジェクト検知
- Player、EnemyBase、ShieldBlock、障害物との衝突判定
- ステージCreatorのStageSize境界チェック
- 各マス地点での占有状況確認

## 移動実行詳細
- executor.transform.positionを直接変更（瞬間移動）
- 移動アニメーションはなし
- 移動後のCheckGroundInteraction呼び出し（地形効果適用）
- StartCooldown()でクールタイム開始

## 期待値
- 前方向に最大3マス瞬間移動
- 障害物手前での適切な停止
- 地形制約を無視した移動
- クールタイム管理の正常動作