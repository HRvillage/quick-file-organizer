# Changelog / 版本紀錄

All notable public changes to this project will be documented in this file.

本文件記錄公開版本的重要變更。公開前的原型與介面測試版本已整合為第一個正式版本 `v1.0.0`，不另以公開版本號列出。

## [1.0.0] — 2026-07-17

### Added / 新增

- Batch file renaming from a selected folder
- 選擇資料夾後批次重新命名檔案
- Optional Name 1 and Name 2 segments
- 可選用名稱 1 與名稱 2，空白欄位自動略過
- Date sources: no date, today, manual date, created time, and modified time
- 日期來源支援不加入日期、今日日期、手動日期、建立時間與修改時間
- Date position before or after custom names
- 日期可放在自訂名稱之前或之後
- Configurable starting number and sequence digit count
- 可設定起始號碼與流水號位數
- Continued numbering for matching naming combinations
- 可延續相同命名組合的下一個流水號
- File type categories for images, videos, documents, and archives
- 圖片、影片、文件與壓縮檔分類選取
- Category-level select all with individual extension control
- 可一次全選分類，也可個別選擇副檔名
- Video support for MP4, MOV, and AVI
- 影片支援 MP4、MOV 與 AVI
- Live single-line filename preview
- 即時單行檔名預覽
- Folder drag-and-drop
- 支援拖曳資料夾
- Conflict detection before renaming
- 重新命名前檢查同名衝突
- Temporary two-phase rename flow to avoid internal filename collisions
- 以暫存名稱分兩階段處理，避免批次內部檔名衝突
- One-step undo for the latest successful rename
- 可還原上一次成功重新命名
- Chinese and English interface
- 中文與英文介面切換
- Portable local settings and undo data
- 可攜式本機設定與復原紀錄
- Clickable HeroRaye copyright link
- 可點擊的 HeroRaye 版權連結

### Interface / 介面

- Compact card-based Windows interface
- 緊湊卡片式 Windows 介面
- Fixed-size single-screen layout without scrolling
- 固定尺寸單頁畫面，不需要捲動
- Clear primary, secondary, and undo action styles
- 明確區分主要、次要與復原操作按鈕
- Optimized language switching to update interface text in one refresh
- 語言切換採單次刷新，減少逐項更新造成的延遲

### Distribution / 發布

- .NET 8 self-contained single-file Windows build
- .NET 8 自包含單檔 Windows 建置
- Portable `Data` directory for preferences and last rename record
- 使用 `Data` 資料夾保存偏好設定與上一次重新命名紀錄
- No executable compression in the release build
- 正式建置不使用執行檔壓縮
