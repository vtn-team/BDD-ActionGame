# SkillBaseクラス設計


# 概要
- スキルの基本制御をするベースクラス


# 実装
- このクラスを派生させてスキルを表現する。このクラスはabstract classである


# SerializeFieldで設定するprivate変数
- actionInterval: 次の行動までの行動制限時間
- coolTime: スキル再使用までのクールタイム


# private変数
- coolTimeTimer: クールタイムのカウント変数


# 外部インタフェース
- NextActionInterval: 行動制限時間を返す
- CoolTime: クールタイム中の場合、再使用までの時間を返す
- CheckExecute: スキルが実行可能かを確認する
- Execute: スキルを実行する。処理するGameObjectを引数に渡す。


# 処理フロー
- 派生先に倣う


# 期待値
- 派生先に倣う


# エッジケース
- なし
