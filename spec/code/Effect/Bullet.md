# Bulletクラス設計


# 概要
- 直線状に移動する弾


# 実装
- MonoBehaviourを継承する
- 生成時に攻撃力、速度、生成者IDが渡される
- BoxColliderを持つ
- staticのBuilder関数を持つ。攻撃力、速度、生成者ID、射出元のposition、移動方向のベクトルを渡す必要がある。


# 処理フロー
1. Builder関数によって生成(Instantiate)される。
	1. BulletのPrefabはResourcesから読み込む。
	2. 渡された射出元のpositionから、移動方向のベクトル方向1マス先にInstantiateする。
	3. rotationを進行方向に向くように修正する
2. Updateで引数でもらった移動方向のベクトル方向に進む。speed秒で1セル分進む。
3. ステージ外に出たら自分自身を消す


# 期待値
- 当たり判定を以下のロジックで行う
	- OnCollisionEnterで当たり判定を行う
	- 対象のGameObjectからIHitTargetを取得する。
		- IHitTargetがない場合は判定を終了する。
	- 生成者IDと異なるIDを持つオブジェクトに当たった場合、IHitTargetのDamageを呼び出し、攻撃力を引数に渡す
- 弾同士の衝突処理
	- 弾同士が衝突した場合、両方の弾が消滅する
	- 生成者IDが同じ弾同士は衝突しない（味方の弾は通り抜ける）
	- 生成者IDが異なる弾同士のみ衝突して消滅する
