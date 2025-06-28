# GEMINI.md

このファイルはGeminiがこのリポジトリでコードを扱う際のガイドラインを提供します。

## プロジェクト概要
Unity6を使用したアクションゲームプロジェクト。キューブ型のプレイヤーキャラクターがスキルを使用して敵を倒すゲーム。BDD（振る舞い駆動開発）手法を採用し、Gherkinシナリオベースで仕様を管理している。

### ゲーム概要
- プレイヤー：スキルを使えるキューブ型キャラクター
- 勝利条件：すべての敵を倒すこと
- 敗北条件：プレイヤーのHPが0になること
- 入力：WASD/方向キーで移動、ZXCV/○×△□でスキル発動

## プロジェクト構造
```
/
├── spec/           # 仕様書群（BDD形式）
│   ├── code/       # 各クラスの詳細仕様
│   ├── gamedesign/ # ゲームデザイン仕様
│   ├── rule/       # コーディングルールとUnity規則
│   ├── ubi/        # ユビキタス言語定義
│   └── usecase/    # Gherkinシナリオ
├── unity/          # Unity プロジェクト
│   └── Assets/
│       ├── Scripts/    # C# スクリプト群
│       ├── Prefabs/    # プレハブ群
│       ├── Materials/  # マテリアル群
│       └── Scenes/     # シーン群
└── GEMINI.md       # このファイル
```

## Unity開発

### 使用技術
- Unity 6（最新LTS）
- Unity Input System（新しい入力システム）
- Cinemachine（カメラ制御）
- URP（Universal Render Pipeline）

### 必須ライブラリ
- SubclassSelector：インターフェースのインスペクタ指定用
  - `/unity/Assets/ThirdParty/Runtime/SubclassSelectorAttribute.cs`
  - `/unity/Assets/ThirdParty/Editor/SubclassSelectorDrawer.cs`

### 開発フロー
1. GUID辞書の更新（unity/guid_dictionary.yaml）
2. 仕様書ベースでの実装
3. アセット生成時のAssetDatabase.Refresh実行
4. 参照アサイン時のGUID使用

## アーキテクチャ概要

### 主要コンポーネント
- **GameManager**：ゲーム全体の制御、勝利/敗北判定
- **Player**：プレイヤーキャラクター、移動・スキル実行
- **EnemyBase**：敵キャラクターの基底クラス
- **SkillBase**：スキルシステムの基底クラス
- **IHitTarget**：ダメージ処理のインターフェース
- **StageCreator**：ステージ生成・管理

### 設計原則
- クラス間の疎結合
- インターフェースによる抽象化
- 基底クラスによる共通機能の提供
- Builderパターンの活用（HitPointGauge）

## ゲームフロー

### 初期化フロー
1. GameManager.Awake()でプレイヤー・敵・カメラを初期化
2. Cinemachine TargetGroupにプレイヤー・敵を登録
3. HitPointGaugeを各キャラクターに生成

### ゲームループ
1. Player.Update()で入力処理・移動・スキル実行
2. GameManager.Update()で勝利/敗北条件チェック
3. 各Enemy.Update()で敵の行動処理
4. 結果判定とResult表示

### 状態管理
- **アイドル状態**：何もアクションしていない状態
- **移動中**：移動アニメーション実行中
- **スキル実行中**：スキルのクールタイム中
- **ポーズ状態**：ゲーム進行停止状態

## コーディング標準

### 命名規則
- private変数：アンダースコアプリフィックス（`_variableName`）
- public変数：使用禁止（アクセサを作成）
- 関数名：アッパーキャメルケース（`FunctionName`）
- クラス名：spec/code以下の仕様名と一致

### ファイル配置規則
- スクリプト：`/unity/Assets/Scripts/`以下にspec/codeの階層構造を維持
- プレハブ：`/unity/Assets/Prefabs/`または`/unity/Assets/Resources/`
- マテリアル：`/unity/Assets/Materials/`

### インターフェース実装
- SerializeReference + SubclassSelector属性の使用
- インスペクタでの具象クラス選択対応



## AI開発コンテキスト
このプロジェクトは包括的仕様書によるAI支援開発を使用します。
## 基本フロー
1. **必ず最初に行う**`/COMMAND.md`を解析し、プロンプトと一致するコマンドがあるか確認する
	1. 一致するコマンドがある場合は、記載の処理フローに従う
	2. [ログ出力]を行う
	3. このファイルはセッション中更新されることはない。次回からの実行を早くするべく、メモリに情報を保存し、次回からは解析を省略する。
2. 対応する仕様書ファイルを参照
3. 既存コードの**確立されたアーキテクチャ**パターンに従う
4. `spec/rule/coding.md`のコーディングルールに従って**指定されたライブラリを**使用
5. `spec/ubi/ubiquitous.md`の**ユビキタス言語との一貫性**を維持
6. `spec/usecase/*.md`の**Gherkinシナリオを正確に**実装
7. `spec/code/*.md`の詳細仕様に従ってコードを出力する
8. [ログ出力]を行う

## ログ出力
1. 以下リストアップする内容を`/LAST_COMMAND_LOG.md`に上書き保存する
	1. 最終更新日時：[実行日時]
	2. 仕様書の更新内容
	3. 更新したファイルの情報
2. (5)の内容を`/log/`に追記保存する
	4. ログ名は日付形式 (2025-06-01.md など)
	5. ファイルは必ず追記すること


仕様書は、人間の可読性と保守性を維持しながらAIコード生成に十分な包括性を持つよう設計されています。



## 実装状況

### 完成済み機能
- **基本ゲームシステム**
  - プレイヤー移動（WASD/方向キー）
  - スキルシステム（ZXCV/○×△□）
  - HP システムとダメージ処理
  - 勝利/敗北判定

- **コアコンポーネント**
  - GameManager（ゲーム制御）
  - Player（プレイヤー制御）
  - EnemyBase（敵基底クラス）
  - SkillBase（スキル基底クラス）
  - IHitTarget（ダメージインターフェース）

- **UI・表示系**
  - HitPointGauge（HP表示）
  - Result（結果表示）
  - Cinemachine カメラシステム

- **開発環境**
  - Unity Input System設定
  - URP設定
  - SubclassSelector統合

### 実装中・未完成機能
- **具体的な敵キャラクター**（SimpleEnemyの行動AI）
- **具体的なスキル**（各スキルの効果実装）
- **ステージシステム**（StageCreator詳細実装）
- **アイテムシステム**（HeartItem等）
- **UI拡張**（メニュー、設定画面等）

### 技術的な課題
- 敵AIパターンの実装
- スキルエフェクトシステム
- セーブ/ロードシステム
- パフォーマンス最適化
