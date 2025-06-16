# コーディングルール

## 基本要素
- ファイルおよびクラスはなるべく分割する事
- クラス間の関係性は疎結合である事
- テストを書く事で契約的な実装である事を保証する

## ライブラリの利用
- Unity6のネイティブ実装を利用し、外部ライブラリは使用しない
- interfaceのインスペクタ指定のために、SubClassSelectorを使用する。
	- ```/unity/Assets/ThirdParty/Runtime/SubclassSelectorAttribute.cs```と```/unity/Assets/ThirdParty/Editor/SubclassSelectorDrawer.cs```が該当する

## 変数について
- private変数の先頭には_をつけること
- public変数は使用しないこと
	- 設計上必要な場合はアクセスを作成すること
- 関数名はアッパーキャメルケースにすること 

## スクリプトの配置
- /spec/codeに記載している仕様は、すべてUnityのC#スクリプトである
- /spec/code以下に記載の仕様名でクラス名とファイル名を生成し、/unity/Assets/Scripts以下に配置する
	- code以下の階層は維持すること
