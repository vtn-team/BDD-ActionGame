# AI開発コマンド一覧
以下に一致するプロンプトだった時、処理フローに従った処理を実行する

# `update` または `更新`
## 処理フロー
1. 処理を開始した時間を覚えておく。これを[実行時間]と定義する。
	1. [前回の処理時間]としてメモリ上に保存しておく
2. `/LAST_COMMAND_LOG.md`を解析し、最も新しい[最終更新日時]を取得する
	1. [前回の処理時間]がメモリ上にある場合はそれを使用し、解析を省略する
	2. ファイルが存在しない、または解析に失敗した場合は、当日の00:00を[最終更新日]とする
3. `/spec`内を対象に、[最終更新日時]よりも更新日時が新しいファイルを検索する
	1. `find ./spec -type f -newermt [最終更新日時]`
4. (2)でリストアップされた仕様ファイルの差分を解読し、実装する
5. `/spec`内を対象としたファイルの差分をgit commitする。これは以下の手順で行う。
	1. ステージされているファイルをすべて外す
	2. `/spec`以下の差分があるファイルをステージにあげていく
	3. 仕様書の更新内容をワンライナーでまとめてコミットメッセージにする
	4. コミットする
6. [ログ出力]を行う


# `check` または `精査`
## 処理フロー
1. `/spec`にあるファイルを全解析する
2. `/unity`にある実装を全解析し、実装差異があるファイルをリストアップする
3. `/spec/workcheck`に、`check_[現在の日時].md`というログでまとめて出力する


# `analyze` または `解析`
## 処理フロー
1. `/unity`にある実装を全解析し、設計上の問題点やデータフローの問題を分析する
	1. この分析時には、`/spec/rule`にあるファイルも参考にすること
2. `/spec/workcheck`に、`analyze_[現在の日時].md`というログでまとめて出力する