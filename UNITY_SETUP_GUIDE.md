# Unity シーン構築手順書

このドキュメントは、BDD-ActionGameプロジェクトのUnityシーンを構築するための完全な手順書です。

## 📋 事前準備

### 必要なPackages
以下のパッケージがインストールされていることを確認してください：
- `com.unity.inputsystem` (1.14.0)
- `com.unity.render-pipelines.universal` (17.0.4)
- `com.unity.ugui` (2.0.0)

## 🎯 Step 1: Input System設定

### 1.1 Input Actions Asset作成
1. **Assets/Settings** フォルダを作成（存在しない場合）
2. **右クリック → Create → Input Actions** で `PlayerInputActions.inputactions` を作成
3. Input Actions Assetを開き、以下を設定：

```
Action Maps: Player
├── Move (Value, Vector2)
│   └── Binding: Keyboard WASD / Arrow Keys
├── Attack01 (Button)
│   └── Binding: Keyboard Z / Gamepad South (○)
├── Attack02 (Button)
│   └── Binding: Keyboard X / Gamepad East (×)
├── Attack03 (Button)
│   └── Binding: Keyboard C / Gamepad North (△)
├── Attack04 (Button)
│   └── Binding: Keyboard V / Gamepad West (□)
└── Pause (Button)
    └── Binding: Keyboard Escape
```

4. **Save Asset** をクリック
5. **Generate C# Class** をチェックして、**Apply** をクリック

## 🎮 Step 2: Prefab作成

### 2.1 Player Prefab作成
1. **Assets/Prefabs** フォルダを作成
2. **GameObject → 3D Object → Cube** で作成し、「Player」と命名
3. **Transform**:
   - Position: (0, 0.5, 0)
   - Scale: (0.8, 0.8, 0.8)
4. **Components追加**:
   - **Player** スクリプト
   - **Player Input** コンポーネント
     - Actions: 作成した PlayerInputActions
     - Default Map: Player
     - Behavior: Send Messages
   - **Box Collider**
     - Is Trigger: ✓
   - **Rigidbody**
     - Use Gravity: ❌
     - Is Kinematic: ✓
5. **Player Script設定**:
   - Generator ID: 1
   - Hit Point: 100
   - Move Speed: 2.0
   - Move Interval: 0.5
   - Skill List: [4] (後で設定)
6. **Prefabとして保存**

### 2.2 Bullet Prefab作成
1. **GameObject → 3D Object → Capsule** で作成し、「Bullet」と命名
2. **Transform**:
   - Scale: (0.1, 0.3, 0.1)
3. **Components追加**:
   - **Bullet** スクリプト
   - **Box Collider**
     - Is Trigger: ✓
4. **Material作成** (オプション):
   - 黄色のマテリアルを作成してCapsuleに適用
5. **Prefabとして保存**

### 2.3 Enemy Prefab作成
1. **GameObject → 3D Object → Cube** で作成し、「SimpleEnemy」と命名
2. **Transform**:
   - Position: (0, 0.5, 0)
   - Scale: (0.8, 0.8, 0.8)
3. **Components追加**:
   - **Simple Enemy** スクリプト
   - **Box Collider**
     - Is Trigger: ✓
4. **Material作成** (オプション):
   - 赤色のマテリアルを作成してCubeに適用
5. **Simple Enemy Script設定**:
   - Hit Point: 50
   - Attack Interval: 3.0
   - Generator ID: 2
   - Skill: (後で設定)
6. **Prefabとして保存**

### 2.4 Ground Prefabs作成
1. **GameObject → 3D Object → Plane** で作成し、「GroundTile」と命名
2. **Transform**:
   - Scale: (0.1, 1, 0.1) ※1x1のタイルサイズ
3. **Material作成**:
   - 緑色のマテリアルを作成して適用
4. **Prefabとして保存**

### 2.5 Shield Block Prefab作成
1. **GameObject → 3D Object → Cube** で作成し、「ShieldBlock」と命名
2. **Transform**:
   - Position: (0, 0.5, 0)
   - Scale: (1, 1, 1)
3. **Components追加**:
   - **Shield Block** スクリプト
   - **Box Collider**
     - Is Trigger: ✓
4. **Material作成**:
   - 青色のマテリアルを作成して適用
5. **Shield Block Script設定**:
   - Hit Point: 5
   - Generator ID: -1
6. **Prefabとして保存**

## 🎨 Step 3: Skill Assets作成

### 3.1 ShootBullet Skill作成
1. **Assets/Skills** フォルダを作成
2. **右クリック → Create → Skills → ShootBullet**
3. **設定**:
   - Action Interval: 0.2
   - Cool Time: 1.0
   - Attack Power: 10
   - Bullet Speed: 10.0
   - Bullet Prefab: 作成したBullet Prefab

### 3.2 ThreeLineShoot Skill作成
1. **右クリック → Create → Skills → ThreeLineShoot**
2. **設定**:
   - Action Interval: 0.5
   - Cool Time: 3.0
   - Attack Power: 15
   - Bullet Speed: 8.0
   - Bullet Prefab: 作成したBullet Prefab

## 🏗️ Step 4: メインシーン構築

### 4.1 基本シーン設定
1. **新しいシーンを作成**: File → New Scene → Basic (URP)
2. **シーンを保存**: Assets/Scenes/GameScene.unity

### 4.2 カメラ設定
1. **Main Camera**を選択
2. **Transform**:
   - Position: (5, 8, -5)
   - Rotation: (45, -45, 0)
3. **Camera**:
   - Field of View: 60
   - Clipping Planes Near: 0.3, Far: 100

### 4.3 ライティング設定
1. **Directional Light**を選択
2. **Transform**:
   - Rotation: (45, 45, 0)
3. **Light**:
   - Intensity: 1.5

### 4.4 GameManager設定
1. **GameObject → Create Empty** で「GameManager」作成
2. **Components追加**:
   - **Game Manager** スクリプト
3. **Game Manager Script設定**:
   - Player: Player Prefab
   - Enemy: SimpleEnemy Prefab
   - Result: (後で設定)

### 4.5 UI設定
1. **GameObject → UI → Canvas** で「ResultCanvas」作成
2. **Canvas**設定:
   - Render Mode: Screen Space - Overlay
3. **Canvas下に Panel作成**:
   - 名前: ResultPanel
   - Color: (0, 0, 0, 0.8) ※半透明黒
   - 初期状態: Inactive
4. **Panel下に Text作成**:
   - 名前: ResultText
   - Text: "Game Result"
   - Font Size: 48
   - Alignment: Center Middle
   - Color: White
5. **GameObject → Create Empty** で「Result」作成
6. **Components追加**:
   - **Result** スクリプト
7. **Result Script設定**:
   - Result Text: 作成したResultText
   - Result Panel: 作成したResultPanel
8. **GameManagerのResult**に設定

### 4.6 StageCreator設定
1. **GameObject → Create Empty** で「StageCreator」作成
2. **Components追加**:
   - **Stage Creator** スクリプト
3. **Stage Creator Script設定**:
   - Stage Width: 10
   - Stage Height: 10
   - Cell Prefab List: [GroundTile, null, ShieldBlock, GroundTile] ※CellList enumに対応
   - Player Pos: (5, 5)
   - Enemy List: 敵の配置設定
   - Cell Rule List: セル生成ルール

## 🎪 Step 5: プレイヤーとスキル設定

### 5.1 Player Prefabにスキル設定
1. **Player Prefab**を開く
2. **Player Script**の**Skill List**に設定:
   - Element 0: ShootBullet
   - Element 1: ThreeLineShoot
   - Element 2: (null)
   - Element 3: (null)
3. **Prefab適用**

### 5.2 Enemy Prefabにスキル設定
1. **SimpleEnemy Prefab**を開く
2. **Simple Enemy Script**の**Skill**に**ShootBullet**を設定
3. **Prefab適用**

## 🎮 Step 6: シーンにオブジェクト配置

### 6.1 プレイヤー配置
1. **Player Prefab**をシーンにドラッグ
2. **Position**: (5, 0.5, 5)

### 6.2 敵配置
1. **SimpleEnemy Prefab**を複数配置:
   - Enemy1: (2, 0.5, 8)
   - Enemy2: (8, 0.5, 8)
   - Enemy3: (5, 0.5, 2)

### 6.3 地面とステージ生成
1. **StageCreator**を選択
2. **Inspector**で**右クリック → Generate Stage**
3. または**手動で地面タイル配置**

## ⚙️ Step 7: 最終設定

### 7.1 Physics Settings
1. **Edit → Project Settings → Physics**
2. **Layer Collision Matrix**で必要に応じて調整

### 7.2 Input System Settings
1. **Edit → Project Settings → XR Plug-in Management → Input System Package**
2. **Active Input Handling**: Input System Package (New)

### 7.3 タグとレイヤー設定（オプション）
1. **Edit → Project Settings → Tags and Layers**
2. 必要に応じてPlayerやEnemyタグを追加

## 🚀 Step 8: テストプレイ

### 8.1 動作確認項目
- [ ] WASD/矢印キーでプレイヤー移動
- [ ] Z/X/C/Vキーでスキル発動
- [ ] 弾丸が敵に当たってダメージ
- [ ] 敵が自動的にスキル発動
- [ ] 全敵撃破で勝利表示
- [ ] プレイヤーHP0で敗北表示

### 8.2 トラブルシューティング
- **Input Systemが動作しない**: PlayerInputActionsが正しく設定されているか確認
- **当たり判定が動作しない**: CollidersがTriggerに設定されているか確認
- **スキルが発動しない**: SkillAssetが正しく作成・設定されているか確認

## 📚 参考情報

### ファイル構成
```
Assets/
├── Scenes/GameScene.unity
├── Scripts/ (生成済み)
├── Prefabs/
│   ├── Player.prefab
│   ├── SimpleEnemy.prefab
│   ├── Bullet.prefab
│   ├── GroundTile.prefab
│   └── ShieldBlock.prefab
├── Skills/
│   ├── ShootBullet.asset
│   └── ThreeLineShoot.asset
├── Settings/
│   └── PlayerInputActions.inputactions
└── Materials/ (オプション)
    ├── PlayerMaterial.mat
    ├── EnemyMaterial.mat
    ├── BulletMaterial.mat
    └── GroundMaterial.mat
```

この手順書に従って構築すれば、完全に動作するアクションゲームが完成します！