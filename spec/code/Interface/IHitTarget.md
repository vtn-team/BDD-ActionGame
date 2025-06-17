# IHitTargetインタフェース設計


# 概要
- Bulletの当たり判定を持つオブジェクト


# 実装
- 以下の実装を束縛するインタフェース


# インタフェース
- Damage: ダメージ処理。攻撃が通ったらtrue、通らなかったらfalseを返す。
- GetGeneratorID: 生成者IDを返す
- MaxHitPoint: 最大HitPointを返す(読み取り専用)
- HitPoint: 現在HitPointを返す(読み取り専用)
- HeadPos: 頭の位置に該当するVector3の位置を返す